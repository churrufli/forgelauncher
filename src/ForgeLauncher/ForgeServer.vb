Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks

''' <summary>Progress of a download, reported to the user interface.</summary>
Public Structure TransferProgress
    ''' <summary>Creates a progress report.</summary>
    ''' <param name="received">Bytes received so far.</param>
    ''' <param name="total">Total size, when the server reported it.</param>
    Public Sub New(received As Long, total As Long?)
        Me.Received = received
        Me.Total = total
    End Sub

    ''' <summary>Bytes received so far.</summary>
    Public ReadOnly Property Received As Long

    ''' <summary>Total size in bytes, or <c>Nothing</c> when unknown.</summary>
    Public ReadOnly Property Total As Long?

    ''' <summary>Completed percentage (0-100), or -1 when the total size is unknown.</summary>
    Public ReadOnly Property Percent As Integer
        Get
            If Not Total.HasValue OrElse Total.Value <= 0 Then Return -1
            Return CInt(Math.Min(100L, Received * 100L \ Total.Value))
        End Get
    End Property
End Structure

''' <summary>
''' GitHub answered with an error status. Keeps the status code, so the user and the problem report can tell a
''' missing file (404) from rate limiting (403/429) or a GitHub outage (5xx).
''' </summary>
Public NotInheritable Class GitHubHttpException
    Inherits HttpRequestException

    ''' <summary>Creates the exception.</summary>
    Public Sub New(statusCode As HttpStatusCode, reasonPhrase As String, url As String, retryAfter As TimeSpan?)
        MyBase.New($"GitHub answered HTTP {CInt(statusCode)} ({reasonPhrase}) for {url}.")
        Me.StatusCode = statusCode
        Me.Url = url
        Me.RetryAfter = retryAfter
    End Sub

    ''' <summary>HTTP status code of the answer.</summary>
    Public ReadOnly Property StatusCode As HttpStatusCode

    ''' <summary>Requested URL.</summary>
    Public ReadOnly Property Url As String

    ''' <summary>How long GitHub asked to wait before trying again, when it said so.</summary>
    Public ReadOnly Property RetryAfter As TimeSpan?
End Class

''' <summary>The latest Forge build of a channel, as published on GitHub.</summary>
Public NotInheritable Class RemoteBuild
    ''' <summary>Creates a description of a published build.</summary>
    Public Sub New(channel As UpdateChannel, version As String, buildDate As Date?, packageUrl As String, releasePageUrl As String)
        Me.Channel = channel
        Me.Version = version
        Me.BuildDate = buildDate
        Me.PackageUrl = packageUrl
        Me.ReleasePageUrl = releasePageUrl
    End Sub

    ''' <summary>Channel the build belongs to.</summary>
    Public ReadOnly Property Channel As UpdateChannel

    ''' <summary>Version, for example <c>2.0.16-SNAPSHOT-10.06</c> or <c>2.0.15</c>.</summary>
    Public ReadOnly Property Version As String

    ''' <summary>Build time from the published <c>build.txt</c>, if available.</summary>
    Public ReadOnly Property BuildDate As Date?

    ''' <summary>Download URL of the <c>forge-installer-*.tar.bz2</c> package.</summary>
    Public ReadOnly Property PackageUrl As String

    ''' <summary>GitHub page of the release that contains the package.</summary>
    Public ReadOnly Property ReleasePageUrl As String

    ''' <summary>Human-readable description, for example <c>2.0.15 (built 2026-09-28 22:22)</c>.</summary>
    Public ReadOnly Property Description As String
        Get
            Return ForgeInstallation.Describe(Version, BuildDate)
        End Get
    End Property
End Class

''' <summary>
''' Talks to GitHub: finds the latest Forge build of each channel, finds the latest release of any repository,
''' and downloads files with progress reporting and cancellation.
''' </summary>
''' <remarks>
''' The GitHub API is not used, because anonymous API calls are rate limited. Instead the launcher reads the
''' small text files Forge publishes next to its packages, and follows the redirect of
''' <c>/releases/latest</c>, which points to the newest release tag.
''' </remarks>
Public NotInheritable Class ForgeServer
    Implements IDisposable

    ''' <summary>Forge's GitHub repository.</summary>
    Public Const ForgeRepositoryUrl As String = "https://github.com/Card-Forge/forge"

    ''' <summary>Release that always holds the newest daily snapshot and its <c>version.txt</c>/<c>build.txt</c>.</summary>
    Private Const SnapshotUrl As String = ForgeRepositoryUrl & "/releases/download/daily-snapshots/"

    ''' <summary>Forge tags its stable releases as <c>forge-2.0.15</c>.</summary>
    Private Const ForgeReleaseTagPrefix As String = "forge-"

    Private Const BufferSize As Integer = 81920
    Private Const BytesPerMB As Long = 1024L * 1024L

    ''' <summary>A download needs room for the package plus its extracted files (about 2.5 times the package).</summary>
    Private Const FreeSpaceFactor As Double = 2.5

    ''' <summary>Timeout for small requests (text files and redirects). Downloads have no overall timeout.</summary>
    Private Shared ReadOnly RequestTimeout As TimeSpan = TimeSpan.FromSeconds(30)

    ''' <summary>How many times a request is sent before giving up on temporary errors.</summary>
    Private Const MaxAttempts As Integer = 3

    ''' <summary>Longer <c>Retry-After</c> waits are not done automatically; the user is told to try later.</summary>
    Private Shared ReadOnly MaxRetryWait As TimeSpan = TimeSpan.FromSeconds(15)

    Private ReadOnly _client As HttpClient
    Private ReadOnly _clientWithoutRedirects As HttpClient

    ''' <summary>Creates the HTTP clients.</summary>
    ''' <param name="userAgent">User agent sent to GitHub, for example <c>ForgeLauncher/3.1.0</c>.</param>
    Public Sub New(userAgent As String)
        _client = CreateClient(True, userAgent)
        _clientWithoutRedirects = CreateClient(False, userAgent)
    End Sub

    ''' <summary>Finds the latest Forge build of a channel.</summary>
    ''' <param name="channel">Snapshot or release.</param>
    ''' <param name="cancellationToken">Cancels the lookup.</param>
    Public Async Function GetLatestAsync(channel As UpdateChannel, cancellationToken As CancellationToken) As Task(Of RemoteBuild)
        If channel = UpdateChannel.Release Then
            Return Await GetLatestForgeReleaseAsync(cancellationToken).ConfigureAwait(False)
        End If
        Return Await GetLatestSnapshotAsync(cancellationToken).ConfigureAwait(False)
    End Function

    ''' <summary>
    ''' Returns the tag of the latest release of a GitHub repository by reading where <c>/releases/latest</c>
    ''' redirects to (for example <c>.../releases/tag/forge-2.0.15</c>).
    ''' </summary>
    ''' <param name="repositoryUrl">Repository URL, for example <c>https://github.com/Card-Forge/forge</c>.</param>
    ''' <param name="cancellationToken">Cancels the lookup.</param>
    ''' <returns>
    ''' The tag, or <c>Nothing</c> when GitHub answers 404: the repository has no releases, does not exist
    ''' or is private (private repositories are invisible without signing in).
    ''' </returns>
    Public Async Function GetLatestReleaseTagAsync(repositoryUrl As String, cancellationToken As CancellationToken) As Task(Of String)
        Dim latestUrl = repositoryUrl.TrimEnd("/"c) & "/releases/latest"
        Using timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            timeout.CancelAfter(RequestTimeout)
            Using response = Await SendWithRetryAsync(Function(token) _clientWithoutRedirects.SendAsync(New HttpRequestMessage(HttpMethod.Head, latestUrl), token),
                                                      timeout.Token).ConfigureAwait(False)
                If response.StatusCode = HttpStatusCode.NotFound Then Return Nothing

                Dim location = response.Headers.Location
                If location Is Nothing Then
                    EnsureSuccess(response, latestUrl)
                    Throw New InvalidDataException($"GitHub did not report a latest release for {repositoryUrl}.")
                End If
                If Not location.IsAbsoluteUri Then location = New Uri(New Uri(latestUrl), location)

                ' The redirect target looks like /owner/repository/releases/tag/<tag>.
                Dim segments = location.AbsolutePath.Split({"/"c}, StringSplitOptions.RemoveEmptyEntries)
                Dim tagIndex = Array.IndexOf(segments, "tag")
                If tagIndex < 0 OrElse tagIndex = segments.Length - 1 Then Return Nothing
                Return Uri.UnescapeDataString(segments(tagIndex + 1))
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Downloads a file. The response is streamed to disk in blocks, progress is reported about five times
    ''' per second, and cancelling closes the connection immediately.
    ''' </summary>
    ''' <param name="url">File to download.</param>
    ''' <param name="destination">Where to save it; an existing file is overwritten.</param>
    ''' <param name="progress">Receives progress reports; may be <c>Nothing</c>.</param>
    ''' <param name="cancellationToken">Cancels the download.</param>
    ''' <param name="checkFreeSpace">When <c>True</c>, fails early if the disk cannot hold the package and its extracted files.</param>
    Public Async Function DownloadAsync(url As String, destination As String, progress As IProgress(Of TransferProgress),
                                        cancellationToken As CancellationToken, Optional checkFreeSpace As Boolean = True) As Task
        Using response = Await SendWithRetryAsync(Function(token) _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token),
                                                  cancellationToken).ConfigureAwait(False)
            EnsureSuccess(response, url)
            Dim total = response.Content.Headers.ContentLength
            If checkFreeSpace AndAlso total.HasValue Then EnsureFreeSpace(destination, CLng(total.Value * FreeSpaceFactor))

            ' On .NET Framework a pending read ignores the cancellation token, so disposing the response is what
            ' actually aborts a stalled download when the user clicks Cancel.
            Dim registration = cancellationToken.Register(Sub() response.Dispose())
            Try
                Directory.CreateDirectory(Path.GetDirectoryName(destination))
                Using source As Stream = Await response.Content.ReadAsStreamAsync().ConfigureAwait(False)
                    Using target As New FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, True)
                        Dim buffer(BufferSize - 1) As Byte
                        Dim received As Long = 0
                        Dim sinceLastReport = Stopwatch.StartNew()
                        Do
                            Dim read = Await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(False)
                            If read = 0 Then Exit Do
                            Await target.WriteAsync(buffer, 0, read, cancellationToken).ConfigureAwait(False)
                            received += read
                            If sinceLastReport.ElapsedMilliseconds >= 200 Then
                                progress?.Report(New TransferProgress(received, total))
                                sinceLastReport.Restart()
                            End If
                        Loop
                        progress?.Report(New TransferProgress(received, total))
                        If total.HasValue AndAlso received <> total.Value Then
                            Throw New IOException($"The download is incomplete ({received:N0} of {total.Value:N0} bytes).")
                        End If
                    End Using
                End Using
            Catch ex As Exception When cancellationToken.IsCancellationRequested AndAlso Not TypeOf ex Is OperationCanceledException
                ' Disposing the response makes the pending read fail with a network error; report it as a cancellation.
                Throw New OperationCanceledException(cancellationToken)
            Finally
                registration.Dispose()
            End Try
        End Using
    End Function

    ''' <summary>Releases the HTTP clients.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        _client.Dispose()
        _clientWithoutRedirects.Dispose()
    End Sub

    ''' <summary>
    ''' Reads the newest snapshot from the "daily-snapshots" release: <c>version.txt</c> holds the version
    ''' (for example <c>2.0.16-SNAPSHOT-10.06</c>) and <c>build.txt</c> the build time.
    ''' </summary>
    Private Async Function GetLatestSnapshotAsync(cancellationToken As CancellationToken) As Task(Of RemoteBuild)
        Dim version = Await GetTextAsync(SnapshotUrl & "version.txt", cancellationToken).ConfigureAwait(False)
        If String.IsNullOrWhiteSpace(version) Then Throw New InvalidDataException("The snapshot version file is empty.")
        Dim buildText = Await TryGetTextAsync(SnapshotUrl & ForgeInstallation.BuildFileName, cancellationToken).ConfigureAwait(False)
        Dim packageUrl = $"{SnapshotUrl}forge-installer-{Uri.EscapeDataString(version)}.tar.bz2"
        Dim pageUrl = ForgeRepositoryUrl & "/releases/tag/daily-snapshots"
        Return New RemoteBuild(UpdateChannel.Snapshot, version, ForgeInstallation.ParseBuildDate(buildText), packageUrl, pageUrl)
    End Function

    ''' <summary>
    ''' Reads the newest stable release: its tag (for example <c>forge-2.0.15</c>) gives the version, and the
    ''' release's <c>build.txt</c> gives the build time.
    ''' </summary>
    Private Async Function GetLatestForgeReleaseAsync(cancellationToken As CancellationToken) As Task(Of RemoteBuild)
        Dim tag = Await GetLatestReleaseTagAsync(ForgeRepositoryUrl, cancellationToken).ConfigureAwait(False)
        If String.IsNullOrEmpty(tag) Then Throw New InvalidDataException("GitHub did not report a latest Forge release.")

        Dim version = If(tag.StartsWith(ForgeReleaseTagPrefix, StringComparison.OrdinalIgnoreCase), tag.Substring(ForgeReleaseTagPrefix.Length), tag)
        Dim baseUrl = $"{ForgeRepositoryUrl}/releases/download/{Uri.EscapeDataString(tag)}/"
        Dim buildText = Await TryGetTextAsync(baseUrl & ForgeInstallation.BuildFileName, cancellationToken).ConfigureAwait(False)
        Dim packageUrl = $"{baseUrl}forge-installer-{Uri.EscapeDataString(version)}.tar.bz2"
        Dim pageUrl = $"{ForgeRepositoryUrl}/releases/tag/{Uri.EscapeDataString(tag)}"
        Return New RemoteBuild(UpdateChannel.Release, version, ForgeInstallation.ParseBuildDate(buildText), packageUrl, pageUrl)
    End Function

    ''' <summary>Downloads a small text file and returns it trimmed. Fails on HTTP errors and after the request timeout.</summary>
    Private Async Function GetTextAsync(url As String, cancellationToken As CancellationToken) As Task(Of String)
        Using timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            timeout.CancelAfter(RequestTimeout)
            Using response = Await SendWithRetryAsync(Function(token) _client.GetAsync(url, token), timeout.Token).ConfigureAwait(False)
                EnsureSuccess(response, url)
                Dim text = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
                Return text.Trim()
            End Using
        End Using
    End Function

    ''' <summary>Like <see cref="GetTextAsync"/>, but returns <c>Nothing</c> instead of failing (used for optional files).</summary>
    Private Async Function TryGetTextAsync(url As String, cancellationToken As CancellationToken) As Task(Of String)
        Try
            Return Await GetTextAsync(url, cancellationToken).ConfigureAwait(False)
        Catch ex As Exception When (TypeOf ex Is HttpRequestException OrElse TypeOf ex Is OperationCanceledException) AndAlso Not cancellationToken.IsCancellationRequested
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Sends a request and retries it when GitHub answers with a temporary error (429, 5xx, or 403 with
    ''' <c>Retry-After</c>) or the connection fails. Waits 2, then 4 seconds, or what <c>Retry-After</c> asks for
    ''' when that is short. The last response is returned as is, so the caller can report its status code.
    ''' </summary>
    ''' <param name="send">Sends a new request each time (a request message cannot be sent twice).</param>
    ''' <param name="cancellationToken">Cancels the request and the waits between attempts.</param>
    Private Shared Async Function SendWithRetryAsync(send As Func(Of CancellationToken, Task(Of HttpResponseMessage)),
                                                     cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Dim attempt = 0
        Do
            attempt += 1
            Dim isLastAttempt = attempt >= MaxAttempts
            Dim response As HttpResponseMessage = Nothing
            Try
                response = Await send(cancellationToken).ConfigureAwait(False)
            Catch ex As HttpRequestException When Not isLastAttempt AndAlso Not cancellationToken.IsCancellationRequested
                ' Connection problem: wait and try again below.
            End Try

            Dim delay = TimeSpan.FromSeconds(2 ^ attempt)
            If response IsNot Nothing Then
                If response.IsSuccessStatusCode OrElse isLastAttempt OrElse Not IsTemporary(response) Then Return response
                Dim requested = GetRetryAfter(response)
                If requested.HasValue Then
                    If requested.Value > MaxRetryWait Then Return response
                    If requested.Value > delay Then delay = requested.Value
                End If
                response.Dispose()
            End If
            Await Task.Delay(delay, cancellationToken).ConfigureAwait(False)
        Loop
    End Function

    ''' <summary><c>True</c> for answers that usually go away by waiting a little.</summary>
    Private Shared Function IsTemporary(response As HttpResponseMessage) As Boolean
        Dim code = CInt(response.StatusCode)
        Return code = 429 OrElse code >= 500 OrElse (code = 403 AndAlso response.Headers.RetryAfter IsNot Nothing)
    End Function

    ''' <summary>How long GitHub asks to wait (<c>Retry-After</c> header), if it says so.</summary>
    Private Shared Function GetRetryAfter(response As HttpResponseMessage) As TimeSpan?
        Dim header = response.Headers.RetryAfter
        If header Is Nothing Then Return Nothing
        If header.Delta.HasValue Then Return header.Delta.Value
        If header.Date.HasValue Then
            Dim wait = header.Date.Value - DateTimeOffset.UtcNow
            Return If(wait < TimeSpan.Zero, TimeSpan.Zero, wait)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Throws a <see cref="GitHubHttpException"/> that keeps the status code, the URL and the requested wait
    ''' when the response is not a success.
    ''' </summary>
    Private Shared Sub EnsureSuccess(response As HttpResponseMessage, url As String)
        If response.IsSuccessStatusCode Then Return
        Throw New GitHubHttpException(response.StatusCode, response.ReasonPhrase, url, GetRetryAfter(response))
    End Sub

    ''' <summary>
    ''' Fails before downloading when the destination drive is too small. Network drives are not checked,
    ''' because their free space cannot be read reliably.
    ''' </summary>
    Private Shared Sub EnsureFreeSpace(filePath As String, requiredBytes As Long)
        Dim available As Long
        Try
            Dim root = Path.GetPathRoot(Path.GetFullPath(filePath))
            If String.IsNullOrEmpty(root) OrElse root.StartsWith("\\", StringComparison.Ordinal) Then Return
            available = New DriveInfo(root).AvailableFreeSpace
        Catch ex As Exception When TypeOf ex Is ArgumentException OrElse TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return
        End Try

        If available < requiredBytes Then
            Throw New IOException($"Not enough free disk space: about {requiredBytes \ BytesPerMB:N0} MB are needed and {available \ BytesPerMB:N0} MB are available.")
        End If
    End Sub

    ''' <summary>
    ''' Creates an HTTP client. Downloads can take minutes, so the client itself has no timeout;
    ''' small requests apply <see cref="RequestTimeout"/> individually.
    ''' </summary>
    Private Shared Function CreateClient(followRedirects As Boolean, userAgent As String) As HttpClient
        Dim handler As New HttpClientHandler With {.AllowAutoRedirect = followRedirects}
        Dim client As New HttpClient(handler) With {.Timeout = Timeout.InfiniteTimeSpan}
        client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent)
        Return client
    End Function
End Class
