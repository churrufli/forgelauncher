Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

''' <summary>Visual style of a <see cref="DarkButton"/>.</summary>
Public Enum ButtonVariant
    ''' <summary>Dark gray button with a border (most buttons).</summary>
    Secondary
    ''' <summary>Green button for the main action of a window.</summary>
    Primary
    ''' <summary>Button with red text for destructive actions.</summary>
    Danger
    ''' <summary>Text-only button without background, for actions in the status bar.</summary>
    Subtle
End Enum

''' <summary>
''' Flat dark button that shows an icon glyph before its text. The glyph comes from the Windows icon font, so it
''' scales cleanly and needs no image files; on systems without that font only the text is shown.
''' </summary>
Public Class DarkButton
    Inherits Button

    Private _glyph As String = String.Empty
    Private _variant As ButtonVariant = ButtonVariant.Secondary
    Private _hover As Boolean
    Private _pressed As Boolean

    ''' <summary>Creates the button and switches it to custom painting.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        FlatStyle = FlatStyle.Flat
        Cursor = Cursors.Hand
    End Sub

    ''' <summary>Icon glyph drawn before the text (see <see cref="Theme.Glyphs"/>).</summary>
    <DefaultValue(""), Category("Appearance"), Description("Icon glyph drawn before the text.")>
    Public Property Glyph As String
        Get
            Return _glyph
        End Get
        Set(value As String)
            _glyph = If(value, String.Empty)
            Invalidate()
        End Set
    End Property

    ''' <summary>Visual style of the button.</summary>
    <DefaultValue(GetType(ButtonVariant), "Secondary"), Category("Appearance")>
    Public Property [Variant] As ButtonVariant
        Get
            Return _variant
        End Get
        Set(value As ButtonVariant)
            _variant = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        _hover = True
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        _hover = False
        _pressed = False
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then _pressed = True
        Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        _pressed = False
        Invalidate()
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        Invalidate()
        MyBase.OnEnabledChanged(e)
    End Sub

    ''' <summary>Draws background, border, focus outline, glyph and text according to the variant and state.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Background))

        Dim fill As Color
        Dim border As Color
        Dim foreground As Color
        Select Case _variant
            Case ButtonVariant.Primary
                fill = If(_pressed, Theme.AccentPressed, If(_hover, Theme.AccentHover, Theme.Accent))
                border = fill
                foreground = Color.White
            Case ButtonVariant.Danger
                fill = If(_pressed, Theme.Selection, If(_hover, Theme.Hover, Theme.SurfaceRaised))
                border = Theme.DangerBorder
                foreground = Theme.DangerText
            Case ButtonVariant.Subtle
                fill = If(_pressed, Theme.Selection, If(_hover, Theme.Hover, Color.Empty))
                border = Color.Empty
                foreground = Theme.Text
            Case Else
                fill = If(_pressed, Theme.Selection, If(_hover, Theme.Hover, Theme.SurfaceRaised))
                border = Theme.BorderStrong
                foreground = Theme.Text
        End Select

        If Not Enabled Then
            fill = If(_variant = ButtonVariant.Subtle, Color.Empty, Theme.Surface)
            border = If(border = Color.Empty, Color.Empty, Theme.Border)
            foreground = Theme.TextMuted
        End If

        Dim box = New Rectangle(0, 0, Width - 1, Height - 1)
        If fill <> Color.Empty Then Theme.DrawRoundedBox(g, box, Theme.Scale(4), fill, border)
        If Focused AndAlso ShowFocusCues Then
            Theme.DrawRoundedBox(g, Rectangle.Inflate(box, -2, -2), Theme.Scale(3), If(fill = Color.Empty, Theme.Background, fill), Theme.AccentBright)
        End If

        DrawContent(g, foreground)
    End Sub

    ''' <summary>Draws the glyph and the text centered as one group, with a small gap between them.</summary>
    Private Sub DrawContent(g As Graphics, foreground As Color)
        Dim flags = TextFormatFlags.NoPadding Or TextFormatFlags.SingleLine
        Dim glyphFontName = Theme.GlyphFontName
        Dim showGlyph = _glyph.Length > 0 AndAlso glyphFontName IsNot Nothing
        Dim textSize = If(Text.Length > 0, TextRenderer.MeasureText(g, Text, Font, Size.Empty, flags), Size.Empty)

        If Not showGlyph Then
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, foreground, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)
            Return
        End If

        Using glyphFont As New Font(glyphFontName, Font.SizeInPoints + 1.0F)
            Dim glyphSize = TextRenderer.MeasureText(g, _glyph, glyphFont, Size.Empty, flags)
            Dim gap = If(Text.Length > 0, Theme.Scale(7), 0)
            Dim totalWidth = glyphSize.Width + gap + textSize.Width
            Dim left = Math.Max(Theme.Scale(6), (Width - totalWidth) \ 2)
            Dim glyphBounds As New Rectangle(left, 0, glyphSize.Width, Height)
            TextRenderer.DrawText(g, _glyph, glyphFont, glyphBounds, foreground, flags Or TextFormatFlags.VerticalCenter)
            If Text.Length > 0 Then
                Dim textBounds As New Rectangle(glyphBounds.Right + gap, 0, Math.Max(0, Width - glyphBounds.Right - gap - Theme.Scale(4)), Height)
                TextRenderer.DrawText(g, Text, Font, textBounds, foreground, flags Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)
            End If
        End Using
    End Sub
