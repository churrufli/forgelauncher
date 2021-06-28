
Imports System.ComponentModel
Imports System.Net
Imports System.Timers
Imports System.Text
Imports System.Text.RegularExpressions

Public Class fl
    Private Sub fl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Public WithEvents downloader As WebClient
    Dim second As Integer

    Sub TimerElapsed(sender As Object, e As ElapsedEventArgs)
        Dim time As DateTime = e.SignalTime
    End Sub

    Private Sub Fl_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        fn.WriteUserLog("Loading data..." & vbCrLf)
        Timer1.Interval = 10
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        second = second + 1
        If second >= 1 Then
            Timer1.Stop()
            If vars.InitAll Then
                InitProgram()
                vars.InitAll = True
            End If
        End If
    End Sub

    Sub InitProgram()
        fn.CheckIfICSharpCodeExist()
        fn.CheckLog()
        SetComboboxes()
        Me.Text = "Forge Launcher v2.1.7"

        fn.SearchFolders(False)
        fn.WriteUserLog("Checking Forge Version..." & vbCrLf)
        Try
            If fn.ReadLogUser("launchmode").ToString = "advanced" Then launchforge.Text = "Launch Forge (Advanced Mode)" Else launchforge.Text = "Launch Forge (Normal Mode)"
        Catch
        End Try

        fn.CheckIfPreviousProfileProperties()
        DisableStuffs()
        fn.CheckforForgeUpdates(False)
    End Sub

    Sub SetComboboxes()
        Try
            typeofupdate.SelectedItem = fn.ReadLogUser("typeofupdate", False)
        Catch
            typeofupdate.SelectedItem = typeofupdate.Items(1)
        End Try
    End Sub

    Sub DisableStuffs()
        'if Forge.exe is not in the same folder, the installation panel is displayed from scratch.
        If fn.CheckIfForgeExists() = False Then
            GroupForgeOptions.Visible = False
            group_install.Visible = True
        Else
            GroupForgeOptions.Visible = True
            group_install.Visible = False
        End If
    End Sub

    Private Sub launchforge_Click(sender As Object, e As EventArgs) Handles launchforge.Click
        fn.Launch()
    End Sub

    Private Sub fl_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        fn.DeleteDownloaded()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        If rbt_normal.Checked = False And rbt_properties.Checked = False Then
            MsgBox("Please select normal or portable install.")
            Exit Sub
        End If
        fn.CheckforForgeUpdates()
    End Sub

    Public Shared Sub downloader_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) _
        Handles downloader.DownloadProgressChanged
        fl.ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub ForgeDiscordChannelToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ForgeDiscordChannelToolStripMenuItem.Click
        Process.Start("https://discord.gg/3v9JCVr")
    End Sub

    Private Sub ForgeForumToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ForgeForumToolStripMenuItem.Click
        Process.Start("https://www.slightlymagic.net/forum/viewforum.php?f=26")
    End Sub

    Private Sub ForgeWikiToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ForgeWikiToolStripMenuItem.Click
        Process.Start("https://git.cardforge.org/core-developers/forge/-/wikis/home")
    End Sub

    Private Sub RestartForgeLauncherToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles RestartForgeLauncherToolStripMenuItem.Click
        Application.Restart()
    End Sub

    Private Sub LogFileToolStripMenuItem_Click(sender As Object, e As EventArgs)
        fn.OpenLogFile()
    End Sub

    Private Sub RestoreForgePreferencesToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles RestoreForgePreferencesToolStripMenuItem.Click
        fn.RestoreForgePreferences()
    End Sub

    Private Sub ReadForgeLogFileToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ReadForgeLogFileToolStripMenuItem.Click
        fn.OpenLogFile()
    End Sub

    Private Sub typeofupdate_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles typeofupdate.SelectedIndexChanged
        Dim result = fn.ReadLogUser("typeofupdate").ToString
        If typeofupdate.Text <> result And typeofupdate.Text <> "" Then
            fn.UpdateLog("typeofupdate", typeofupdate.Text.ToString)
            Application.Restart()
        End If
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim opened = False
        For Each frm As Form In Application.OpenForms
            If frm.Name.Equals("preferences") Then
                frm.Show()
                opened = True
            End If
        Next
        If opened = False Then
            Dim box = New preferences()
            box.Show()
        End If
    End Sub

    Private Sub btnlaunchmode_Click(sender As Object, e As EventArgs) Handles btnlaunchmode.Click
        Dim opened = False
        For Each frm As Form In Application.OpenForms
            If frm.Name.Equals("lm") Then
                frm.Show()
                opened = True
            End If
        Next
        If opened = False Then
            Dim box = New lm()
            box.Show()
        End If
    End Sub

    Public Shared Function IsFormOpen(FormType As Type) As Boolean
        For Each OpenForm In Application.OpenForms
            If OpenForm.GetType() = FormType Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub SettingsToolStripMenuItem1_Click(sender As Object, e As EventArgs) _
        Handles SettingsToolStripMenuItem1.Click
        Dim opened = False
        For Each frm As Form In Application.OpenForms
            If frm.Name.Equals("preferences") Then
                frm.Show()
                opened = True
            End If
        Next
        If opened = False Then
            Dim box = New preferences()
            box.Show()
        End If
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        fn.CheckforForgeUpdates(True)
    End Sub

    Private Sub fl_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        fn.DeleteDownloaded()
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs)
        Dim opened = False
        For Each frm As Form In Application.OpenForms
            If frm.Name.Equals("lm") Then
                frm.Show()
                opened = True
            End If
        Next
        If opened = False Then
            Dim box = New lm()
            box.Show()
        End If
    End Sub

    Private Sub AboutLauncherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutLauncherToolStripMenuItem.Click
        Process.Start("https://github.com/churrufli/forgelauncher")
    End Sub
End Class