Public Class settings
    Private Sub preferences_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        If fn.ReadLogUser("removepreviousjarfiles", False) = "yes" Then
            removepreviousjarfiles.Checked = True
        Else
            removepreviousjarfiles.Checked = False
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        fn.UpdateLog("removepreviousjarfiles", IIf(removepreviousjarfiles.Checked, "yes", "no"))
        Main.txlog.Text = ""
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