End Class

''' <summary>
''' Two or more options side by side, one of them selected (for example "Snapshot | Release").
''' Click an option, or use the arrow keys when it has the focus.
''' </summary>
<DefaultEvent("SelectedIndexChanged")>
Public Class SegmentedSelector
    Inherits Control

    Private _items As String() = Array.Empty(Of String)()
    Private _selectedIndex As Integer = -1
    Private _hoverIndex As Integer = -1

    ''' <summary>Raised when the user (or code) selects another option.</summary>
    Public Event SelectedIndexChanged As EventHandler

    ''' <summary>Creates the selector and switches it to custom painting.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.Selectable, True)
        TabStop = True
        Cursor = Cursors.Hand
    End Sub

    ''' <summary>Texts of the options, from left to right.</summary>
    <Category("Data"), Description("Texts of the options, from left to right.")>
    Public Property Items As String()
        Get
            Return _items
        End Get
        Set(value As String())
            _items = If(value, Array.Empty(Of String)())
            If _selectedIndex >= _items.Length Then _selectedIndex = _items.Length - 1
            Invalidate()
        End Set
    End Property

    ''' <summary>Index of the selected option, or -1 for none.</summary>
    <DefaultValue(-1), Category("Behavior")>
    Public Property SelectedIndex As Integer
        Get
            Return _selectedIndex
        End Get
        Set(value As Integer)
            Dim clamped = Math.Max(-1, Math.Min(_items.Length - 1, value))
            If clamped = _selectedIndex Then Return
            _selectedIndex = clamped
            Invalidate()
            RaiseEvent SelectedIndexChanged(Me, EventArgs.Empty)
        End Set
    End Property

    ''' <summary>Returns the option under a horizontal position.</summary>
    Private Function IndexAt(x As Integer) As Integer
        If _items.Length = 0 Then Return -1
        Return Math.Max(0, Math.Min(_items.Length - 1, x * _items.Length \ Math.Max(1, Width)))
    End Function

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Dim index = IndexAt(e.X)
        If index <> _hoverIndex Then
            _hoverIndex = index
            Invalidate()
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        _hoverIndex = -1
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button = MouseButtons.Left AndAlso Enabled Then
            Focus()
            SelectedIndex = IndexAt(e.X)
        End If
        MyBase.OnMouseDown(e)
    End Sub

    ''' <summary>Lets the arrow keys reach <see cref="OnKeyDown"/> instead of moving the focus.</summary>
    Protected Overrides Function IsInputKey(keyData As Keys) As Boolean
        If keyData = Keys.Left OrElse keyData = Keys.Right Then Return True
        Return MyBase.IsInputKey(keyData)
    End Function

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyCode = Keys.Left AndAlso _selectedIndex > 0 Then SelectedIndex -= 1
        If e.KeyCode = Keys.Right AndAlso _selectedIndex < _items.Length - 1 Then SelectedIndex += 1
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        Invalidate()
        MyBase.OnGotFocus(e)
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        Invalidate()
        MyBase.OnLostFocus(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        Invalidate()
        MyBase.OnEnabledChanged(e)
    End Sub

    ''' <summary>Draws the outer box and every option; the selected one gets a lighter background.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Background))
        Dim outer As New Rectangle(0, 0, Width - 1, Height - 1)
        Theme.DrawRoundedBox(g, outer, Theme.Scale(6), Theme.Input, If(Focused AndAlso ShowFocusCues, Theme.AccentBright, Theme.Border))
        If _items.Length = 0 Then Return

        Dim inset = Theme.Scale(3)
        Dim segmentWidth = (Width - inset * 2) / _items.Length
        For index = 0 To _items.Length - 1
            Dim bounds As New Rectangle(CInt(inset + index * segmentWidth), inset, CInt(segmentWidth), Height - inset * 2)
            If index = _selectedIndex Then
                Theme.DrawRoundedBox(g, bounds, Theme.Scale(4), If(Enabled, Theme.Selection, Theme.Surface), Color.Empty)
            ElseIf index = _hoverIndex AndAlso Enabled Then
                Theme.DrawRoundedBox(g, bounds, Theme.Scale(4), Theme.Hover, Color.Empty)
            End If
            Dim segmentColor = If(Not Enabled, Theme.TextMuted, If(index = _selectedIndex, Theme.Text, Theme.TextSecondary))
            TextRenderer.DrawText(g, _items(index), Font, bounds, segmentColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine)
        Next
    End Sub
End Class

''' <summary>On/off switch with its label on the left, used instead of check boxes.</summary>
Public Class ToggleSwitch
    Inherits CheckBox

    ''' <summary>Creates the switch and switches it to custom painting.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        AutoSize = False
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnCheckedChanged(e As EventArgs)
        Invalidate()
        MyBase.OnCheckedChanged(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        Invalidate()
        MyBase.OnEnabledChanged(e)
    End Sub

    ''' <summary>Draws the label, and the switch track and knob at the right edge.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Background))
        Dim trackWidth = Theme.Scale(34)
        Dim trackHeight = Theme.Scale(18)
        Dim track As New Rectangle(Width - trackWidth - 2, (Height - trackHeight) \ 2, trackWidth, trackHeight)

        Dim textColor = If(Enabled, Theme.Text, Theme.TextMuted)
        Dim textBounds As New Rectangle(0, 0, Math.Max(0, track.Left - Theme.Scale(10)), Height)
        TextRenderer.DrawText(g, Text, Font, textBounds, textColor, TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)

        Dim trackColor = If(Checked, If(Enabled, Theme.Accent, Theme.Selection), Theme.BorderStrong)
        Theme.DrawRoundedBox(g, track, trackHeight \ 2, trackColor, Color.Empty)
        If Focused AndAlso ShowFocusCues Then
            Using pen As New Pen(Theme.AccentBright)
                g.SmoothingMode = SmoothingMode.AntiAlias
                Using outline = Theme.RoundedRectangle(Rectangle.Inflate(track, 2, 2), trackHeight \ 2 + 2)
                    g.DrawPath(pen, outline)
                End Using
            End Using
        End If

        Dim knobSize = trackHeight - Theme.Scale(6)
        Dim knobX = If(Checked, track.Right - knobSize - Theme.Scale(3), track.Left + Theme.Scale(3))
        g.SmoothingMode = SmoothingMode.AntiAlias
        Using knob As New SolidBrush(If(Checked, Color.White, Theme.TextSecondary))
            g.FillEllipse(knob, knobX, track.Top + Theme.Scale(3), knobSize, knobSize)
        End Using
    End Sub
