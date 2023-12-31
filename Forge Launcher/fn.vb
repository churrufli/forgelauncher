﻿Imports System.ComponentModel
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports ICSharpCode.SharpZipLib.BZip2
Imports ICSharpCode.SharpZipLib.Tar
Imports ICSharpCode.SharpZipLib.Zip

Public Class fn
    Shared WithEvents downloader As WebClient

    Public Shared Function CheckAddress(URL As String) As Boolean
        Try
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Shared Sub HitToLauncherUpdates(Optional ByVal forzar As Boolean = False)
        Try
            'compruebo sí existe
            If File.Exists(vars.UserDir & "\fldata\" & vars.ServerLogName) Then
                'si, compruebo ultima fecha
                Dim myLastDate = ReadLogUser("lastupdate")
                If myLastDate <> DateTime.Now.ToString("dd'/'MM'/'yyyy") Then
                    forzar = True
                End If
                forzar = True

                If forzar Then
                    If CheckAddress(vars.BaseUrl & vars.ServerLogName) Then
                        Try
                            File.Delete(vars.UserDir & "\fldata\" & vars.ServerLogName)
                        Catch
                        End Try
                        'fn.WriteUserLog("Checking for updates..." & vbCrLf)
                        'descargar y actualizar
                        DownloadFile(vars.BaseUrl & vars.ServerLogName, vars.UserDir & "\fldata\" & vars.ServerLogName)
                        'UpdateLog("lastupdate", DateTime.Now.ToString("dd'/'MM'/'yyyy"))
                    End If

                End If
            Else
                'no existe
                If CheckAddress(vars.BaseUrl & vars.ServerLogName) Then
                    DownloadFile(vars.BaseUrl & vars.ServerLogName, vars.UserDir & "\fldata\" & vars.ServerLogName)
                End If
            End If

            Try
                vars.MyLogServer = File.ReadAllText(vars.UserDir & "\fldata\" & vars.ServerLogName).ToString & ""
            Catch ex As Exception
            End Try

            If vars.MyLogServer = "" Then
                Try
                    vars.MyLogServer = File.ReadAllText(vars.UserDir & "\fldata\" & vars.ServerLogName)
                Catch ex As Exception

                End Try
            End If
        Catch
        End Try
    End Sub

    Public Shared Sub CheckLauncherUpdates()
        Try
            WriteUserLog("Checking for Launcher updates..." & vbCrLf)

            Dim logu, logse As String
            logu = ReadLogUser("launcher_version", False, True)
            logse = ReadLogServer("launcher_version", False)
            If logse = "" Then Exit Sub

            If logu <> logse Then
                If _
                    MsgBox("Forge Launcher New version Available. Update now?", MsgBoxStyle.YesNoCancel, logse) =
                    MsgBoxResult.Yes Then
                    Try
                        WriteUserLog("Downloading " & logse & vbCrLf)
                        DownloadFile(vars.BaseUrl & "Forge Launcher.zip", "Forge Launcher.zip")
                        WriteUserLog("Unpacking " & logse & " In " & Directory.GetCurrentDirectory() & "..." & vbCrLf)
                        UnzipFile(Directory.GetCurrentDirectory() & "/" & "Forge Launcher.zip",
                                  Directory.GetCurrentDirectory() & "/fltmp")
                        'UpdateLog("showwhatsnew", "yes")
                        File.Delete("Forge Launcher.zip")
                        File.Move("Forge Launcher.exe", "fltmp/Forge Launcher.bak")
                        File.Copy("fltmp/Forge Launcher.exe", "Forge Launcher.exe")
                        Try
                            File.Copy("fltmp/ICSharpCode.SharpZipLib.dll", "ICSharpCode.SharpZipLib.dll")
                        Catch

                        End Try
                    Catch
                    End Try

                    Try
                        File.Delete("fldata/fl_whatsnew.txt")
                    Catch
                    End Try
                    Try
                        UpdateLog("launcher_version", logse)
                    Catch
                    End Try

                    Application.Restart()
                End If
            Else
                WriteUserLog("Your Launcher is up to date:" & logu & vbCrLf)
            End If
        Catch

        End Try
    End Sub


    Public Shared Function ReadLogServer(idlog As String, Optional ShowMsg As Boolean = True)
        Try
            DownloadFile(vars.BaseUrl & vars.ServerLogName, "fldata/" & vars.ServerLogName)
        Catch
            PrintError(Err.Description)
            Exit Function
        End Try
        'End If

        Dim LogServer_data = ""

        Try
            LogServer_data = File.ReadAllText("fldata\" & vars.ServerLogName).ToString
        Catch
            PrintError(Err.Description)
            Exit Function
        End Try

        Try
            LogServer_data = FindIt(LogServer_data, "<" & idlog & ">", "</" & idlog & ">")
        Catch
            PrintError(Err.Description)
            Exit Function
        End Try
        ReadLogServer = LogServer_data
    End Function

    Public Shared Sub DeleteDownloaded()
        Try
            File.Delete("fldata/updates.txt")
        Catch
        End Try
        Try
            Directory.Delete("fltmp", True)
        Catch
        End Try
        Try
            Dim files =
                    Directory.GetFiles(Environment.CurrentDirectory, "forge*", SearchOption.AllDirectories).Where(
                        Function(s) s.EndsWith(".tar.bz") OrElse s.EndsWith(".bz2") OrElse s.EndsWith(".tar"))
            For Each Foundedfile As String In files
                File.Delete(Foundedfile)
            Next
        Catch ex As Exception
            ex = ex
        End Try
    End Sub

    Public Shared Sub DownloadFile(address As String, fileName As String, Optional force_download As Boolean = False)
        If File.Exists(fileName) And force_download = False Then Exit Sub
        Try
            Dim instance As New WebClient
            If File.Exists(fileName) Then File.Delete(fileName)
            instance.DownloadFile(address, fileName)
        Catch
            PrintError(Err.Description)
        End Try
    End Sub

    Public Shared Function ReadLogUser(idlog As String, Optional ShowMsg As Boolean = False,
                                       Optional ByVal CompareWithServer As Boolean = True) As String
        If idlog = "profileproperties" Then CompareWithServer = False

        If CompareWithServer Then
            'If File.Exists("\fldata\" & vars.ServerLogName) = False Then
            '    Try
            '        DownloadFile(vars.BaseUrl & vars.ServerLogName, vars.UserDir & "/fldata/" & vars.ServerLogName)
            '    Catch
            '        PrintError(Err.Description)
            '    End Try
            'End If
        End If

        Dim LogServer = ""
        Try
            LogServer = vars.MyLogServer
            LogServer = FindIt(LogServer, "<" & idlog & ">", "</" & idlog & ">")
        Catch
            PrintError(Err.Description)
        End Try

        If File.Exists(vars.LogName) = False Then
            Directory.CreateDirectory(Directory.GetCurrentDirectory() & "/fldata")

            File.Create(vars.LogName).Dispose()
        End If


        Dim ladire = vars.UserDir & "\" & vars.LogName
        ladire = Replace(ladire, "/", "\")
        ladire = Replace(ladire, "\", "/")
        Dim LogUser = ""
        Try
            LogUser = File.ReadAllText(ladire).ToString
        Catch
        End Try
        If LogUser = "" Then
            Try
                LogUser = File.ReadAllText(vars.UserDir & ladire).ToString
            Catch
            End Try
        End If
        If LogUser = "" Then
            Try
                LogUser = File.ReadAllText(Directory.GetCurrentDirectory & ladire).ToString
            Catch
            End Try
        End If

        Dim log_user As String = FindIt(LogUser, "<" & idlog & ">", "</" & idlog & ">")

        idlog = Replace(idlog, "_", " ")
        idlog = StrConv(idlog, VbStrConv.ProperCase)
        If ShowMsg Then
            If LogServer = "" Then
                Return ""
                Exit Function
            End If
            If LogServer <> log_user Then
                WriteUserLog("New " & idlog & " available! " & LogServer & "." & vbCrLf)
            Else
                WriteUserLog("Your " & idlog & " is up to date: " & log_user & "." & vbCrLf)
            End If
        End If
        ReadLogUser = log_user
    End Function

    Public Shared Function OpenLogFile()
        Dim logfile As String = Directory.GetCurrentDirectory & "\user\forge.log"
        Dim logfile2 As String = Directory.GetCurrentDirectory & "\UserDir\forge.log"

        If File.Exists(logfile) = True Then
            Shell("c:\windows\notepad.exe " & logfile)
            Exit Function
        End If

        If File.Exists(logfile2) = True Then
            Shell("c:\windows\notepad.exe " & logfile2)
            Exit Function
        End If

        MsgBox("I can't find forge.log file!.")
    End Function

    Public Shared Function UninstallForgeLauncher()
        Dim m As String
        m = "Are you sure to uninstall Forge Launcher and generated files? (Forge files will not be deleted)"
        If MsgBox(m, MsgBoxStyle.YesNoCancel, "Warning!") = MsgBoxResult.Yes Then
            Try
                File.Delete("fldata/fl_whatsnew.txt")
            Catch
            End Try
            Try
                File.Delete("fldata/version.txt")
            Catch
            End Try
            Try
                File.Delete("fldata/updates.txt")
            Catch
            End Try
            Try
                Uninstall()
            Catch ex As Exception
                WriteUserLog(ex.Message.ToString)
            End Try
        End If
    End Function

    Public Shared Sub Uninstall()
        Process.Start("cmd.exe", "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath)
        Dim p As New ProcessStartInfo("cmd.exe")
        p.Arguments = "/C choice /C Y /N /D Y /T 3 & Del  " & ControlChars.Quote & Application.ExecutablePath &
                      ControlChars.Quote
        p.CreateNoWindow = True
        p.ErrorDialog = False
        p.WindowStyle = ProcessWindowStyle.Hidden
        Process.Start(p)
        Application.Exit()
        Try
            Environment.Exit(1)
        Catch
        End Try
    End Sub

    Public Shared Sub RestoreForgePreferences()
        Dim m As String
        m =
            "If Forge does not load or freeze in the start screen, try restoring preferences (previous preferences will be removed) " &
            vbCrLf & " Are you sure?"
        If MsgBox(m, MsgBoxStyle.YesNoCancel, "Warning!") = MsgBoxResult.Yes Then
            Try
                Dim dir As String = Replace(ReadLogUser("decks_dir"), "decks", "preferences")
                File.Delete(dir & "/forge.preferences")
                MsgBox("Done!")
            Catch ex As Exception
                WriteUserLog(ex.Message.ToString)
            End Try
        End If
    End Sub

    Public Shared Sub UpdateLog(idlog, myvalue)
        Dim mylog As String = My.Computer.FileSystem.ReadAllText(vars.LogName)
        Dim previousmyvalue = FindIt(mylog, "<" & idlog & ">", "</" & idlog & ">")
        Dim newlog = Replace(mylog, "<" & idlog & ">" & previousmyvalue & "</" & idlog & ">",
                             "<" & idlog & ">" & myvalue & "</" & idlog & ">")
        Try
            File.Delete(vars.UserDir & "/" & vars.LogName)
        Catch
        End Try
        Try
            File.WriteAllText(vars.UserDir & "/" & vars.LogName, newlog)
        Catch
        End Try
    End Sub

    Public Shared Sub PrintError(tx)
        Try
            If InStr(vars.TxtError.ToString & "", tx, CompareMethod.Text) = 0 Then
                vars.TxtError = vars.TxtError & tx & vbCrLf
                WriteUserLog(vars.TxtError)
            End If
        Catch
        End Try
    End Sub

    'Public Shared Function ReadLogServer(idlog As String, Optional ShowMsg As Boolean = True, Optional newbaseurl As Boolean = False)
    '    Try
    '        If newbaseurl = False Then
    '            DownloadFile(vars.BaseUrl & vars.ServerLogName, "fldata/" & vars.ServerLogName)
    '        Else
    '            DownloadFile(vars.NewBaseUrl & vars.ServerLogName, "fldata/" & vars.ServerLogName)
    '        End If
    '    Catch
    '        PrintError(Err.Description)
    '        Exit Function
    '    End Try

    '    Dim LogServer_data = ""

    '    Try
    '        LogServer_data = File.ReadAllText("fldata\" & vars.ServerLogName).ToString
    '    Catch
    '        PrintError(Err.Description)
    '        Exit Function
    '    End Try

    '    Try
    '        LogServer_data = FindIt(LogServer_data, "<" & idlog & ">", "</" & idlog & ">")
    '    Catch
    '        PrintError(Err.Description)
    '        Exit Function
    '    End Try
    '    ReadLogServer = LogServer_data
    'End Function

    Public Shared Sub CheckIfICSharpCodeExist()
        If File.Exists(vars.MyDll) = False Then
            MsgBox("You need " & vars.MyDll & " in the same directory to extract .tar.bz2 files!")
            Application.Exit()
        End If
    End Sub

    Public Shared Sub ExtractToDirectory(archive As ZipArchive, destinationDirectoryName As String, overwrite As Boolean)
        Dim mycount As Integer
        If Not overwrite Then
            archive.ExtractToDirectory(destinationDirectoryName)
            Return
        End If
        For Each file As ZipArchiveEntry In archive.Entries
            Dim completeFileName As String = Path.Combine(destinationDirectoryName, file.FullName)
            If file.Name = "" Then
                Try
                    If Directory.Exists(Path.GetDirectoryName(completeFileName)) = False Then
                        Directory.CreateDirectory(Path.GetDirectoryName(completeFileName))
                    End If
                Catch
                End Try
                Continue For
            End If
            file.ExtractToFile(completeFileName, True)
            WriteUserLog("Extracting " & file.FullName & vbCrLf)
            mycount = mycount + 1
        Next
        WriteUserLog("Done!." & vbCrLf)
    End Sub

    Public Shared Function ReadWeb(MyUrl As String)
        Try
            Dim client As WebClient = New WebClient()
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim reply As String = client.DownloadString(MyUrl)
            Return reply
        Catch
        End Try
    End Function

    Public Shared Function GetCheckAutomatic()
        Dim LineLink = ""
        Dim alltext = ""
        Dim MyTx As String = ReadWeb("https://downloads.cardforge.org/dailysnapshots/")
        Dim mytxfind As String = MyTx
        Dim prelink As String = LineLink
        Dim abr() As String = Split(MyTx, Environment.NewLine)
        For Each MyLines As String In abr
            If InStr(MyLines, "tar.bz2", CompareMethod.Text) > 0 Then
                MyTx = Replace(MyLines, """", "'")
                MyTx = FindIt(MyTx, "<a href='", ".tar.bz2'>")
                LineLink = "https://downloads.cardforge.org/dailysnapshots/" & MyTx & ".tar.bz2"
            End If
        Next

        Dim mydate As String = FindIt(mytxfind, ".tar.bz2</a>", "</pre><hr></body>")
        mydate = Split(mydate, "           ")(0).ToString
        mydate = Trim(mydate)
        Dim l As String = FindIt(LineLink, "forge-gui-desktop-", ".tar")
        Return "Forge " & l & " " & mydate & "#" & LineLink
    End Function

    Public Shared Function CheckForgeVersion(Optional ShowMsg As Boolean = True, Optional ShowText As Boolean = False)
        Dim typeofupdate As String = ReadLogUser("typeofupdate")
        Select Case typeofupdate
            Case "snapshot"
                vars.LinkLine = GetCheckAutomatic()
            Case "release"
                vars.LinkLine = CheckRelease()
        End Select

        If vars.LinkLine = "" Then
            vars.LinkLine = CheckRelease()
        End If

        Dim mycompare = ""

        If vars.LinkLine <> "" Then

            If InStr(vars.LinkLine, "#") > 0 Then
                vars.LinkLine = Split(vars.LinkLine, "#")(0).ToString
            End If

            Select Case typeofupdate
                Case "release"
                    mycompare = ReadLogUser("forge_version", False)
                Case "snapshot"
                    mycompare = ReadLogUser("release_version", False)
                Case "spanish snapshot"
                    mycompare = ReadLogUser("other_version", False)
            End Select

        End If

        Dim myread, urltoshow As String

        Select Case typeofupdate
            Case "snapshot"
                myread = "forge_version"
                urltoshow = vars.SnapshotUrl
                urltoshow = "https://downloads.cardforge.org/dailysnapshots"
            Case "release"
                myread = "release_version"
                urltoshow = vars.url_release
        End Select

        If ShowMsg Or ShowText Then
            Dim UserVersion = ReadLogUser(myread, False)
            If UserVersion = Nothing Then UserVersion = ""
            Dim LocalVersion = mycompare
            Try
                If LocalVersion.Contains("#") Then LocalVersion = Split(LocalVersion, "#")(0)
            Catch
            End Try
            Dim x = typeofupdate & " "
            If Not IsNothing(UserVersion) Then
                If UserVersion.Contains("#") Then UserVersion = Split(UserVersion, "#")(0)
            End If
            If UserVersion <> vars.LinkLine Then
                If Trim(x) = "release" Then
                    WriteUserLog(
                        "Founded Forge release " & Replace(vars.LinkLine, urltoshow, "") & "." & vbCrLf &
                        "You're running " & UserVersion & vbCrLf)
                Else
                    WriteUserLog(
                        "New Forge " & x & "version available! " & Replace(vars.LinkLine, urltoshow, "") & "." & vbCrLf &
                        "You're running " & UserVersion & vbCrLf)
                End If

            Else
                If UserVersion.Contains("#") Then UserVersion = Split(UserVersion, "#")(0)
                If UserVersion <> Nothing And vars.LinkLine <> Nothing Then
                    WriteUserLog(
                        "Your Forge " & x & "version is up to date: " & Replace(vars.LinkLine, urltoshow, "") & "." &
                        vbCrLf)
                    If _
                        MsgBox(
                            "Your Forge " & x & "version is up to date." & vbCrLf &
                            "Do you want to start Forge and close Launcher?", MsgBoxStyle.YesNo, "Forge is up to date") =
                        MsgBoxResult.Yes Then
                        Launch()
                        Application.Exit()
                        Try
                            Environment.Exit(1)
                        Catch
                        End Try
                        Exit Function
                    End If
                End If
            End If
        Else
            If vars.LinkLine = "" Or vars.LinkLine = "not found" Then
                Return ""
                Exit Function
            End If

            Dim UserVersion = ReadLogUser("forge_version", False)
            If ShowMsg Or ShowText Then
                If UserVersion <> vars.LinkLine Then
                    WriteUserLog("Can't connect to downloads.cardforge.org/dailysnapshots!" & vbCrLf)
                Else
                    WriteUserLog(
                        "Your Forge version is up to date: " & Replace(vars.LinkLine, vars.SnapshotUrl, "") & "." &
                        vbCrLf)
                End If
            End If
        End If
        Return vars.LinkLine
    End Function

    Public Shared Function CheckRelease()
        Dim LinkLine = ""
        Dim alltext = ""
        Dim url_snap = "https://snapshots.cardforge.org/"
        Try

            Dim client2 = New WebClient()
            Dim reader2 = New StreamReader(client2.OpenRead(vars.url_release))

            alltext += reader2.ReadToEnd
            Dim alltext2() = Split(alltext, "  ")
            Dim betterdate As DateTime = DateTime.Now.AddYears(-1)

            For i = 0 To alltext2.Length - 1

                If IsDate(alltext2(i)) Then
                    If CDate(alltext2(i)) > CDate(betterdate) Then
                        betterdate = alltext2(i)
                    End If
                End If
            Next i

            Dim betterdatetxt = betterdate.ToString


            Dim ar() As String = Split(betterdatetxt, "/")
            Dim fdef = ""

            Dim mes = ""
            Select Case ar(1)
                Case "01"
                    mes = "Jan"
                Case "02"
                    mes = "Feb"
                Case "03"
                    mes = "Mar"
                Case "04"
                    mes = "Apr"
                Case "05"
                    mes = "May"
                Case "06"
                    mes = "Jun"
                Case "07"
                    mes = "Jul"
                Case "08"
                    mes = "Aug"
                Case "09"
                    mes = "Sep"
                Case "10"
                    mes = "Oct"
                Case "11"
                    mes = "Nov"
                Case "12"
                    mes = "Dec"
            End Select

            If Len(ar(0)) = 1 Then ar(0) = "0" & ar(0)
            Dim MyHour = ar(2)
            Dim MyYear = Split(ar(2), " ")(0)
            MyHour = Replace(MyHour, DateTime.Now.Year.ToString, "")
            MyHour = Replace(MyHour, DateTime.Now.AddYears(-1).ToString, "")
            MyHour = Replace(MyHour, DateTime.Now.AddYears(+1).ToString, "")
            MyHour = Replace(MyHour, "2020", "")
            MyHour = Trim(MyHour)

            Dim hora() = Split(MyHour, ":")
            For i = 0 To hora.Length - 1
                If Len(hora(i)) = 1 Then hora(i) = "0" & hora(i)
            Next i

            fdef = ar(0) & "-" & mes & "-" & MyYear & " " & hora(0) & ":" & hora(1)

            fdef = Replace(fdef, ":00 ", "")

            'File.WriteAllText("fltx.txt", alltext)

            Using reader3 As New StreamReader(alltext)

                While Not reader3.EndOfStream
                    Dim line As String = reader3.ReadLine()
                    If InStr(line.ToString, fdef) > 0 Then
                        LinkLine = line
                        Exit While
                    End If
                End While
            End Using
            'Try
            '    File.Delete("fltx.txt")

            'Catch ex As Exception

            'End Try
            LinkLine = Replace(LinkLine, """", "'")
            LinkLine = FindIt(LinkLine, "href='", "'>")
        Catch
        End Try

        If InStr(LinkLine, "tar.bz2", CompareMethod.Text) > 0 Then
            LinkLine = url_snap & LinkLine
            CheckRelease = LinkLine
            Exit Function
        Else
            Dim abrir As String = ReadWeb(vars.url_release & LinkLine)
            Dim prelink As String = LinkLine

            Dim abr() As String = Split(abrir, Environment.NewLine)
            For Each linea As String In abr
                If InStr(linea, "tar.bz2", CompareMethod.Text) > 0 Then
                    abrir = Replace(linea, """", "'")
                    abrir = FindIt(abrir, "<a href='", ".tar.bz2'>")
                    LinkLine = vars.url_release & prelink & abrir & ".tar.bz2"
                    Exit For
                End If
            Next
            CheckRelease = LinkLine
        End If
        CheckRelease = LinkLine
    End Function

    Public Shared Sub AlertAboutVersion(Optional ByVal ignorarigual = False)
        Dim leer As String
        Dim urltoshow As String
        Dim result = fl.typeofupdate.Text.ToString
        If result = "snapshot" Then
            leer = "forge_version"
            urltoshow = vars.SnapshotUrl
        Else
            leer = "release_version"
            urltoshow = vars.url_release
        End If
        Try
            Dim vs, vu As String
            vs = CheckForgeVersion(False, False)
            If vs = "" Then
                PrintError("Can't get last version.")
                Exit Sub
            End If
            Try
                vu = ReadLogUser(leer, False).ToString
            Catch
                vu = ""
            End Try

            If vu.Contains("#") Then
                    vu = Split(vu, "#")(0).ToString
                End If

                If Trim(vs) = Trim(vu) Then
                    If ignorarigual = False Then

                        If _
                        MsgBox(
                            "It's appears your version is up to date, Do you want to download again and reinstall it?",
                            MsgBoxStyle.YesNo, "Warning!") = MsgBoxResult.Yes Then
                            Dim link = Split(GetCheckAutomatic(), "#")(1)
                            UpdateForge(link)
                        End If
                    End If

                Else
                    If _
                    MsgBox("Do you want to install " & Replace(vs, urltoshow, "") & " in " & vars.UserDir & "?",
                           MsgBoxStyle.YesNoCancel, "Version Available") = MsgBoxResult.Yes Then
                        Dim link = Split(GetCheckAutomatic(), "#")(1)

                        UpdateForge(link)

                    End If
                End If
            Catch
                PrintError(Err.Description)
        End Try
    End Sub

    Public Shared Sub UpdateForge(vtoupdate)
        fl.vtoupdate.Text = vtoupdate
        fl.MenuGeneral.Enabled = False
        fl.GroupForgeOptions.Enabled = False
        If vtoupdate = "" Then vtoupdate = CheckForgeVersion(False)
        Dim urlcomplete = vtoupdate
        Dim myfile = Path.GetFileName(urlcomplete)
        If File.Exists(myfile) = True Then File.Delete(myfile)
        WriteUserLog(vbCrLf & "Downloading update... please wait!" & vbCrLf)
        If InStr(urlcomplete, "http") > 0 Then
        End If
        DownloadStart(urlcomplete, Path.GetFileName(urlcomplete))
    End Sub

    Public Shared Sub DownloadStart(dwl, fn)
        fl.ProgressBar1.Visible = True
        downloader = New WebClient
        downloader.DownloadFileAsync(New Uri(dwl), fn)
    End Sub

    Public Shared Sub downloader_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) _
        Handles downloader.DownloadProgressChanged
        fl.ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Public Shared Sub downloader_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) _
        Handles downloader.DownloadFileCompleted
        ContinueInstallingForge(fl.vtoupdate.Text)
    End Sub

    Public Shared Function FindIt(total As String, first As String, last As String) As String
        If last.Length < 1 Then
            FindIt = total.Substring(total.IndexOf(first))
        End If
        If first.Length < 1 Then
            FindIt = total.Substring(0, (total.IndexOf(last)))
        End If
        Try
            FindIt =
                ((total.Substring(total.IndexOf(first), (total.IndexOf(last) - total.IndexOf(first)))).Replace(first, "")) _
                    .Replace(last, "")
        Catch
        End Try
    End Function


    Public Shared Function SearchFolders(Optional ShowMsg As Boolean = True, Optional idlog As String = "decks_dir")

        Dim MyFolder As String = ReadLogUser(idlog, False)

        If MyFolder <> "" Then
            If ShowMsg Then
                WriteUserLog(
                    MyFolder & " -> user decks directory (You may change the directories in Settings)." & vbCrLf)
            End If
            Return MyFolder
            Exit Function
        End If

        MyFolder = ""
        Dim files() As String = Directory.GetFiles(vars.UserDir)
        Dim lines As String() = {}
        For Each file As String In files
            If file = vars.UserDir & "\forge.profile.properties" Then
                Dim text As String = IO.File.ReadAllText(vars.UserDir & "\forge.profile.properties")
                lines = IO.File.ReadAllLines(vars.UserDir & "\forge.profile.properties")
                For Each line As String In lines
                    If InStr(LCase(line.ToString), "UserDir") > 0 Then
                        Try
                            Dim PossibleDir = Split(line, "=")(1).ToString
                            If PossibleDir <> "" And Directory.Exists(PossibleDir) Then
                                If PossibleDir <> "" Then
                                    PossibleDir = PossibleDir & "\decks"
                                    If CheckIfForgeExists() Then
                                        If ShowMsg Then
                                            WriteUserLog(
                                                "Detected " & PossibleDir &
                                                " As Custom User Directory (You can change the directories In Settings)." &
                                                vbCrLf)
                                        End If
                                    End If
                                    MyFolder = PossibleDir
                                    SearchFolders = PossibleDir
                                    Exit Function
                                End If
                            End If
                        Catch
                        End Try
                    End If
                Next
            End If
        Next

        'MyFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Forge\decks"

        'If CheckIfForgeExists() Then
        '    If ShowMsg Then
        '        WriteUserLog("Detected " & MyFolder & " As Content Directory." & vbCrLf)
        '    End If
        'End If

        'SearchFolders = MyFolder

        'If MyFolder <> "" Then
        '    UpdateLog("decks_dir", MyFolder)
        'End If
    End Function

    Public Shared Function CheckIfForgeExists()
        Return IIf(File.Exists("forge.exe"), True, False)
    End Function

    Public Shared Sub CheckIfPreviousProfileProperties()
        Try
            Dim aa As String = ReadLogUser("profileproperties", False).ToString
            If aa.ToString <> (IIf(File.Exists("forge.profile.properties"), "yes", "no")).ToString Then
                UpdateLog("profileproperties", IIf(File.Exists("forge.profile.properties"), "yes", "no"))
                UpdateLog("decks_dir", Directory.GetCurrentDirectory() & "\user\decks")
            End If
        Catch
        End Try
    End Sub

    Public Shared Sub RewriteLog()
        Try
            Dim readText As String = File.ReadAllText(vars.UserDir & "/" & vars.LogName)
            Dim WriteLog = False
            WriteLog = True
            If WriteLog Then
                Try
                    File.Delete(vars.LogName)
                Catch
                End Try
                File.WriteAllText(vars.LogName, readText)
            End If
        Catch
        End Try
    End Sub

    Public Shared Sub CheckLog()
        'Try
        Dim hoy As String = DateTime.Now.ToString("dd'/'MM'/'yyyy")
        Dim existpp As String = IIf(File.Exists("forge.profile.properties"), "yes", "no")
        If File.Exists(vars.LogName) = False Then
            Dim t As String
            t = "<forge_version>Not found</forge_version>" & vbCrLf
            t = "<release_version>Not found</release_version>" & vbCrLf
            t = "<other_version>Not found</other_version>" & vbCrLf
            t = "<forge_previous_version></forge_previous_version>" & vbCrLf
            t = t & "<profileproperties>" & existpp & "</profileproperties>" & vbCrLf
            t = t & "<lastupdate>" & hoy & "</lastupdate>" & vbCrLf
            t = t & "<removepreviousjarfiles>yes</removepreviousjarfiles>" & vbCrLf
            t = t & "<typeofupdate>snapshot</typeofupdate>" & vbCrLf
            t = t & "<launchmode>normal</launchmode>" & vbCrLf
            t = t & "<launchline></launchline>" & vbCrLf
            If File.Exists(vars.LogName) = False Then
                If Directory.Exists(Directory.GetCurrentDirectory() & "\fldata") = False Then
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() & "\fldata")
                End If
                File.Create(vars.LogName).Dispose()
            End If
            File.WriteAllText(vars.LogName, t)
        Else
            Dim readText As String = File.ReadAllText(vars.UserDir & "\" & vars.LogName)
            Dim WriteLog = False

            'Dim tx = Split(readText, vbCrLf)
            'Dim FinalResult = ""
            '    Dim MyCount = 0
            '    For i = 0 To tx.Length - 1

            '        If tx(i) = " < CheckLauncherUpdates() > no</checklauncherupdates>" Then
            '            MyCount = MyCount + 1
            '            If MyCount > 1 Then
            '                tx(i) = ""
            '            End If
            '        End If
            '        If tx(i) = "<checklauncherupdates>yes</checklauncherupdates>" Then
            '            MyCount = MyCount + 1
            '            If MyCount > 1 Then
            '                tx(i) = ""
            '            End If
            '        End If

            '        If tx(i) <> "" Then
            '            FinalResult = FinalResult & tx(i) & vbCrLf
            '        End If
            '    Next


            'readText = FinalResult

            If InStr(readText, "<forge_version>", CompareMethod.Text) = 0 Then
                readText = readText & "<forge_version>Not found</forge_version>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<release_version>", CompareMethod.Text) = 0 Then
                readText = readText & "<release_version>Not found</release_version>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<other_version>", CompareMethod.Text) = 0 Then
                readText = readText & "<other_version>Not found</other_version>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<forge_previous_version>", CompareMethod.Text) = 0 Then
                readText = readText & "<forge_previous_version></forge_previous_version>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<launcher_version>", CompareMethod.Text) = 0 Then
                readText = readText & "<launcher_version>Forge Launcher V.1.1</launcher_version>" &
                           Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<profileproperties>", CompareMethod.Text) = 0 Then
                readText = readText & "<profileproperties>" & existpp & "</profileproperties>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<removepreviousjarfiles>", CompareMethod.Text) = 0 Then
                readText = readText & "<removepreviousjarfiles>yes</removepreviousjarfiles>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<typeofupdate>", CompareMethod.Text) = 0 Then
                readText = readText & "<typeofupdate>snapshot</typeofupdate>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, "<launchmode>", CompareMethod.Text) = 0 Then
                readText = readText & "<launchmode>normal</launchmode>" & Environment.NewLine
                WriteLog = True
            End If


            If InStr(readText, "<launchline>", CompareMethod.Text) = 0 Then
                readText = readText & "<launchline></launchline>" & Environment.NewLine
                WriteLog = True
            End If

            If InStr(readText, Environment.NewLine & Environment.NewLine, CompareMethod.Text) > 0 Then
                readText = Replace(readText, Environment.NewLine & Environment.NewLine, Environment.NewLine)
                WriteLog = True
            End If

            If WriteLog Then
                Try
                    File.Delete(vars.LogName)
                Catch
                End Try
                File.WriteAllText(vars.LogName, readText)
            End If
        End If
    End Sub


    Public Shared Function UnzipFile(sZipFile As String, sDestPath As String) As Boolean
        Dim bResult = False
        On Error GoTo Problem
        'Determine the archive type
        Select Case Path.GetExtension(sZipFile)
            Case ".zip"
                'If it's a zip, then simply extract the contents
                Dim fZ = New FastZip()
                fZ.ExtractZip(sZipFile, sDestPath, "") '//use "" for File Filter if you want all files
            Case ".bz2"
                'If it's a tarball, then first decompress the tar, then extract the tar's contents

                'Get the filename of the resulting tar file
                Dim sTarFileName As String = Path.GetDirectoryName(sZipFile) & "\" &
                                             Path.GetFileNameWithoutExtension(sZipFile)
                'Create a new file stream for the tar ball
                Dim fsIn As FileStream = File.OpenRead(sZipFile)

                'If the resulting tar file exists alreay, delete it before creating it
                If File.Exists(sTarFileName) Then
                    File.Delete(sTarFileName)
                End If

                'Create a file stream to receive the decompressed tar file
                Dim fsOut As FileStream = File.Create(sTarFileName)

                'Perform the decompression of the tar file
                BZip2.Decompress(fsIn, fsOut)

                'Open a new file stream for the tar file
                Dim fsTar As FileStream = File.OpenRead(sTarFileName)
                'Create a TarArchive object for the tar file
                Dim tArch As TarArchive = TarArchive.CreateInputTarArchive(fsTar)

                'Extract the tar's contents
                tArch.ExtractContents(sDestPath)

                'Get a list of the files that were extracted within the tar folder
                Dim sExtractedFiles() As String =
                        Directory.GetFiles(sDestPath & "\" & Path.GetFileNameWithoutExtension(sTarFileName))

                'Move all the files to the requested destination directory
                Dim sFile As String
                For Each sFile In sExtractedFiles
                    File.Move(sFile, sDestPath & "\" & Path.GetFileName(sFile))
                Next

                'Delete the tar folder
                Directory.Delete(sDestPath & "\" & Path.GetFileNameWithoutExtension(sTarFileName))

                'Close the open file streams
                tArch.Close()
                fsIn.Close()

                'Delete the decompressed tar file

                File.Delete(sTarFileName)

        End Select

        bResult = True

Problem:
        Return bResult
    End Function

    Public Shared Sub ContinueInstallingForge(vtoupdate As String, Optional isabackup As Boolean = False)

        Dim myfile = fl.vtoupdate.Text
        myfile = Replace(myfile, "https://snapshots.cardforge.org/", "")
        myfile = Replace(myfile, "https://downloads.cardforge.org/dailysnapshots/", "")

        WriteUserLog("Done!" & vbCrLf)

        'If ReadLogUser("removepreviousjarfiles", False) = "yes" Then

        Try
                Dim x As Integer
                Dim paths() As String = Directory.GetFiles(vars.UserDir, "forge-gui-desktop-*-jar-with-dependencies.jar")
                If paths.Length > 0 Then
                    For x = 0 To paths.Length - 1
                        File.Delete(paths(x))
                    Next
                End If
            Catch

            End Try
        'End If

        Dim urlcomplete = vtoupdate
        WriteUserLog("Unpacking in " & Directory.GetCurrentDirectory() & "... please wait!" & vbCrLf)
        UnzipFile(Directory.GetCurrentDirectory() & "/" & myfile, Directory.GetCurrentDirectory())
        WriteUserLog("Done!." & vbCrLf)

        Try
            File.Delete(myfile)
        Catch
        End Try

        Try
            DeleteDownloaded()
        Catch
        End Try

        If fl.rbt_properties.Checked = True Then
            Dim Path = Directory.GetCurrentDirectory() & "\forge.profile.properties"
            If File.Exists(Path) Then
                File.Delete(Path)
            End If
            Dim t As String
            Using fs As FileStream = File.Create(Path)
                t = "userDir=" & Directory.GetCurrentDirectory() & "/user" + Environment.NewLine
                t = t + "cacheDir=" & Directory.GetCurrentDirectory() & "/cache" + Environment.NewLine
                t = t + "cardPicsDir="
                t = Replace(t, "\", "/")
                Dim info As Byte() = New UTF8Encoding(True).GetBytes(t)
                fs.Write(info, 0, info.Length)
            End Using
        End If

        Dim v As String = fl.vtoupdate.Text
        Dim upv As String = ReadLogUser("forge_previous_version")

        If upv = "" Then
            UpdateLog("forge_previous_version", v)
        End If

        Dim actual As String = fl.typeofupdate.SelectedItem.ToString

        Select Case actual
            Case "snapshot"
                actual = "forge_version"
            Case "release"
                actual = "release_version"
        End Select

        Select Case actual
            Case "forge_version"
                Dim hi = GetCheckAutomatic()
                UpdateLog(actual, hi)
                UpdateLog("release_version", "Not found")
                UpdateLog("other_version", "Not found")
            Case "release_version"
                UpdateLog(actual, myfile)
                UpdateLog("forge_version", "Not found")
                UpdateLog("other_version", "Not found")
        End Select

        DeleteDownloaded()

        Launch()

        Application.Exit()
        Try
            Environment.Exit(1)
        Catch
        End Try

        Exit Sub
    End Sub

    Public Shared Sub Launch()

        If ReadLogUser("launchmode") = "advanced" Then
            WriteUserLog("Launching PlayForge.bat ..." & vbCrLf)
            Try
                Process.Start("PlayForge.bat")
                Application.Exit()
                Try
                    Environment.Exit(1)
                Catch
                End Try
                Exit Sub
            Catch
                WriteUserLog("Can't find PlayForge.bat ..." & vbCrLf)
            End Try

        Else
            WriteUserLog("Launching Forge.exe ..." & vbCrLf)
            If File.Exists("Forge.exe") Then
                Process.Start("Forge.exe")
                Exit Sub
            End If
            WriteUserLog("Can't find Forge.exe!." & vbCrLf)
        End If
    End Sub

    Public Shared Sub WriteUserLog(msg)
        fl.txlog.SelectedText = msg
        fl.txlog.SelectionStart = fl.txlog.Text.Length
        fl.txlog.ScrollToCaret()
    End Sub

End Class
