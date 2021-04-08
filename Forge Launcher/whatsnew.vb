Public Class whatsnew
    Private Sub whatsnew_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists("fldata/fl_whatsnew.txt") = False Then
            Try
                fn.DownloadFile(vars.BaseUrl & "fl_whatsnew.txt", "fldata/fl_whatsnew.txt")
            Catch
            End Try
        End If
        Try
            Dim readText As String = File.ReadAllText("fldata/fl_whatsnew.txt")
            If InStr(readText, vbNewLine) = 0 Then readText = Replace(readText, vbLf, vbNewLine)

            TextBox1.Text = readText

        Catch
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles LinkLabel1.LinkClicked
        Process.Start("https://www.slightlymagic.net/forum/viewtopic.php?f=26&t=21514")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles LinkLabel2.LinkClicked
        Process.Start("https://www.mtggoldfish.com")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles LinkLabel3.LinkClicked
        Process.Start("http://www.mtgtop8.com")
    End Sub

    Private Sub whatsnew_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            fn.UpdateLog("showwhatsnew", "no")
        Catch
        End Try
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles LinkLabel4.LinkClicked
        Process.Start("https://github.com/churrufli")
    End Sub
End Class