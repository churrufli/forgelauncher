Public Class preferences
    Private Sub preferences_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        decks_dir.Text = fn.ReadLogUser("decks_dir", False)
        gauntlet_dir.Text = fn.ReadLogUser("gauntlet_dir", False)
        tournamentsdecks_dir.Text = fn.ReadLogUser("tournamentsdecks_dir", False)
        downloadeddecks_dir.Text = fn.ReadLogUser("downloadeddecks_dir", False)
        pics_dir.Text = fn.ReadLogUser("pics_dir", False)

        FolderBrowserDialog1.SelectedPath = decks_dir.Text
        FolderBrowserDialog2.SelectedPath = gauntlet_dir.Text
        FolderBrowserDialog3.SelectedPath = pics_dir.Text

        preservedecksnumber.SelectedItem = preservedecksnumber.Items(0)

        If fn.ReadLogUser("preserve_decks", False) = "yes" Then
            chk_preservedecks.Checked = True
            Dim pdn = fn.ReadLogUser("preserve_decks_number")
            preservedecksnumber.SelectedItem = pdn
        Else
            chk_preservedecks.Checked = False
        End If

        If fn.ReadLogUser("removepreviousjarfiles", False) = "yes" Then
            removepreviousjarfiles.Checked = True
        Else
            removepreviousjarfiles.Checked = False
        End If
    End Sub

    Private Sub decksdirbutton_Click(sender As Object, e As EventArgs) Handles decksdirbutton.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            decks_dir.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub gauntletsdirbutton_Click(sender As Object, e As EventArgs) Handles gauntletsdirbutton.Click
        If (FolderBrowserDialog2.ShowDialog() = DialogResult.OK) Then
            gauntlet_dir.Text = FolderBrowserDialog2.SelectedPath
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        fn.UpdateLog("decks_dir", decks_dir.Text)
        fn.UpdateLog("gauntlet_dir", gauntlet_dir.Text)
        fn.UpdateLog("tournamentsdecks_dir", tournamentsdecks_dir.Text)
        fn.UpdateLog("downloadeddecks_dir", downloadeddecks_dir.Text)
        fn.UpdateLog("preserve_decks", IIf(chk_preservedecks.Checked, "yes", "no"))
        fn.UpdateLog("pics_dir", pics_dir.Text)
        fn.UpdateLog("preserve_decks_number", preservedecksnumber.SelectedItem.ToString)
        fn.UpdateLog("removepreviousjarfiles", IIf(removepreviousjarfiles.Checked, "yes", "no"))
        fl.txlog.Text = ""
        Application.Restart()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If _
            MsgBox("Do your want to restore Forge Launcher to default settings? (restart needed)",
                   MsgBoxStyle.YesNoCancel, "Warning") = MsgBoxResult.Yes Then
            Try
                File.Delete("fldata/version.txt")
                Application.Restart()
            Catch
            End Try
        End If
    End Sub

    Private Sub picsdirbutton_Click(sender As Object, e As EventArgs) Handles picsdirbutton.Click
        If (FolderBrowserDialog3.ShowDialog() = DialogResult.OK) Then
            pics_dir.Text = FolderBrowserDialog3.SelectedPath
        End If
    End Sub

    Private Sub UFL_Click(sender As Object, e As EventArgs) Handles UFL.Click
        fn.UninstallForgeLauncher()
    End Sub

    Private Sub downloadeddecks_dir_TextChanged(sender As Object, e As EventArgs) _
        Handles downloadeddecks_dir.TextChanged
    End Sub
End Class