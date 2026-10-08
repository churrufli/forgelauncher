Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms

''' <summary>
''' The launcher's main window. It shows the installed Forge version and its update state, checks for updates,
''' installs and restores Forge versions, starts Forge, and keeps a log of everything it does.
''' </summary>
Public Class MainForm
    Private Const ForgeWebsiteUrl As String = "https://card-forge.github.io/forge/"
    Private Const ForgeDiscordUrl As String = "https://discord.gg/3v9JCVr"
    Private Const BytesPerMB As Double = 1024.0 * 1024.0
    Private Shared ReadOnly Nl As String = Environment.NewLine

    ''' <summary>Why an update check runs, which decides which questions the user is asked.</summary>
    Private Enum CheckMode
        ''' <summary>Automatic check when the launcher starts.</summary>
        Startup
        ''' <summary>The user clicked "Check for updates" (or pressed F5): always ask, even to reinstall.</summary>
        Manual
        ''' <summary>The channel was changed: only an available update is offered.</summary>
        Quiet
    End Enum

    ''' <summary>What the header under the Forge version says.</summary>
    Private Enum HeaderState
        NotInstalled
        NotChecked
        Checking
        UpToDate
        UpdateAvailable
        InstalledIsNewer
        Skipped
        CheckFailed
    End Enum

    Private ReadOnly _paths As LauncherPaths
    Private ReadOnly _settings As LauncherSettings
    Private ReadOnly _server As ForgeServer
    Private ReadOnly _store As PackageStore
    Private ReadOnly _log As LauncherLog
    Private ReadOnly _selfUpdater As SelfUpdater
    Private ReadOnly _justUpdated As Boolean
    Private ReadOnly _startupMessages As New List(Of String)
    Private ReadOnly _speedSamples As New Queue(Of KeyValuePair(Of Double, Long))
    Private ReadOnly _speedClock As New Stopwatch()

    Private _installation As ForgeInstallation
    Private _latest As RemoteBuild
    Private _cancellation As CancellationTokenSource
    Private _statusAction As Func(Of Task)
    Private _busy As Boolean
    Private _cancellable As Boolean
    Private _extracting As Boolean
    Private _closingWithoutQuestions As Boolean
    Private _updatingControls As Boolean

    ''' <summary>Creates the main window.</summary>
    ''' <param name="paths">Launcher paths of the Forge folder.</param>
    ''' <param name="justUpdated"><c>True</c> when the launcher was restarted by its own update.</param>
    Public Sub New(paths As LauncherPaths, justUpdated As Boolean)
        InitializeComponent()
        _paths = paths
        _justUpdated = justUpdated
        _log = New LauncherLog(paths.LogFile)
        _settings = LauncherSettings.Load(paths, AddressOf _startupMessages.Add)
        _server = New ForgeServer($"ForgeLauncher/{SelfUpdater.CurrentVersion}")
        _store = New PackageStore(paths.PackagesFolder)
        _selfUpdater = New SelfUpdater(_server, paths)
        _installation = ForgeInstallation.Detect(paths.ForgeFolder)

        Text = $"Forge Launcher {SelfUpdater.CurrentVersion}"
        Icon = Theme.AppIcon(32)
        picForge.Image = Theme.AppIcon(64).ToBitmap()
        txtLog.Font = Theme.CreateMonoFont()
        menuMain.Renderer = New DarkMenuRenderer()
        Theme.StyleToolTip(tipMain)
        Theme.Apply(Me)

        btnSettings.Glyph = Theme.Glyphs.Settings
        btnCheck.Glyph = Theme.Glyphs.Refresh
        btnStatusAction.Glyph = Theme.Glyphs.Cancel
        If Theme.GlyphFontName Is Nothing Then btnSettings.Text = "..."
        AcceptButton = btnLaunch

        tipMain.SetToolTip(segChannel, "Snapshot: daily development builds. Release: stable versions.")
        tipMain.SetToolTip(chkPortable, "Keep Forge's settings, decks and card images inside this folder (creates forge.profile.properties).")
        tipMain.SetToolTip(btnSettings, "Settings (Ctrl+,)")
        tipMain.SetToolTip(btnCheck, "Check for updates (F5)")
        tipMain.SetToolTip(cmbTarget, "File started by 'Launch Forge'.")

        _updatingControls = True
        segChannel.SelectedIndex = If(_settings.Channel = UpdateChannel.Release, 1, 0)
        _updatingControls = False
        RefreshInstallation()
        UpdateHeader(If(_installation.IsInstalled, HeaderState.NotChecked, HeaderState.NotInstalled))
    End Sub

#Region "Startup and shutdown"

    ''' <summary>
    ''' Runs once the window is visible: logs the session start, finishes a launcher update, checks for a new
    ''' launcher version and then for Forge updates, as configured.
    ''' </summary>
    Private Async Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        _log.StartSession($"Forge Launcher {SelfUpdater.CurrentVersion}")
        For Each message In _startupMessages
            Log(message)
        Next
        If _justUpdated Then
            SelfUpdater.CleanUp(_paths)
            Log($"Forge Launcher updated to {SelfUpdater.CurrentVersion}.")
        End If
        PackageInstaller.DeleteLeftovers(_paths.DownloadsFolder)

        Log($"Forge folder: {_paths.ForgeFolder}")
        Log(If(_installation.IsInstalled, $"Installed Forge: {_installation.Description}", "Forge is not installed in this folder."))

        If _settings.CheckLauncherUpdates AndAlso Not _justUpdated Then
            If Await CheckLauncherUpdateAsync(manual:=False) Then Return
        End If

        If _settings.CheckOnStartup Then
            Await CheckForUpdatesAsync(CheckMode.Startup)
        ElseIf _installation.IsInstalled Then
            SetStatus("Ready.")
            FocusLaunchButton()
        Else
            SetStatus("Choose a channel and click 'Install Forge'.")
        End If
    End Sub

    ''' <summary>
    ''' Protects running work: closing is refused while files are being extracted, and needs confirmation while a
    ''' download is running.
    ''' </summary>
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _closingWithoutQuestions OrElse Not _busy Then Return
        If _extracting Then
            e.Cancel = True
            DarkDialog.ShowMessage(Me, DialogKind.Warning, "Please wait", "Forge files are being installed. Closing now could leave Forge incomplete.")
        ElseIf _cancellable Then
            If Ask(DialogKind.Question, "Cancel the download?", "A download is in progress. Cancel it and close Forge Launcher?", "Cancel and close", String.Empty, "Keep downloading") Then
                _cancellation?.Cancel()
            Else
                e.Cancel = True
            End If
        End If
    End Sub

    ''' <summary>Releases the network clients when the window closes.</summary>
    Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _server.Dispose()
    End Sub

    ''' <summary>Keyboard shortcuts: F5 checks for updates and Ctrl+, opens Settings (Enter is the launch button).</summary>
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.F5 Then
            If Not _busy Then btnCheck_Click(btnCheck, EventArgs.Empty)
            Return True
        End If
        If keyData = (Keys.Control Or Keys.Oemcomma) Then
            If Not _busy Then OpenSettings()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

