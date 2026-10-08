Imports System
Imports System.Globalization
Imports System.IO
Imports System.Text

''' <summary>
''' Persistent log in <c>fldata\launcher.log</c>. Every line shown in the main window is also appended here,
''' so users can send it when they report a problem. The file is trimmed when it grows beyond
''' <see cref="MaxSizeBytes"/>, keeping only the most recent part.
''' </summary>
Public NotInheritable Class LauncherLog
    ''' <summary>Size above which the log is trimmed when the launcher starts.</summary>
    Public Const MaxSizeBytes As Long = 1024L * 1024L

    ''' <summary>Amount of the most recent log kept after trimming.</summary>
    Private Const KeepBytesAfterTrim As Integer = 512 * 1024

    Private Shared ReadOnly FileEncoding As New UTF8Encoding(False)
    Private ReadOnly _filePath As String
    Private _writable As Boolean = True

    ''' <summary>Opens (or creates) the log file and trims it when it is too large.</summary>
    ''' <param name="filePath">Full path of <c>launcher.log</c>.</param>
    Public Sub New(filePath As String)
        _filePath = filePath
        TrimIfNeeded()
    End Sub

    ''' <summary>Full path of the log file.</summary>
    Public ReadOnly Property FilePath As String
        Get
            Return _filePath
        End Get
    End Property

    ''' <summary>Writes a separator line that marks the start of a new launcher session.</summary>
    ''' <param name="title">Text shown in the separator, typically the launcher version.</param>
    Public Sub StartSession(title As String)
        Append($"{Environment.NewLine}===== {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} · {title} ====={Environment.NewLine}")
    End Sub

    ''' <summary>Appends one timestamped line.</summary>
    ''' <param name="message">Text to write.</param>
    Public Sub Write(message As String)
        Append($"[{DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture)}] {message}{Environment.NewLine}")
    End Sub

    ''' <summary>Returns the last lines of the log, used by "Report a problem".</summary>
    ''' <param name="maxLines">Maximum number of lines to return.</param>
    Public Function ReadTail(maxLines As Integer) As String
        Try
            If Not File.Exists(_filePath) Then Return String.Empty
            Dim lines = File.ReadAllLines(_filePath, FileEncoding)
            Dim first = Math.Max(0, lines.Length - maxLines)
            Return String.Join(Environment.NewLine, lines, first, lines.Length - first)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Appends text to the file. Logging must never break the launcher, so when the folder is not writable
    ''' the log silently stops writing for the rest of the session.
    ''' </summary>
    Private Sub Append(text As String)
        If Not _writable Then Return
        Try
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath))
            File.AppendAllText(_filePath, text, FileEncoding)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            _writable = False
        End Try
    End Sub

    ''' <summary>
    ''' Keeps the log small: when it is larger than <see cref="MaxSizeBytes"/>, only the last
    ''' <see cref="KeepBytesAfterTrim"/> bytes are kept, starting at a line break so no line is cut in half.
    ''' </summary>
    Private Sub TrimIfNeeded()
        Try
            Dim info As New FileInfo(_filePath)
            If Not info.Exists OrElse info.Length <= MaxSizeBytes Then Return

            Dim bytes = File.ReadAllBytes(_filePath)
            Dim start = bytes.Length - KeepBytesAfterTrim
            Do While start < bytes.Length AndAlso bytes(start) <> 10
                start += 1
            Loop
            If start >= bytes.Length - 1 Then
                File.WriteAllBytes(_filePath, Array.Empty(Of Byte)())
                Return
            End If
            Dim tail(bytes.Length - start - 2) As Byte
            Array.Copy(bytes, start + 1, tail, 0, tail.Length)
            File.WriteAllBytes(_filePath, tail)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
        End Try
    End Sub
End Class
