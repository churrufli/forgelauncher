Imports System.ComponentModel
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports ICSharpCode.SharpZipLib.BZip2
Imports ICSharpCode.SharpZipLib.Tar
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
            Process.Start("notepad.exe", logfile)
            Exit Sub
        End If

        If File.Exists(logfile2) = True Then
            Process.Start("notepad.exe", logfile2)
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
            "If Forge does not load or freeze in the start screen, try restoring user preferences (previous preferences will be removed) " &
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

    Public Shared Function GetCheckAutomatic() As String
        Dim lineLink As String = ""
        Dim myTx As String = ReadWeb("https://downloads.cardforge.org/dailysnapshots/")
        Dim myDate As String = ""


        Dim takedate As String

        Try
            takedate = FindIt(myTx, "<a href=""forge-installer", "<a")

            Dim pattern As String = "\b\d{1,2}-[a-zA-Z]{3}-\d{4} \d{1,2}:\d{2}\b"
            Dim match As Match = Regex.Match(takedate, pattern)

            If match.Success Then
                myDate = match.Value
            End If
        Catch ex As Exception
            ' Manejar la excepción si es necesario
        End Try


        If String.IsNullOrEmpty(myDate) Then
            myDate = FindIt(myTx, "SNAPSHOT-", ".tar.bz2")
            myDate = Replace(myDate, ".", "-")
        End If

        Dim lineLinkPattern As String = "<a href='"
        Dim lineLinkSuffix As String = ".tar.bz2'>"

        For Each line As String In myTx.Split(Environment.NewLine)
            If line.Contains("tar.bz2") Then
                line = Replace(line, """", "'")
                Dim linkPart As String = FindIt(line, lineLinkPattern, lineLinkSuffix)
                lineLink = "https://downloads.cardforge.org/dailysnapshots/" & linkPart & ".tar.bz2"
                Exit For
            End If
        Next

        Dim version As String = FindIt(lineLink, "forge-installer-", ".tar")

        If Not String.IsNullOrEmpty(version) AndAlso Not String.IsNullOrEmpty(myDate) AndAlso Not String.IsNullOrEmpty(lineLink) Then
            Return $"Forge {version} {myDate}#{lineLink}"
        Else
            If MsgBox("Error trying to get new version from https://downloads.cardforge.org/dailysnapshots/" & vbCrLf & "Do you want to open the site in a browser?", MsgBoxStyle.YesNo, "Warning!") = MsgBoxResult.Yes Then
                Process.Start("https://downloads.cardforge.org/dailysnapshots/")
            End If
            Return Nothing
        End If
    End Function


    Public Shared Function CheckRelease()
        Dim metadata = vars.url_release + "maven-metadata.xml"
        Dim xml = fn.ReadWeb(metadata)
        Dim doc As New System.Xml.XmlDocument()
        doc.LoadXml(xml)
        Dim nodes As System.Xml.XmlNodeList = doc.DocumentElement.SelectNodes("//version")
        Dim lastVersion = nodes.Item(nodes.Count - 1).InnerText
        Dim finalURL = String.Format("{0}{1}/forge-installer-{1}.tar.bz2", vars.url_release, lastVersion)
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
        Try
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

        Catch

        End Try

    End Sub

    Public Shared Sub UpdateForge(vtoupdate)

        Main.vtoupdate.Text = vtoupdate
        Main.MenuGeneral.Enabled = False
        Main.GroupForgeOptions.Enabled = False
        If vtoupdate = "" Then

            Dim typeofupdate As String = ReadLogUser("typeofupdate")
            Select Case typeofupdate
                Case "snapshot"
                    vars.LinkLine = GetCheckAutomatic()
                Case Else
                    vars.LinkLine = CheckRelease()
            End Select
        End If

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
        If String.IsNullOrEmpty(total) OrElse (String.IsNullOrEmpty(first) AndAlso String.IsNullOrEmpty(last)) Then
            Return Nothing
        End If

        Dim startIndex As Integer = If(String.IsNullOrEmpty(first), 0, total.IndexOf(first))
        Dim endIndex As Integer

        If String.IsNullOrEmpty(last) Then
            endIndex = total.Length
        Else
            endIndex = If(startIndex >= 0, total.IndexOf(last, startIndex + first.Length), -1)
        End If

        If startIndex >= 0 AndAlso endIndex >= 0 AndAlso endIndex > startIndex Then
            Return total.Substring(startIndex, endIndex - startIndex).Replace(first, "").Replace(last, "")
        Else
            Return Nothing
        End If
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
        Dim existpp As String = If(File.Exists("forge.profile.properties"), "yes", "no")
        Dim logFilePath As String = Path.Combine(vars.UserDir, vars.LogName)

        If Not File.Exists(logFilePath) Then
            Dim t As String = "<forge_version>Not found</forge_version>" & vbCrLf &
                          "<release_version>Not found</release_version>" & vbCrLf &
                          "<other_version>Not found</other_version>" & vbCrLf &
                          "<forge_previous_version></forge_previous_version>" & vbCrLf &
                          "<profileproperties>" & existpp & "</profileproperties>" & vbCrLf &
                          "<lastupdate>" & hoy & "</lastupdate>" & vbCrLf &
                          "<removepreviousjarfiles>no</removepreviousjarfiles>" & vbCrLf &
                          "<typeofupdate>snapshot</typeofupdate>" & vbCrLf &
                          "<launchmode>normal</launchmode>" & vbCrLf &
                          "<launchline></launchline>" & vbCrLf &
                          "<exeselected></exeselected>" & vbCrLf

            If Not Directory.Exists(Path.GetDirectoryName(logFilePath)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath))
            End If

            File.WriteAllText(logFilePath, t)
        Else
            Dim readText As String = File.ReadAllText(logFilePath)
            Dim WriteLog As Boolean = False

            If Not readText.Contains("<forge_version>") Then
                readText = readText & "<forge_version>Not found</forge_version>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<release_version>") Then
                readText = readText & "<release_version>Not found</release_version>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<other_version>") Then
                readText = readText & "<other_version>Not found</other_version>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<forge_previous_version>") Then
                readText = readText & "<forge_previous_version></forge_previous_version>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<launcher_version>") Then
                readText = readText & "<launcher_version>Forge Launcher V.1.1</launcher_version>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<profileproperties>") Then
                readText = readText & "<profileproperties>" & existpp & "</profileproperties>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<removepreviousjarfiles>") Then
                readText = readText & "<removepreviousjarfiles>no</removepreviousjarfiles>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<typeofupdate>") Then
                readText = readText & "<typeofupdate>snapshot</typeofupdate>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<launchmode>") Then
                readText = readText & "<launchmode>normal</launchmode>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<launchline>") Then
                readText = readText & "<launchline></launchline>" & Environment.NewLine
                WriteLog = True
            End If

            If Not readText.Contains("<exeselected>") Then
                readText = readText & "<exeselected></exeselected>" & Environment.NewLine
                WriteLog = True
            End If

            If readText.Contains(Environment.NewLine & Environment.NewLine) Then
                readText = readText.Replace(Environment.NewLine & Environment.NewLine, Environment.NewLine)
                WriteLog = True
            End If

            If WriteLog Then
                Try
                    File.Delete(logFilePath)
                Catch
                End Try
                File.WriteAllText(logFilePath, readText)
            End If
        End If
    End Sub

    Public Shared Sub DescomprimirTarBz2(archivoBz2 As String, directorioDestino As String)
        ' Crear el directorio de destino si no existe
        If Not Directory.Exists(directorioDestino) Then
            Directory.CreateDirectory(directorioDestino)
        End If

        ' Crear un flujo de entrada para el archivo tar.bz2
        Using archivoStream As New FileStream(archivoBz2, FileMode.Open, FileAccess.Read)
            ' Crear un flujo de descompresión BZip2
            Using bz2Stream As New BZip2InputStream(archivoStream)
                ' Crear un lector de archivos tar
                Using tarArchive As New TarInputStream(bz2Stream)
                    ' Iterar a través de los archivos en el archivo tar
                    Dim entry As TarEntry = tarArchive.GetNextEntry()
                    While entry IsNot Nothing
                        ' Crear el nombre del archivo de destino
                        Dim a = entry.Name
                        If a.Contains("custom_card_pics/Chandra") Then
                            a = a
                        End If
                        a = Replace(a, "" & ChrW(25) & "", "'")
                        Dim archivoDestino As String = Path.Combine(directorioDestino, a)

                        ' Crear el directorio si es un directorio
                        If entry.IsDirectory Then
                            If Not Directory.Exists(archivoDestino) Then
                                Directory.CreateDirectory(archivoDestino)
                            End If
                        Else
                            ' Crear un flujo de salida para el archivo de destino
                            Using fileStream As New FileStream(archivoDestino, FileMode.Create, FileAccess.Write)
                                ' Leer y escribir los datos del archivo
                                tarArchive.CopyEntryContents(fileStream)
                            End Using
                        End If

                        ' Obtener el siguiente archivo en el archivo tar
                        entry = tarArchive.GetNextEntry()
                    End While
                End Using
            End Using
        End Using
    End Sub


    Public Shared Function UnzipFile(sZipFile As String, sDestPath As String) As Boolean
        Select Case Path.GetExtension(sZipFile).ToLower()
            Case ".zip"
                ZipFile.ExtractToDirectory(sZipFile, sDestPath)

            Case ".bz2"
                DescomprimirTarBz2(sZipFile, sDestPath)
        End Select

    End Function




    Public Shared Sub ContinueInstallingForge(vtoupdate As String, Optional isabackup As Boolean = False)
        Dim myfile = Path.GetFileName(vtoupdate)

        WriteUserLog("Done!" & vbCrLf)

        If ReadLogUser("removepreviousjarfiles", False) = "yes" Then
            Try
                Dim pathsAdventure() As String = Directory.GetFiles(vars.UserDir, "forge-adventure-*-jar-with-dependencies.jar")
                Dim pathsGuiDesktop() As String = Directory.GetFiles(vars.UserDir, "forge-gui-desktop-*-jar-with-dependencies.jar")
                Dim pathsGuiMobileDev() As String = Directory.GetFiles(vars.UserDir, "forge-gui-mobile-dev-*-jar-with-dependencies.jar")

                ' Combina todos los arrays en uno solo
                Dim paths() As String = pathsAdventure.Concat(pathsGuiDesktop).Concat(pathsGuiMobileDev).ToArray()

                ' Verifica si hay archivos que borrar
                If paths.Length > 0 Then
                    For Each path As String In paths
                        File.Delete(path)
                    Next
                End If
            Catch
            End Try
        End If

        Dim urlcomplete = vtoupdate
        WriteUserLog("Unpacking in " & Directory.GetCurrentDirectory() & "... please wait!" & vbCrLf)
        UnzipFile(Path.Combine(Directory.GetCurrentDirectory(), myfile), Directory.GetCurrentDirectory())
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
            Dim propPath = Path.Combine(Directory.GetCurrentDirectory(), "forge.profile.properties")
            If File.Exists(propPath) Then
                File.Delete(propPath)
            End If

            Dim t As String = "userDir=./user" + Environment.NewLine
            t = t + "cacheDir=./cache" + Environment.NewLine
            t = t + "cardPicsDir="
            t = t.Replace("\", "/")

            Using fs As FileStream = File.Create(propPath)
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
        Dim myexe As String = "Forge.exe"
        Try
            If Main.listofexes.Visible = True And Main.listofexes.SelectedItem <> Nothing Then
                myexe = Main.listofexes.SelectedItem.ToString
            End If
        Catch
        End Try


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
            WriteUserLog("Launching " & myexe & " ..." & vbCrLf)
            If File.Exists(myexe) Then
                Process.Start(myexe)
                Exit Sub
            End If
            WriteUserLog("Can't find " & myexe & "!." & vbCrLf)
        End If
    End Sub

    Public Shared Sub WriteUserLog(msg)
        If Main.txlog.Text.Contains(msg) = False Then
            Main.txlog.SelectedText = msg
            Main.txlog.SelectionStart = Main.txlog.Text.Length
            Main.txlog.ScrollToCaret()
        End If

    End Sub

    Public Shared Sub LoadListofExes()
        If fn.ReadLogUser("launchmode").ToString <> "advanced" Then
            Main.listofexes.Visible = True
        Else
            Main.listofexes.Visible = False
            Exit Sub
        End If

        Main.listofexes.Items.Clear()
        Dim exes As String() = Directory.GetFiles(Directory.GetCurrentDirectory, "*.exe")

        For Each exe As String In exes
            If My.Computer.FileSystem.GetFileInfo(exe).Name <> "Forge Launcher.exe" Then
                Main.listofexes.Items.Add(My.Computer.FileSystem.GetFileInfo(exe).Name)
            End If
        Next

        Dim cmds As String() = Directory.GetFiles(Directory.GetCurrentDirectory, "*.cmd")

        For Each cmd As String In cmds
            Main.listofexes.Items.Add(My.Computer.FileSystem.GetFileInfo(cmd).Name)
        Next

        'reading preferences and set if exists
        Dim pref As String = fn.ReadLogUser("exeselected").ToString
        If pref <> "" Then
            Try
                Main.listofexes.SelectedIndex = Main.listofexes.FindStringExact(pref)
            Catch
            End Try
            Exit Sub
        End If
        Try
            Main.listofexes.SelectedIndex = Main.listofexes.FindStringExact("Forge.exe")
        Catch
        End Try
    End Sub

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

    Public Shared Function CheckAddress(URL As String) As Boolean
        Try
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Shared Sub CheckLauncherUpdates()
        Try
            WriteUserLog("Checking for Launcher updates..." & vbCrLf)

            ' Obtener la página de lanzamientos desde GitHub
            Dim pageContent As String = fn.ReadWeb("https://github.com/churrufli/forgelauncher/releases/")
            Dim currentVersionTitle As String = Main.GetTitle()
            Dim latestVersionNumber As String = GetDelimitedText(pageContent, "Forge Launcher v", "<", 1)

            ' Verificar si se obtuvo la versión más reciente
            If String.IsNullOrEmpty(latestVersionNumber) Then
                MsgBox("Can't retrieve the latest version from GitHub.")
                Exit Sub
            End If

            Dim latestVersionTitle As String = "Forge Launcher v" & latestVersionNumber
            If currentVersionTitle = latestVersionTitle Then
                WriteUserLog("Your Launcher is up to date: " & latestVersionTitle & vbCrLf)
                Exit Sub
            End If

            ' Solicitar al usuario si desea actualizar
            If MsgBox("New version of Forge Launcher available. Update now?", MsgBoxStyle.YesNo, latestVersionTitle) = MsgBoxResult.Yes Then
                Try
                    WriteUserLog("Downloading new version from GitHub..." & vbCrLf)
                    Dim downloadUrl = "https://github.com/churrufli/forgelauncher/releases/download/v" & latestVersionNumber & "/Forge.Launcher.zip"
                    Dim zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Forge Launcher New Version.zip")

                    DownloadFile(downloadUrl, zipFilePath)
                    WriteUserLog("Unpacking new version..." & vbCrLf)

                    ' Crear o limpiar la carpeta temporal fltmp
                    Dim extractionPath = Path.Combine(Directory.GetCurrentDirectory(), "fltmp")
                    If Directory.Exists(extractionPath) Then
                        Directory.Delete(extractionPath, True) ' Eliminar carpeta y contenido si ya existe
                    End If
                    Directory.CreateDirectory(extractionPath) ' Crear la carpeta vacía

                    UnzipFile(zipFilePath, extractionPath)
                    File.Delete(zipFilePath)

                    ' Reemplazar el ejecutable actual
                    Dim executablePath = Path.Combine(Directory.GetCurrentDirectory(), "Forge Launcher.exe")
                    Dim newExecutablePath = Path.Combine(extractionPath, "Forge Launcher.exe")

                    ' Asegurarse de que el lanzador actual esté cerrado antes de reemplazar
                    WriteUserLog("Closing current launcher instance..." & vbCrLf)
                    Process.GetCurrentProcess().Kill()

                    ' Mover el nuevo ejecutable al directorio principal
                    File.Copy(newExecutablePath, executablePath, True)

                    ' Limpiar archivos temporales
                    Directory.Delete(extractionPath, True)

                    ' Reiniciar el lanzador
                    WriteUserLog("Starting the new version of the launcher..." & vbCrLf)
                    Process.Start(executablePath)
                Catch ex As Exception
                    WriteUserLog("Error updating launcher: " & ex.Message & vbCrLf)
                    MsgBox("Failed to update. Please try again later.", MsgBoxStyle.Critical)
                End Try
            End If
        Catch ex As Exception
            WriteUserLog("Unexpected error: " & ex.Message & vbCrLf)
        End Try
    End Sub





    Public Shared Function GetDelimitedText(Text As String, OpenDelimiter As String,
  CloseDelimiter As String, index As Long) As String
        Dim i As Long, j As Long

        If index = 0 Then index = 1

        ' search the opening mark
        i = InStr(index, Text, OpenDelimiter, vbTextCompare)
        If i = 0 Then
            index = 0
            Exit Function
        End If
        i = i + Len(OpenDelimiter)

        ' search the closing mark
        j = InStr(i + 1, Text, CloseDelimiter, vbTextCompare)
        If j = 0 Then
            index = 0
            Exit Function
        End If

        ' get the text between the two Delimiters
        GetDelimitedText = Mid$(Text, i, j - i)

        ' advanced the index after the closing Delimiter
        index = j + Len(CloseDelimiter)

    End Function
End Class