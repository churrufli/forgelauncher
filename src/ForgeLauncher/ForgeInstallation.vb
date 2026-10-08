Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>Information read from one Forge jar file in the Forge folder.</summary>
Public NotInheritable Class ForgeJar
    ''' <summary>Full path of the jar.</summary>
    Public Property FilePath As String
    ''' <summary>Version taken from the file name, for example <c>2.0.15</c> or <c>2.0.16-SNAPSHOT</c>.</summary>
    Public Property Version As String
    ''' <summary>Build time read from the <c>build.txt</c> stored inside the jar, if any.</summary>
    Public Property BuildDate As Date?
    ''' <summary>Last modification time of the jar file, used to break ties.</summary>
    Public Property LastWriteTimeUtc As Date
End Class

''' <summary>
''' Describes the Forge installation found in a folder: whether Forge is installed, which version and
''' build it is, and where Forge keeps its user data.
''' </summary>
''' <remarks>
''' Every Forge package contains a <c>build.txt</c> file with the build time (for example
''' <c>2026-10-06 18:28:55</c>). Forge's build process publishes the very same file on GitHub, so comparing
''' the two tells exactly whether the installed build is the latest one. The same file is also stored inside
''' the desktop jar, which lets the launcher identify installations that lack the copy in the Forge folder.
''' </remarks>
Public NotInheritable Class ForgeInstallation
    ''' <summary>Name of the file that holds the build time.</summary>
    Public Const BuildFileName As String = "build.txt"

    ''' <summary>Forge's profile file, which can move Forge's user data and cache folders.</summary>
    Public Const ProfileFileName As String = "forge.profile.properties"

    ''' <summary>Suffix shared by every Forge application jar.</summary>
    Public Const JarSuffix As String = "-jar-with-dependencies.jar"

    Private Const BuildDateFormat As String = "yyyy-MM-dd HH:mm:ss"

    ''' <summary>Extracts the version from jar names such as <c>forge-gui-desktop-2.0.15-jar-with-dependencies.jar</c>.</summary>
    Private Shared ReadOnly JarVersionPattern As New Regex(
        "^forge-gui-(?:desktop|mobile-dev)-(?<version>.+)-jar-with-dependencies\.jar$",
        RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)

    Private Sub New(folder As String, isInstalled As Boolean, version As String, buildDate As Date?)
        Me.Folder = folder
        Me.IsInstalled = isInstalled
        Me.Version = version
        Me.BuildDate = buildDate
    End Sub

    ''' <summary>The Forge folder that was inspected.</summary>
    Public ReadOnly Property Folder As String

    ''' <summary><c>True</c> when the folder contains forge.exe or a Forge desktop jar.</summary>
    Public ReadOnly Property IsInstalled As Boolean

    ''' <summary>Installed version, taken from the desktop jar name; <c>Nothing</c> when unknown.</summary>
    Public ReadOnly Property Version As String

    ''' <summary>Installed build time; <c>Nothing</c> when it could not be determined.</summary>
    Public ReadOnly Property BuildDate As Date?

    ''' <summary>Human-readable description, for example <c>2.0.15 (built 2026-09-28 22:22)</c>.</summary>
    Public ReadOnly Property Description As String
        Get
            If Not IsInstalled Then Return "not installed"
            Return Describe(Version, BuildDate)
        End Get
    End Property

    ''' <summary>
    ''' Inspects a folder. The build time comes from <c>build.txt</c> in the folder or, for older
    ''' installations without that file, from <c>build.txt</c> inside the newest desktop jar.
    ''' </summary>
    ''' <param name="folder">Forge folder to inspect.</param>
    Public Shared Function Detect(folder As String) As ForgeInstallation
        Dim desktopJar = FindNewestJar(folder, ForgeApp.Desktop)
        Dim hasForgeExe = File.Exists(Path.Combine(folder, "forge.exe"))
        Dim buildDate = ParseBuildDate(ReadText(Path.Combine(folder, BuildFileName)))
        Dim version As String = Nothing

        If desktopJar IsNot Nothing Then
            version = desktopJar.Version
            If Not buildDate.HasValue Then buildDate = desktopJar.BuildDate
        End If

        Return New ForgeInstallation(folder, hasForgeExe OrElse desktopJar IsNot Nothing, version, buildDate)
    End Function

    ''' <summary>Formats a version and an optional build time the same way everywhere in the launcher.</summary>
    ''' <param name="version">Version text; <c>Nothing</c> shows "unknown version".</param>
    ''' <param name="buildDate">Build time, if known.</param>
    Public Shared Function Describe(version As String, buildDate As Date?) As String
        Dim versionText = If(String.IsNullOrEmpty(version), "unknown version", version)
        Return If(buildDate.HasValue, $"{versionText} (built {FormatBuildDate(buildDate.Value)})", versionText)
    End Function

    ''' <summary>File name prefix of the jar of a Forge application.</summary>
    ''' <param name="app">Forge application.</param>
    Public Shared Function JarPrefix(app As ForgeApp) As String
        Return If(app = ForgeApp.Adventure, "forge-gui-mobile-dev-", "forge-gui-desktop-")
    End Function

    ''' <summary>
    ''' Finds the newest jar of a Forge application. Old jars can remain from earlier versions,
    ''' so the jar with the latest build time (or, failing that, the latest file time) wins.
    ''' </summary>
    ''' <param name="folder">Forge folder.</param>
    ''' <param name="app">Forge application whose jar is wanted.</param>
    ''' <returns>The jar, or <c>Nothing</c> when the folder has none.</returns>
    Public Shared Function FindNewestJar(folder As String, app As ForgeApp) As ForgeJar
        Return SafeGetFiles(folder, JarPrefix(app) & "*" & JarSuffix).
            Select(AddressOf InspectJar).
            OrderByDescending(Function(jar) jar.BuildDate.GetValueOrDefault(Date.MinValue)).
            ThenByDescending(Function(jar) jar.LastWriteTimeUtc).
            FirstOrDefault()
    End Function

    ''' <summary>Parses the <c>yyyy-MM-dd HH:mm:ss</c> format used by <c>build.txt</c>.</summary>
    ''' <param name="text">File content; surrounding whitespace is ignored.</param>
    ''' <returns>The build time, or <c>Nothing</c> when the text is empty or invalid.</returns>
    Public Shared Function ParseBuildDate(text As String) As Date?
        If String.IsNullOrWhiteSpace(text) Then Return Nothing
        Dim value As Date
        If Date.TryParseExact(text.Trim(), BuildDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, value) Then Return value
        Return Nothing
    End Function

    ''' <summary>Formats a build time for display (minutes precision).</summary>
    Public Shared Function FormatBuildDate(value As Date) As String
        Return value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    ''' Folder where Forge stores decks, preferences and its log: the <c>userDir</c> set in
    ''' forge.profile.properties, or <c>%APPDATA%\Forge</c> by default (the same rule Forge uses).
    ''' </summary>
    Public Function GetUserDataFolder() As String
        Dim configured = ReadProfileValue("userDir")
        If Not String.IsNullOrWhiteSpace(configured) Then Return ResolvePath(configured)
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Forge")
    End Function

    ''' <summary>Forge's own log file inside the user data folder.</summary>
    Public Function GetLogFile() As String
        Return Path.Combine(GetUserDataFolder(), "forge.log")
    End Function

    ''' <summary>Forge's preferences file; deleting it resets Forge to default preferences.</summary>
    Public Function GetPreferencesFile() As String
        Return Path.Combine(GetUserDataFolder(), "preferences", "forge.preferences")
    End Function

    ''' <summary>
    ''' Files that can be started in normal launch mode: the .exe, .cmd and .bat files of the Forge folder,
    ''' with forge.exe first and forge-adventure.exe second.
    ''' </summary>
    ''' <param name="excludedFileName">File name to leave out (the launcher itself).</param>
    Public Function GetLaunchTargets(excludedFileName As String) As List(Of String)
        Dim names As New List(Of String)
        For Each pattern In {"*.exe", "*.cmd", "*.bat"}
            names.AddRange(SafeGetFiles(Folder, pattern).Select(Function(filePath) Path.GetFileName(filePath)))
        Next
        Return names.
            Where(Function(name) Not String.Equals(name, excludedFileName, StringComparison.OrdinalIgnoreCase)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(name) LaunchPriority(name)).
            ThenBy(Function(name) name, StringComparer.OrdinalIgnoreCase).
            ToList()
    End Function

    ''' <summary>
    ''' Creates forge.profile.properties for a portable installation, so Forge keeps its user data and cache
    ''' inside the Forge folder. An existing profile is never overwritten. The file is written without a byte
    ''' order mark, because Java would read the mark as part of the first key name.
    ''' </summary>
    ''' <returns><c>True</c> when the file was created.</returns>
    Public Function CreatePortableProfile() As Boolean
        Dim profile = Path.Combine(Folder, ProfileFileName)
        If File.Exists(profile) Then Return False
        Dim content = "userDir=./user/" & Environment.NewLine & "cacheDir=./cache/" & Environment.NewLine
        File.WriteAllText(profile, content, New UTF8Encoding(False))
        Return True
    End Function

    ''' <summary>
    ''' Returns <c>True</c> when Forge is running from this folder. Java keeps the Forge jars open, so trying to
    ''' open a jar exclusively fails while Forge (desktop or Adventure) is running.
    ''' </summary>
    Public Function IsInUse() As Boolean
        For Each jarPath In SafeGetFiles(Folder, "*" & JarSuffix)
            Try
                Using stream As New FileStream(jarPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                End Using
            Catch ex As IOException
                Return True
            Catch ex As UnauthorizedAccessException
            End Try
        Next
        Return False
    End Function

    ''' <summary>
    ''' Reads one key from forge.profile.properties using Java properties rules for comments,
    ''' separators (<c>=</c> or <c>:</c>) and doubled backslashes.
    ''' </summary>
    Private Function ReadProfileValue(key As String) As String
        Dim profile = Path.Combine(Folder, ProfileFileName)
        If Not File.Exists(profile) Then Return Nothing

        Dim lines As String()
        Try
            lines = File.ReadAllLines(profile)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return Nothing
        End Try

        For Each rawLine In lines
            Dim line = rawLine.Trim()
            If line.Length = 0 OrElse line.StartsWith("#", StringComparison.Ordinal) OrElse line.StartsWith("!", StringComparison.Ordinal) Then Continue For
            Dim separator = line.IndexOfAny({"="c, ":"c})
            If separator <= 0 Then Continue For
            If Not String.Equals(line.Substring(0, separator).Trim(), key, StringComparison.Ordinal) Then Continue For
            Return line.Substring(separator + 1).Trim().Replace("\\", "\")
        Next
        Return Nothing
    End Function

    ''' <summary>Resolves a profile path; relative paths are relative to the Forge folder, as in Forge.</summary>
    Private Function ResolvePath(value As String) As String
        Dim normalized = value.Replace("/"c, Path.DirectorySeparatorChar)
        Try
            Return Path.GetFullPath(If(Path.IsPathRooted(normalized), normalized, Path.Combine(Folder, normalized)))
        Catch ex As Exception When TypeOf ex Is ArgumentException OrElse TypeOf ex Is NotSupportedException OrElse TypeOf ex Is PathTooLongException
            Return Path.Combine(Folder, normalized)
        End Try
    End Function

    ''' <summary>Reads the version from a jar's name and the build time from the <c>build.txt</c> inside it.</summary>
    Private Shared Function InspectJar(jarPath As String) As ForgeJar
        Dim match = JarVersionPattern.Match(Path.GetFileName(jarPath))
        Dim jar As New ForgeJar With {
            .FilePath = jarPath,
            .Version = If(match.Success, match.Groups("version").Value, Nothing),
            .LastWriteTimeUtc = File.GetLastWriteTimeUtc(jarPath)}

        Try
            Using archive = ZipFile.OpenRead(jarPath)
                Dim entry = archive.GetEntry(BuildFileName)
                If entry IsNot Nothing Then
                    Using reader As New StreamReader(entry.Open(), Encoding.UTF8)
                        jar.BuildDate = ParseBuildDate(reader.ReadToEnd())
                    End Using
                End If
            End Using
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is InvalidDataException OrElse TypeOf ex Is UnauthorizedAccessException
        End Try

        Return jar
    End Function

    ''' <summary>Sort order of the launch file list: forge.exe, forge-adventure.exe, other .exe, .cmd, .bat.</summary>
    Private Shared Function LaunchPriority(name As String) As Integer
        If String.Equals(name, "forge.exe", StringComparison.OrdinalIgnoreCase) Then Return 0
        If String.Equals(name, "forge-adventure.exe", StringComparison.OrdinalIgnoreCase) Then Return 1
        Select Case Path.GetExtension(name).ToLowerInvariant()
            Case ".exe" : Return 2
            Case ".cmd" : Return 3
            Case Else : Return 4
        End Select
    End Function

    ''' <summary>Reads a whole text file, returning <c>Nothing</c> when it is missing or unreadable.</summary>
    Private Shared Function ReadText(filePath As String) As String
        If Not File.Exists(filePath) Then Return Nothing
        Try
            Return File.ReadAllText(filePath)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return Nothing
        End Try
    End Function

    ''' <summary>Lists the files of a folder (not its subfolders), returning an empty list on errors.</summary>
    ''' <param name="folder">Folder to list.</param>
    ''' <param name="pattern">Wildcard pattern, for example <c>*.jar</c>.</param>
    Friend Shared Function SafeGetFiles(folder As String, pattern As String) As String()
        Try
            Return Directory.GetFiles(folder, pattern, SearchOption.TopDirectoryOnly)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return Array.Empty(Of String)()
        End Try
    End Function
End Class