#End Region

#Region "Buttons and menus"

    ''' <summary>"Launch Forge" starts Forge; when Forge is not installed the same button reads "Install Forge".</summary>
    Private Async Sub btnLaunch_Click(sender As Object, e As EventArgs) Handles btnLaunch.Click
        If _busy Then Return
        If _installation.IsInstalled Then
            LaunchAndClose()
        Else
            Await CheckForUpdatesAsync(CheckMode.Manual)
        End If
    End Sub

    ''' <summary>Checks for Forge updates on request.</summary>
    Private Async Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        If _busy Then Return
        Await CheckForUpdatesAsync(CheckMode.Manual)
    End Sub

    ''' <summary>The status bar button cancels a download, or retries a failed update check.</summary>
    Private Async Sub btnStatusAction_Click(sender As Object, e As EventArgs) Handles btnStatusAction.Click
        If _cancellable Then
            _cancellation?.Cancel()
            btnStatusAction.Enabled = False
        ElseIf _statusAction IsNot Nothing AndAlso Not _busy Then
            Dim action = _statusAction
            HideStatusAction()
            Await action()
        End If
    End Sub

    ''' <summary>Changing the channel saves it and checks that channel right away.</summary>
    Private Async Sub segChannel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles segChannel.SelectedIndexChanged
        If _updatingControls Then Return
        _settings.Channel = If(segChannel.SelectedIndex = 1, UpdateChannel.Release, UpdateChannel.Snapshot)
        SaveSettings()
        Log($"Update channel: {ChannelName(_settings.Channel)}")
        Await CheckForUpdatesAsync(CheckMode.Quiet)
    End Sub

    ''' <summary>Remembers the selected launch file.</summary>
    Private Sub cmbTarget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTarget.SelectedIndexChanged
        If _updatingControls OrElse _settings.Mode <> LaunchMode.Normal Then Return
        Dim target = TryCast(cmbTarget.SelectedItem, String)
        If String.IsNullOrEmpty(target) Then Return
        _settings.LaunchTarget = target
        SaveSettings()
        UpdateControlState()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click, mnuSettings.Click
        OpenSettings()
    End Sub

    Private Sub mnuOpenForgeFolder_Click(sender As Object, e As EventArgs) Handles mnuOpenForgeFolder.Click
        OpenFolder(_paths.ForgeFolder)
    End Sub

    Private Sub mnuOpenUserFolder_Click(sender As Object, e As EventArgs) Handles mnuOpenUserFolder.Click
        OpenFolder(_installation.GetUserDataFolder())
    End Sub

    Private Sub mnuOpenForgeLog_Click(sender As Object, e As EventArgs) Handles mnuOpenForgeLog.Click
        OpenTextFile(_installation.GetLogFile(), "forge.log")
    End Sub

    Private Async Sub mnuRestore_Click(sender As Object, e As EventArgs) Handles mnuRestore.Click
        If _busy Then Return
        Await RestorePreviousVersionAsync()
    End Sub

    ''' <summary>Deletes forge.preferences after confirmation; this fixes most "Forge does not start" problems.</summary>
    Private Sub mnuResetPreferences_Click(sender As Object, e As EventArgs) Handles mnuResetPreferences.Click
        Dim preferences = _installation.GetPreferencesFile()
        If Not File.Exists(preferences) Then
            DarkDialog.ShowMessage(Me, DialogKind.Info, "Nothing to reset", $"Forge's preferences file was not found:{Nl}{preferences}")
            Return
        End If
        Dim question = "If Forge doesn't start or freezes on the splash screen, resetting its preferences may help. " &
                       $"Your decks and other data are kept.{Nl}{Nl}Close Forge before continuing."
        If Not Ask(DialogKind.Warning, "Reset Forge preferences?", question, "Reset preferences", Theme.Glyphs.Delete, "Cancel", danger:=True) Then Return
        Try
            File.Delete(preferences)
            Log("Forge preferences reset.")
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            DarkDialog.ShowMessage(Me, DialogKind.Error, "The preferences couldn't be reset", FriendlyErrors.Describe(ex))
        End Try
    End Sub

    Private Async Sub mnuReportProblem_Click(sender As Object, e As EventArgs) Handles mnuReportProblem.Click
        Await ReportProblemAsync()
    End Sub

    Private Sub mnuForgeWebsite_Click(sender As Object, e As EventArgs) Handles mnuForgeWebsite.Click
        OpenUrl(ForgeWebsiteUrl)
    End Sub

    Private Sub mnuForgeReleases_Click(sender As Object, e As EventArgs) Handles mnuForgeReleases.Click
        OpenUrl(ForgeServer.ForgeRepositoryUrl & "/releases")
    End Sub

    Private Sub mnuForgeDiscord_Click(sender As Object, e As EventArgs) Handles mnuForgeDiscord.Click
        OpenUrl(ForgeDiscordUrl)
    End Sub

    Private Sub mnuLauncherWebsite_Click(sender As Object, e As EventArgs) Handles mnuLauncherWebsite.Click
        OpenUrl(SelfUpdater.RepositoryUrl)
    End Sub

    Private Async Sub mnuCheckLauncherUpdates_Click(sender As Object, e As EventArgs) Handles mnuCheckLauncherUpdates.Click
        If _busy Then Return
        Await CheckLauncherUpdateAsync(manual:=True)
    End Sub

    Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
        DarkDialog.ShowMessage(Me, DialogKind.Info, $"Forge Launcher {SelfUpdater.CurrentVersion}",
                               $"Installs, updates and starts Forge on Windows.{Nl}{Nl}Forge: {ForgeWebsiteUrl}{Nl}Forge Launcher: {SelfUpdater.RepositoryUrl}")
    End Sub

