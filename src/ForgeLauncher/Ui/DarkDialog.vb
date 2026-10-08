Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>Icon shown on the left of a <see cref="DarkDialog"/>.</summary>
Public Enum DialogKind
    ''' <summary>Information (blue-gray "i").</summary>
    Info
    ''' <summary>A question or a decision.</summary>
    Question
    ''' <summary>Something the user should be careful about.</summary>
    Warning
    ''' <summary>Something failed.</summary>
    [Error]
    ''' <summary>An update is available.</summary>
    Download
    ''' <summary>Something finished successfully.</summary>
    Success
End Enum

''' <summary>Everything a <see cref="DarkDialog"/> shows.</summary>
Public NotInheritable Class DialogOptions
    ''' <summary>Window title.</summary>
    Public Property Title As String = "Forge Launcher"
    ''' <summary>Bold first line.</summary>
    Public Property Heading As String
    ''' <summary>Explanation below the heading; may contain line breaks.</summary>
    Public Property Message As String
    ''' <summary>Icon on the left.</summary>
    Public Property Kind As DialogKind = DialogKind.Info
    ''' <summary>Text of the main button.</summary>
    Public Property PrimaryText As String = "OK"
    ''' <summary>Icon glyph of the main button.</summary>
    Public Property PrimaryGlyph As String = String.Empty
    ''' <summary>Shows the main button in red instead of green (for destructive actions).</summary>
    Public Property PrimaryIsDanger As Boolean
    ''' <summary>Text of the second button; <c>Nothing</c> shows only the main button.</summary>
    Public Property SecondaryText As String
    ''' <summary>Text of an optional link, such as "What's new".</summary>
    Public Property LinkText As String
    ''' <summary>Web address opened by the link.</summary>
    Public Property LinkUrl As String
    ''' <summary>Text of an optional switch, such as "Don't offer this build again".</summary>
    Public Property CheckText As String
    ''' <summary>Initial state of the switch.</summary>
    Public Property CheckValue As Boolean
End Class

''' <summary>What the user chose in a <see cref="DarkDialog"/>.</summary>
Public NotInheritable Class DialogAnswer
    ''' <summary><c>True</c> when the main button was clicked (or Enter pressed).</summary>
    Public Property Accepted As Boolean
    ''' <summary>Final state of the optional switch.</summary>
    Public Property Checked As Boolean
End Class

