Imports System
Imports System.Text.RegularExpressions

''' <summary>Result of comparing the installed Forge build with the latest published one.</summary>
Public Enum UpdateStatus
    ''' <summary>The folder has no Forge installation.</summary>
    NotInstalled
    ''' <summary>The installed build is the latest one.</summary>
    UpToDate
    ''' <summary>A newer build is available.</summary>
    UpdateAvailable
    ''' <summary>The installed build is newer than the latest build of the selected channel.</summary>
    InstalledIsNewer
    ''' <summary>The installed build could not be identified; an update is offered to be safe.</summary>
    InstalledUnknown
End Enum

''' <summary>Decides whether the installed Forge build needs an update.</summary>
Public NotInheritable Class UpdateChecker
    ''' <summary>Leading numeric part of a version, for example <c>2.0.16</c> in <c>2.0.16-SNAPSHOT-10.06</c>.</summary>
    Private Shared ReadOnly NumericVersionPattern As New Regex("^\d+(?:\.\d+){1,3}", RegexOptions.CultureInvariant)

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Compares the installed build with the latest one.
    ''' Build times decide whenever both are known, because Forge publishes the same <c>build.txt</c> it ships.
    ''' Otherwise the numeric versions are compared; when they are equal, a release counts as newer than the
    ''' snapshots that preceded it.
    ''' </summary>
    ''' <param name="installed">Local installation.</param>
    ''' <param name="latest">Latest build of the selected channel.</param>
    Public Shared Function Compare(installed As ForgeInstallation, latest As RemoteBuild) As UpdateStatus
        If Not installed.IsInstalled Then Return UpdateStatus.NotInstalled

        If installed.BuildDate.HasValue AndAlso latest.BuildDate.HasValue Then
            Return FromComparison(latest.BuildDate.Value.CompareTo(installed.BuildDate.Value))
        End If

        Dim installedVersion = ParseVersion(installed.Version)
        Dim latestVersion = ParseVersion(latest.Version)
        If installedVersion Is Nothing OrElse latestVersion Is Nothing Then Return UpdateStatus.InstalledUnknown

        Dim result = latestVersion.CompareTo(installedVersion)
        If result <> 0 Then Return FromComparison(result)

        Dim installedIsSnapshot = IsSnapshot(installed.Version)
        Dim latestIsSnapshot = IsSnapshot(latest.Version)
        If installedIsSnapshot = latestIsSnapshot Then
            ' Two snapshots with the same numeric version cannot be told apart without build times.
            Return If(installedIsSnapshot, UpdateStatus.InstalledUnknown, UpdateStatus.UpToDate)
        End If
        Return If(installedIsSnapshot, UpdateStatus.UpdateAvailable, UpdateStatus.InstalledIsNewer)
    End Function

    ''' <summary>
    ''' Returns <c>True</c> when the latest build is one the user chose not to be offered again after restoring a
    ''' previous version: it was published at or before the skipped build.
    ''' </summary>
    ''' <param name="latest">Latest build of the selected channel.</param>
    ''' <param name="settings">Settings holding the skipped build, if any.</param>
    Public Shared Function IsSkipped(latest As RemoteBuild, settings As LauncherSettings) As Boolean
        Return settings.SkippedBuild.HasValue AndAlso latest.BuildDate.HasValue AndAlso
               latest.BuildDate.Value <= settings.SkippedBuild.Value
    End Function

    ''' <summary>Maps a "latest compared to installed" result to a status.</summary>
    Private Shared Function FromComparison(latestComparedToInstalled As Integer) As UpdateStatus
        If latestComparedToInstalled > 0 Then Return UpdateStatus.UpdateAvailable
        If latestComparedToInstalled < 0 Then Return UpdateStatus.InstalledIsNewer
        Return UpdateStatus.UpToDate
    End Function

    ''' <summary>Parses the leading numeric part of a version; <c>Nothing</c> when there is none.</summary>
    Private Shared Function ParseVersion(text As String) As Version
        If String.IsNullOrEmpty(text) Then Return Nothing
        Dim match = NumericVersionPattern.Match(text)
        Dim result As Version = Nothing
        If match.Success AndAlso Version.TryParse(match.Value, result) Then Return result
        Return Nothing
    End Function

    ''' <summary><c>True</c> for snapshot versions such as <c>2.0.16-SNAPSHOT</c>.</summary>
    Private Shared Function IsSnapshot(text As String) As Boolean
        Return text IsNot Nothing AndAlso text.IndexOf("SNAPSHOT", StringComparison.OrdinalIgnoreCase) >= 0
    End Function
End Class
