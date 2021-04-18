Imports System.Text
Imports System.Text.RegularExpressions

Public Class ext
    Shared todas As String
    Shared cards

    Public Shared Function getTitDeck(tx) As String
        Dim DeckTitle = ""
        DeckTitle = fn.FindIt(tx, "<title>", "Deck for Magic: the Gathering")
        DeckTitle = Replace(DeckTitle, "&#39;", "'")
        If DeckTitle = "" Then DeckTitle = fn.FindIt(tx, "<title>", " Deck")
        DeckTitle = Trim(DeckTitle)
        DeckTitle = Replace(DeckTitle, """", "'")
        DeckTitle = Replace(DeckTitle, "&amp;", "and")
        DeckTitle = Replace(DeckTitle, ":", " ")
        If DeckTitle <> Nothing Then
            If DeckTitle.Contains(" by ") Then getTitDeck = Split(DeckTitle, " by ")(0).ToString
        Else
            DeckTitle = DeckTitle
        End If

        getTitDeck = DeckTitle
    End Function

    Public Shared Sub ExtractTopMtggoldfish(metag As String, hm As Object, puttop As Object, customurl As String,
                                            Optional ByVal customfolder As String = "")
        fl.txlog.Text = ""

        Dim url = ""
        If Not IsNothing(customurl) Then url = customurl
        If url = "" Then
            Select Case (metag)
                Case "Duel Commander", "Arena Singleton", "Historic Brawl", "Artisan Historic", "Cascade", "Oathbreaker",
                    "Canadian Highlander", "Old School", "No Banned List Modern", "Frontier", "Tiny Leaders", "Limited",
                    "Block", "Free Form"
                    url = vars.mtggf & "/deck/custom/" & LCase(Replace(metag, " ", "_")) & "#paper"
                Case "Budget Modern", "Budget Standard"
                    url = vars.mtggf & "/decks/budget/" & LCase(Replace(Replace(metag, "Budget ", ""), " ", "/")) &
                          "#paper"
                Case "Budget Commander"
                    url = vars.mtggf & "/decks/budget/commander/" & "#paper"

                Case "Standard", "Modern", "Pioneer", "Pauper", "Legacy", "Vintage", "Historic", "Penny Dreadful"
                    url = vars.mtggf & "/metagame/" & LCase(fn.Normalize(metag)) & "/full#paper"
                Case Else
                    url = vars.mtggf & "/metagame/" & LCase(fn.Normalize(metag)) & "/full#paper"
            End Select
        End If

        fl.extract1.Enabled = False
        Dim tx1 As String

        tx1 = fn.ReadWeb(url)
        Dim MyDir = ""
        Dim MyFolder = ""

        Select Case metag
            Case "Standard", "Modern", "Pioneer", "Pauper", "Legacy", "Vintage", "Historic", "Penny Dreadful",
                "Budget Modern", "Budget Standard"
                MyDir = fn.GetForgeDecksDir() & "\constructed"
                If fn.ReadLogUser("preserve_decks", False) = "yes" Then
                    MyFolder = MyDir & "\" & metag & "\" & (Replace(DateTime.Today.ToShortDateString, "/", "-")) &
                               "\"
                    Dim ruta As String = MyDir & "\" & metag & "\"
                    Try
                        Dim counter =
                                New DirectoryInfo(ruta).GetDirectories().OrderBy(Function(x) x.CreationTime)

                        Dim s = ""
                        For Each f As DirectoryInfo In counter
                            s = s & f.Name & ","
                        Next

                        Dim s1() As String = Split(s, ",")

                        Dim mycount As Integer = CStr(counter.Count)
                        Dim x1 = 0
                        Dim total As Integer = fn.ReadLogUser("preserve_decks_number")
                        While mycount > CStr(total)
                            Try
                                Directory.Delete(ruta & s1(x1), True)
                            Catch
                            End Try
                            x1 = x1 + 1
                            counter =
                                New DirectoryInfo(ruta).GetDirectories().OrderBy(Function(x) x.CreationTime)
                            mycount = CStr(counter.Count)
                        End While
                    Catch
                    End Try
                Else
                    MyFolder = MyDir & "\" & metag & "\"
                End If
                MyFolder = Replace(MyFolder, "//", "/")
            Case "Commander", "Commander 1v1", "Budget Commander"
                MyDir = fn.GetForgeDecksDir() & "\commander\"
                MyFolder = MyDir
            Case "Tiny Leaders"
                MyDir = fn.GetForgeDecksDir() & "\tiny_leaders\"
                MyFolder = MyDir
            Case "Brawl"
                MyDir = fn.GetForgeDecksDir() & "\brawl\"
                MyFolder = MyDir
            Case Else
                MyDir = fn.GetForgeDecksDir() & "\constructed\"
                MyFolder = MyDir

        End Select
        MyFolder = Replace(MyFolder, "\\", "\")

        fn.DeleteDecks(MyFolder, "[" & metag & "] *")

        fn.CheckFolder(MyFolder)

        fn.WriteUserLog("Extracting " & metag & " Decks in " & MyFolder & vbCrLf)

        Select Case metag
            Case "Standard", "Modern", "Pioneer", "Pauper", "Legacy", "Vintage", "Historic", "Penny Dreadful"
                tx1 = extmtggoldfish(tx1, "/archetype/", "#paper")
            Case "Budget Modern", "Budget Standard"
                tx1 = extmtggoldfish(tx1, "/deck/", "#paper", "/deck/custom")
            Case "Duel Commander", "Arena Singleton", "Historic Brawl", "Artisan Historic", "Cascade", "Oathbreaker",
                "Canadian Highlander", "Old School", "No Banned List Modern", "Frontier", "Tiny Leaders", "Limited",
                "Block", "Free Form"
                tx1 = extmtggoldfish(tx1, "/deck/", "#paper", "/deck/custom")
            Case "Arena Standard"
                tx1 = extmtggoldfish(tx1, "/archetype/", "#paper")
            Case "Commander 1v1", "Commander", "Brawl"
                tx1 = extmtggoldfish(tx1, "/archetype/", "#paper")
            Case Else
                tx1 = extmtggoldfish(tx1, "/deck/", "#paper")
        End Select

        Dim num As String
        Dim checkurls() As String
        checkurls = Split(tx1, vbCrLf)
        Dim lasurls As String = tx1

        Try

            Dim countimes As Long = 2
            If checkurls.Length < hm Then
                Dim tx2
                While checkurls.Length < hm

                    If _
                        LCase(metag) = "standard" Or LCase(metag) = "legacy" Or LCase(metag) = "vintage" Or
                        LCase(metag) = "pauper" Or LCase(metag) = "pioneer" Or LCase(metag) = "historic" Then
                        If countimes = 2 Then
                            tx2 = ""
                            Select Case LCase(metag)
                                Case "legacy"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-315fcdf4-70d6-41b8-bd39-699032073591#paper")
                                Case "vintage"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-4b59bfe3-e589-45f1-bf1b-5312d945f2d3#paper")
                                Case "pauper"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-e2f79915-5636-44cc-9c6f-7a74834fd316#paper")
                                Case "pioneer"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-2309ea0a-91b7-44c4-b0cd-945fdc82dd90#paper")
                                Case "historic"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-061deb94-cf17-4926-a252-571799137b88#paper")
                                Case Else
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/" & LCase(metag) & "-other-eld#paper")


                            End Select
                            tx2 = extmtggoldfish(tx2, "/deck/", "", "custom")

                        Else

                            Select Case LCase(metag)

                                Case "legacy"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-315fcdf4-70d6-41b8-bd39-699032073591/decks?page=" &
                                            countimes)
                                Case "vintage"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-4b59bfe3-e589-45f1-bf1b-5312d945f2d3/decks?page=" &
                                            countimes)
                                Case "pauper"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-e2f79915-5636-44cc-9c6f-7a74834fd316/decks?page=" &
                                            countimes)
                                Case "pioneer"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-2309ea0a-91b7-44c4-b0cd-945fdc82dd90/decks?page=" &
                                            countimes)
                                Case "historic"
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/other-061deb94-cf17-4926-a252-571799137b88/decks?page=" &
                                            countimes)
                                Case Else
                                    tx2 =
                                        fn.ReadWeb(
                                            "https://www.mtggoldfish.com/archetype/" & LCase(metag) & "-other-eld#paper")
                            End Select

                            tx2 = extmtggoldfish(tx2, "/deck/", "", "custom")
                        End If
                    Else
                        url = vars.mtggf & "/metagame/" & Replace(LCase(metag), " ", "_") & "/full?page=" & countimes &
                              "#paper"
                        tx2 = fn.ReadWeb(url)
                        tx2 = extmtggoldfish(tx2, "/archetype/", "#paper", "custom")
                    End If

                    lasurls = lasurls & tx2
                    checkurls = Split(lasurls, vbCrLf)
                    countimes = countimes + 1
                    If tx2 = "" Then Exit While
                End While
            End If
        Catch
        End Try
        Dim urls()
        urls = Split(lasurls, vbCrLf)

        For i = 0 To urls.Length - 1

            If urls(i).ToString <> "" Then

                If i >= CInt(hm) Then Exit For

                Dim DeckPage = ""
                Dim UrlDeck = ""
                Dim Deck = ""
                Dim MyCmd = ""
                Dim DeckTitle = ""

                Dim laweb As String = vars.mtggf & urls(i)
                DeckPage = fn.ReadWeb(laweb)

                If _
                    (InStr(metag, "Commander") > 0 Or InStr(metag, "Tiny") > 0) And
                    InStr(DeckPage, "<h3>Similar Decks</h3>", CompareMethod.Text) > 0 Then
                    Dim t2 As String = Split(DeckPage, "<h3>Similar Decks</h3>")(1).ToString
                    DeckTitle = getTitDeck(DeckPage)
                    Dim links = extlinks(t2, "/deck/")
                    Dim aurls() As String = Split(links, vbCrLf)
                    For a = 0 To urls.Length - 1
                        Dim web = ("https//www.mtggoldfish.com" & aurls(0))
                        DeckPage = fn.ReadWeb(web)
                        If a = 0 Then Exit For
                    Next a
                End If

                UrlDeck = extmtggoldfish(DeckPage, "/deck/download/")

                If DeckTitle = "" Then
                    DeckTitle = getTitDeck(DeckPage)
                End If

                Dim pasar = False
                If InStr(metag, "Commander") > 0 Or InStr(metag, "Tiny") > 0 Or InStr(metag, "Brawl") > 0 Then
                    pasar = True
                End If

                If UrlDeck <> "" Then
                    pasar = True
                End If

                If pasar Then
                    If InStr(UrlDeck, vbCrLf) > 0 Then
                        UrlDeck = Split(UrlDeck, vbCrLf)(0)
                    End If

                    If Deck = "" Then Deck = fn.ReadWeb(vars.mtggf & "" & UrlDeck)
                    If Deck <> "" And Deck <> "Throttled" Then
                        Deck = Replace(Deck, vbLf, vbCrLf)
                        If InStr(Deck, "container-fluid layout-container-fluid", CompareMethod.Text) > 0 Then
                            Deck = Mtggoldfishnewformat(Deck)
                            Deck = extmtggoldfish(Deck, "/deck/download/")
                            Deck = fn.ReadWeb(Deck)
                        End If

                        num = (i + 1)
                        Select Case Len(hm)
                            Case 3
                                'c
                                Select Case Len(num)
                                    Case 1 '
                                        num = "00" & num
                                    Case 2
                                        num = "0" & num
                                End Select
                            Case Else
                                If Len(num) <= 1 Then
                                    num = "0" & num
                                End If
                        End Select

                        If puttop Then
                            DeckTitle = "#" & num & " - " & DeckTitle
                        End If

                        Deck = Replace(Deck, vbCr, "")
                        Deck = Replace(Deck, vbLf, vbCrLf)
                        Deck = Replace(Deck, "'" & vbCrLf & "<div id='error'" & vbCrLf & "</div" & vbCrLf, "")
                        Deck = Replace(Deck, "sideboard", "sideboard")
                        Deck = Replace(Deck, vbCrLf & vbCrLf, vbCrLf & "[sideboard]" & vbCrLf)

                        Dim IsACommander As Boolean = False

                        If InStr(metag, "Commander") > 0 Or InStr(metag, "Tiny") > 0 Or InStr(metag, "Brawl") > 0 Then

                            IsACommander = True

                            Dim SearchFor As String
                            SearchFor = fn.HTMLToText(DeckPage)
                            SearchFor = fn.RemoveWhitespace(SearchFor)
                            MyCmd = fn.FindIt(SearchFor, "Tabletop Arena MTGO Commander", "Creatures")
                            If MyCmd = "" Then _
                                MyCmd = fn.FindIt(SearchFor, "Tabletop Arena MTGO Commander", "Planeswalkers")
                            If MyCmd = "" Then MyCmd = fn.FindIt(SearchFor, "Tabletop Arena MTGO Commander", "Spells")

                            If Len(MyCmd) > 100 Then
                                MyCmd = fn.FindIt(SearchFor, "Tabletop Arena MTGO Commander", "Spells")
                            End If

                            If Len(MyCmd) > 100 Then
                                MyCmd = fn.FindIt(SearchFor, "Tabletop Arena MTGO Commander", "Planeswalkerss")
                            End If
                            Try
                                If MyCmd.Contains(" Â") Then MyCmd = Replace(MyCmd, " Â", "")
                            Catch
                            End Try

                            If InStr(MyCmd, " Companion ") > 0 Then
                                    MyCmd = Split(MyCmd, " Companion ")(0).ToString
                                End If
                                MyCmd = Replace(MyCmd, "$", "")

                                Dim CommanderCount As Long = 1
                                Try
                                    Dim partirlinea() = Split(MyCmd, " 1 ")
                                    If partirlinea(2) <> "" Then
                                        CommanderCount = 2
                                    End If
                                    If partirlinea(4) <> "" Then
                                        CommanderCount = 3
                                    End If
                                    If partirlinea(6) <> "" Then
                                        CommanderCount = 4
                                    End If
                                    If partirlinea(8) <> "" Then
                                        CommanderCount = 5
                                    End If
                                Catch

                                End Try

                                Dim sb As New StringBuilder
                                If Not IsNothing(MyCmd) Then
                                    For Each c As Char In MyCmd
                                        If Not Char.IsNumber(c) Then
                                            sb.Append(c)
                                        End If
                                    Next
                                End If

                                MyCmd = sb.ToString
                                MyCmd = Replace(MyCmd, "&#;", "'")
                                MyCmd = Replace(MyCmd, "$", "")
                                If InStr(MyCmd, "<title>") > 0 Then
                                    MyCmd = Split(MyCmd, "<title>")(0).ToString
                                End If
                                MyCmd = Trim(MyCmd)
                                Dim spcomm = Split(MyCmd, ".")
                                Dim concatcomm = ""

                                For xy = 0 To spcomm.Length - 1
                                    If Len(spcomm(xy).ToString) > 3 Then
                                        concatcomm += "1 " & spcomm(xy).ToString & vbCrLf
                                    End If

                                Next xy

                                MyCmd = concatcomm

                                MyCmd = fn.RemoveWhitespace(MyCmd)
                                MyCmd = Trim(MyCmd)

                                Dim substr As String = MyCmd
                                If MyCmd <> "" Then

                                    substr = Trim(Split(substr, " ")(0))

                                    If substr = "1" Then

                                        MyCmd = Replace(MyCmd, " 1 ", vbCrLf & "1 ")
                                        MyCmd = LTrim((RTrim(MyCmd)))

                                        Dim spcmndlines = Split(MyCmd, "1 ")
                                        Dim lines = ""
                                        For ab = 0 To spcmndlines.Length - 1
                                            If spcmndlines(ab) <> "" Then
                                                Dim uno As String = Trim(fn.RemoveWhitespace(Trim(spcmndlines(ab))))
                                                If InStr(Deck, uno) > 0 Then
                                                    lines = lines & spcmndlines(ab)
                                                    Deck = Replace(Deck, "1 " & uno & vbCrLf, "")
                                                End If
                                            End If
                                        Next ab
                                    End If
                                End If
                            End If

                            If Deck <> "" And InStr(Deck, "fb-root") = 0 Then
                            DeckTitle = "[" & metag & "] " & DeckTitle
                            DeckTitle = Replace(DeckTitle, "&#39;", "'")
                            If InStr(DeckTitle, "</title>") > 0 Then
                                Try
                                    DeckTitle = Split(DeckTitle, "</title>")(0).ToString
                                Catch

                                End Try
                            End If

                            DeckTitle = fn.RemoveStuffs(DeckTitle)
                            Deck = Replace(Deck, "King Caesar, Ancient Guardian", "Huntmaster Liger")
                            Deck = Replace(Deck, "Godzilla, Doom Inevitable", "Yidaro, Wandering Monster")
                            Deck = Replace(Deck, "Gigan, Cyberclaw Terror", "Gyruda, Doom of Depths")
                            Deck = Replace(Deck, "Godzilla, King of the Monsters", "Zilortha, Strength Incarnate")
                            Deck = Replace(Deck, "Ghidorah, King of the Cosmos", "Illuna, Apex of Wishes")
                            Deck = Replace(Deck, "Bio-Quartz Spacegozilla", "Brokkos, Apex of Forever")
                            Deck = Replace(Deck, "Biollante, Plant Beast Form", "Nethroi, Apex of Death")
                            Deck = Replace(Deck, "Mothra, Supersonic Queen", "Luminous Broodmoth")
                            Deck = Replace(Deck, "King Caesar, Awoken Titan", "Snapdax, Apex of the Hunt")
                            Deck = Replace(Deck, "Godzilla, Primeval Champion", "Titanoth Rex")
                            Deck = Replace(Deck, "Destoroyah, Perfect Lifeform", "Everquill Phoenix")
                            Deck = Replace(Deck, "Battra, the Destruction Beast", "Dirge Bat")
                            Deck = Replace(Deck, "Anguirus, Armored Killer", "Gemrazer")
                            Deck = Replace(Deck, "Rodan, Titan of Winged Fury", "Vadrok, Apex of Thunderimage")
                            Deck = Replace(Deck, "Mechagodzilla, Decisive Battle", "Crystalline Giant")
                            Deck = Replace(Deck, "Dorat, the Perfect Pet", "Sprite Dragon")
                            Deck = Replace(Deck, "Babygodzilla, Ruin Reborn", "Pollywog Symbiote")
                            Deck = Replace(Deck, "Mothra's Giant Cocoon", "Mysterious Egg")
                            Deck = Replace(Deck, "Spacegodzilla, Void Invader", "Void Beckoner")



                            Deck = fn.FormatDeck(Deck, DeckTitle, MyCmd)
                            If InStr(Deck, "1 Companion") > 0 Then
                                Dim hasacompanion = ""
                            End If

                            fn.WriteUserLog(fn.StringToDeck(MyFolder, Deck, DeckTitle))
                            num = num + 1
                        End If

                    End If
                End If
            End If
        Next i

        fl.extract1.Enabled = True
        fl.txlog.Text += CInt(num - 1) & " Decks extracted." & vbCrLf
    End Sub

    Public Shared Function RemoveDigits(S As String) As String
        Return Regex.Replace(S, "\d", "")
    End Function

    Public Shared Function extlinks(str As String, condition As String, Optional ByVal negate As String = "") _
        As String
        If str = Nothing Then
            str = ""
            Exit Function
        End If

        Dim RegexPattern = "href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))"

        ' Find matches.
        Dim matches As MatchCollection = Regex.Matches(str,
                                                       RegexPattern,
                                                       RegexOptions _
                                                          .
                                                          IgnoreCase)

        Dim MatchList(matches.Count - 1) As String

        ' Report on each match.
        Dim c = 0
        Dim r = ""
        For Each match As Match In matches


            MatchList(c) = match.Groups("url").Value

            'validamos 
            Dim link As String = match.ToString
            If condition = "" Then condition = "?e="
            Dim anadir = True
            If InStr(link, condition, CompareMethod.Text) > 0 Then

                link = Replace(link, "href=", "")
                link = Split(link, ">")(0).ToString
                link = Replace(link, """", "")

                If InStr(r, link, CompareMethod.Text) = 0 Then

                    If negate <> "" Then
                        If InStr(link, negate, CompareMethod.Text) > 0 Then
                            anadir = False
                        End If
                    End If

                    If anadir Then
                        r = r + link & vbCrLf
                    End If
                    c += 1

                End If

            End If

        Next match
        extlinks = r
    End Function

    Public Shared Function CommanderFormat(t) As String
        Dim t2 As String = Split(t, "<h3>Similar Decks</h3>")(1).ToString

        Dim links = extlinks(t2, "/deck/")
        Dim laweb = ""
        'MsgBox(tx1)
        Dim urls() As String = Split(links, vbCrLf)
        For i = 0 To urls.Length - 1
            Dim web = ("https://www.mtggoldfish.com" & urls(0))
            t = fn.ReadWeb(web)
            If i = 0 Then Exit For
        Next i

        t = fn.FindIt(t, "<td class='deck-header' colspan='4'>" & vbLf & "Commander", "100 Cards Total")

        t = Regex.Replace(t, "<td class='deck-col-price'>.*?</td>", "" _
                          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        t = Replace(t, "<td class='deck-col-qty'>", "")
        t = Regex.Replace(t, "<td class='deck-col-qty'>.*?</td>", "" _
                          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        t = Regex.Replace(t, "<a data-full-image=.*?>", "" _
                          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        t = Regex.Replace(t, "<td class='deck-col-mana'.*?</td>", "" _
                          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        t = Regex.Replace(t, "<td class='deck-header'.*?</td>", "" _
                          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)


        t = Replace(t, vbLf & "</td>" & vbLf & "<td class='deck-col-card'>" & vbLf, " ")
        t = fn.HTMLToText(t)
        t = Replace(t, vbLf & vbLf & vbLf & vbLf & vbLf & vbLf & vbLf, vbLf)
        t = Replace(t, vbLf & vbLf & vbLf & vbLf & vbLf & vbLf, vbLf)
        t = Replace(t, vbLf & vbLf & vbLf & vbLf & vbLf, vbLf)
        t = Replace(t, vbLf & vbLf & vbLf & vbLf, vbLf)
        t = Replace(t, vbLf & vbLf & vbLf, vbLf)
        t = Replace(t, vbLf & vbLf, vbLf)
        Return t
    End Function

    Public Shared Function Mtggoldfishnewformat(tx)
        tx = fn.FindIt(tx, "container-fluid layout-container-fluid", "<div class='hidden")
        tx = Replace(tx, vbCrLf & vbCrLf, vbCrLf)
        tx = Replace(tx, vbLf & "<div id='error'>" & vbLf & vbLf & "</div>" & vbLf, "")
        tx = Replace(tx, ">", "")
        tx = fn.HTMLToText(tx)
        Mtggoldfishnewformat = tx
    End Function

    Public Shared Function extmtggoldfish(str As String, condition As String, Optional condition2 As String = "",
                                          Optional excludelinks As String = "") As String

        If str = Nothing Then
            str = ""
            Exit Function
        End If

        Dim RegexPattern = "href\s*=\s*(?:    [""'](?<1>[^""']*)[""']|(?<1>\S+))"

        ' Find matches.
        Dim matches As MatchCollection = Regex.Matches(str,
                                                       RegexPattern,
                                                       RegexOptions _
                                                          .
                                                          IgnoreCase)

        Dim MatchList(matches.Count - 1) As String

        ' Report on each match.
        Dim c = 0
        Dim r = ""
        For Each match As Match In matches
            MatchList(c) = match.Groups("url").Value

            Dim link As String = match.ToString
            If condition = "" Then condition = "/archetype/"
            Dim SkipThis = False
            If InStr(link, condition) = 0 Then SkipThis = True
            If condition2 <> "" Then
                If InStr(link, condition2) = 0 Then SkipThis = True
            End If

            If excludelinks <> "" Then
                If InStr(link, excludelinks) > 0 Then
                    SkipThis = True
                End If
            End If

            If SkipThis = False Then
                link = Replace(link, "href=", "")
                link = Split(link, ">")(0).ToString
                link = Replace(link, """", "")
                r = r + link & vbCrLf
            End If
            c += 1

        Next match
        extmtggoldfish = r
    End Function

    Public Shared Sub ExtractTournamentMtgtop8(Optional ByVal tournament_url As String = "",
                                               Optional ByVal maxdecks As Integer = 100)

        fl.txlog.Clear()

        fn.WriteUserLog("Connecting..." & vbCrLf)
        Dim MyDir As String = fn.GetForgeDecksDir() & "\constructed\" & fn.ReadLogUser("tournamentsdecks_dir", False) &
                              "\"

        fl.extract1.Enabled = False
        Dim tx1 As String

        tx1 = fn.ReadWeb(tournament_url)

        Dim tourname = ""

        Dim res As String = tx1
        tourname = fn.FindIt(res, "<title>", "</title>")
        tourname = Replace(tourname, " @ mtgtop8.com", "")
        Dim NumPlayers As String = fn.FindIt(res, "star.png></div>", "<div class=S10")
        If NumPlayers = "" Then
            NumPlayers = fn.FindIt(res, "bigstar.png height=16></div>", "<div class=S10")
        End If
        If NumPlayers <> "" Then
            tourname = tourname & " - " & NumPlayers
        End If
        If tourname Is Nothing Then tourname = ""

        tourname = tourname.Replace("/", "")
        tourname = Replace(tourname, vbCrLf, "")

        tourname = tourname.Trim()
        tourname = Replace(tourname, ":", "")

        Dim MyFolder As String = MyDir & tourname & "\"

        If Directory.Exists(MyFolder) Then
            If _
                MsgBox(
                    "Folder " & tourname & " exists, do you want to download decks again? " & vbCrLf & vbCrLf &
                    " (Decks inside the folder will be deleted)", MsgBoxStyle.YesNoCancel, "Warning!") = MsgBoxResult.No _
                Then
                fn.WriteUserLog(tourname & " folder exists. Operation cancelled." & vbCrLf)
                Exit Sub
            End If
        End If

        Try
            Directory.Delete(MyFolder, True)
        Catch

        End Try
        fn.CheckFolder(MyFolder)
        fn.WriteUserLog("Creating " & MyFolder & vbCrLf)


        If InStr(tournament_url, "mtggoldfish", CompareMethod.Text) = 0 Then
            tx1 = extlinks(tx1, "?e=")
        Else
            tx1 = extlinks(tx1, "/deck/")
        End If


        Dim urls() As String = Split(tx1, vbCrLf)
        For i = 0 To urls.Length - 1

            If urls(i).ToString <> "" And urls(i).ToString <> "/deck/custom/standard" Then
                Dim DeckPage = ""
                Dim UrlDeck = ""

                DeckPage = fn.ReadWeb(vars.mtgtop8 & "/event" & urls(i))

                UrlDeck = extlinks(DeckPage, "mtgo?d=")
                Dim Deck = ""
                Dim DeckTitle = ""

                Deck = fn.ReadWeb(vars.mtgtop8 & "/" & UrlDeck)


                Deck = Replace(Deck, "sideboard", "[sideboard]")
                Deck = Replace(Deck, "[[", "[")
                Deck = Replace(Deck, "]]", "]")
                DeckTitle = fn.FindIt(DeckPage, "<title>", "@")
                DeckTitle = Replace(DeckTitle, "_", " ")
                DeckTitle = Replace(DeckTitle, """", "'")
                DeckTitle = fn.NormalizeName(DeckTitle)
                Dim num As String = (i + 1).ToString
                If Len(num) <= 1 Then num = "0" & num

                DeckTitle = "#" & num & " - " & DeckTitle

                Deck = fn.FormatDeck(Deck, DeckTitle)
                fn.StringToDeck(MyFolder, Deck, DeckTitle)
                fn.WriteUserLog("Saving " & DeckTitle & vbCrLf)
            End If

        Next i
        fl.extract1.Enabled = True
        fn.WriteUserLog("Completed")
    End Sub

    Public Shared Sub ExtractFromMtgtop8(Optional ByVal maxdecks As Integer = 100)

        Dim MyFormat = ""
        Select Case fl.ComboBox2.SelectedItem.ToString
            Case "Vintage"
                MyFormat = "VI"
            Case "Legacy"
                MyFormat = "LE"
            Case "Modern"
                MyFormat = "MO"
            Case "Standard"
                MyFormat = "ST"
            Case "Pauper"
                MyFormat = "PAU"
            Case "Commander"
                MyFormat = "EDH"
        End Select

        Dim tx1 = fn.ReadWeb(vars.mtgtop8 & "/format?f=" & MyFormat)
        Dim tx2 = extlinks(tx1, "event?e=")

        Dim urls() As String = Split(tx2, vbCrLf)

        Dim max = 0
        Select Case fl.maxtournm.SelectedItem.ToString
            Case "Last One"
                max = 1
            Case Else
                max = Replace(fl.maxtournm.SelectedItem.ToString, "Last ", "")
        End Select
        For i = 0 To urls.Length - 1
            If i > (max - 1) Then Exit For
            Dim MyUrl As String = vars.mtgtop8 & "/" & urls(i)
            ExtractTournamentMtgtop8(MyUrl, maxdecks)
        Next
    End Sub
End Class
