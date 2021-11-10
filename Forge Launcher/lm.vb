Imports System.Text
Imports Microsoft.VisualBasic.Devices

Public Class lm
    Dim haschanges As Boolean = False

    Private Sub lm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getAvailableRAM()

        Select Case fn.ReadLogUser("launchmode", False).ToString
            Case "normal"
                rbt_normalmode.Checked = True
                rbt_advancedmode.Checked = False
                GroupBox1.Enabled = False
            Case "advanced"
                rbt_normalmode.Checked = False
                rbt_advancedmode.Checked = True
                GroupBox1.Enabled = True
                tb_launchline.Text = fn.ReadLogUser("launchline", False)
                tb_launchline.DeselectAll()

        End Select
        tb_launchline.DeselectAll()
    End Sub

    Public Sub getAvailableRAM()
        Dim CI = New ComputerInfo()
        Dim mem As ULong = ULong.Parse(CI.TotalPhysicalMemory.ToString())
        Dim a = Math.Floor(mem/(1024*1024)) & " MB"
        lblram.Text = a
    End Sub

    Private Sub rbt_normalmode_CheckedChanged(sender As Object, e As EventArgs) Handles rbt_normalmode.CheckedChanged
        checkestados()
    End Sub

    Private Sub rbt_advancedmode_CheckedChanged(sender As Object, e As EventArgs) _
        Handles rbt_advancedmode.CheckedChanged
        checkestados()
    End Sub

    Sub checkestados()
        If rbt_normalmode.Checked = False Then
            GroupBox1.Enabled = True
            rbt_advancedmode.Checked = True
            tb_launchline.Text = fn.ReadLogUser("launchline")
        Else
            GroupBox1.Enabled = False
            rbt_advancedmode.Checked = False
            tb_launchline.Text = ""
        End If
    End Sub

    Private Sub lm_Closed(sender As Object, e As EventArgs) Handles Me.Closed

        If rbt_advancedmode.Checked And tb_launchline.Text = "" Then
            MsgBox(
                "Please, fill text for Advanced Mode. Click in blue question icon near Advanced Mode for get an example.")
            Exit Sub
        End If

        Dim SelectedMode As String
        If rbt_advancedmode.Checked Then
            SelectedMode = "advanced"
        Else
            SelectedMode = "normal"

        End If

        If SelectedMode <> fn.ReadLogUser("launchmode") Then
            haschanges = True
        End If

        If rbt_advancedmode.Checked = True And fn.ReadLogUser("launchline") <> tb_launchline.Text Then
            haschanges = True
        End If

        If haschanges Then
            If rbt_advancedmode.Checked = True Then
                fn.UpdateLog("launchmode", "advanced")
                fn.UpdateLog("launchline", tb_launchline.Text)

                Dim sTextFile As New StringBuilder
                sTextFile.AppendLine(Trim(tb_launchline.Text))
                Dim sFileName As String = Environment.CurrentDirectory & "\PlayForge.bat"
                If File.Exists(sFileName) Then File.Delete(sFileName)
                File.AppendAllText(sFileName, sTextFile.ToString)
            Else
                Dim sFileName As String = Environment.CurrentDirectory & "\PlayForge.bat"
                If File.Exists(sFileName) Then File.Delete(sFileName)
                fn.UpdateLog("launchmode", "normal")

            End If
            Application.Restart()
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If _
            MsgBox(
                "Do you want to load an example? Please be sure about your RAM min and max value. (Example Values:2GBmin 4GBmax)",
                MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            Dim t As String = "@ECHO OFF" & vbCrLf
            t = t & "FOR /F %%I in ('DIR forge-gui-desktop-*-with-dependencies.jar /B /O:D') DO SET J=%%I" & vbCrLf
            t = t & "java ^" & vbCrLf
            t = t & "-Xms2048m ^" & vbCrLf
            t = t & "-Xmx4096m ^" & vbCrLf
            t = t & "-Dfile.encoding=UTF-8 ^" & vbCrLf
            t = t & "-jar %J%"
            tb_launchline.Text = t
        End If
    End Sub
End Class