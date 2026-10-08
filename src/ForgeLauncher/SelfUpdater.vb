Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Reflection
Imports System.Threading
Imports System.Threading.Tasks

''' <summary>A Forge Launcher release published on GitHub.</summary>
Public NotInheritable Class LauncherRelease
    ''' <summary>Release version, for example 3.1.0.</summary>
    Public Property Version As Version
    ''' <summary>Release tag, for example <c>v3.1.0</c>.</summary>
    Public Property Tag As String
    ''' <summary>Download URL of the <c>ForgeLauncher-X.Y.Z-win.zip</c> package.</summary>
    Public Property PackageUrl As String
    ''' <summary>GitHub page of the release, shown as "What's new".</summary>
    Public Property PageUrl As String
End Class

''' <summary>
''' Keeps Forge Launcher itself up to date from its GitHub repository.
''' </summary>
''' <remarks>
''' <para>Publishing a new version: create a GitHub release tagged <c>vX.Y.Z</c> (matching the version in
''' AssemblyInfo.vb) and attach <c>ForgeLauncher-X.Y.Z-win.zip</c> containing <c>Forge Launcher.exe</c> and
''' <c>ICSharpCode.SharpZipLib.dll</c> (at the root of the zip or in one folder).</para>
''' <para>Windows does not allow overwriting a running executable, but it does allow renaming it. The update therefore
''' moves the running exe and DLL into <c>fldata\update</c> as <c>.old</c> files, moves the new files into place, starts
''' the new version and exits. If anything fails half way, the old files are moved back. The new version deletes the
''' <c>.old</c> files when it starts.</para>
''' <para>While the repository is private, GitHub answers 404 to anonymous requests; the check then simply finds
''' nothing, and starts working once the repository is public.</para>
''' </remarks>
Public NotInheritable Class SelfUpdater
    ''' <summary>Repository that publishes Forge Launcher releases.</summary>
    Public Const RepositoryUrl As String = "https://github.com/churrufli/forgelauncher"

    ''' <summary>Command line switch that tells a freshly updated launcher which process to wait for.</summary>
    Public Const AfterUpdateSwitch As String = "--after-update"

    Private Const ExeFileName As String = "Forge Launcher.exe"

    Private ReadOnly _server As ForgeServer
    Private ReadOnly _paths As LauncherPaths

    ''' <summary>Creates the updater.</summary>
    ''' <param name="server">HTTP access to GitHub.</param>
    ''' <param name="paths">Launcher paths (for the <c>fldata\update</c> folder and the running files).</param>
    Public Sub New(server As ForgeServer, paths As LauncherPaths)
        _server = server
        _paths = paths
    End Sub

    ''' <summary>Version of the running launcher (major.minor.build).</summary>
    Public Shared ReadOnly Property CurrentVersion As Version
        Get
            Dim full = Assembly.GetExecutingAssembly().GetName().Version
            Return New Version(full.Major, full.Minor, Math.Max(0, full.Build))
        End Get
    End Property

    ''' <summary>Looks for a launcher release newer than the running one.</summary>
    ''' <param name="cancellationToken">Cancels the lookup.</param>
    ''' <returns>
    ''' The newer release; <c>Nothing</c> when the running version is the latest, the repository has no releases,
    ''' or the repository cannot be seen (for example because it is private).
    ''' </returns>
    Public Async Function CheckAsync(cancellationToken As CancellationToken) As Task(Of LauncherRelease)
        Dim tag = Await _server.GetLatestReleaseTagAsync(RepositoryUrl, cancellationToken).ConfigureAwait(False)
        If String.IsNullOrEmpty(tag) Then Return Nothing

        Dim versionText = tag.TrimStart("v"c, "V"c)
        Dim latest As Version = Nothing
        If Not Version.TryParse(versionText, latest) Then Return Nothing
        latest = New Version(latest.Major, latest.Minor, Math.Max(0, latest.Build))
        If latest.CompareTo(CurrentVersion) <= 0 Then Return Nothing

        Return New LauncherRelease With {
            .Version = latest,
            .Tag = tag,
            .PackageUrl = $"{RepositoryUrl}/releases/download/{Uri.EscapeDataString(tag)}/ForgeLauncher-{versionText}-win.zip",
            .PageUrl = $"{RepositoryUrl}/releases/tag/{Uri.EscapeDataString(tag)}"}
    End Function

    ''' <summary>
    ''' Downloads a release and unpacks its exe and DLL into <c>fldata\update\new</c>.
    ''' </summary>
    ''' <param name="release">Release to download.</param>
    ''' <param name="progress">Receives download progress.</param>
    ''' <param name="cancellationToken">Cancels the download.</param>
    ''' <returns>The folder that holds the new files.</returns>
    Public Async Function DownloadAsync(release As LauncherRelease, progress As IProgress(Of TransferProgress), cancellationToken As CancellationToken) As Task(Of String)
        Dim updateFolder = LauncherPaths.Ensure(_paths.UpdateFolder)
        Dim zipPath = Path.Combine(updateFolder, $"ForgeLauncher-{release.Version}-win.zip")
        Await _server.DownloadAsync(release.PackageUrl, zipPath, progress, cancellationToken, checkFreeSpace:=False).ConfigureAwait(False)

        Dim stagingFolder = Path.Combine(updateFolder, "new")
        If Directory.Exists(stagingFolder) Then Directory.Delete(stagingFolder, True)
        Directory.CreateDirectory(stagingFolder)

        ' Only the two files the launcher consists of are taken from the zip, wherever they are inside it.
        Using archive = ZipFile.OpenRead(zipPath)
            For Each entry In archive.Entries
                If String.Equals(entry.Name, ExeFileName, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(entry.Name, LauncherPaths.ZipLibraryFileName, StringComparison.OrdinalIgnoreCase) Then
                    entry.ExtractToFile(Path.Combine(stagingFolder, entry.Name), True)
                End If
            Next
        End Using
        PackageInstaller.TryDelete(zipPath)

        If Not File.Exists(Path.Combine(stagingFolder, ExeFileName)) Then
            Throw New InvalidDataException($"The update package does not contain {ExeFileName}.")
        End If
        Return stagingFolder
    End Function

    ''' <summary>
    ''' Replaces the running launcher with the files in <paramref name="stagingFolder"/>.
    ''' The running exe keeps its current file name, even if the user renamed it.
    ''' </summary>
    ''' <param name="stagingFolder">Folder returned by <see cref="DownloadAsync"/>.</param>
    Public Sub Install(stagingFolder As String)
        Dim stagedDll = Path.Combine(stagingFolder, LauncherPaths.ZipLibraryFileName)
        Dim replacements As New List(Of String()) From {
            New String() {Path.Combine(stagingFolder, ExeFileName), _paths.LauncherExe}}
        If File.Exists(stagedDll) Then replacements.Add(New String() {stagedDll, _paths.ZipLibrary})
        SwapFiles(replacements, LauncherPaths.Ensure(_paths.UpdateFolder))
    End Sub

    ''' <summary>
    ''' Moves each target file into the backup folder as <c>.old</c> and moves its replacement into place.
    ''' If any step fails, every file already replaced is restored and the error is rethrown.
    ''' </summary>
    ''' <param name="replacements">Pairs of {new file, target file}.</param>
    ''' <param name="backupFolder">Folder for the <c>.old</c> files; it must be on the same drive as the targets.</param>
    Public Shared Sub SwapFiles(replacements As IList(Of String()), backupFolder As String)
        Dim done As New List(Of String())
        Try
            For Each pair In replacements
                Dim staged = pair(0)
                Dim target = pair(1)
                Dim backup = Path.Combine(backupFolder, Path.GetFileName(target) & ".old")
                PackageInstaller.TryDelete(backup)
                If File.Exists(target) Then File.Move(target, backup)
                done.Add(New String() {target, backup})
                File.Move(staged, target)
            Next
        Catch
            For Each restored In done
                PackageInstaller.TryDelete(restored(0))
                If File.Exists(restored(1)) Then File.Move(restored(1), restored(0))
            Next
            Throw
        End Try
    End Sub

    ''' <summary>Starts the updated launcher, telling it to wait until this process has exited.</summary>
    Public Sub StartUpdatedLauncher()
        Dim arguments = $"{AfterUpdateSwitch} {Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture)}"
        Process.Start(New ProcessStartInfo(_paths.LauncherExe, arguments) With {
            .WorkingDirectory = _paths.ForgeFolder,
            .UseShellExecute = False})?.Dispose()
    End Sub

    ''' <summary>
    ''' Waits until the previous launcher process has exited. Called at startup when the launcher was started by
    ''' <see cref="StartUpdatedLauncher"/>.
    ''' </summary>
    ''' <param name="processId">Id of the previous launcher process.</param>
    Public Shared Sub WaitForPreviousProcess(processId As Integer)
        Try
            Using previous = Process.GetProcessById(processId)
                previous.WaitForExit(15000)
            End Using
        Catch ex As ArgumentException
        Catch ex As InvalidOperationException
        End Try
    End Sub

    ''' <summary>Deletes the <c>.old</c> files and the staging folder left by an update.</summary>
    ''' <param name="paths">Launcher paths.</param>
    Public Shared Sub CleanUp(paths As LauncherPaths)
        If Not Directory.Exists(paths.UpdateFolder) Then Return
        Try
            Directory.Delete(paths.UpdateFolder, True)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
        End Try
    End Sub
End Class