''' <summary>
''' Dark dialog used for every question, warning and error, because the standard Windows message box is always light.
''' It is built in code because its content changes every time; it is centered on the launcher window.
''' </summary>
Public NotInheritable Class DarkDialog
    Inherits Form

    Private ReadOnly _options As DialogOptions
    Private ReadOnly _check As ToggleSwitch

    ''' <summary>Builds the dialog from its options.</summary>
    Private Sub New(options As DialogOptions)
        _options = options
        Font = New Font("Segoe UI", 9.0F)
        Text = options.Title
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        ShowInTaskbar = False
        ShowIcon = False
        AutoScaleMode = AutoScaleMode.None

        Dim width = Theme.Scale(440)
        Dim padding = Theme.Scale(18)
        Dim iconWidth = Theme.Scale(30)
        Dim textLeft = padding + iconWidth + Theme.Scale(10)
        Dim textWidth = width - textLeft - padding
        Dim y = padding

        If Theme.GlyphFontName IsNot Nothing Then
            Dim icon As New Label With {
                .Text = GlyphFor(options.Kind),
                .Font = New Font(Theme.GlyphFontName, 18.0F),
                .ForeColor = ColorFor(options.Kind),
                .AutoSize = True,
                .Location = New Point(padding, y)}
            Controls.Add(icon)
        Else
            textLeft = padding
            textWidth = width - padding * 2
        End If

        If Not String.IsNullOrEmpty(options.Heading) Then
            Dim heading = AddText(options.Heading, New Font("Segoe UI Semibold", 10.5F), Theme.Text, textLeft, y, textWidth)
            y = heading.Bottom + Theme.Scale(6)
        End If
        If Not String.IsNullOrEmpty(options.Message) Then
            Dim message = AddText(options.Message, Font, Theme.TextSecondary, textLeft, y, textWidth)
            y = message.Bottom + Theme.Scale(8)
        End If
        If Not String.IsNullOrEmpty(options.LinkText) Then
            Dim link As New LinkLabel With {
                .Text = options.LinkText,
                .AutoSize = True,
                .Location = New Point(textLeft, y),
                .LinkColor = Theme.AccentBright,
                .ActiveLinkColor = Theme.Text,
                .VisitedLinkColor = Theme.AccentBright,
                .LinkBehavior = LinkBehavior.HoverUnderline}
            AddHandler link.LinkClicked, Sub(sender, e) OpenUrl(options.LinkUrl)
            Controls.Add(link)
            y = link.Bottom + Theme.Scale(8)
        End If
        If Not String.IsNullOrEmpty(options.CheckText) Then
            _check = New ToggleSwitch With {
                .Text = options.CheckText,
                .Checked = options.CheckValue,
                .Location = New Point(textLeft, y),
                .Size = New Size(textWidth, Theme.Scale(30))}
            Controls.Add(_check)
            y = _check.Bottom + Theme.Scale(4)
        End If

        Dim footerHeight = Theme.Scale(54)
        Dim footer As New Panel With {.BackColor = Theme.SurfaceRaised, .Dock = DockStyle.Bottom, .Height = footerHeight}
        Controls.Add(footer)

        Dim buttonHeight = Theme.Scale(30)
        Dim right = width - padding
        Dim primary = CreateButton(options.PrimaryText, options.PrimaryGlyph,
                                   If(options.PrimaryIsDanger, ButtonVariant.Danger, ButtonVariant.Primary))
        primary.DialogResult = System.Windows.Forms.DialogResult.OK
        primary.Location = New Point(right - primary.Width, (footerHeight - buttonHeight) \ 2)
        footer.Controls.Add(primary)
        AcceptButton = primary
        CancelButton = primary

        If Not String.IsNullOrEmpty(options.SecondaryText) Then
            Dim secondary = CreateButton(options.SecondaryText, String.Empty, ButtonVariant.Secondary)
            secondary.DialogResult = System.Windows.Forms.DialogResult.Cancel
            secondary.Location = New Point(primary.Left - Theme.Scale(8) - secondary.Width, primary.Top)
            footer.Controls.Add(secondary)
            CancelButton = secondary
        End If

        ClientSize = New Size(width, Math.Max(y, padding + iconWidth) + padding \ 2 + footerHeight)
        Theme.Apply(Me)
    End Sub

    ''' <summary>Shows a dialog centered on its owner and returns the user's choice.</summary>
    ''' <param name="owner">Launcher window; <c>Nothing</c> centers the dialog on the screen.</param>
    ''' <param name="options">Content of the dialog.</param>
    Public Shared Function Ask(owner As IWin32Window, options As DialogOptions) As DialogAnswer
        Using dialog As New DarkDialog(options)
            dialog.StartPosition = If(owner Is Nothing, FormStartPosition.CenterScreen, FormStartPosition.CenterParent)
            Dim result = dialog.ShowDialog(owner)
            Return New DialogAnswer With {
                .Accepted = result = System.Windows.Forms.DialogResult.OK,
                .Checked = If(dialog._check IsNot Nothing, dialog._check.Checked, options.CheckValue)}
        End Using
    End Function

    ''' <summary>Shows a message with a single button.</summary>
    Public Shared Sub ShowMessage(owner As IWin32Window, kind As DialogKind, heading As String, message As String)
        Ask(owner, New DialogOptions With {.Kind = kind, .Heading = heading, .Message = message, .PrimaryText = "OK"})
    End Sub

    ''' <summary>Adds a wrapped text label and returns it, so the next control can be placed below.</summary>
    Private Function AddText(content As String, textFont As Font, textColor As Color, left As Integer, top As Integer, maxWidth As Integer) As Label
        Dim textLabel As New Label With {
            .Text = content,
            .Font = textFont,
            .ForeColor = textColor,
            .AutoSize = True,
            .MaximumSize = New Size(maxWidth, 0),
            .Location = New Point(left, top)}
        Controls.Add(textLabel)
        Return textLabel
    End Function

    ''' <summary>Creates a footer button sized to its text.</summary>
    Private Function CreateButton(caption As String, glyph As String, buttonVariant As ButtonVariant) As DarkButton
        Dim button As New DarkButton With {.Text = caption, .Glyph = glyph, .Variant = buttonVariant, .Font = Font}
        Dim textWidth = TextRenderer.MeasureText(caption, Font).Width
        Dim glyphWidth = If(String.IsNullOrEmpty(glyph) OrElse Theme.GlyphFontName Is Nothing, 0, Theme.Scale(24))
        button.Size = New Size(Math.Max(Theme.Scale(88), textWidth + glyphWidth + Theme.Scale(28)), Theme.Scale(30))
        Return button
    End Function

    ''' <summary>Icon glyph for each kind of dialog.</summary>
    Private Shared Function GlyphFor(kind As DialogKind) As String
        Select Case kind
            Case DialogKind.Warning : Return Theme.Glyphs.Warning
            Case DialogKind.Error : Return Theme.Glyphs.ErrorBadge
            Case DialogKind.Download : Return Theme.Glyphs.Download
            Case DialogKind.Success : Return Theme.Glyphs.CheckMark
            Case DialogKind.Question : Return Theme.Glyphs.Help
            Case Else : Return Theme.Glyphs.Info
        End Select
    End Function

    ''' <summary>Icon color for each kind of dialog.</summary>
    Private Shared Function ColorFor(kind As DialogKind) As Color
        Select Case kind
            Case DialogKind.Warning : Return Theme.Warning
            Case DialogKind.Error : Return Theme.DangerText
            Case DialogKind.Download, DialogKind.Success : Return Theme.AccentBright
            Case Else : Return Theme.TextSecondary
        End Select
    End Function

    ''' <summary>Opens a web page in the default browser, ignoring failures.</summary>
    Private Shared Sub OpenUrl(url As String)
        If String.IsNullOrEmpty(url) Then Return
        Try
            Process.Start(New ProcessStartInfo(url) With {.UseShellExecute = True})?.Dispose()
        Catch ex As Exception When TypeOf ex Is ComponentModel.Win32Exception OrElse TypeOf ex Is InvalidOperationException
        End Try
    End Sub
End Class
