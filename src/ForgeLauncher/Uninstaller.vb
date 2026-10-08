Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Text

''' <summary>
''' Removes Forge Launcher from a Forge folder: the <c>fldata</c> folder, the launcher executable and its DLL.
''' Forge itself, the user's data and forge.profile.properties are never touched.
''' </summary>
''' <remarks>
''' A running program cannot delete its own executable. The launcher therefore writes a small hidden batch file to
''' the temporary folder, starts it and exits. The batch file waits until the launcher process has really ended
''' (instead of waiting a fixed time, which can fail on slow computers), deletes the files and finally deletes itself.
''' </remarks>
Public NotInheritable Class Uninstaller
    Private Sub New()
    End Sub

    ''' <summary>Starts the removal. The caller must close the launcher right after this returns.</summary>
    ''' <param name="paths">Launcher paths of the Forge folder.</param>
    Public Shared Sub Start(paths As LauncherPaths)
        Dim processId = Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture)
        Dim script As New StringBuilder()
        script.AppendLine("@echo off")
        script.AppendLine(":wait")
        ' While the launcher is running, tasklist prints a line containing ".exe"; afterwards it prints only an info message.
        script.AppendLine($"tasklist /FI ""PID eq {processId}"" /NH | find /I "".exe"" >nul && (timeout /t 1 /nobreak >nul & goto wait)")
        script.AppendLine($"rmdir /s /q ""{paths.DataFolder}""")
        script.AppendLine($"del /f /q ""{paths.LauncherExe}""")
        script.AppendLine($"del /f /q ""{paths.ZipLibrary}""")
        ' "(goto) 2>nul" ends the script before "del" runs, which lets the batch file delete itself without an error.
        script.AppendLine("(goto) 2>nul & del ""%~f0""")

        Dim scriptPath = Path.Combine(Path.GetTempPath(), $"ForgeLauncher-uninstall-{processId}.cmd")
        ' cmd.exe reads batch files in the console (OEM) code page, so folder names with accents must use it too.
        Dim oemEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
        File.WriteAllText(scriptPath, script.ToString(), oemEncoding)

        Process.Start(New ProcessStartInfo("cmd.exe", $"/c ""{scriptPath}""") With {
            .UseShellExecute = False,
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .WorkingDirectory = Path.GetTempPath()})?.Dispose()
    End Sub
End Class
