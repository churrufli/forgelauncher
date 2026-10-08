Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

''' <summary>
''' Entry point of Forge Launcher. It prepares the process, makes sure only one launcher runs per Forge folder and
''' opens the main window.
''' </summary>
Friend Module Program
    Private Const AppTitle As String = "Forge Launcher"
    Private Const SwRestore As Integer = 9

    ''' <summary>Starts the launcher.</summary>
    ''' <param name="args">
    ''' Command line. <c>--after-update &lt;process id&gt;</c> is passed by the previous version after it updated the
    ''' launcher, so this instance waits until that process has exited.
    ''' </param>
    <STAThread>
    Friend Sub Main(args As String())
        ' Windows 7 and 8.1 may not enable TLS 1.2 by default, and GitHub requires it. Windows 10 and later negotiate
        ' the best protocol (including TLS 1.3) themselves, so they are left alone.
        If Environment.OSVersion.Version.Major < 10 Then
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol Or SecurityProtocolType.Tls12
        End If

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
        AddHandler Application.ThreadException, AddressOf OnThreadException

        Dim paths As New LauncherPaths(Path.GetDirectoryName(Application.ExecutablePath), Application.ExecutablePath)
        Directory.SetCurrentDirectory(paths.ForgeFolder)

        If Not File.Exists(paths.ZipLibrary) Then
            ShowError($"{LauncherPaths.ZipLibraryFileName} must be in the same folder as Forge Launcher.")
            Return
        End If

        Dim previousProcessId = ParseAfterUpdate(args)
        If previousProcessId > 0 Then SelfUpdater.WaitForPreviousProcess(previousProcessId)

        Using instanceLock As New Mutex(False, InstanceName(paths.ForgeFolder))
            If Not TryAcquire(instanceLock, If(previousProcessId > 0, 5000, 0)) Then
                ' Another launcher already manages this folder: bring its window to the front instead of showing an error.
                If Not BringExistingWindowToFront(paths.LauncherExe) Then ShowError("Forge Launcher is already running for this folder.")
                Return
            End If
            Try
                Application.Run(New MainForm(paths, previousProcessId > 0))
            Finally
                instanceLock.ReleaseMutex()
            End Try
        End Using
    End Sub

    ''' <summary>Reads the process id that follows <c>--after-update</c>, or returns 0.</summary>
    Private Function ParseAfterUpdate(args As String()) As Integer
        For index = 0 To args.Length - 2
            If String.Equals(args(index), SelfUpdater.AfterUpdateSwitch, StringComparison.OrdinalIgnoreCase) Then
                Dim processId As Integer
                If Integer.TryParse(args(index + 1), NumberStyles.Integer, CultureInfo.InvariantCulture, processId) Then Return processId
            End If
        Next
        Return 0
    End Function

    ''' <summary>
    ''' Tries to become the only launcher for this folder. A lock left by a launcher that crashed counts as acquired.
    ''' </summary>
    Private Function TryAcquire(instanceLock As Mutex, timeoutMilliseconds As Integer) As Boolean
        Try
            Return instanceLock.WaitOne(timeoutMilliseconds, False)
        Catch ex As AbandonedMutexException
            Return True
        End Try
    End Function

    ''' <summary>
    ''' Name of the lock shared by launchers of the same Forge folder. Folder paths can be longer than lock names
    ''' allow and contain invalid characters, so a hash of the path is used.
    ''' </summary>
    Private Function InstanceName(folder As String) As String
        Using sha = SHA256.Create()
            Dim hash = sha.ComputeHash(Encoding.UTF8.GetBytes(folder.ToUpperInvariant()))
            Return "ForgeLauncher-" & BitConverter.ToString(hash, 0, 12).Replace("-", String.Empty)
        End Using
    End Function

    ''' <summary>Finds the window of the launcher already running from the same executable and activates it.</summary>
    ''' <returns><c>True</c> when the window was found.</returns>
    Private Function BringExistingWindowToFront(exePath As String) As Boolean
        Dim currentId = Process.GetCurrentProcess().Id
        For Each candidate In Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exePath))
            Using candidate
                Try
                    If candidate.Id = currentId OrElse candidate.MainWindowHandle = IntPtr.Zero Then Continue For
                    If Not String.Equals(candidate.MainModule.FileName, exePath, StringComparison.OrdinalIgnoreCase) Then Continue For
                    If NativeMethods.IsIconic(candidate.MainWindowHandle) Then NativeMethods.ShowWindow(candidate.MainWindowHandle, SwRestore)
                    NativeMethods.SetForegroundWindow(candidate.MainWindowHandle)
                    Return True
                Catch ex As Exception When TypeOf ex Is ComponentModel.Win32Exception OrElse TypeOf ex Is InvalidOperationException
                End Try
            End Using
        Next
        Return False
    End Function

    ''' <summary>Last-resort handler: shows unexpected errors instead of letting the launcher crash silently.</summary>
    Private Sub OnThreadException(sender As Object, e As ThreadExceptionEventArgs)
        ShowError($"Unexpected error:{Environment.NewLine}{Environment.NewLine}{e.Exception.GetBaseException().Message}")
    End Sub

    ''' <summary>Shows an error before or outside the main window.</summary>
    Private Sub ShowError(message As String)
        DarkDialog.ShowMessage(Nothing, DialogKind.Error, AppTitle, message)
    End Sub

    ''' <summary>Windows functions used to activate an existing window.</summary>
    Private NotInheritable Class NativeMethods
        <DllImport("user32.dll")>
        Public Shared Function SetForegroundWindow(hwnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll")>
        Public Shared Function ShowWindow(hwnd As IntPtr, command As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll")>
        Public Shared Function IsIconic(hwnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function
    End Class
End Module
