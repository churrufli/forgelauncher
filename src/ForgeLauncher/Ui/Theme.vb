Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>
''' The launcher's dark theme: colors, fonts, icon glyphs and the native calls that make the title bar and
''' scroll bars dark on Windows 10 and 11.
''' </summary>
''' <remarks>
''' Windows Forms on .NET Framework has no dark mode, so standard controls are recolored here and the controls that
''' cannot be recolored (buttons, toggles, progress bars, menus) are drawn by the classes in DarkControls.vb.
''' Text colors meet the WCAG contrast recommendations: white on black is 21:1 and the green accent behind white
''' text is 5:1.
''' </remarks>
Friend NotInheritable Class Theme
    ''' <summary>Window background (near black).</summary>
    Public Shared ReadOnly Background As Color = Color.FromArgb(13, 13, 13)
    ''' <summary>Panels and grouped sections.</summary>
    Public Shared ReadOnly Surface As Color = Color.FromArgb(21, 21, 21)
    ''' <summary>Dialog footers and menus.</summary>
    Public Shared ReadOnly SurfaceRaised As Color = Color.FromArgb(28, 28, 28)
    ''' <summary>Text boxes, combo boxes and lists.</summary>
    Public Shared ReadOnly Input As Color = Color.FromArgb(26, 26, 26)
    ''' <summary>Hovered items.</summary>
    Public Shared ReadOnly Hover As Color = Color.FromArgb(38, 38, 38)
    ''' <summary>Selected items and the active segment of a selector.</summary>
    Public Shared ReadOnly Selection As Color = Color.FromArgb(46, 46, 46)
    ''' <summary>Thin borders.</summary>
    Public Shared ReadOnly Border As Color = Color.FromArgb(42, 42, 42)
    ''' <summary>Borders of buttons and inputs.</summary>
    Public Shared ReadOnly BorderStrong As Color = Color.FromArgb(58, 58, 58)
    ''' <summary>Main text (white).</summary>
    Public Shared ReadOnly Text As Color = Color.FromArgb(245, 245, 245)
    ''' <summary>Secondary text, such as hints and dates.</summary>
    Public Shared ReadOnly TextSecondary As Color = Color.FromArgb(163, 163, 163)
    ''' <summary>Least important text, such as log timestamps and disabled controls.</summary>
    Public Shared ReadOnly TextMuted As Color = Color.FromArgb(115, 115, 115)
    ''' <summary>Accent green taken from the launcher icon, darkened so white text on it stays readable.</summary>
    Public Shared ReadOnly Accent As Color = Color.FromArgb(78, 122, 36)
    ''' <summary>Accent while the mouse is over it.</summary>
    Public Shared ReadOnly AccentHover As Color = Color.FromArgb(68, 108, 31)
    ''' <summary>Accent while pressed.</summary>
    Public Shared ReadOnly AccentPressed As Color = Color.FromArgb(58, 94, 27)
    ''' <summary>Brighter green for progress bars, focus outlines and the "up to date" dot.</summary>
    Public Shared ReadOnly AccentBright As Color = Color.FromArgb(127, 176, 74)
    ''' <summary>"Update available" dot.</summary>
    Public Shared ReadOnly Warning As Color = Color.FromArgb(217, 164, 65)
    ''' <summary>Text of dangerous actions such as uninstalling.</summary>
    Public Shared ReadOnly DangerText As Color = Color.FromArgb(240, 138, 138)
    ''' <summary>Border of dangerous actions.</summary>
    Public Shared ReadOnly DangerBorder As Color = Color.FromArgb(90, 42, 42)

    ''' <summary>Icon glyphs of the Windows icon fonts (Segoe Fluent Icons / Segoe MDL2 Assets).</summary>
    Public NotInheritable Class Glyphs
        Public Shared ReadOnly Play As String = Char.ConvertFromUtf32(&HE768)
        Public Shared ReadOnly Refresh As String = Char.ConvertFromUtf32(&HE72C)
        Public Shared ReadOnly Download As String = Char.ConvertFromUtf32(&HE896)
        Public Shared ReadOnly Settings As String = Char.ConvertFromUtf32(&HE713)
        Public Shared ReadOnly History As String = Char.ConvertFromUtf32(&HE81C)
        Public Shared ReadOnly Folder As String = Char.ConvertFromUtf32(&HE8B7)
        Public Shared ReadOnly Document As String = Char.ConvertFromUtf32(&HE8A5)
        Public Shared ReadOnly Delete As String = Char.ConvertFromUtf32(&HE74D)
        Public Shared ReadOnly Cancel As String = Char.ConvertFromUtf32(&HE711)
        Public Shared ReadOnly CheckMark As String = Char.ConvertFromUtf32(&HE73E)
        Public Shared ReadOnly OpenFile As String = Char.ConvertFromUtf32(&HE8E5)
        Public Shared ReadOnly Info As String = Char.ConvertFromUtf32(&HE946)
        Public Shared ReadOnly Warning As String = Char.ConvertFromUtf32(&HE7BA)
        Public Shared ReadOnly ErrorBadge As String = Char.ConvertFromUtf32(&HEA39)
        Public Shared ReadOnly Help As String = Char.ConvertFromUtf32(&HE897)
        Public Shared ReadOnly Copy As String = Char.ConvertFromUtf32(&HE8C8)
        Public Shared ReadOnly Link As String = Char.ConvertFromUtf32(&HE71B)
        Public Shared ReadOnly RadioOn As String = Char.ConvertFromUtf32(&HECCB)
        Public Shared ReadOnly RadioOff As String = Char.ConvertFromUtf32(&HECCA)
    End Class

    Private Shared _glyphFontName As String
    Private Shared _glyphFontChecked As Boolean
    Private Shared _dpiScale As Single = 0

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Name of the icon font available on this computer: Segoe Fluent Icons (Windows 11), Segoe MDL2 Assets
    ''' (Windows 10), or <c>Nothing</c> on older Windows, where buttons show text only.
    ''' </summary>
    Public Shared ReadOnly Property GlyphFontName As String
        Get
            If Not _glyphFontChecked Then
                _glyphFontChecked = True
                Using fonts As New InstalledFontCollection()
                    Dim names = fonts.Families.Select(Function(family) family.Name).ToList()
                    _glyphFontName = {"Segoe Fluent Icons", "Segoe MDL2 Assets"}.FirstOrDefault(Function(name) names.Contains(name))
                End Using
            End If
            Return _glyphFontName
        End Get
    End Property

    ''' <summary>Monospaced font for the log: Cascadia Mono (Windows 11) or Consolas.</summary>
    Public Shared Function CreateMonoFont() As Font
        Using fonts As New InstalledFontCollection()
            Dim hasCascadia = fonts.Families.Any(Function(family) family.Name = "Cascadia Mono")
            Return New Font(If(hasCascadia, "Cascadia Mono", "Consolas"), 9.0F)
        End Using
    End Function

    ''' <summary>
    ''' Ratio between the screen DPI and 96 DPI. Custom controls multiply their pixel sizes by it so they keep their
    ''' proportions on high-resolution screens.
    ''' </summary>
    Public Shared ReadOnly Property DpiScale As Single
        Get
            If _dpiScale = 0 Then
                Using graphics = Drawing.Graphics.FromHwnd(IntPtr.Zero)
                    _dpiScale = graphics.DpiX / 96.0F
                End Using
            End If
            Return _dpiScale
        End Get
    End Property

    ''' <summary>Scales a size in 96-DPI pixels to the current screen.</summary>
    Public Shared Function Scale(pixels As Integer) As Integer
        Return CInt(Math.Round(pixels * DpiScale))
    End Function

    ''' <summary>Loads the launcher icon embedded in the executable at the size closest to the one requested.</summary>
    ''' <param name="size">Wanted size in pixels.</param>
    Public Shared Function AppIcon(size As Integer) As Icon
        Using stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ForgeLauncher.ic_launcher.ico")
            If stream Is Nothing Then Return Icon.ExtractAssociatedIcon(Application.ExecutablePath)
            Return New Icon(stream, New Size(size, size))
        End Using
    End Function

    ''' <summary>
    ''' Applies the theme to a form and all its controls: colors of standard controls, dark title bar,
    ''' dark scroll bars and dark drop-down lists.
    ''' </summary>
    ''' <param name="form">Form to theme; call it after InitializeComponent.</param>
    Public Shared Sub Apply(form As Form)
        form.BackColor = Background
        form.ForeColor = Text
        If form.IsHandleCreated Then
            UseDarkTitleBar(form.Handle)
        Else
            AddHandler form.HandleCreated, Sub(sender, e) UseDarkTitleBar(form.Handle)
        End If
        ApplyToChildren(form)
    End Sub

    ''' <summary>Recolors the standard controls inside a container (custom controls draw themselves).</summary>
    Private Shared Sub ApplyToChildren(parent As Control)
        For Each child As Control In parent.Controls
            Select Case True
                Case TypeOf child Is TextBox
                    Dim box = DirectCast(child, TextBox)
                    box.BackColor = Input
                    box.ForeColor = Text
                    If box.Multiline Then UseDarkScrollBars(box)
                Case TypeOf child Is ComboBox
                    StyleComboBox(DirectCast(child, ComboBox))
                Case TypeOf child Is NumericUpDown
                    child.BackColor = Input
                    child.ForeColor = Text
                Case TypeOf child Is ListBox
                    child.BackColor = Input
                    child.ForeColor = Text
                    UseDarkScrollBars(child)
            End Select
            If child.HasChildren Then ApplyToChildren(child)
        Next
    End Sub

    ''' <summary>
    ''' Makes a drop-down list dark: flat style, owner-drawn items in theme colors and, on Windows 10/11, the dark
    ''' system theme for its arrow and pop-up list.
    ''' </summary>
    Public Shared Sub StyleComboBox(combo As ComboBox)
        combo.FlatStyle = FlatStyle.Flat
        combo.BackColor = Input
        combo.ForeColor = Text
        combo.DrawMode = DrawMode.OwnerDrawFixed
        AddHandler combo.DrawItem,
            Sub(sender, e)
                If e.Index < 0 Then Return
                Dim selected = (e.State And DrawItemState.Selected) = DrawItemState.Selected AndAlso
                               (e.State And DrawItemState.ComboBoxEdit) <> DrawItemState.ComboBoxEdit
                Using back As New SolidBrush(If(selected, Selection, Input))
                    e.Graphics.FillRectangle(back, e.Bounds)
                End Using
                Dim itemText = combo.GetItemText(combo.Items(e.Index))
                Dim itemColor = If(combo.Enabled, Text, TextMuted)
                TextRenderer.DrawText(e.Graphics, itemText, combo.Font, Rectangle.Inflate(e.Bounds, -4, 0), itemColor,
                                      TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)
            End Sub
        UseDarkSystemTheme(combo, "DarkMode_CFD")
    End Sub

    ''' <summary>Uses the dark Explorer theme for a control's scroll bars (Windows 10 version 1809 and later).</summary>
    Public Shared Sub UseDarkScrollBars(control As Control)
        UseDarkSystemTheme(control, "DarkMode_Explorer")
    End Sub

    ''' <summary>
    ''' Asks Windows to draw the title bar of a window dark. Attribute 20 is used by Windows 10 20H1 and later,
    ''' attribute 19 by earlier Windows 10 builds; older systems ignore both.
    ''' </summary>
    Public Shared Sub UseDarkTitleBar(handle As IntPtr)
        Dim enabled = 1
        Try
            If NativeMethods.DwmSetWindowAttribute(handle, 20, enabled, 4) <> 0 Then
                NativeMethods.DwmSetWindowAttribute(handle, 19, enabled, 4)
            End If
        Catch ex As Exception When TypeOf ex Is DllNotFoundException OrElse TypeOf ex Is EntryPointNotFoundException
        End Try
    End Sub

    ''' <summary>Fills a rounded rectangle and optionally draws its border.</summary>
    ''' <param name="graphics">Target surface.</param>
    ''' <param name="bounds">Rectangle to draw.</param>
    ''' <param name="radius">Corner radius in pixels.</param>
    ''' <param name="fill">Fill color.</param>
    ''' <param name="border">Border color; <see cref="Color.Empty"/> draws no border.</param>
    Public Shared Sub DrawRoundedBox(graphics As Graphics, bounds As Rectangle, radius As Integer, fill As Color, border As Color)
        If bounds.Width <= 1 OrElse bounds.Height <= 1 Then Return
        Dim previous = graphics.SmoothingMode
        graphics.SmoothingMode = SmoothingMode.AntiAlias
        Using path = RoundedRectangle(bounds, radius)
            Using brush As New SolidBrush(fill)
                graphics.FillPath(brush, path)
            End Using
            If border <> Color.Empty Then
                Using pen As New Pen(border)
                    graphics.DrawPath(pen, path)
                End Using
            End If
        End Using
        graphics.SmoothingMode = previous
    End Sub

    ''' <summary>Builds the outline of a rectangle with rounded corners.</summary>
    Public Shared Function RoundedRectangle(bounds As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter = Math.Max(1, Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height)))
        Dim arc As New Rectangle(bounds.X, bounds.Y, diameter, diameter)
        path.AddArc(arc, 180, 90)
        arc.X = bounds.Right - diameter
        path.AddArc(arc, 270, 90)
        arc.Y = bounds.Bottom - diameter
        path.AddArc(arc, 0, 90)
        arc.X = bounds.X
        path.AddArc(arc, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    ''' <summary>
    ''' Makes a tool tip dark by drawing it ourselves. The native tool tip ignores BackColor when visual styles are on.
    ''' </summary>
    Public Shared Sub StyleToolTip(tip As ToolTip)
        tip.OwnerDraw = True
        tip.BackColor = SurfaceRaised
        tip.ForeColor = Text
        AddHandler tip.Draw,
            Sub(sender, e)
                Using back As New SolidBrush(SurfaceRaised)
                    e.Graphics.FillRectangle(back, e.Bounds)
                End Using
                Using pen As New Pen(BorderStrong)
                    e.Graphics.DrawRectangle(pen, New Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1))
                End Using
                TextRenderer.DrawText(e.Graphics, e.ToolTipText, e.Font, e.Bounds, Text,
                                      TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            End Sub
    End Sub

    ''' <summary>Applies a dark system theme to a control once its window exists.</summary>
    Private Shared Sub UseDarkSystemTheme(control As Control, themeName As String)
        Dim applyTheme As Action =
            Sub()
                Try
                    NativeMethods.SetWindowTheme(control.Handle, themeName, Nothing)
                Catch ex As Exception When TypeOf ex Is DllNotFoundException OrElse TypeOf ex Is EntryPointNotFoundException
                End Try
            End Sub
        If control.IsHandleCreated Then
            applyTheme()
        Else
            AddHandler control.HandleCreated, Sub(sender, e) applyTheme()
        End If
    End Sub

    ''' <summary>Windows functions used by the theme.</summary>
    Private NotInheritable Class NativeMethods
        <DllImport("dwmapi.dll")>
        Public Shared Function DwmSetWindowAttribute(hwnd As IntPtr, attribute As Integer, ByRef value As Integer, size As Integer) As Integer
        End Function

        <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
        Public Shared Function SetWindowTheme(hwnd As IntPtr, appName As String, idList As String) As Integer
        End Function
    End Class
End Class
