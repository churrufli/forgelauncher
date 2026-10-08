Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>
''' Shows download and installation progress on the launcher's taskbar button (Windows 7 and later), so it can be
''' followed while the window is minimized. Every failure is ignored: the progress bar is a convenience only.
''' </summary>
Friend NotInheritable Class TaskbarProgress
    ''' <summary>Taskbar progress states defined by Windows.</summary>
    Private Enum ProgressState
        NoProgress = 0
        Indeterminate = 1
        Normal = 2
        [Error] = 4
    End Enum

    ''' <summary>The first methods of the Windows ITaskbarList3 interface, in their exact order.</summary>
    <ComImport(), Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface ITaskbarList3
        Sub HrInit()
        Sub AddTab(hwnd As IntPtr)
        Sub DeleteTab(hwnd As IntPtr)
        Sub ActivateTab(hwnd As IntPtr)
        Sub SetActiveAlt(hwnd As IntPtr)
        Sub MarkFullscreenWindow(hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> fullscreen As Boolean)
        Sub SetProgressValue(hwnd As IntPtr, completed As ULong, total As ULong)
        Sub SetProgressState(hwnd As IntPtr, state As ProgressState)
    End Interface

    Private Shared _taskbar As ITaskbarList3
    Private Shared _unavailable As Boolean

    Private Sub New()
    End Sub

    ''' <summary>Shows a percentage on the window's taskbar button.</summary>
    ''' <param name="form">Window whose taskbar button shows the progress.</param>
    ''' <param name="percent">Completed percentage (0-100).</param>
    Public Shared Sub SetValue(form As Form, percent As Integer)
        Invoke(form, Sub(taskbar, handle)
                         taskbar.SetProgressState(handle, ProgressState.Normal)
                         taskbar.SetProgressValue(handle, CULng(Math.Max(0, Math.Min(100, percent))), 100UL)
                     End Sub)
    End Sub

    ''' <summary>Shows the taskbar button in red, used when an installation fails.</summary>
    Public Shared Sub SetError(form As Form)
        Invoke(form, Sub(taskbar, handle) taskbar.SetProgressState(handle, ProgressState.Error))
    End Sub

    ''' <summary>Removes the progress from the taskbar button.</summary>
    Public Shared Sub Clear(form As Form)
        Invoke(form, Sub(taskbar, handle) taskbar.SetProgressState(handle, ProgressState.NoProgress))
    End Sub

    ''' <summary>Creates the taskbar object on first use and runs an action on it, ignoring any failure.</summary>
    Private Shared Sub Invoke(form As Form, action As Action(Of ITaskbarList3, IntPtr))
        If _unavailable OrElse Not form.IsHandleCreated Then Return
        Try
            If _taskbar Is Nothing Then
                Dim taskbarType = Type.GetTypeFromCLSID(New Guid("56FDF344-FD6D-11d0-958A-006097C9A090"))
                _taskbar = DirectCast(Activator.CreateInstance(taskbarType), ITaskbarList3)
                _taskbar.HrInit()
            End If
            action(_taskbar, form.Handle)
        Catch ex As Exception When TypeOf ex Is COMException OrElse TypeOf ex Is InvalidCastException OrElse
                                   TypeOf ex Is ArgumentException OrElse TypeOf ex Is NotSupportedException OrElse
                                   TypeOf ex Is NotImplementedException OrElse TypeOf ex Is TypeLoadException
            _unavailable = True
        End Try
    End Sub
End Class
