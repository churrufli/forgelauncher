

Public Class vars
    Public Shared MyDll As String = "ICSharpCode.SharpZipLib.dll"
    Public Shared ServerLogName As String = "updates.txt"
    Public Shared MyLogServer As String = ""
    Public Shared LogName As String = "fldata/version.txt"
    Public Shared UserDir As String = Directory.GetCurrentDirectory()
    Public Shared SnapshotUrl As String = "https://downloads.cardforge.org/dailysnapshots/"
    Public Shared url_release As String = "https://releases.cardforge.org/forge/forge-gui-desktop/"
    Public Shared LinkLine As String = ""
    Public Shared TxtError As String
    Public Shared txlogserver As String
    Public Shared InitAll As Boolean = True
    Public Shared ForgeDecksDir, ForgePicsDir As String
    Public Shared continueLooping As Boolean = True
    Public Shared BaseUrl As String = "http://forgedecks.000webhostapp.com/"
End Class