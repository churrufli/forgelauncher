Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Devices

''' <summary>Action the user started from the Settings window, run by the main window after it closes.</summary>
Public Enum SettingsAction
    ''' <summary>Just save the settings.</summary>
    None
    ''' <summary>Open "Restore a previous version".</summary>
    Restore
    ''' <summary>Uninstall Forge Launcher.</summary>
    Uninstall
End Enum

''' <summary>
''' Settings window with four sections: Updates, Previous versions, Launch and Maintenance.
''' It edits a copy of the settings; nothing changes until the user clicks Save.
''' </summary>
Public Class SettingsForm
    Private Const PackageSizeMB As Integer = 300

    Private ReadOnly _settings As LauncherSettings
    Private ReadOnly _paths As LauncherPaths
    Private ReadOnly _store As PackageStore
    Private ReadOnly _ready As Boolean

    ''' <summary>Creates the window and loads the values to edit.</summary>
    ''' <param name="settings">A copy of the settings; it receives the edits when the user saves.</param>
    ''' <param name="paths">Launcher paths, for the maintenance buttons.</param>
    ''' <param name="store">Saved Forge versions, to show the disk space they use.</param>
    Public Sub New(settings As LauncherSettings, paths As LauncherPaths, store As PackageStore)
        InitializeComponent()
        _settings = settings
        _paths = paths
        _store = store
        Theme.Apply(Me)

        btnRestore.Glyph = Theme.Glyphs.History
        btnBrowseJava.Glyph = Theme.Glyphs.OpenFile
        If Theme.GlyphFontName Is Nothing Then btnBrowseJava.Text = "..."
        btnOpenData.Glyph = Theme.Glyphs.Folder
        btnOpenLog.Glyph = Theme.Glyphs.Document
        btnUninstall.Glyph = Theme.Glyphs.Delete
        btnSave.Glyph = Theme.Glyphs.CheckMark

        Dim installedMB = InstalledMemoryMB()
        If installedMB > 0 Then
            lblRam.Text = $"Installed RAM: {installedMB:N0} MB"
            numMemory.Maximum = Math.Max(numMemory.Minimum, installedMB)
        End If

        tglCheckForge.Checked = settings.CheckOnStartup
        tglCheckLauncher.Checked = settings.CheckLauncherUpdates
        tglAskToLaunch.Checked = settings.AskToLaunchWhenUpToDate
        segKeep.SelectedIndex = settings.KeepPreviousVersions
        segMode.SelectedIndex = If(settings.Mode = LaunchMode.CustomJava, 1, 0)
        cmbApp.SelectedIndex = If(settings.CustomApp = ForgeApp.Adventure, 1, 0)
        txtJava.Text = settings.JavaPath
        numMemory.Value = Math.Min(numMemory.Maximum, Math.Max(numMemory.Minimum, settings.MaxMemoryMB))
        txtArgs.Text = settings.JvmArguments

        _ready = True
        UpdateLaunchFields()
        UpdateKeepInfo()
    End Sub

    ''' <summary>The edited settings (valid after the window closed with OK).</summary>
    Public ReadOnly Property EditedSettings As LauncherSettings
        Get
            Return _settings
        End Get
    End Property

    ''' <summary>Action to run after the window closes.</summary>
    Public Property RequestedAction As SettingsAction = SettingsAction.None

    ''' <summary>Copies the values of the controls into the edited settings.</summary>
    Private Sub ApplyToSettings()
        _settings.CheckOnStartup = tglCheckForge.Checked
        _settings.CheckLauncherUpdates = tglCheckLauncher.Checked
        _settings.AskToLaunchWhenUpToDate = tglAskToLaunch.Checked
        _settings.KeepPreviousVersions = Math.Max(0, segKeep.SelectedIndex)
        _settings.Mode = If(segMode.SelectedIndex = 1, LaunchMode.CustomJava, LaunchMode.Normal)
        _settings.CustomApp = If(cmbApp.SelectedIndex = 1, ForgeApp.Adventure, ForgeApp.Desktop)
        _settings.JavaPath = If(String.IsNullOrWhiteSpace(txtJava.Text), LauncherSettings.DefaultJavaPath, txtJava.Text.Trim())
        _settings.MaxMemoryMB = CInt(numMemory.Value)
        _settings.JvmArguments = LauncherSettings.SingleLine(txtArgs.Text)
    End Sub

    ''' <summary>Saves and closes.</summary>
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ApplyToSettings()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    ''' <summary>Saves, closes and asks the main window to open "Restore a previous version".</summary>
    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        ApplyToSettings()
        RequestedAction = SettingsAction.Restore
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    ''' <summary>Saves, closes and asks the main window to uninstall the launcher (it asks for confirmation).</summary>
    Private Sub btnUninstall_Click(sender As Object, e As EventArgs) Handles btnUninstall.Click
        ApplyToSettings()
        RequestedAction = SettingsAction.Uninstall
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    ''' <summary>Opens the fldata folder in Explorer.</summary>
    Private Sub btnOpenData_Click(sender As Object, e As EventArgs) Handles btnOpenData.Click
        Try
            LauncherPaths.Ensure(_paths.DataFolder)
            Process.Start(New ProcessStartInfo("explorer.exe", ForgeRunner.Quote(_paths.DataFolder)) With {.UseShellExecute = True})?.Dispose()
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException OrElse TypeOf ex Is Win32Exception
            DarkDialog.ShowMessage(Me, DialogKind.Error, "Couldn't open the folder", ex.Message)
        End Try
    End Sub

    ''' <summary>Opens launcher.log in Notepad.</summary>
    Private Sub btnOpenLog_Click(sender As Object, e As EventArgs) Handles btnOpenLog.Click
        If Not File.Exists(_paths.LogFile) Then
            DarkDialog.ShowMessage(Me, DialogKind.Info, "launcher.log not found", "The log is created the first time the launcher writes to it.")
            Return
        End If
        Try
            Process.Start(New ProcessStartInfo("notepad.exe", ForgeRunner.Quote(_paths.LogFile)) With {.UseShellExecute = True})?.Dispose()
        Catch ex As Win32Exception
            DarkDialog.ShowMessage(Me, DialogKind.Error, "Couldn't open the log", ex.Message)
        End Try
    End Sub

    ''' <summary>Lets the user pick java.exe or javaw.exe.</summary>
    Private Sub btnBrowseJava_Click(sender As Object, e As EventArgs) Handles btnBrowseJava.Click
        Using dialog As New OpenFileDialog With {
            .Title = "Select java.exe or javaw.exe",
            .Filter = "Java (java.exe, javaw.exe)|java.exe;javaw.exe|Programs (*.exe)|*.exe",
            .CheckFileExists = True}
            If dialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then txtJava.Text = dialog.FileName
        End Using
    End Sub

    Private Sub segMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles segMode.SelectedIndexChanged
        UpdateLaunchFields()
    End Sub

    Private Sub segKeep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles segKeep.SelectedIndexChanged
        UpdateKeepInfo()
    End Sub

    ''' <summary>The Java fields only apply to the custom Java mode, so they are disabled in normal mode.</summary>
    Private Sub UpdateLaunchFields()
        If Not _ready Then Return
        Dim custom = segMode.SelectedIndex = 1
        For Each fieldLabel In {lblApp, lblJava, lblMemory, lblArgs}
            fieldLabel.ForeColor = If(custom, Theme.Text, Theme.TextMuted)
        Next
        cmbApp.Enabled = custom
        txtJava.Enabled = custom
        btnBrowseJava.Enabled = custom
        numMemory.Enabled = custom
        txtArgs.Enabled = custom
    End Sub

    ''' <summary>
    ''' Shows how much space saved versions use now, and warns when the new limit is lower than what is saved
    ''' (the extra versions are deleted when the settings are saved).
    ''' </summary>
    Private Sub UpdateKeepInfo()
        If Not _ready Then Return
        Dim packages = _store.GetAll()
        Dim usedMB = packages.Sum(Function(package) package.SizeBytes) / (1024.0 * 1024.0)
        Dim keep = Math.Max(0, segKeep.SelectedIndex)
        Dim allowed = If(keep = 0, 0, keep + 1)
        If packages.Count > allowed Then
            lblKeepInfo.Text = "Extra saved versions are deleted on Save"
            lblKeepInfo.ForeColor = Theme.Warning
        Else
            lblKeepInfo.Text = $"About {PackageSizeMB} MB each · In use: {usedMB:N0} MB"
            lblKeepInfo.ForeColor = Theme.TextSecondary
        End If
    End Sub

    ''' <summary>Installed physical memory in megabytes, or 0 when it cannot be read.</summary>
    Private Shared Function InstalledMemoryMB() As Integer
        Try
            Dim totalMB = New ComputerInfo().TotalPhysicalMemory \ (1024UL * 1024UL)
            Return If(totalMB > CULng(Integer.MaxValue), Integer.MaxValue, CInt(totalMB))
        Catch ex As Exception When TypeOf ex Is InvalidOperationException OrElse TypeOf ex Is Win32Exception
            Return 0
        End Try
    End Function
End Class
