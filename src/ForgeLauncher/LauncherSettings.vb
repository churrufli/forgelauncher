Imports System
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>Which Forge builds the launcher installs.</summary>
Public Enum UpdateChannel
    ''' <summary>Daily development builds (GitHub release "daily-snapshots").</summary>
    Snapshot
    ''' <summary>Stable versions (GitHub "latest" release).</summary>
    Release
End Enum

''' <summary>How "Launch Forge" starts Forge.</summary>
Public Enum LaunchMode
    ''' <summary>Start the file selected in the main window (forge.exe, a .cmd file...).</summary>
    Normal
    ''' <summary>Start Java directly with the memory and JVM arguments chosen in Settings.</summary>
    CustomJava
End Enum

''' <summary>Which Forge application the custom Java command starts.</summary>
Public Enum ForgeApp
    ''' <summary>The classic Forge desktop application.</summary>
    Desktop
    ''' <summary>Forge Adventure mode.</summary>
    Adventure
End Enum

''' <summary>
''' User settings, stored as simple <c>Key=Value</c> lines in <c>fldata\settings.ini</c>.
''' The file is created the first time a setting is saved. Settings from Forge Launcher 3.0.0
''' (<c>forgelauncher.ini</c> in the Forge folder) and from the original launcher
''' (<c>fldata\version.txt</c>) are migrated automatically.
''' </summary>
Public NotInheritable Class LauncherSettings
    ''' <summary>Default Java executable used by the custom Java launch mode.</summary>
    Public Const DefaultJavaPath As String = "javaw"

    ''' <summary>Default maximum Java heap, in megabytes.</summary>
    Public Const DefaultMaxMemoryMB As Integer = 4096

    ''' <summary>Largest number of previous Forge versions the user can keep for rollback.</summary>
    Public Const MaxPreviousVersions As Integer = 3

    ''' <summary>
    ''' Default JVM arguments for the custom Java launch mode: the module access flags Forge needs on
    ''' Java 17 and later (copied from Forge's own forge-adventure.cmd) plus two system properties.
    ''' </summary>
    Public Shared ReadOnly DefaultJvmArguments As String = String.Join(" ", {
        "--add-opens java.desktop/java.beans=ALL-UNNAMED",
        "--add-opens java.desktop/javax.swing.border=ALL-UNNAMED",
        "--add-opens java.desktop/javax.swing.event=ALL-UNNAMED",
        "--add-opens java.desktop/sun.swing=ALL-UNNAMED",
        "--add-opens java.desktop/java.awt.image=ALL-UNNAMED",
        "--add-opens java.desktop/java.awt.color=ALL-UNNAMED",
        "--add-opens java.desktop/sun.awt.image=ALL-UNNAMED",
        "--add-opens java.desktop/javax.swing=ALL-UNNAMED",
        "--add-opens java.desktop/java.awt=ALL-UNNAMED",
        "--add-opens java.base/java.util=ALL-UNNAMED",
        "--add-opens java.base/java.lang=ALL-UNNAMED",
        "--add-opens java.base/java.lang.reflect=ALL-UNNAMED",
        "--add-opens java.base/java.text=ALL-UNNAMED",
        "--add-opens java.desktop/java.awt.font=ALL-UNNAMED",
        "--add-opens java.base/jdk.internal.misc=ALL-UNNAMED",
        "--add-opens java.base/sun.nio.ch=ALL-UNNAMED",
        "--add-opens java.base/java.nio=ALL-UNNAMED",
        "--add-opens java.base/java.math=ALL-UNNAMED",
        "--add-opens java.base/java.util.concurrent=ALL-UNNAMED",
        "--add-opens java.base/java.net=ALL-UNNAMED",
        "-Dio.netty.tryReflectionSetAccessible=true",
        "-Dfile.encoding=UTF-8"})

    Private Const BuildDateFormat As String = "yyyy-MM-dd HH:mm:ss"

    Private _filePath As String

    ''' <summary>Update channel shown in the main window.</summary>
    Public Property Channel As UpdateChannel = UpdateChannel.Snapshot

    ''' <summary>Check for Forge updates every time the launcher starts.</summary>
    Public Property CheckOnStartup As Boolean = True

    ''' <summary>Check for a new Forge Launcher version every time the launcher starts.</summary>
    Public Property CheckLauncherUpdates As Boolean = True

    ''' <summary>
    ''' When Forge is up to date at startup: <c>True</c> shows a dialog offering to start Forge,
    ''' <c>False</c> shows no dialog and leaves the "Launch Forge" button focused (Enter starts Forge).
    ''' </summary>
    Public Property AskToLaunchWhenUpToDate As Boolean = True

    ''' <summary>How many previous Forge versions to keep for rollback (0, 1, 2 or 3).</summary>
    Public Property KeepPreviousVersions As Integer = 0

    ''' <summary>How "Launch Forge" starts Forge.</summary>
    Public Property Mode As LaunchMode = LaunchMode.Normal

    ''' <summary>File started in normal launch mode, relative to the Forge folder.</summary>
    Public Property LaunchTarget As String = "forge.exe"

    ''' <summary>Application started by the custom Java command.</summary>
    Public Property CustomApp As ForgeApp = ForgeApp.Desktop

    ''' <summary>Java executable used by the custom Java command (a name on the PATH or a full path).</summary>
    Public Property JavaPath As String = DefaultJavaPath

    ''' <summary>Maximum Java heap for the custom Java command, in megabytes (passed as <c>-Xmx</c>).</summary>
    Public Property MaxMemoryMB As Integer = DefaultMaxMemoryMB

    ''' <summary>Extra JVM arguments for the custom Java command, on a single line.</summary>
    Public Property JvmArguments As String = DefaultJvmArguments

    ''' <summary>
    ''' Build time of a Forge build the user left by restoring a previous version and chose not to be offered again.
    ''' Builds published at or before this time are not offered at startup. Cleared when a new build is installed.
    ''' </summary>
    Public Property SkippedBuild As Date?

    ''' <summary>Version text of <see cref="SkippedBuild"/>, shown in the main window header.</summary>
    Public Property SkippedVersion As String

    ''' <summary>
    ''' Loads the settings for a Forge folder. When <c>fldata\settings.ini</c> does not exist yet,
    ''' settings from older launcher versions are migrated into it.
    ''' </summary>
    ''' <param name="paths">Launcher paths of the Forge folder.</param>
    ''' <param name="log">Receives a message for every migration performed; may be <c>Nothing</c>.</param>
    Public Shared Function Load(paths As LauncherPaths, Optional log As Action(Of String) = Nothing) As LauncherSettings
        Dim settings As New LauncherSettings With {._filePath = paths.SettingsFile}

        If File.Exists(paths.SettingsFile) Then
            settings.ReadFile(paths.SettingsFile)
            Return settings
        End If

        ' Forge Launcher 3.0.0 kept the same keys in forgelauncher.ini next to Forge.exe: read it, save the
        ' values into fldata\settings.ini and remove the old file so the Forge folder stays tidy.
        If File.Exists(paths.LegacyRootSettingsFile) Then
            settings.ReadFile(paths.LegacyRootSettingsFile)
            If settings.Save() Then
                TryDeleteFile(paths.LegacyRootSettingsFile)
                log?.Invoke("Settings moved from forgelauncher.ini to fldata\settings.ini.")
            End If
            Return settings
        End If

        ' The original launcher stored pseudo-XML in fldata\version.txt; only the channel and the selected
        ' executable are still meaningful.
        If File.Exists(paths.LegacyVersionFile) Then
            settings.ImportLegacyVersionFile(paths.LegacyVersionFile)
            If settings.Save() Then
                TryDeleteFile(paths.LegacyVersionFile)
                log?.Invoke("Settings imported from the previous Forge Launcher.")
            End If
        End If
        Return settings
    End Function

    ''' <summary>Writes the settings to <c>fldata\settings.ini</c>, creating the folder when needed.</summary>
    ''' <returns><c>True</c> when the file was written; <c>False</c> when the folder is not writable.</returns>
    Public Function Save() As Boolean
        Dim lines = {
            "Channel=" & Channel.ToString(),
            "CheckOnStartup=" & CheckOnStartup.ToString(),
            "CheckLauncherUpdates=" & CheckLauncherUpdates.ToString(),
            "AskToLaunchWhenUpToDate=" & AskToLaunchWhenUpToDate.ToString(),
            "KeepPreviousVersions=" & KeepPreviousVersions.ToString(CultureInfo.InvariantCulture),
            "LaunchMode=" & Mode.ToString(),
            "LaunchTarget=" & LaunchTarget,
            "CustomApp=" & CustomApp.ToString(),
            "JavaPath=" & JavaPath,
            "MaxMemoryMB=" & MaxMemoryMB.ToString(CultureInfo.InvariantCulture),
            "JvmArguments=" & SingleLine(JvmArguments),
            "SkippedBuild=" & If(SkippedBuild.HasValue, SkippedBuild.Value.ToString(BuildDateFormat, CultureInfo.InvariantCulture), String.Empty),
            "SkippedVersion=" & If(SkippedVersion, String.Empty)}
        Try
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath))
            File.WriteAllLines(_filePath, lines, New UTF8Encoding(False))
            Return True
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return False
        End Try
    End Function

    ''' <summary>Returns an independent copy, used by the Settings window so "Cancel" discards edits.</summary>
    Public Function Clone() As LauncherSettings
        Return DirectCast(MemberwiseClone(), LauncherSettings)
    End Function

    ''' <summary>Copies every setting from another instance (used when the Settings window is saved).</summary>
    ''' <param name="other">Settings to copy from.</param>
    Public Sub CopyFrom(other As LauncherSettings)
        Channel = other.Channel
        CheckOnStartup = other.CheckOnStartup
        CheckLauncherUpdates = other.CheckLauncherUpdates
        AskToLaunchWhenUpToDate = other.AskToLaunchWhenUpToDate
        KeepPreviousVersions = other.KeepPreviousVersions
        Mode = other.Mode
        LaunchTarget = other.LaunchTarget
        CustomApp = other.CustomApp
        JavaPath = other.JavaPath
        MaxMemoryMB = other.MaxMemoryMB
        JvmArguments = other.JvmArguments
        SkippedBuild = other.SkippedBuild
        SkippedVersion = other.SkippedVersion
    End Sub

    ''' <summary>Forgets the build skipped after a rollback, so every newer build is offered again.</summary>
    Public Sub ClearSkippedBuild()
        SkippedBuild = Nothing
        SkippedVersion = Nothing
    End Sub

    ''' <summary>Collapses all whitespace (including line breaks) into single spaces.</summary>
    ''' <param name="text">Text to normalize; <c>Nothing</c> becomes an empty string.</param>
    Public Shared Function SingleLine(text As String) As String
        If text Is Nothing Then Return String.Empty
        Return Regex.Replace(text, "\s+", " ").Trim()
    End Function

    ''' <summary>Reads <c>Key=Value</c> lines, ignoring blank lines, comments and unknown keys.</summary>
    Private Sub ReadFile(filePath As String)
        Dim lines As String()
        Try
            lines = File.ReadAllLines(filePath, Encoding.UTF8)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return
        End Try

        For Each rawLine In lines
            Dim line = rawLine.Trim()
            If line.Length = 0 OrElse line.StartsWith("#", StringComparison.Ordinal) OrElse line.StartsWith(";", StringComparison.Ordinal) Then Continue For
            Dim separator = line.IndexOf("="c)
            If separator <= 0 Then Continue For
            SetValue(line.Substring(0, separator).Trim(), line.Substring(separator + 1).Trim())
        Next
    End Sub

    ''' <summary>Applies one key/value pair. Invalid values keep the current (default) value.</summary>
    Private Sub SetValue(key As String, value As String)
        Select Case key.ToLowerInvariant()
            Case "channel"
                Channel = ParseEnum(value, Channel)
            Case "checkonstartup"
                CheckOnStartup = ParseBoolean(value, CheckOnStartup)
            Case "checklauncherupdates"
                CheckLauncherUpdates = ParseBoolean(value, CheckLauncherUpdates)
            Case "asktolaunchwhenuptodate"
                AskToLaunchWhenUpToDate = ParseBoolean(value, AskToLaunchWhenUpToDate)
            Case "keeppreviousversions"
                KeepPreviousVersions = Math.Max(0, Math.Min(MaxPreviousVersions, ParseInteger(value, KeepPreviousVersions, allowZero:=True)))
            Case "launchmode"
                Mode = ParseEnum(value, Mode)
            Case "launchtarget"
                If value.Length > 0 Then LaunchTarget = value
            Case "customapp"
                CustomApp = ParseEnum(value, CustomApp)
            Case "javapath"
                If value.Length > 0 Then JavaPath = value
            Case "maxmemorymb"
                MaxMemoryMB = ParseInteger(value, MaxMemoryMB, allowZero:=False)
            Case "jvmarguments"
                JvmArguments = value
            Case "skippedbuild"
                SkippedBuild = ForgeInstallation.ParseBuildDate(value)
            Case "skippedversion"
                SkippedVersion = If(value.Length > 0, value, Nothing)
        End Select
    End Sub

    ''' <summary>Imports the channel and selected executable from the original launcher's <c>fldata\version.txt</c>.</summary>
    Private Sub ImportLegacyVersionFile(filePath As String)
        Dim text As String
        Try
            text = File.ReadAllText(filePath)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return
        End Try

        If String.Equals(ReadLegacyValue(text, "typeofupdate"), "release", StringComparison.OrdinalIgnoreCase) Then
            Channel = UpdateChannel.Release
        End If
        Dim selectedExe = ReadLegacyValue(text, "exeselected")
        If Not String.IsNullOrWhiteSpace(selectedExe) Then LaunchTarget = selectedExe.Trim()
    End Sub

    ''' <summary>Reads the text between <c>&lt;key&gt;</c> and <c>&lt;/key&gt;</c> in the legacy file.</summary>
    Private Shared Function ReadLegacyValue(text As String, key As String) As String
        Dim match = Regex.Match(text, "<" & key & ">(.*?)</" & key & ">", RegexOptions.Singleline)
        Return If(match.Success, match.Groups(1).Value.Trim(), Nothing)
    End Function

    ''' <summary>Parses an enum name case-insensitively, rejecting numbers that are not defined values.</summary>
    Private Shared Function ParseEnum(Of T As Structure)(value As String, fallback As T) As T
        Dim result As T = Nothing
        If [Enum].TryParse(value, True, result) AndAlso [Enum].IsDefined(GetType(T), result) Then Return result
        Return fallback
    End Function

    ''' <summary>Parses <c>True</c>/<c>False</c> case-insensitively.</summary>
    Private Shared Function ParseBoolean(value As String, fallback As Boolean) As Boolean
        Dim result As Boolean
        Return If(Boolean.TryParse(value, result), result, fallback)
    End Function

    ''' <summary>Parses a non-negative integer written with invariant digits.</summary>
    Private Shared Function ParseInteger(value As String, fallback As Integer, allowZero As Boolean) As Integer
        Dim result As Integer
        If Integer.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, result) AndAlso
           (result > 0 OrElse (allowZero AndAlso result = 0)) Then
            Return result
        End If
        Return fallback
    End Function

    ''' <summary>Deletes a file, ignoring files that are locked or read-only.</summary>
    Private Shared Sub TryDeleteFile(filePath As String)
        Try
            File.Delete(filePath)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
        End Try
    End Sub
End Class
