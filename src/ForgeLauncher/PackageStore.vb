Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text

''' <summary>A Forge package kept in <c>fldata\packages</c> so the user can return to it later.</summary>
Public NotInheritable Class StoredPackage
    ''' <summary>Full path of the <c>.tar.bz2</c> package.</summary>
    Public Property ArchivePath As String
    ''' <summary>Full path of the <c>.info</c> file that describes the package.</summary>
    Public Property InfoPath As String
    ''' <summary>Channel the package came from.</summary>
    Public Property Channel As UpdateChannel
    ''' <summary>Forge version of the package.</summary>
    Public Property Version As String
    ''' <summary>Build time of the package.</summary>
    Public Property BuildDate As Date?
    ''' <summary>When the package was installed.</summary>
    Public Property InstalledAt As Date?
    ''' <summary>Size of the package file in bytes.</summary>
    Public Property SizeBytes As Long

    ''' <summary>Human-readable description, for example <c>2.0.15 (built 2026-09-28 22:22)</c>.</summary>
    Public ReadOnly Property Description As String
        Get
            Return ForgeInstallation.Describe(Version, BuildDate)
        End Get
    End Property
End Class

''' <summary>
''' Keeps downloaded Forge packages for rollback. Each package (<c>forge-installer-*.tar.bz2</c>) is stored next to
''' a small <c>.info</c> text file with its channel, version, build time and install time, so the list of
''' saved versions can be shown instantly without opening 300 MB packages.
''' </summary>
Public NotInheritable Class PackageStore
    Private Const InfoExtension As String = ".info"
    Private Const DateFormat As String = "yyyy-MM-dd HH:mm:ss"

    Private ReadOnly _folder As String

    ''' <summary>Creates a store over a folder; the folder is created when the first package is added.</summary>
    ''' <param name="folder">The <c>fldata\packages</c> folder.</param>
    Public Sub New(folder As String)
        _folder = folder
    End Sub

    ''' <summary>Lists the saved packages, newest build first. Entries whose package file is missing are ignored.</summary>
    Public Function GetAll() As List(Of StoredPackage)
        Dim packages As New List(Of StoredPackage)
        If Not Directory.Exists(_folder) Then Return packages

        For Each infoPath In ForgeInstallation.SafeGetFiles(_folder, "*" & InfoExtension)
            Dim package = ReadInfo(infoPath)
            If package IsNot Nothing Then packages.Add(package)
        Next
        Return packages.
            OrderByDescending(Function(package) package.BuildDate.GetValueOrDefault(Date.MinValue)).
            ThenByDescending(Function(package) package.InstalledAt.GetValueOrDefault(Date.MinValue)).
            ToList()
    End Function

    ''' <summary>Total disk space used by the saved packages, in bytes.</summary>
    Public Function TotalSize() As Long
        Return GetAll().Sum(Function(package) package.SizeBytes)
    End Function

    ''' <summary>
    ''' Moves a freshly installed package into the store and writes its <c>.info</c> file.
    ''' A package with the same file name (a reinstall of the same version) is replaced.
    ''' </summary>
    ''' <param name="downloadedArchive">Downloaded package in <c>fldata\downloads</c>.</param>
    ''' <param name="build">Build that the package contains.</param>
    Public Function Add(downloadedArchive As String, build As RemoteBuild) As StoredPackage
        Directory.CreateDirectory(_folder)
        Dim archivePath = Path.Combine(_folder, PackageInstaller.PackageFileName(build.PackageUrl))
        PackageInstaller.TryDelete(archivePath)
        File.Move(downloadedArchive, archivePath)

        Dim package As New StoredPackage With {
            .ArchivePath = archivePath,
            .InfoPath = archivePath & InfoExtension,
            .Channel = build.Channel,
            .Version = build.Version,
            .BuildDate = build.BuildDate,
            .InstalledAt = Date.Now,
            .SizeBytes = New FileInfo(archivePath).Length}
        WriteInfo(package)
        Return package
    End Function

    ''' <summary>Records that a saved package was installed again (used after a rollback).</summary>
    Public Sub MarkInstalled(package As StoredPackage)
        package.InstalledAt = Date.Now
        WriteInfo(package)
    End Sub

    ''' <summary>
    ''' Deletes packages the user no longer wants to keep: the package of the installed build plus the
    ''' <paramref name="keepPrevious"/> newest other packages are kept. With 0, every package is deleted.
    ''' </summary>
    ''' <param name="keepPrevious">Number of previous versions to keep (0, 1 or 2).</param>
    ''' <param name="installedBuild">Build time of the installed Forge, used to recognize its package.</param>
    ''' <returns>The deleted packages.</returns>
    Public Function Prune(keepPrevious As Integer, installedBuild As Date?) As List(Of StoredPackage)
        Dim deleted As New List(Of StoredPackage)
        Dim packages = GetAll()
        Dim keptOthers = 0

        For Each package In packages
            Dim isInstalled = installedBuild.HasValue AndAlso package.BuildDate.HasValue AndAlso package.BuildDate.Value = installedBuild.Value
            Dim keep As Boolean
            If keepPrevious <= 0 Then
                keep = False
            ElseIf isInstalled Then
                keep = True
            Else
                keep = keptOthers < keepPrevious
                If keep Then keptOthers += 1
            End If
            If Not keep Then
                Delete(package)
                deleted.Add(package)
            End If
        Next
        Return deleted
    End Function

    ''' <summary>Deletes a saved package and its <c>.info</c> file.</summary>
    Public Sub Delete(package As StoredPackage)
        PackageInstaller.TryDelete(package.ArchivePath)
        PackageInstaller.TryDelete(package.InfoPath)
    End Sub

    ''' <summary>Reads an <c>.info</c> file; returns <c>Nothing</c> when it is unreadable or its package is gone.</summary>
    Private Shared Function ReadInfo(infoPath As String) As StoredPackage
        Dim archivePath = infoPath.Substring(0, infoPath.Length - InfoExtension.Length)
        If Not File.Exists(archivePath) Then Return Nothing

        Dim values As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Try
            For Each line In File.ReadAllLines(infoPath, Encoding.UTF8)
                Dim separator = line.IndexOf("="c)
                If separator > 0 Then values(line.Substring(0, separator).Trim()) = line.Substring(separator + 1).Trim()
            Next
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return Nothing
        End Try

        Dim channel As UpdateChannel = UpdateChannel.Snapshot
        Dim channelText As String = Nothing
        If values.TryGetValue("Channel", channelText) Then [Enum].TryParse(channelText, True, channel)

        Return New StoredPackage With {
            .ArchivePath = archivePath,
            .InfoPath = infoPath,
            .Channel = channel,
            .Version = GetValue(values, "Version"),
            .BuildDate = ForgeInstallation.ParseBuildDate(GetValue(values, "BuildDate")),
            .InstalledAt = ForgeInstallation.ParseBuildDate(GetValue(values, "InstalledAt")),
            .SizeBytes = New FileInfo(archivePath).Length}
    End Function

    ''' <summary>Writes an <c>.info</c> file as <c>Key=Value</c> lines.</summary>
    Private Shared Sub WriteInfo(package As StoredPackage)
        Dim lines = {
            "Channel=" & package.Channel.ToString(),
            "Version=" & package.Version,
            "BuildDate=" & FormatDate(package.BuildDate),
            "InstalledAt=" & FormatDate(package.InstalledAt)}
        File.WriteAllLines(package.InfoPath, lines, New UTF8Encoding(False))
    End Sub

    ''' <summary>Formats an optional date in the same format as <c>build.txt</c>.</summary>
    Private Shared Function FormatDate(value As Date?) As String
        Return If(value.HasValue, value.Value.ToString(DateFormat, CultureInfo.InvariantCulture), String.Empty)
    End Function

    ''' <summary>Returns a dictionary value or <c>Nothing</c>.</summary>
    Private Shared Function GetValue(values As Dictionary(Of String, String), key As String) As String
        Dim value As String = Nothing
        Return If(values.TryGetValue(key, value), value, Nothing)
    End Function
End Class
