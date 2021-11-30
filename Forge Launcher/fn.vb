Imports System.ComponentModel
Imports System.Net
Imports System.Text
Imports ICSharpCode.SharpZipLib.BZip2
Imports ICSharpCode.SharpZipLib.Tar
Imports ICSharpCode.SharpZipLib.Zip

Public Class fn
    Shared WithEvents downloader As WebClient

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

    Public Shared Sub OpenLogFile()
        Dim logfile As String = vars.ForgeData & "\forge.log"
        Dim logfile2 As String = Directory.GetCurrentDirectory & "\UserDir\forge.log"

        If File.Exists(logfile) = True Then
            Shell("c:\windows\notepad.exe " & logfile)
            Exit Sub
        End If

        If File.Exists(logfile2) = True Then
            Shell("c:\windows\notepad.exe " & logfile2)
            Exit Sub
        End If

        MsgBox("I can't find forge.log file!.")

    End Sub

    Public Shared Sub UninstallForgeLauncher()
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
    End Sub

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
                Dim dir As String = Path.Combine(vars.ForgeData, "preferences")
                File.Delete(dir & "\forge.preferences")
                MsgBox("Done!")
            Catch ex As Exception
                WriteUserLog(ex.Message.ToString)
            End Try
        End If
    End Sub

    Public Shared Sub UpdateLog(idlog, myvalue)
        Try
            CheckLog()
        Catch
        End Try
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

    Public Shared Sub CheckIfICSharpCodeExist()
        If File.Exists(vars.MyDll) = False Then
            MsgBox("You need " & vars.MyDll & " in the same directory to extract .tar.bz2 files!")
            Application.Exit()
        End If
    End Sub

    Public Shared Function ReadWeb(MyUrl As String)
        Try
            Dim client As WebClient = New WebClient()
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim reply As String = client.DownloadString(MyUrl)
            Return reply
        Catch
            Return Nothing
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
        Dim metadata = vars.url_release + "maven-metadata.xml"
        Dim xml = fn.ReadWeb(metadata)
        Dim doc As New System.Xml.XmlDocument()
        doc.LoadXml(xml)
        Dim nodes As System.Xml.XmlNodeList = doc.DocumentElement.SelectNodes("//version")
        Dim lastVersion = nodes.Item(nodes.Count - 1).InnerText
        Dim finalURL = String.Format("{0}{1}/forge-gui-desktop-{1}.tar.bz2", vars.url_release, lastVersion)
        CheckRelease = finalURL
    End Function
    Public Shared Function StringToStream(input As String, enc As Encoding) As Stream
        Dim memoryStream = New MemoryStream()
        Dim streamWriter = New StreamWriter(memoryStream, enc)
        streamWriter.Write(input)
        streamWriter.Flush()
        memoryStream.Position = 0
        Return memoryStream
    End Function
    Public Shared Sub CheckforForgeUpdates(Optional ByVal AskforReinstall = False, Optional ByVal NewInstall = False)
        If NewInstall Then
            If Main.rbt_normal.Checked = False And Main.rbt_properties.Checked = False Then
                MsgBox("Please select normal or portable install.")
                Exit Sub
            End If

        End If
        Dim readversion As String
        Dim urltoshow As String
        Dim result = Main.typeofupdate.Text.ToString
        If result = "snapshot" Then
            readversion = "forge_version"
            urltoshow = vars.SnapshotUrl
        Else
            readversion = "release_version"
            urltoshow = vars.url_release
        End If


        Dim typeofupdate As String = ReadLogUser("typeofupdate")
        Select Case typeofupdate
            Case "snapshot"
                vars.LinkLine = GetCheckAutomatic()
            Case "release"
                vars.LinkLine = CheckRelease()
        End Select

        Dim vs, vu As String
        Try
            vu = ReadLogUser(readversion, False).ToString
        Catch
            vu = ""
        End Try

        vs = vars.LinkLine

        If vs.Contains("#") Then
            vs = Split(vs, "#")(0).ToString
        End If

        If vu.Contains("#") Then
            vu = Split(vu, "#")(0).ToString
        End If

        If vu = "" Then
            If CheckIfForgeExists() = False Then
                If Main.rbt_normal.Checked = False And Main.rbt_properties.Checked = False Then
                    MsgBox("Please select normal or portable install.")
                    Exit Sub
                End If
            End If
        End If


        If vs = vu Then
            WriteUserLog("Your Forge " & typeofupdate & " version is up to date (" & vu & ")." & vbCrLf)

            If AskforReinstall = True Then

                If MsgBox(
                            "It's appears your Forge " & typeofupdate & " version is up to date (" & vu & "). Do you want to download again and reinstall it?",
                            MsgBoxStyle.YesNo, "Warning!") = MsgBoxResult.Yes Then
                    Dim link = If(typeofupdate = "snapshot", Split(GetCheckAutomatic(), "#")(1), vs)
                    UpdateForge(link)
                End If
                Exit Sub
            Else

            End If

            If MsgBox("Your Forge " & typeofupdate & " version is up to date." & vbCrLf &
             "Do you want to start Forge and close Launcher?", MsgBoxStyle.YesNo, "Forge is up to date") =
            MsgBoxResult.Yes Then
                Launch()
                Application.Exit()
                Try
                    Environment.Exit(1)
                Catch
                End Try
                Exit Sub
            End If

        Else
            WriteUserLog("New Forge " & typeofupdate & " version is available (" & vs & ")." & vbCrLf & "You're running: " & vu & vbCrLf)
            If _
  MsgBox("Do you want to install " & vs & " in " & vars.UserDir & "?",
         MsgBoxStyle.YesNoCancel, "Version Available") = MsgBoxResult.Yes Then
                Dim link = If(typeofupdate = "snapshot", Split(GetCheckAutomatic(), "#")(1), vs)
                UpdateForge(link)
            End If
        End If

    End Sub

    Public Shared Sub UpdateForge(vtoupdate)
        Main.vtoupdate.Text = vtoupdate
        Main.MenuGeneral.Enabled = False
        Main.GroupForgeOptions.Enabled = False
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
        Main.ProgressBar1.Visible = True
        downloader = New WebClient
        downloader.DownloadFileAsync(New Uri(dwl), fn)
    End Sub

    Public Shared Sub downloader_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) _
        Handles downloader.DownloadProgressChanged
        Main.ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Public Shared Sub downloader_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) _
        Handles downloader.DownloadFileCompleted
        ContinueInstallingForge(Main.vtoupdate.Text)
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
                    If Not line.StartsWith("#") AndAlso InStr(LCase(line.ToString), "userdir") > 0 Then
                        Try
                            Dim PossibleDir = Split(line, "=")(1).ToString
                            If PossibleDir <> "" And Directory.Exists(PossibleDir) Then
                                vars.ForgeData = PossibleDir
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
                        Catch
                        End Try
                    End If
                Next
            End If
        Next
    End Function

    Public Shared Function CheckIfForgeExists()
        Return IIf(File.Exists("forge.exe"), True, False)
    End Function

    Public Shared Sub CheckIfPreviousProfileProperties()
        Try
            Dim aa As String = ReadLogUser("profileproperties", False).ToString
            If aa.ToString <> (IIf(File.Exists("forge.profile.properties"), "yes", "no")).ToString Then
                UpdateLog("profileproperties", IIf(File.Exists("forge.profile.properties"), "yes", "no"))
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
            t = t & "<removepreviousjarfiles>no</removepreviousjarfiles>" & vbCrLf
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
                readText = readText & "<removepreviousjarfiles>no</removepreviousjarfiles>" & Environment.NewLine
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

                ''Get a list of the files that were extracted within the tar folder
                'Dim sExtractedFiles() As String =
                '        Directory.GetFiles(sDestPath & "\" & Path.GetFileNameWithoutExtension(sTarFileName))

                ''Move all the files to the requested destination directory
                'Dim sFile As String
                'For Each sFile In sExtractedFiles
                '    File.Move(sFile, sDestPath & "\" & Path.GetFileName(sFile))
                'Next

                ''Delete the tar folder
                'Directory.Delete(sDestPath & "\" & Path.GetFileNameWithoutExtension(sTarFileName))

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

        Dim myfile = Path.GetFileName(vtoupdate)

        WriteUserLog("Done!" & vbCrLf)

        If ReadLogUser("removepreviousjarfiles", False) = "yes" Then

            Try
                Dim x As Integer
                Dim paths() As String = Directory.GetFiles(vars.UserDir, "forge-*-jar-with-dependencies.jar")
                If paths.Length > 0 Then
                    For x = 0 To paths.Length - 1
                        File.Delete(paths(x))
                    Next
                End If
            Catch

            End Try
        End If

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

        If Main.rbt_properties.Checked = True Then
            Dim Path = Directory.GetCurrentDirectory() & "\forge.profile.properties"
            If File.Exists(Path) Then
                File.Delete(Path)
            End If
            Dim t As String
            Using fs As FileStream = File.Create(Path)
                t = "userDir=./user" + Environment.NewLine
                t = t + "cacheDir=./cache" + Environment.NewLine
                t = t + "cardPicsDir="
                t = Replace(t, "\", "/")
                Dim info As Byte() = New UTF8Encoding(True).GetBytes(t)
                fs.Write(info, 0, info.Length)
            End Using
            vars.ForgeData = "./user"
        End If

        Dim v As String = Main.vtoupdate.Text
        Dim upv As String = ReadLogUser("forge_previous_version")

        If upv = "" Then
            UpdateLog("forge_previous_version", v)
        End If

        Dim actual As String = Main.typeofupdate.SelectedItem.ToString

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
                UpdateLog(actual, vtoupdate)
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
        If Main.txlog.Text.Contains(msg) = False Then
            Main.txlog.SelectedText = msg
            Main.txlog.SelectionStart = Main.txlog.Text.Length
            Main.txlog.ScrollToCaret()
        End If

    End Sub

End Class