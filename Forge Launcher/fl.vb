
Imports System.ComponentModel
Imports System.Net
Imports System.Timers

Public Class fl
    Shared todas

    Public WithEvents downloader As WebClient
    Dim second As Integer

    Sub TimerElapsed(sender As Object, e As ElapsedEventArgs)
        Dim time As DateTime = e.SignalTime
        Console.WriteLine("TIME: " + time)
    End Sub

    Private Sub Fl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TabControl1.TabPages.Remove(TabControl1.TabPages(1))
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

        fn.MoveFilestoFLDataFolder()
        fn.CheckIfICSharpCodeExist()
        fn.WriteUserLog("Checking Forge Version..." & vbCrLf)
        fn.RewriteLog()
        fn.CheckLog()

        If fn.ReadLogUser("launchmode").ToString = "advanced" Then
            launchforge.Text = "Launch Forge (Advanced Mode)"
        Else
            launchforge.Text = "Launch Forge (Normal Mode)"
        End If

        fn.MaintainVersionInAdvancedLaunchMode()
        SetComboboxes()
        fn.CheckIfPreviousProfileProperties()
        DisableStuffs()
        fn.HitToLauncherUpdates()
        fn.CheckLauncherUpdates()

        fn.CheckForgeVersion(False, True)
        Dim result = fn.ReadLogUser("enableprompt", False, False)
        If result = "yes" Then
            fn.AlertAboutVersion(True)
        End If

        fn.ShowingWhatsNew()
        fn.SearchFolders(True)
    End Sub

    Sub SetComboboxes()
        Dim comboSource As New Dictionary(Of String, String)()
        comboSource.Add("8", "Top 8")
        comboSource.Add("12", "Top 12")
        comboSource.Add("16", "Top 16")
        comboSource.Add("25", "Top 25")
        comboSource.Add("50", "Top 50")

        Dim comboSource2 As New Dictionary(Of String, String)()
        comboSource2.Add("8", "Last 8")
        comboSource2.Add("12", "Last 12")
        comboSource2.Add("16", "Last 16")
        comboSource2.Add("25", "Last 25")
        comboSource2.Add("50", "Last 50")

        '// a little trick to get more decks
        If File.Exists("gimmiemoredecks.txt") Then
            comboSource.Add("99", "Top 99")
            comboSource.Add("120", "Top 120")
            comboSource.Add("150", "Top 150")
            comboSource.Add("200", "Top 200")
            comboSource2.Add("99", "Last 99")
            comboSource2.Add("120", "Last 120")
            comboSource2.Add("200", "Last 200")
        End If

        howmuch2.DataSource = New BindingSource(comboSource2, Nothing)
        howmuch2.DisplayMember = "Value"
        howmuch2.ValueMember = "Key"

        howmuch.DataSource = New BindingSource(comboSource, Nothing)
        howmuch.DisplayMember = "Value"
        howmuch.ValueMember = "Key"

        metagame.SelectedIndex = 0
        metag2.SelectedIndex = 0

        Try
            typeofupdate.SelectedItem = fn.ReadLogUser("typeofupdate", False, False)
        Catch
            typeofupdate.SelectedItem = typeofupdate.Items(1)
        End Try

        ComboBox2.SelectedItem = ComboBox2.Items(0)
        maxtournm.SelectedItem = maxtournm.Items(0)
        fromweb.SelectedItem = fromweb.Items(0)
        maxtournamentsdecks.SelectedItem = maxtournamentsdecks.Items(0)

        Dim a As String = fn.ReadLogUser("launchforgeafterupdate", False, False)
        If a = "yes" Then
            chklaunchforgeafterupdate.Checked = True
        Else
            chklaunchforgeafterupdate.Checked = False
        End If

        a = fn.ReadLogUser("enableprompt", False, False)
        If a = "yes" Then
            chkenableprompt.Checked = True
        Else
            chkenableprompt.Checked = False
        End If
    End Sub

    Sub DisableStuffs()
        If fn.CheckIfForgeExists() = False Then
            GroupForgeOptions.Visible = False
            group_install.Visible = True
            GroupExtras.Enabled = False
        Else
            GroupForgeOptions.Visible = True
            group_install.Visible = False
            GroupExtras.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            vars.UserDir = FolderBrowserDialog1.SelectedPath
            My.Settings.userdir = vars.UserDir
            My.Settings.Save()
            fn.SearchFolders(False)
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
        fn.AlertAboutVersion()
    End Sub

    Public Shared Sub downloader_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) _
        Handles downloader.DownloadProgressChanged
        fl.ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub extract_Click(sender As Object, e As EventArgs) Handles extract1.Click
        fn.CheckUnsupportedCards()
        If InStr(metagame.SelectedItem.ToString, "-") = 0 Then
            ext.ExtractTopMtggoldfish(metagame.SelectedItem.ToString, howmuch.SelectedValue, chktopnumber.Checked, Nothing)
            Exit Sub
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        ext.ExtractTournamentMtgtop8()
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
        Process.Start("https://www.slightlymagic.net/wiki/Forge")
    End Sub

    Private Sub RestartForgeLauncherToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles RestartForgeLauncherToolStripMenuItem.Click
        Application.Restart()
    End Sub

    Private Sub GauntletContestFolderToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Try
            Process.Start(fn.ReadLogUser("gauntlet_dir", False))
        Catch
            fn.PrintError(Err.Description)
        End Try
    End Sub

    Private Sub extract2_Click(sender As Object, e As EventArgs)
        ext.ExtractTournamentMtgtop8()
    End Sub

    Private Sub WhatsNewToolStripMenuItem_Click_2(sender As Object, e As EventArgs) _
        Handles WhatsNewToolStripMenuItem.Click

        Dim opened = False
        For Each frm As Form In Application.OpenForms
            If frm.Name.Equals("whatsnew") Then
                frm.Show()
                opened = True
            End If
        Next

        If Not opened Then
            Dim box = New whatsnew()
            box.Show()
        End If
    End Sub

    Private Sub ForzeUpdateForgeLauncherToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ForzeUpdateForgeLauncherToolStripMenuItem.Click
        fn.UpdateLog("launcher_version", "")
        fn.UpdateLog("lastupdate", "")
        Application.Restart()
    End Sub

    Private Sub CheckForLauncherUpdatesToolStripMenuItem_Click(sender As Object, e As EventArgs)
        fn.CheckLauncherUpdates()
    End Sub

    Private Sub CancelButton1_Click(sender As Object, e As EventArgs)
        vars.continueLooping = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles extract3.Click
        fn.CheckUnsupportedCards()
        If InStr(metag2.SelectedItem.ToString, "-") = 0 Then
            ext.ExtractTopMtggoldfish(metag2.SelectedItem.ToString, howmuch2.SelectedValue, False, Nothing)
        End If
    End Sub

    Function RemoveNumbers(t) As String
        t = Replace(t, "0", "")
        t = Replace(t, "1", "")
        t = Replace(t, "2", "")
        t = Replace(t, "3", "")
        t = Replace(t, "4", "")
        t = Replace(t, "5", "")
        t = Replace(t, "6", "")
        t = Replace(t, "7", "")
        t = Replace(t, "8", "")
        t = Replace(t, "9", "")
        Return t
    End Function

    Private Sub CheckForUpdatesToolStripMenuItem_Click(sender As Object, e As EventArgs)
        fn.CheckLauncherUpdates()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Try
            Dim MyFolder = fn.ReadLogUser("decks_dir", False)
            Process.Start(MyFolder)
        Catch
            fn.PrintError(Err.Description)
        End Try
    End Sub

    Private Sub DecksFolderToolStripMenuItem_Click_1(sender As Object, e As EventArgs)
        Try
            Dim MyFolder = fn.ReadLogUser("decks_dir", False)
            Process.Start(MyFolder)
        Catch
            fn.PrintError(Err.Description)
        End Try
    End Sub

    Private Sub LogFileToolStripMenuItem_Click(sender As Object, e As EventArgs)
        fn.OpenLogFile()
    End Sub

    Private Sub CopyMyExternalIPToClipboardToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        fn.CopyIPtoClipboard()
    End Sub

    Private Sub RestoreForgePreferencesToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles RestoreForgePreferencesToolStripMenuItem.Click
        fn.RestoreForgePreferences()
    End Sub

    Private Sub ReadForgeLogFileToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles ReadForgeLogFileToolStripMenuItem.Click
        fn.OpenLogFile()
    End Sub

    Private Sub OpenDecksFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles OpenDecksFolderToolStripMenuItem.Click
        Try
            Dim MyFolder = fn.ReadLogUser("decks_dir", False)
            Process.Start(MyFolder)
        Catch
            fn.PrintError(Err.Description)
        End Try
    End Sub

    Private Sub typeofupdate_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles typeofupdate.SelectedIndexChanged
        If typeofupdate.SelectedItem.ToString <> fn.ReadLogUser("typeofupdate").ToString Then
            fn.UpdateLog("typeofupdate", typeofupdate.SelectedItem.ToString)
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

    Private Sub extract4_Click(sender As Object, e As EventArgs) Handles extract4.Click
        fn.CheckUnsupportedCards()
        Select Case fromweb.SelectedItem.ToString
            Case "mtgtop8"
                ext.ExtractFromMtgtop8(Replace(maxtournamentsdecks.SelectedItem.ToString, "Limit ", ""))
            Case "mtggoldfish"
                Dim w As String =
                        fn.ReadWeb(
                            "https://www.mtggoldfish.com/tournaments/" & ComboBox2.SelectedItem.ToString & "#paper")
                Dim links = ext.extlinks(w, "/tournament/")
                Dim laweb = ""
                Dim t = ""
                Dim urls() As String = Split(links, vbCrLf)
                For i = 0 To urls.Length - 1
                    If urls(i) <> "" Then

                        Dim tournament_url = ("https://www.mtggoldfish.com" & urls(i))
                        extracttournamentmtggoldfish(tournament_url,
                                                     Replace(maxtournamentsdecks.SelectedItem.ToString, "Limit ", ""))
                        Dim max = 5
                        Select Case maxtournm.SelectedItem.ToString
                            Case "Last One"
                                max = 1
                            Case "Last 2"
                                max = 2
                            Case "Last 5"
                                max = 5
                            Case "Last 10"
                                max = 10
                            Case Else
                                max = 100
                        End Select
                        If (i + 1) >= max Then
                            Exit For
                        End If

                    End If

                Next i

        End Select
    End Sub

    Public Sub extracttournamentmtggoldfish(Optional ByVal tournament_url As String = "", Optional ByVal maxdecks As Integer = 100)

        Dim MyDir As String = fn.GetForgeDecksDir() & "\constructed\" & fn.ReadLogUser("tournamentsdecks_dir", False) & "\"
        Dim tx1 As String
        tx1 = fn.ReadWeb(tournament_url)
        Dim tourname As String = ""
        Dim res As String = tx1
        tourname = fn.FindIt(tx1, "<title>", "</title>")
        tourname = Replace(tourname, " (" & ComboBox2.SelectedItem.ToString & ") Decks", "")
        If tourname Is Nothing Then tourname = ""
        tourname = tourname.Trim()
        tourname = Replace(tourname, ":", "")
        Dim MyFolder As String = MyDir & tourname & "\"
        If Directory.Exists(MyFolder) Then
            If _
                MsgBox(
                    "Folder " & tourname & " exists, do you want to download decks again? " & vbCrLf & vbCrLf &
                    " (Decks inside the folder will be deleted)", MsgBoxStyle.YesNoCancel, "Warning!") = MsgBoxResult.No _
                Then
                fn.WriteUserLog(tourname & " folder exists. Operation cancelled." & vbCrLf)
                Exit Sub
            End If
        End If
        Try
            Directory.Delete(MyFolder, True)
        Catch

        End Try
        fn.CheckFolder(MyFolder)
        txlog.Text = ""
        fn.WriteUserLog("Creating " & MyFolder & vbCrLf)
        tx1 = ext.extlinks(tx1, "/deck/", "/deck/custom/standard")
        Dim lasurls() As String = Split(tx1, vbCrLf)
        Dim mx = Replace(maxtournamentsdecks.SelectedItem.ToString, "Limit ", "")
        Dim contadorposicion = 0
        For a = 0 To lasurls.Length - 1
            If _
                lasurls(a).ToString <> "" And lasurls(a).ToString <> "/deck/custom/" & LCase(ComboBox2.SelectedItem.ToString) Then
                If a > mx Then Exit For
                Dim DeckPage = ""
                Dim UrlDeck = ""
                DeckPage = fn.ReadWeb(vars.mtggf & lasurls(a))
                UrlDeck = ext.extmtggoldfish(DeckPage, "/deck/download/")
                Dim Deck = ""
                Dim DeckTitle = ""

                if UrlDeck.Contains(vbCrLf) Then UrlDeck = split(UrlDeck,vbcrlf)(0).ToString()

                Deck = fn.ReadWeb(vars.mtggf & "/" & UrlDeck)
                Deck = Replace(Deck, "sideboard", "[sideboard]")
                Deck = Replace(Deck, vbCrLf & vbCrLf, vbCrLf & "[sideboard]" & vbCrLf)
                Deck = Replace(Deck, "[[", "[")
                Deck = Replace(Deck, "]]", "]")
                DeckTitle = fn.FindIt(DeckPage, "<title>", "</title>")
                DeckTitle = Replace(DeckTitle, "_", " ")
                DeckTitle = Replace(DeckTitle, """", "'")
                Dim num As String = (contadorposicion + 1).ToString
                If Len(num) <= 1 Then num = "0" & num
                DeckTitle = "#" & num & " - " & DeckTitle
                contadorposicion = (contadorposicion + 1).ToString
                Deck = fn.FormatDeck(Deck, DeckTitle)
                If Deck <> Nothing Then
                    fn.StringToDeck(MyFolder, Deck, DeckTitle)
                    fn.WriteUserLog("Saving " & DeckTitle & vbCrLf)
                End If

            End If

        Next a
        extract1.Enabled = True
        fn.WriteUserLog("Completed")
    End Sub

    Private Sub chklaunchforgeafterupdate_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chklaunchforgeafterupdate.CheckedChanged
        Dim myCheck As Boolean
        If fn.ReadLogUser("launchforgeafterupdate", False, False) = "yes" Then
            myCheck = True
        Else
            myCheck = False
        End If
        If myCheck <> IIf(chklaunchforgeafterupdate.Checked, True, False) Then
            fn.UpdateLog("launchforgeafterupdate", IIf(chklaunchforgeafterupdate.Checked, "yes", "no"))
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
        fn.AlertAboutVersion(False)
    End Sub

    Private Sub CccToolStripMenuItem_Click(sender As Object, e As EventArgs)
        fn.CheckLauncherUpdates()
    End Sub

    Private Sub CccToolStripMenuItem_Click_1(sender As Object, e As EventArgs)
        fn.CheckLauncherUpdates()
    End Sub

    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles Button1.Click

        Dim ValidateYear = False
        Dim ValidateFormat = False
        If lbgauntletyear.SelectedIndex <> -1 Then
            ValidateYear = True
        End If
        If ValidateYear = False Then
            MsgBox("Select Year")
            Exit Sub
        End If

        If lbgauntletformat.SelectedIndex <> -1 Then
            ValidateFormat = True
        End If
        If ValidateFormat = False Then
            MsgBox("Select Format")
            Exit Sub
        End If

        Dim MyYear = lbgauntletyear.SelectedItem.ToString
        Dim MyFormat = lbgauntletformat.SelectedItem.ToString
        Button1.Enabled = False
        Try
            File.Delete("gauntlet.zip")
        Catch
        End Try
        Try
            fn.DownloadFile(vars.BaseUrl & "gauntlets/" & LCase(MyFormat) & "/" & MyYear & ".zip", "gauntlet.zip", True)
        Catch
            fn.WriteUserLog("Unable to get from server temporarily, please try later." & vbCrLf)
            Exit Sub
        End Try
        'Try
        Dim mycount As Long = 0
        'vars.UserDir = My.Settings.myuser_directory
        vars.UserDir = Replace(vars.UserDir, "/user", "")
        vars.UserDir = Replace(vars.UserDir, "\user", "")
        Dim userfolder = fn.ReadLogUser("gauntlet_dir", False, False)

        Using archive As ZipArchive = ZipFile.OpenRead("gauntlet.zip")
            For Each entry As ZipArchiveEntry In archive.Entries
                entry.ExtractToFile(Path.Combine(userfolder & "\", entry.FullName), True)
                fn.WriteUserLog("Extracting Gauntlet " & entry.Name & vbCrLf)
                mycount += 1
            Next
        End Using

        Try
            File.Delete("gauntlet.zip")
        Catch
        End Try
        Button1.Enabled = True
    End Sub

    Private Sub Button3_Click_3(sender As Object, e As EventArgs) Handles Button3.Click
        Dim userfolder = fn.ReadLogUser("gauntlet_dir", False, False)
        Dim directoryName As String = userfolder
        Try
            Dim MyCount = 0
            For Each deleteFile In Directory.GetFiles(directoryName, "LOCKED_*.*", SearchOption.TopDirectoryOnly)
                If deleteFile.Contains("LOCKED_DotP") = False Then
                    If deleteFile.Contains("LOCKED_Starting") = False Then
                        If deleteFile.Contains("LOCKED_Swimming") = False Then
                            File.Delete(deleteFile)
                            fn.WriteUserLog("Deleting " & deleteFile & vbCrLf)
                            MyCount = MyCount + 1
                        End If
                    End If
                End If

            Next
            fn.WriteUserLog(MyCount & " downloaded Gauntlet has been deleted." & vbCrLf)

        Catch
        End Try
    End Sub

    Private Sub CheckForForgeLauncherUpdatesToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles CheckForForgeLauncherUpdatesToolStripMenuItem.Click
        fn.CheckLauncherUpdates()
    End Sub

    Private Sub chkenableprompt_CheckedChanged(sender As Object, e As EventArgs) Handles chkenableprompt.CheckedChanged
        Dim shit As Boolean
        If fn.ReadLogUser("enableprompt", False, False) = "yes" Then
            shit = True
        Else
            shit = False
        End If
        If shit <> IIf(chkenableprompt.Checked, True, False) Then
            fn.UpdateLog("enableprompt", IIf(chkenableprompt.Checked, "yes", "no"))
        End If
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
End Class