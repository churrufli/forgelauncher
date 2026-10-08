Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports Microsoft.Win32

''' <summary>
''' Collects the information needed to diagnose a problem (versions of Windows, .NET, Java, Forge and the launcher)
''' and builds the text that "Report a problem" copies to the clipboard.
''' </summary>
Public NotInheritable Class SystemInfo
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Builds the problem report: system versions, launcher settings that affect behavior, and the end of
    ''' <c>launcher.log</c>.
    ''' </summary>
    ''' <param name="installation">Current Forge installation.</param>
    ''' <param name="settings">Launcher settings.</param>
    ''' <param name="log">Persistent launcher log.</param>
    Public Shared Function BuildReport(installation As ForgeInstallation, settings As LauncherSettings, log As LauncherLog) As String
        Dim report As New StringBuilder()
        report.AppendLine("Forge Launcher problem report")
        report.AppendLine("=============================")
        report.AppendLine($"Forge Launcher: {SelfUpdater.CurrentVersion}")
        report.AppendLine($"Windows: {WindowsVersion()}")
        report.AppendLine($".NET Framework: {DotNetVersion()}")
        report.AppendLine($"Java: {JavaVersion()}")
        report.AppendLine($"Forge: {installation.Description}")
        report.AppendLine($"Forge folder: {installation.Folder}")
        report.AppendLine($"Channel: {settings.Channel}")
        report.AppendLine($"Launch mode: {settings.Mode}{If(settings.Mode = LaunchMode.CustomJava, $" ({settings.CustomApp}, {settings.MaxMemoryMB} MB, {settings.JavaPath})", $" ({settings.LaunchTarget})")}")
        report.AppendLine($"Previous versions kept: {settings.KeepPreviousVersions}")
        report.AppendLine()
        report.AppendLine("--- launcher.log (most recent lines) ---")
        report.AppendLine(log.ReadTail(200))
        Return report.ToString()
    End Function

    ''' <summary>
    ''' Windows edition, version and build, for example <c>Windows 11 Pro 23H2 (build 22631.4317)</c>.
    ''' Windows 11 still reports "Windows 10" in the registry product name, so the build number decides.
    ''' </summary>
    Public Shared Function WindowsVersion() As String
        Try
            Using key = OpenLocalMachine("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                If key Is Nothing Then Return Environment.OSVersion.VersionString
                Dim product = CStr(key.GetValue("ProductName", "Windows"))
                Dim display = CStr(key.GetValue("DisplayVersion", key.GetValue("ReleaseId", String.Empty)))
                Dim build = CStr(key.GetValue("CurrentBuild", String.Empty))
                Dim revision = key.GetValue("UBR")
                Dim buildNumber As Integer
                If Integer.TryParse(build, buildNumber) AndAlso buildNumber >= 22000 Then
                    product = product.Replace("Windows 10", "Windows 11")
                End If
                Dim buildText = If(revision Is Nothing, build, $"{build}.{Convert.ToString(revision, CultureInfo.InvariantCulture)}")
                Return $"{product} {display} (build {buildText})".Replace("  ", " ")
            End Using
        Catch ex As Exception When TypeOf ex Is UnauthorizedAccessException OrElse TypeOf ex Is Security.SecurityException OrElse TypeOf ex Is IOException
            Return Environment.OSVersion.VersionString
        End Try
    End Function

    ''' <summary>Installed .NET Framework 4.x version, read from the "Release" number Microsoft documents.</summary>
    Public Shared Function DotNetVersion() As String
        Try
            Using key = OpenLocalMachine("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full")
                Dim release = If(key Is Nothing, 0, Convert.ToInt32(key.GetValue("Release", 0), CultureInfo.InvariantCulture))
                If release >= 533320 Then Return $"4.8.1 or later (release {release})"
                If release >= 528040 Then Return $"4.8 (release {release})"
                If release > 0 Then Return $"older than 4.8 (release {release})"
                Return "unknown"
            End Using
        Catch ex As Exception When TypeOf ex Is UnauthorizedAccessException OrElse TypeOf ex Is Security.SecurityException OrElse TypeOf ex Is IOException
            Return "unknown"
        End Try
    End Function

    ''' <summary>
    ''' First line of <c>java -version</c> for the Java found on the PATH, or a note when Java is not found.
    ''' Java prints its version on the error stream.
    ''' </summary>
    Public Shared Function JavaVersion() As String
        Try
            Dim startInfo As New ProcessStartInfo("java", "-version") With {
                .UseShellExecute = False,
                .CreateNoWindow = True,
                .RedirectStandardError = True,
                .RedirectStandardOutput = True}
            Using java = Process.Start(startInfo)
                Dim errorTask = java.StandardError.ReadToEndAsync()
                If Not java.WaitForExit(5000) Then Return "java -version did not answer"
                Dim firstLine = errorTask.Result.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                Return If(firstLine.Length > 0, firstLine(0).Trim(), "unknown")
            End Using
        Catch ex As Win32Exception
            Return "not found on the PATH"
        Catch ex As InvalidOperationException
            Return "unknown"
        End Try
    End Function

    ''' <summary>Opens a HKEY_LOCAL_MACHINE key in the 64-bit view on 64-bit Windows, so 32-bit processes read the real values.</summary>
    Private Shared Function OpenLocalMachine(subKey As String) As RegistryKey
        Dim view = If(Environment.Is64BitOperatingSystem, RegistryView.Registry64, RegistryView.Default)
        Using baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view)
            Return baseKey.OpenSubKey(subKey)
        End Using
    End Function
End Class