End Class

''' <summary>Thin progress bar (a few pixels high) in the accent color.</summary>
Public Class ThinProgressBar
    Inherits Control

    Private _value As Integer

    ''' <summary>Creates the bar and switches it to custom painting.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        TabStop = False
    End Sub

    ''' <summary>Completed percentage, from 0 to 100.</summary>
    <DefaultValue(0), Category("Behavior")>
    Public Property Value As Integer
        Get
            Return _value
        End Get
        Set(value As Integer)
            _value = Math.Max(0, Math.Min(100, value))
            Invalidate()
        End Set
    End Property

    ''' <summary>Draws the track and the completed part with rounded ends.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Background))
        Dim radius = Height \ 2
        Theme.DrawRoundedBox(g, New Rectangle(0, 0, Width - 1, Height - 1), radius, Theme.Hover, Color.Empty)
        Dim filled = CInt((Width - 1) * _value / 100.0)
        If filled > Height Then Theme.DrawRoundedBox(g, New Rectangle(0, 0, filled, Height - 1), radius, Theme.AccentBright, Color.Empty)
    End Sub
End Class

''' <summary>Small colored dot that shows the update state next to the installed version.</summary>
Public Class StatusDot
    Inherits Control

    ''' <summary>Creates the dot.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        TabStop = False
    End Sub

    ''' <summary>Draws a filled circle in the control's foreground color.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Background))
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        Using brush As New SolidBrush(ForeColor)
            e.Graphics.FillEllipse(brush, 0, 0, Width - 1, Height - 1)
        End Using
    End Sub

    Protected Overrides Sub OnForeColorChanged(e As EventArgs)
        Invalidate()
        MyBase.OnForeColorChanged(e)
    End Sub
End Class

''' <summary>Panel with a rounded border, used for grouped sections and around the log.</summary>
Public Class BorderPanel
    Inherits Panel

    ''' <summary>Creates the panel and switches it to custom painting.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        BackColor = Theme.Surface
    End Sub

    ''' <summary>Fills the corners with the parent's color and draws the rounded panel on top.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.Clear(If(Parent IsNot Nothing, Parent.BackColor, Theme.Background))
        Theme.DrawRoundedBox(e.Graphics, New Rectangle(0, 0, Width - 1, Height - 1), Theme.Scale(6), BackColor, Theme.Border)
    End Sub
