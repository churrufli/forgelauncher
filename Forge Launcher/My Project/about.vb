Public Class about
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        fn.UpdateLog("showabout", "no")
        Me.Close()
    End Sub
End Class