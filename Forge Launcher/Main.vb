
Imports System.ComponentModel
Imports System.Net
Imports System.Timers
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class Main

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
        Me.Text = GetTitle()

        fn.SearchFolders(False)
        Try
            If fn.ReadLogUser("launchmode").ToString = "advanced" Then launchforge.Text = "Launch Forge (Advanced Mode)" Else launchforge.Text = "Launch Forge (Normal Mode)"
        Catch
        End Try

        fn.CheckIfPreviousProfileProperties()
        DisableStuffs()
        fn.LoadListofExes()
        fn.WriteUserLog("Checking Forge Version..." & vbCrLf)
        Try
            fn.CheckforForgeUpdates(False)
        Catch
        End Try

    End Sub
    Public Shared Function GetTitle() As String
        Dim ass As Assembly = Assembly.GetExecutingAssembly()
        Dim name = ass.GetName()
        Dim Version As String = "v" & name.Version.Major & "." & name.Version.Minor

        If name.Version.Build > 0 Then
            Version += "." & name.Version.Build
        End If

        Dim Title As String = (CType(ass.GetCustomAttributes(GetType(AssemblyTitleAttribute), False)(0), AssemblyTitleAttribute)).Title
        Return Title & " " + Version
    End Function

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
        Process.Start("https://github.com/Card-Forge/forge/wiki")
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
            Dim box = New settings()
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
            Dim box = New LaunchMode()
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
            Dim box = New settings()
            box.Show()
        End If
    End Sub
    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        fn.CheckforForgeUpdates(True)
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
            Dim box = New LaunchMode()
            box.Show()
        End If
    End Sub

    Private Sub AboutLauncherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutLauncherToolStripMenuItem.Click
    End Sub

    Private Sub listofexes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listofexes.SelectedIndexChanged
        Dim result = fn.ReadLogUser("exeselected").ToString
        If listofexes.Text <> result And listofexes.Text <> "" Then
            fn.UpdateLog("exeselected", listofexes.Text.ToString)
        End If
    End Sub

    Private Sub GithubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GithubToolStripMenuItem.Click
        Process.Start("https://github.com/churrufli/forgelauncher")

    End Sub

    Private Sub CheckForUpdatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        Dim box = New about()
        box.Show()
    End Sub

    Private Sub ForgeGithubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForgeGithubToolStripMenuItem.Click
        Process.Start("https://card-forge.github.io/forge/")
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If fn.ReadLogUser("showabout", False) = "yes" Then
            Dim box = New about()
            box.Show()
        End If
    End Sub
End Class