End Class

''' <summary>Draws menus and their drop-downs in the dark theme.</summary>
Public Class DarkMenuRenderer
    Inherits ToolStripProfessionalRenderer

    ''' <summary>Creates the renderer with the dark color table.</summary>
    Public Sub New()
        MyBase.New(New DarkMenuColors())
        RoundedEdges = False
    End Sub

    ''' <summary>Menu texts are white, or gray when disabled.</summary>
    Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
        e.TextColor = If(e.Item.Enabled, Theme.Text, Theme.TextMuted)
        MyBase.OnRenderItemText(e)
    End Sub

    ''' <summary>Sub-menu arrows use the text color.</summary>
    Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        e.ArrowColor = Theme.TextSecondary
        MyBase.OnRenderArrow(e)
    End Sub

    ''' <summary>Separators are a single dark line.</summary>
    Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
        Dim y = e.Item.Height \ 2
        Using pen As New Pen(Theme.Border)
            e.Graphics.DrawLine(pen, Theme.Scale(8), y, e.Item.Width - Theme.Scale(8), y)
        End Using
    End Sub

    ''' <summary>The menu bar itself has no border line.</summary>
    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        If TypeOf e.ToolStrip Is ToolStripDropDown Then MyBase.OnRenderToolStripBorder(e)
    End Sub
End Class

''' <summary>Colors used by <see cref="DarkMenuRenderer"/>.</summary>
Public Class DarkMenuColors
    Inherits ProfessionalColorTable

    Public Overrides ReadOnly Property MenuStripGradientBegin As Color
        Get
            Return Theme.Background
        End Get
    End Property

    Public Overrides ReadOnly Property MenuStripGradientEnd As Color
        Get
            Return Theme.Background
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelected As Color
        Get
            Return Theme.Hover
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
        Get
            Return Theme.Hover
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
        Get
            Return Theme.Hover
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemBorder As Color
        Get
            Return Theme.Hover
        End Get
    End Property

    Public Overrides ReadOnly Property MenuBorder As Color
        Get
            Return Theme.BorderStrong
        End Get
    End Property

    Public Overrides ReadOnly Property SeparatorDark As Color
        Get
            Return Theme.Border
        End Get
    End Property

    Public Overrides ReadOnly Property SeparatorLight As Color
        Get
            Return Theme.SurfaceRaised
        End Get
    End Property
End Class