#End Region

#Region "Forge updates"

    ''' <summary>
    ''' Looks up the latest Forge build of the selected channel and reacts to the result. Network problems are shown in
    ''' the header and the status bar (with a Retry button); a manual check also shows a dialog.
    ''' </summary>
    Private Async Function CheckForUpdatesAsync(mode As CheckMode) As Task
        Dim latest As RemoteBuild = Nothing
        Dim failure As Exception = Nothing
        SetBusy(True)
        UpdateHeader(HeaderState.Checking)
        SetStatus($"Checking the latest {ChannelName(_settings.Channel)}...")
        Try
            latest = Await _server.GetLatestAsync(_settings.Channel, CancellationToken.None)
        Catch ex As Exception
            failure = ex
        Finally
            SetBusy(False)
        End Try

        If failure IsNot Nothing Then
            Dim message = FriendlyErrors.Describe(failure)
            Log($"Could not check for updates: {message}")
            UpdateHeader(HeaderState.CheckFailed)
            SetStatus(message)
            ShowStatusAction("Retry", Theme.Glyphs.Refresh, Function() CheckForUpdatesAsync(mode))
            If mode = CheckMode.Manual AndAlso AskRetry(failure, "Couldn't check for updates") Then
                HideStatusAction()
                Await CheckForUpdatesAsync(mode)
            Else
                FocusLaunchButton()
            End If
            Return
        End If

        Await HandleLatestBuildAsync(latest, mode)
    End Function

    ''' <summary>Compares the installed build with the latest one and asks the user what to do.</summary>
    Private Async Function HandleLatestBuildAsync(latest As RemoteBuild, mode As CheckMode) As Task
        _latest = latest
        RefreshInstallation()
        Dim channel = ChannelName(latest.Channel)
        Log($"Latest {channel}: {latest.Description}")
        Dim status = UpdateChecker.Compare(_installation, latest)
        Dim isUpdate = status = UpdateStatus.UpdateAvailable OrElse status = UpdateStatus.InstalledUnknown
        Dim skipped = isUpdate AndAlso UpdateChecker.IsSkipped(latest, _settings)

        If status = UpdateStatus.NotInstalled Then
            UpdateHeader(HeaderState.NotInstalled)
            SetStatus($"Latest {channel}: {latest.Version}")
            If mode = CheckMode.Manual AndAlso
               Ask(DialogKind.Download, $"Install Forge {latest.Version}?", $"Forge will be installed in:{Nl}{_paths.ForgeFolder}", "Install Forge", Theme.Glyphs.Download, "Cancel") Then
                Await InstallFromGitHubAsync(latest)
            End If

        ElseIf skipped AndAlso mode <> CheckMode.Manual Then
            ' The user restored an older version and asked not to be offered this build again.
            UpdateHeader(HeaderState.Skipped)
            Log($"Not offering {latest.Version}: it was skipped after restoring a previous version.")
            SetStatus("Using a restored version.")
            If mode = CheckMode.Startup Then OfferToStartForge(channel) Else FocusLaunchButton()

        ElseIf isUpdate Then
            Dim installed = If(status = UpdateStatus.InstalledUnknown, "unknown build", _installation.Description)
            UpdateHeader(HeaderState.UpdateAvailable)
            Log($"Update available. Installed: {installed}")
            SetStatus($"Update available: {latest.Version}")
            Dim message = $"Latest: {latest.Description}{Nl}Installed: {installed}"
            If skipped Then message &= $"{Nl}{Nl}You skipped this build after restoring a previous version."
            Dim answer = DarkDialog.Ask(Me, New DialogOptions With {
                .Title = "Update available",
                .Kind = DialogKind.Download,
                .Heading = $"A new Forge {channel} is available",
                .Message = message,
                .LinkText = "What's new",
                .LinkUrl = WhatsNewUrl(latest),
                .PrimaryText = "Install update",
                .PrimaryGlyph = Theme.Glyphs.Download,
                .SecondaryText = "Not now"})
            If answer.Accepted Then
                Await InstallFromGitHubAsync(latest)
            Else
                FocusLaunchButton()
            End If

        Else
            Dim newer = status = UpdateStatus.InstalledIsNewer
            UpdateHeader(If(newer, HeaderState.InstalledIsNewer, HeaderState.UpToDate))
            Dim summary = If(newer, $"The installed build is newer than the latest {channel}.", $"Your Forge {channel} is up to date.")
            Log(summary)
            SetStatus(summary)
            If mode = CheckMode.Manual Then
                Dim question = If(newer, $"Install the {channel} {latest.Description} anyway?", $"Download and reinstall {latest.Description}? This repairs a damaged installation.")
                If Ask(DialogKind.Question, summary, question, If(newer, $"Install {channel}", "Reinstall"), Theme.Glyphs.Download, "Cancel") Then
                    Await InstallFromGitHubAsync(latest)
                Else
                    FocusLaunchButton()
                End If
            ElseIf mode = CheckMode.Startup Then
                OfferToStartForge(channel)
            Else
                FocusLaunchButton()
            End If
        End If
    End Function

    ''' <summary>
    ''' When Forge is up to date at startup: shows the "Start Forge?" dialog if the user wants it, otherwise just
    ''' focuses "Launch Forge" so Enter starts Forge.
    ''' </summary>
    Private Sub OfferToStartForge(channel As String)
        If Not _settings.AskToLaunchWhenUpToDate Then
            FocusLaunchButton()
            Return
        End If
        If Ask(DialogKind.Success, $"Your Forge {channel} is up to date", "Do you want to start Forge and close the launcher?", "Start Forge", Theme.Glyphs.Play, "Not now") Then
            LaunchAndClose()
        Else
            FocusLaunchButton()
        End If
    End Sub

    ''' <summary>
    ''' Downloads a Forge package (retrying on request when it fails), installs it, keeps it for rollback if the user
    ''' wants previous versions, and offers to start Forge.
    ''' </summary>
    Private Async Function InstallFromGitHubAsync(latest As RemoteBuild) As Task
        If Not EnsureForgeClosed() Then Return
        Dim portable = Not _installation.IsInstalled AndAlso chkPortable.Checked
        Dim archivePath = PackageInstaller.GetDownloadPath(_paths.DownloadsFolder, latest.PackageUrl)

        Do
            Dim failure As Exception = Nothing
            Dim cancelled = False
            _cancellation = New CancellationTokenSource()
            SetBusy(True, cancellable:=True, showProgress:=True)
            ResetSpeed()
            Log($"Downloading {latest.PackageUrl}")
            Try
                Await _server.DownloadAsync(latest.PackageUrl, archivePath, New Progress(Of TransferProgress)(AddressOf ShowDownloadProgress), _cancellation.Token)
            Catch ex As OperationCanceledException When _cancellation.IsCancellationRequested
                cancelled = True
            Catch ex As Exception
                failure = ex
            Finally
                _cancellation.Dispose()
                _cancellation = Nothing
                SetBusy(False)
                TaskbarProgress.Clear(Me)
            End Try

            If cancelled Then
                PackageInstaller.TryDelete(archivePath)
                Log("Download cancelled.")
                SetStatus("Download cancelled.")
                Return
            End If
            If failure Is Nothing Then Exit Do

            PackageInstaller.TryDelete(archivePath)
            Log($"Download failed: {FriendlyErrors.Describe(failure)}")
            SetStatus("Download failed.")
            TaskbarProgress.SetError(Me)
            If Not AskRetry(failure, "The download failed") Then
                TaskbarProgress.Clear(Me)
                Return
            End If
        Loop

        If Not Await ApplyPackageAsync(archivePath) Then
            PackageInstaller.TryDelete(archivePath)
            Return
        End If

        If _settings.KeepPreviousVersions > 0 Then
            _store.Add(archivePath, latest)
        Else
            PackageInstaller.TryDelete(archivePath)
        End If
        RefreshInstallation()
        PruneSavedVersions()

        If portable AndAlso _installation.CreatePortableProfile() Then
            Log($"Created {ForgeInstallation.ProfileFileName} (portable install).")
        End If
        If _settings.SkippedBuild.HasValue Then
            _settings.ClearSkippedBuild()
            SaveSettings()
        End If

        Log($"Forge {latest.Version} installed.")
        UpdateHeader(HeaderState.UpToDate)
        SetStatus($"Forge {latest.Version} installed.")
        If Ask(DialogKind.Success, $"Forge {latest.Version} has been installed", "Do you want to start Forge and close the launcher?", "Start Forge", Theme.Glyphs.Play, "Not now") Then
            LaunchAndClose()
        Else
            FocusLaunchButton()
        End If
    End Function

    ''' <summary>
    ''' Installs a package that is already on disk (a download or a saved version): extracts it, removes files the
    ''' previous package had and this one does not, and saves the new file list.
    ''' </summary>
    ''' <returns><c>True</c> when the installation finished.</returns>
    Private Async Function ApplyPackageAsync(archivePath As String) As Task(Of Boolean)
        Dim failure As Exception = Nothing
        _extracting = True
        SetBusy(True, cancellable:=False, showProgress:=True)
        Log($"Installing {Path.GetFileName(archivePath).Replace(".part", String.Empty)}...")
        Try
            Dim extractProgress As New Progress(Of Integer)(Sub(percent) ShowProgress("Installing files", percent))
            Dim extracted = Await Task.Run(Function() PackageInstaller.Extract(archivePath, _paths.ForgeFolder, extractProgress, CancellationToken.None))
            Log($"{extracted.Count:N0} files installed.")

            Dim previous = PackageInstaller.ReadManifest(_paths.ManifestFile)
            If previous IsNot Nothing Then
                Dim removed = PackageInstaller.RemoveOrphans(_paths, previous, extracted)
                If removed.Count > 0 Then Log($"Removed {removed.Count:N0} files that this version doesn't include.")
            Else
                ' First installation managed by this launcher: without a file list only old jars can be identified safely.
                For Each jar In PackageInstaller.RemoveStaleJars(_paths.ForgeFolder, extracted)
                    Log($"Removed old file: {jar}")
                Next
            End If
            PackageInstaller.WriteManifest(_paths.ManifestFile, extracted)
        Catch ex As Exception
            failure = ex
        Finally
            _extracting = False
            SetBusy(False)
            TaskbarProgress.Clear(Me)
        End Try

        If failure Is Nothing Then Return True

        Dim message = FriendlyErrors.Describe(failure)
        Log($"Installation failed: {message}")
        Log("Some files may not have been updated. Install again to repair Forge.")
        SetStatus("Installation failed.")
        TaskbarProgress.SetError(Me)
        DarkDialog.ShowMessage(Me, DialogKind.Error, "Installation failed", $"{message}{Nl}{Nl}Some files may not have been updated. Install again to repair Forge.")
        TaskbarProgress.Clear(Me)
        RefreshInstallation()
        Return False
    End Function

    ''' <summary>Deletes saved Forge versions beyond the number the user wants to keep.</summary>
    Private Sub PruneSavedVersions()
        For Each deleted In _store.Prune(_settings.KeepPreviousVersions, _installation.BuildDate)
            Log($"Removed saved version {deleted.Version}.")
        Next
    End Sub

    ''' <summary>Web page that lists what changed: the release notes, or the commits since the installed snapshot.</summary>
    Private Function WhatsNewUrl(latest As RemoteBuild) As String
        If latest.Channel = UpdateChannel.Release Then Return latest.ReleasePageUrl
        Dim commits = ForgeServer.ForgeRepositoryUrl & "/commits/master/"
        If _installation.BuildDate.HasValue Then
            commits &= "?since=" & _installation.BuildDate.Value.ToString("yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture)
        End If
        Return commits
    End Function

#End Region

#Region "Rollback"

    ''' <summary>
    ''' Lets the user pick a saved Forge version and installs it again, optionally skipping the version left behind
    ''' so it is not offered at the next start.
    ''' </summary>
    Private Async Function RestorePreviousVersionAsync() As Task
        Dim packages = _store.GetAll()
        If packages.Count = 0 Then
            Dim message = If(_settings.KeepPreviousVersions = 0,
                             "Turn on 'Keep previous Forge versions' in Settings. From the next update on, the launcher keeps the versions you install so you can return to them.",
                             "No version has been saved yet. A version is saved every time Forge is updated.")
            DarkDialog.ShowMessage(Me, DialogKind.Info, "No saved versions", message)
            Return
        End If

        Dim selected As StoredPackage
        Dim skipLeftVersion As Boolean
        Using dialog As New RestoreForm(packages, _installation)
            If dialog.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then Return
            selected = dialog.SelectedPackage
            skipLeftVersion = dialog.SkipLeftVersion
        End Using
        If selected Is Nothing OrElse Not EnsureForgeClosed() Then Return

        Dim leftBuild = _installation.BuildDate
        Dim leftVersion = VersionTextFor(_installation)
        Dim leftDescription = _installation.Description
        If Not Await ApplyPackageAsync(selected.ArchivePath) Then Return

        _store.MarkInstalled(selected)
        If skipLeftVersion AndAlso leftBuild.HasValue Then
            _settings.SkippedBuild = leftBuild
            _settings.SkippedVersion = leftVersion
        Else
            _settings.ClearSkippedBuild()
        End If
        SaveSettings()
        RefreshInstallation()

        Log($"Restored Forge {selected.Description} (was {leftDescription}).")
        UpdateHeader(If(_settings.SkippedBuild.HasValue, HeaderState.Skipped, HeaderState.NotChecked))
        SetStatus($"Forge {selected.Version} restored.")
        If Ask(DialogKind.Success, $"Forge {selected.Version} has been restored", "Do you want to start Forge and close the launcher?", "Start Forge", Theme.Glyphs.Play, "Not now") Then
            LaunchAndClose()
        Else
            FocusLaunchButton()
        End If
    End Function

    ''' <summary>
    ''' The most precise version text for an installation: the saved package or the latest published build with the
    ''' same build time (for example <c>2.0.16-SNAPSHOT-10.06</c>), or else the version from the jar name.
    ''' </summary>
    Private Function VersionTextFor(installation As ForgeInstallation) As String
        If installation.BuildDate.HasValue Then
            Dim saved = _store.GetAll().FirstOrDefault(Function(package) package.BuildDate.Equals(installation.BuildDate))
            If saved IsNot Nothing Then Return saved.Version
            If _latest IsNot Nothing AndAlso _latest.BuildDate.Equals(installation.BuildDate) Then Return _latest.Version
        End If
        Return If(installation.Version, "unknown version")
    End Function

#End Region

#Region "Launcher self-update"

    ''' <summary>
    ''' Checks GitHub for a newer Forge Launcher and, if the user agrees, downloads it, replaces the running files and
    ''' restarts. At startup a missing or private repository is only logged; a manual check reports the result.
    ''' </summary>
    ''' <returns><c>True</c> when the launcher is restarting into the new version.</returns>
    Private Async Function CheckLauncherUpdateAsync(manual As Boolean) As Task(Of Boolean)
        Dim release As LauncherRelease = Nothing
        Dim failure As Exception = Nothing
        If manual Then
            SetBusy(True)
            SetStatus("Checking for Forge Launcher updates...")
        End If
        Try
            release = Await _selfUpdater.CheckAsync(CancellationToken.None)
        Catch ex As Exception
            failure = ex
        Finally
            If manual Then SetBusy(False)
        End Try

        If failure IsNot Nothing Then
            Log($"Could not check for Forge Launcher updates: {FriendlyErrors.Describe(failure)}")
            If manual Then DarkDialog.ShowMessage(Me, DialogKind.Error, "Couldn't check for Forge Launcher updates", FriendlyErrors.Describe(failure))
            Return False
        End If

        If release Is Nothing Then
            If manual Then
                SetStatus("Ready.")
                DarkDialog.ShowMessage(Me, DialogKind.Info, "No update found",
                                       $"No newer Forge Launcher was found on GitHub. You have version {SelfUpdater.CurrentVersion}.")
            End If
            Return False
        End If

        Log($"Forge Launcher {release.Version} is available (you have {SelfUpdater.CurrentVersion}).")
        Dim answer = DarkDialog.Ask(Me, New DialogOptions With {
            .Title = "Forge Launcher update",
            .Kind = DialogKind.Download,
            .Heading = $"Forge Launcher {release.Version} is available",
            .Message = $"You have version {SelfUpdater.CurrentVersion}. The launcher restarts after updating.",
            .LinkText = "What's new",
            .LinkUrl = release.PageUrl,
            .PrimaryText = "Update launcher",
            .PrimaryGlyph = Theme.Glyphs.Download,
            .SecondaryText = "Not now"})
        If Not answer.Accepted Then Return False

        Dim stagingFolder As String = Nothing
        Do
            Dim failureDuringDownload As Exception = Nothing
            Dim cancelled = False
            _cancellation = New CancellationTokenSource()
            SetBusy(True, cancellable:=True, showProgress:=True)
            ResetSpeed()
            Try
                stagingFolder = Await _selfUpdater.DownloadAsync(release, New Progress(Of TransferProgress)(AddressOf ShowDownloadProgress), _cancellation.Token)
            Catch ex As OperationCanceledException When _cancellation.IsCancellationRequested
                cancelled = True
            Catch ex As Exception
                failureDuringDownload = ex
            Finally
                _cancellation.Dispose()
                _cancellation = Nothing
                SetBusy(False)
                TaskbarProgress.Clear(Me)
            End Try

            If cancelled Then
                Log("Launcher update cancelled.")
                SetStatus("Launcher update cancelled.")
                Return False
            End If
            If failureDuringDownload Is Nothing Then Exit Do
            Log($"Launcher update download failed: {FriendlyErrors.Describe(failureDuringDownload)}")
            If Not AskRetry(failureDuringDownload, "The launcher update couldn't be downloaded") Then Return False
        Loop

        Try
            _selfUpdater.Install(stagingFolder)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Log($"Launcher update failed: {FriendlyErrors.Describe(ex)}")
            DarkDialog.ShowMessage(Me, DialogKind.Error, "The update couldn't be installed", $"{FriendlyErrors.Describe(ex)}{Nl}{Nl}The current version keeps working.")
            Return False
        End Try

        Log($"Forge Launcher {release.Version} installed. Restarting...")
        _selfUpdater.StartUpdatedLauncher()
        _closingWithoutQuestions = True
        Close()
        Return True
    End Function

#End Region

#Region "Settings, report and uninstall"

    ''' <summary>
    ''' Opens the Settings window on a copy of the settings. Saving applies the copy, removes saved versions beyond the
    ''' new limit, and runs the action requested from the window (restore or uninstall).
    ''' </summary>
    Private Async Sub OpenSettings()
        If _busy Then Return
        Dim edited As LauncherSettings
        Dim action As SettingsAction
        Using dialog As New SettingsForm(_settings.Clone(), _paths, _store)
            If dialog.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then Return
            edited = dialog.EditedSettings
            action = dialog.RequestedAction
        End Using

        _settings.CopyFrom(edited)
        SaveSettings()
        PruneSavedVersions()
        PopulateLaunchTargets()
        UpdateControlState()
        Log("Settings saved.")

        Select Case action
            Case SettingsAction.Restore
                Await RestorePreviousVersionAsync()
            Case SettingsAction.Uninstall
                ConfirmUninstall()
        End Select
    End Sub

    ''' <summary>
    ''' Copies a problem report (system versions, settings and the end of launcher.log) to the clipboard and offers to
    ''' open the Forge Discord to paste it.
    ''' </summary>
    Private Async Function ReportProblemAsync() As Task
        Cursor = Cursors.WaitCursor
        SetStatus("Preparing the problem report...")
        Dim installation = _installation
        Dim report = Await Task.Run(Function() SystemInfo.BuildReport(installation, _settings, _log))
        Cursor = Cursors.Default
        SetStatus("Ready.")
        Try
            Clipboard.SetText(report)
        Catch ex As ExternalException
            DarkDialog.ShowMessage(Me, DialogKind.Error, "The report couldn't be copied", "Another program is using the clipboard. Try again in a moment.")
            Return
        End Try

        Log("Problem report copied to the clipboard.")
        Dim answer = DarkDialog.Ask(Me, New DialogOptions With {
            .Title = "Report a problem",
            .Kind = DialogKind.Success,
            .Heading = "Problem report copied",
            .Message = "The report (versions and the end of launcher.log) is in your clipboard. Paste it into a message on the Forge Discord or a GitHub issue, with a short description of the problem.",
            .PrimaryText = "Open Forge Discord",
            .PrimaryGlyph = Theme.Glyphs.Link,
            .SecondaryText = "Close"})
        If answer.Accepted Then OpenUrl(ForgeDiscordUrl)
    End Function

    ''' <summary>Asks for confirmation and removes Forge Launcher (never Forge or the user's data).</summary>
    Private Sub ConfirmUninstall()
        Dim savedBytes = _store.TotalSize()
        Dim savedText = If(savedBytes > 0, $", plus {savedBytes / BytesPerMB:N0} MB of saved Forge versions", String.Empty)
        Dim message = $"This removes Forge Launcher.exe, ICSharpCode.SharpZipLib.dll and the fldata folder (launcher settings and log{savedText}).{Nl}{Nl}" &
                      "Forge, your decks, preferences and card images are not touched."
        If Not Ask(DialogKind.Warning, "Uninstall Forge Launcher?", message, "Uninstall", Theme.Glyphs.Delete, "Cancel", danger:=True) Then Return

        Log("Uninstalling Forge Launcher.")
        Uninstaller.Start(_paths)
        _closingWithoutQuestions = True
        Close()
    End Sub

#End Region

#Region "Launching Forge"

    ''' <summary>Starts Forge and closes the launcher when that worked.</summary>
    Private Sub LaunchAndClose()
        If LaunchForge() Then
            _closingWithoutQuestions = True
            Close()
        End If
    End Sub

    ''' <summary>Starts Forge as configured; explains the problem when it cannot be started.</summary>
    Private Function LaunchForge() As Boolean
        Try
            Dim started = ForgeRunner.Launch(_paths.ForgeFolder, _settings, TryCast(cmbTarget.SelectedItem, String))
            Log($"Started {started}")
            Return True
        Catch ex As Exception When TypeOf ex Is FileNotFoundException OrElse TypeOf ex Is Win32Exception OrElse TypeOf ex Is InvalidOperationException
            Dim message = ex.Message
            If TypeOf ex Is Win32Exception AndAlso _settings.Mode = LaunchMode.CustomJava Then
                message = $"Java couldn't be started ({ex.Message}). Check the Java executable in Settings > Launch."
            End If
            Log($"Launch failed: {message}")
            DarkDialog.ShowMessage(Me, DialogKind.Error, "Forge couldn't be started", message)
            Return False
        End Try
    End Function

#End Region

#Region "Window state"

    ''' <summary>Detects the installation again and refreshes the controls that depend on it.</summary>
    Private Sub RefreshInstallation()
        _installation = ForgeInstallation.Detect(_paths.ForgeFolder)
        PopulateLaunchTargets()
        UpdateControlState()
    End Sub

    ''' <summary>Fills the launch list: the Forge folder's launch files, or a summary of the custom Java command.</summary>
    Private Sub PopulateLaunchTargets()
        _updatingControls = True
        cmbTarget.BeginUpdate()
        cmbTarget.Items.Clear()
        If _settings.Mode = LaunchMode.CustomJava Then
            cmbTarget.Items.Add($"Custom Java: {ForgeRunner.AppName(_settings.CustomApp)}, {_settings.MaxMemoryMB:N0} MB")
            cmbTarget.SelectedIndex = 0
        Else
            For Each targetName In _installation.GetLaunchTargets(Path.GetFileName(_paths.LauncherExe))
                cmbTarget.Items.Add(targetName)
            Next
            Dim index = cmbTarget.FindStringExact(_settings.LaunchTarget)
            If index < 0 AndAlso cmbTarget.Items.Count > 0 Then index = 0
            cmbTarget.SelectedIndex = index
        End If
        cmbTarget.EndUpdate()
        _updatingControls = False
    End Sub

    ''' <summary>
    ''' Updates the header: Forge version in large type, then the build time and the update state next to a colored
    ''' dot (green: up to date, amber: update available, gray: unknown).
    ''' </summary>
    Private Sub UpdateHeader(state As HeaderState)
        Dim installed = _installation.IsInstalled
        lblForgeVersion.Text = If(installed, $"Forge {If(_installation.Version, "(unknown version)")}", "Forge is not installed")

        Dim stateText As String
        Dim dotColor = Theme.TextMuted
        Select Case state
            Case HeaderState.NotInstalled
                stateText = "Choose a channel and click Install Forge"
            Case HeaderState.Checking
                stateText = "Checking for updates..."
            Case HeaderState.UpToDate
                stateText = "Up to date"
                dotColor = Theme.AccentBright
            Case HeaderState.InstalledIsNewer
                stateText = $"Newer than the latest {ChannelName(_settings.Channel)}"
                dotColor = Theme.AccentBright
            Case HeaderState.UpdateAvailable
                stateText = $"Update available: {If(_latest IsNot Nothing, _latest.Version, "new version")}"
                dotColor = Theme.Warning
            Case HeaderState.Skipped
                stateText = $"Restored version · {If(_settings.SkippedVersion, "newer build")} skipped"
            Case HeaderState.CheckFailed
                stateText = "Couldn't check for updates"
            Case Else
                stateText = "Not checked"
        End Select

        If installed AndAlso _installation.BuildDate.HasValue Then
            stateText = $"Built {ForgeInstallation.FormatBuildDate(_installation.BuildDate.Value)} · {stateText}"
        End If
        lblForgeState.Text = stateText
        dotState.ForeColor = dotColor
    End Sub

    ''' <summary>Enables, shows and labels the controls according to the installation and whether work is running.</summary>
    Private Sub UpdateControlState()
        Dim idle = Not _busy
        Dim installed = _installation IsNot Nothing AndAlso _installation.IsInstalled

        segChannel.Enabled = idle
        chkPortable.Visible = Not installed
        chkPortable.Enabled = idle
        cmbTarget.Visible = installed
        cmbTarget.Enabled = idle AndAlso _settings.Mode = LaunchMode.Normal
        btnSettings.Enabled = idle
        btnCheck.Visible = installed
        btnCheck.Enabled = idle

        btnLaunch.Text = If(installed, "Launch Forge", "Install Forge")
        btnLaunch.Glyph = If(installed, Theme.Glyphs.Play, Theme.Glyphs.Download)
        btnLaunch.Enabled = idle AndAlso (Not installed OrElse _settings.Mode = LaunchMode.CustomJava OrElse cmbTarget.SelectedIndex >= 0)
        tipMain.SetToolTip(btnLaunch, If(installed, "Start Forge and close the launcher (Enter)", "Download and install Forge in this folder (Enter)"))

        mnuForge.Enabled = idle
        mnuSettings.Enabled = idle
        mnuCheckLauncherUpdates.Enabled = idle
        mnuRestore.Enabled = idle AndAlso installed
    End Sub

    ''' <summary>
    ''' Marks the window as busy or idle. While busy, actions are disabled; a cancellable task shows "Cancel" in the
    ''' status bar, and long tasks show the progress bar.
    ''' </summary>
    Private Sub SetBusy(busy As Boolean, Optional cancellable As Boolean = False, Optional showProgress As Boolean = False)
        _busy = busy
        _cancellable = busy AndAlso cancellable
        barProgress.Visible = busy AndAlso showProgress
        If Not barProgress.Visible Then barProgress.Value = 0
        If _cancellable Then
            ShowStatusAction("Cancel", Theme.Glyphs.Cancel, Nothing)
        ElseIf busy OrElse _statusAction Is Nothing Then
            HideStatusAction()
        End If
        UpdateControlState()
    End Sub

    ''' <summary>Shows the status bar button with a text, an icon and the action it runs.</summary>
    Private Sub ShowStatusAction(caption As String, glyph As String, action As Func(Of Task))
        _statusAction = action
        btnStatusAction.Text = caption
        btnStatusAction.Glyph = glyph
        btnStatusAction.Enabled = True
        btnStatusAction.Visible = True
    End Sub

    ''' <summary>Hides the status bar button.</summary>
    Private Sub HideStatusAction()
        _statusAction = Nothing
        btnStatusAction.Visible = False
    End Sub

    ''' <summary>Gives the focus to "Launch Forge", so pressing Enter starts Forge.</summary>
    Private Sub FocusLaunchButton()
        If btnLaunch.Enabled AndAlso btnLaunch.Visible Then ActiveControl = btnLaunch
    End Sub

    ''' <summary>Starts measuring download speed for a new download.</summary>
    Private Sub ResetSpeed()
        _speedSamples.Clear()
        _speedClock.Restart()
    End Sub

    ''' <summary>
    ''' Shows download progress with speed and time left. The speed is averaged over the last five seconds, so the
    ''' estimate stays steady.
    ''' </summary>
    Private Sub ShowDownloadProgress(transfer As TransferProgress)
        Dim now = _speedClock.Elapsed.TotalSeconds
        _speedSamples.Enqueue(New KeyValuePair(Of Double, Long)(now, transfer.Received))
        Do While _speedSamples.Count > 1 AndAlso now - _speedSamples.Peek().Key > 5
            _speedSamples.Dequeue()
        Loop
        Dim oldest = _speedSamples.Peek()
        Dim speed = If(now - oldest.Key > 0.5, (transfer.Received - oldest.Value) / (now - oldest.Key), 0.0)

        Dim text = $"Downloading {transfer.Received / BytesPerMB:N0}"
        If transfer.Total.HasValue Then text &= $" of {transfer.Total.Value / BytesPerMB:N0} MB ({transfer.Percent}%)" Else text &= " MB"
        If speed > 0 Then
            text &= $" · {speed / BytesPerMB:N1} MB/s"
            If transfer.Total.HasValue Then text &= " · " & FormatTimeLeft((transfer.Total.Value - transfer.Received) / speed)
        End If
        SetStatus(text)
        If transfer.Percent >= 0 Then SetProgress(transfer.Percent)
    End Sub

    ''' <summary>Shows a percentage in the status bar, the progress bar and the taskbar button.</summary>
    Private Sub ShowProgress(text As String, percent As Integer)
        SetStatus($"{text} ({percent}%)")
        SetProgress(percent)
    End Sub

    ''' <summary>Moves the progress bar and the taskbar button progress.</summary>
    Private Sub SetProgress(percent As Integer)
        barProgress.Value = percent
        TaskbarProgress.SetValue(Me, percent)
    End Sub

    ''' <summary>Formats a time estimate: seconds below one minute, minutes otherwise.</summary>
    Private Shared Function FormatTimeLeft(seconds As Double) As String
        If seconds < 60 Then Return $"{Math.Max(1, CInt(Math.Ceiling(seconds)))} s left"
        Return $"{CInt(Math.Ceiling(seconds / 60))} min left"
    End Function

#End Region

#Region "Helpers"

    ''' <summary>Writes a line to the log window and to fldata\launcher.log.</summary>
    Private Sub Log(message As String)
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Nl}")
        _log.Write(message)
    End Sub

    ''' <summary>Sets the status bar text.</summary>
    Private Sub SetStatus(text As String)
        lblStatus.Text = text
    End Sub

    ''' <summary>Saves the settings, logging a hint when the folder is not writable.</summary>
    Private Sub SaveSettings()
        If Not _settings.Save() Then Log("Could not save fldata\settings.ini. Check that the folder is writable.")
    End Sub

    ''' <summary>Refuses to continue while Forge is running, because Windows keeps its files locked.</summary>
    Private Function EnsureForgeClosed() As Boolean
        If Not _installation.IsInUse() Then Return True
        DarkDialog.ShowMessage(Me, DialogKind.Warning, "Forge is running", "Close Forge (and Forge Adventure) and try again.")
        Return False
    End Function

    ''' <summary>Shows a question with two buttons and returns <c>True</c> when the main button was chosen.</summary>
    Private Function Ask(kind As DialogKind, heading As String, message As String, primaryText As String, primaryGlyph As String,
                         secondaryText As String, Optional danger As Boolean = False) As Boolean
        Return DarkDialog.Ask(Me, New DialogOptions With {
            .Kind = kind,
            .Heading = heading,
            .Message = message,
            .PrimaryText = primaryText,
            .PrimaryGlyph = primaryGlyph,
            .PrimaryIsDanger = danger,
            .SecondaryText = secondaryText}).Accepted
    End Function

    ''' <summary>
    ''' Explains an error. Problems that can go away by themselves offer "Retry"; the result tells whether the user
    ''' chose it.
    ''' </summary>
    Private Function AskRetry(failure As Exception, heading As String) As Boolean
        Dim message = FriendlyErrors.Describe(failure)
        If Not FriendlyErrors.IsRetryable(failure) Then
            DarkDialog.ShowMessage(Me, DialogKind.Error, heading, message)
            Return False
        End If
        Return Ask(DialogKind.Error, heading, message, "Retry", Theme.Glyphs.Refresh, "Close")
    End Function

    ''' <summary>Display name of a channel.</summary>
    Private Shared Function ChannelName(channel As UpdateChannel) As String
        Return If(channel = UpdateChannel.Release, "release", "snapshot")
    End Function

    ''' <summary>Opens a folder in Explorer, or explains that it does not exist yet.</summary>
    Private Sub OpenFolder(folder As String)
        If Directory.Exists(folder) Then
            StartProcess("explorer.exe", ForgeRunner.Quote(folder))
        Else
            DarkDialog.ShowMessage(Me, DialogKind.Info, "Folder not found", $"The folder doesn't exist yet:{Nl}{folder}")
        End If
    End Sub

    ''' <summary>Opens a text file in Notepad, or explains that it does not exist yet.</summary>
    Private Sub OpenTextFile(filePath As String, displayName As String)
        If File.Exists(filePath) Then
            StartProcess("notepad.exe", ForgeRunner.Quote(filePath))
        Else
            DarkDialog.ShowMessage(Me, DialogKind.Info, $"{displayName} not found", $"The file doesn't exist yet:{Nl}{filePath}")
        End If
    End Sub

    ''' <summary>Opens a web page in the default browser.</summary>
    Private Sub OpenUrl(url As String)
        StartProcess(url)
    End Sub

    ''' <summary>Starts a program or document through the Windows shell, explaining failures.</summary>
    Private Sub StartProcess(fileName As String, Optional arguments As String = "")
        Try
            Process.Start(New ProcessStartInfo(fileName, arguments) With {.UseShellExecute = True})?.Dispose()
        Catch ex As Exception When TypeOf ex Is Win32Exception OrElse TypeOf ex Is InvalidOperationException
            DarkDialog.ShowMessage(Me, DialogKind.Error, "Couldn't open it", ex.Message)
        End Try
    End Sub

#End Region
End Class
