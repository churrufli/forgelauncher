Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO

''' <summary>Starts Forge in normal mode (a file of the Forge folder) or with a custom Java command.</summary>
Public NotInheritable Class ForgeRunner
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Starts Forge with the working directory set to the Forge folder, which is what Forge expects for relative
    ''' paths in forge.profile.properties.
    ''' </summary>
    ''' <param name="folder">Forge folder.</param>
    ''' <param name="settings">Launch settings.</param>
    ''' <param name="target">File to start in normal mode (ignored in custom Java mode).</param>
    ''' <returns>A description of what was started, for the log.</returns>
    ''' <exception cref="FileNotFoundException">The file or jar to start does not exist.</exception>
    ''' <exception cref="ComponentModel.Win32Exception">Windows could not start the program (for example, Java is not installed).</exception>
    Public Shared Function Launch(folder As String, settings As LauncherSettings, target As String) As String
        If settings.Mode = LaunchMode.CustomJava Then
            ' Always use the newest jar, so the custom command keeps working after updates change the jar name.
            Dim jar = ForgeInstallation.FindNewestJar(folder, settings.CustomApp)
            If jar Is Nothing Then
                Throw New FileNotFoundException($"No {AppName(settings.CustomApp)} jar file was found in {folder}.")
            End If
            Dim arguments = BuildJavaArguments(settings.MaxMemoryMB, settings.JvmArguments, Path.GetFileName(jar.FilePath))
            Start(settings.JavaPath, arguments, folder)
            Return $"{settings.JavaPath} {arguments}"
        End If

        If String.IsNullOrEmpty(target) Then Throw New FileNotFoundException("No launch file is selected.")
        Dim targetPath = Path.Combine(folder, target)
        If Not File.Exists(targetPath) Then Throw New FileNotFoundException($"{target} was not found in {folder}.")
        Start(targetPath, String.Empty, folder)
        Return target
    End Function

    ''' <summary>Builds the Java command line: <c>-Xmx&lt;memory&gt;m &lt;arguments&gt; -jar &lt;jar&gt;</c>.</summary>
    ''' <param name="maxMemoryMB">Maximum heap in megabytes; 0 leaves Java's default.</param>
    ''' <param name="jvmArguments">Extra JVM arguments; line breaks are allowed.</param>
    ''' <param name="jarName">Jar file name, relative to the Forge folder.</param>
    Public Shared Function BuildJavaArguments(maxMemoryMB As Integer, jvmArguments As String, jarName As String) As String
        Dim parts As New List(Of String)
        If maxMemoryMB > 0 Then parts.Add($"-Xmx{maxMemoryMB}m")
        Dim extra = LauncherSettings.SingleLine(jvmArguments)
        If extra.Length > 0 Then parts.Add(extra)
        parts.Add("-jar")
        parts.Add(Quote(jarName))
        Return String.Join(" ", parts)
    End Function

    ''' <summary>Display name of a Forge application.</summary>
    Public Shared Function AppName(app As ForgeApp) As String
        Return If(app = ForgeApp.Adventure, "Forge Adventure", "Forge")
    End Function

    ''' <summary>Wraps a value in quotes when it contains spaces and is not quoted already.</summary>
    Public Shared Function Quote(value As String) As String
        If value.IndexOf(" "c) < 0 OrElse value.StartsWith("""", StringComparison.Ordinal) Then Return value
        Return """" & value & """"
    End Function

    ''' <summary>
    ''' Starts a program through the Windows shell, so names on the PATH (such as <c>javaw</c>) and .cmd/.bat files work.
    ''' </summary>
    Private Shared Sub Start(fileName As String, arguments As String, workingDirectory As String)
        Dim startInfo As New ProcessStartInfo(fileName, arguments) With {
            .WorkingDirectory = workingDirectory,
            .UseShellExecute = True}
        Process.Start(startInfo)?.Dispose()
    End Sub
End Class
