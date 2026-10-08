Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Http

''' <summary>
''' Turns technical exceptions into short messages that tell the user what happened and what to do,
''' and decides whether offering "Retry" makes sense.
''' </summary>
Public NotInheritable Class FriendlyErrors
    ''' <summary>Windows error code for a file locked by another program.</summary>
    Private Const SharingViolation As Integer = &H80070020
    ''' <summary>Windows error code for a file region locked by another program.</summary>
    Private Const LockViolation As Integer = &H80070021
    ''' <summary>Windows error code for a full disk.</summary>
    Private Const DiskFull As Integer = &H80070070
    ''' <summary>Windows error code for "end of file / disk full" during writes.</summary>
    Private Const HandleDiskFull As Integer = &H80070027

    Private Sub New()
    End Sub

    ''' <summary>Returns a message that explains the error and suggests a solution.</summary>
    ''' <param name="ex">The exception to describe.</param>
    Public Shared Function Describe(ex As Exception) As String
        If TypeOf ex Is OperationCanceledException Then
            Return "GitHub didn't respond in time. Check your internet connection and try again."
        End If

        Dim web = FindInner(Of WebException)(ex)
        If web IsNot Nothing Then
            Select Case web.Status
                Case WebExceptionStatus.NameResolutionFailure, WebExceptionStatus.ConnectFailure,
                     WebExceptionStatus.ProxyNameResolutionFailure, WebExceptionStatus.ConnectionClosed,
                     WebExceptionStatus.ReceiveFailure, WebExceptionStatus.SendFailure
                    Return "Couldn't reach GitHub. Check your internet connection and try again."
                Case WebExceptionStatus.SecureChannelFailure, WebExceptionStatus.TrustFailure
                    Return "A secure connection to GitHub couldn't be established. Check that your computer's date and time are correct, or your antivirus or proxy settings."
                Case WebExceptionStatus.Timeout
                    Return "GitHub didn't respond in time. Check your internet connection and try again."
            End Select
        End If

        If TypeOf ex Is HttpRequestException AndAlso ex.Message.Contains("404") Then
            Return "The file wasn't found on GitHub. A new version may still be uploading; try again in a few minutes."
        End If
        If TypeOf ex Is HttpRequestException Then
            Return "GitHub returned an error. Try again in a few minutes."
        End If

        If TypeOf ex Is UnauthorizedAccessException Then
            Return "Forge Launcher can't write in this folder. Move Forge to a folder you own (not Program Files) and try again."
        End If

        If TypeOf ex Is IOException Then
            If ex.Message.StartsWith("Not enough free disk space", StringComparison.Ordinal) Then Return ex.Message
            Select Case ex.HResult
                Case SharingViolation, LockViolation
                    Return "A Forge file is in use. Close Forge (and Forge Adventure) and try again."
                Case DiskFull, HandleDiskFull
                    Return "There isn't enough free disk space. Free some space and try again."
            End Select
        End If

        If TypeOf ex Is InvalidDataException OrElse TypeOf ex Is ICSharpCode.SharpZipLib.SharpZipBaseException Then
            Return "The downloaded package is damaged. Try again; if it keeps happening, the file on GitHub may be incomplete."
        End If

        Return ex.GetBaseException().Message
    End Function

    ''' <summary>
    ''' <c>True</c> for problems that can go away by trying again: network errors, timeouts, files still uploading
    ''' and damaged downloads.
    ''' </summary>
    ''' <param name="ex">The exception to classify.</param>
    Public Shared Function IsRetryable(ex As Exception) As Boolean
        Return TypeOf ex Is OperationCanceledException OrElse
               TypeOf ex Is HttpRequestException OrElse
               FindInner(Of WebException)(ex) IsNot Nothing OrElse
               TypeOf ex Is InvalidDataException OrElse
               TypeOf ex Is ICSharpCode.SharpZipLib.SharpZipBaseException OrElse
               (TypeOf ex Is IOException AndAlso (ex.HResult = SharingViolation OrElse ex.HResult = LockViolation OrElse
                                                  ex.Message.StartsWith("The download is incomplete", StringComparison.Ordinal)))
    End Function

    ''' <summary>Finds an exception of a given type in the chain of inner exceptions.</summary>
    Private Shared Function FindInner(Of T As Exception)(ex As Exception) As T
        Dim current = ex
        Do While current IsNot Nothing
            Dim match = TryCast(current, T)
            If match IsNot Nothing Then Return match
            current = current.InnerException
        Loop
        Return Nothing
    End Function
End Class
