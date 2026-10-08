Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

''' <summary>
''' Lists the saved Forge versions so the user can pick one to restore. The installed version is marked and cannot be
''' chosen. A switch lets the user skip the version being left, so it is not offered again at the next start.
''' </summary>
Public Class RestoreForm
    Private ReadOnly _installedBuild As Date?

    ''' <summary>Creates the dialog.</summary>
    ''' <param name="packages">Saved versions, newest first.</param>
    ''' <param name="installation">Current installation, used to mark the installed version.</param>
    Public Sub New(packages As IEnumerable(Of StoredPackage), installation As ForgeInstallation)
        InitializeComponent()
        _installedBuild = installation.BuildDate
        Theme.Apply(Me)
        btnRestore.Glyph = Theme.Glyphs.History
        lstPackages.ItemHeight = Theme.Scale(34)

        For Each package In packages
            lstPackages.Items.Add(package)
        Next

        Dim installedPackage = packages.FirstOrDefault(Function(package) IsInstalled(package))
        Dim leftVersion = If(installedPackage IsNot Nothing, installedPackage.Version, If(installation.Version, "the current version"))
        tglSkip.Text = $"Don't offer {leftVersion} again, only newer builds"
        tglSkip.Checked = True
        tglSkip.Visible = _installedBuild.HasValue

        Dim firstChoice = lstPackages.Items.Cast(Of StoredPackage)().ToList().FindIndex(Function(package) Not IsInstalled(package))
        lstPackages.SelectedIndex = firstChoice
        btnRestore.Enabled = firstChoice >= 0
    End Sub

    ''' <summary>The version chosen by the user.</summary>
    Public ReadOnly Property SelectedPackage As StoredPackage
        Get
            Return TryCast(lstPackages.SelectedItem, StoredPackage)
        End Get
    End Property

    ''' <summary><c>True</c> when the version being left should not be offered again at startup.</summary>
    Public ReadOnly Property SkipLeftVersion As Boolean
        Get
            Return tglSkip.Visible AndAlso tglSkip.Checked
        End Get
    End Property

    ''' <summary><c>True</c> for the saved package of the installed build.</summary>
    Private Function IsInstalled(package As StoredPackage) As Boolean
        Return _installedBuild.HasValue AndAlso package.BuildDate.Equals(_installedBuild)
    End Function

    ''' <summary>The installed version cannot be chosen: selecting it clears the selection.</summary>
    Private Sub lstPackages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstPackages.SelectedIndexChanged
        Dim package = SelectedPackage
        If package IsNot Nothing AndAlso IsInstalled(package) Then lstPackages.SelectedIndex = -1
        btnRestore.Enabled = SelectedPackage IsNot Nothing
    End Sub

    ''' <summary>
    ''' Draws one version: a radio mark, the version, and on the right either an "Installed" badge or the channel and
    ''' build time.
    ''' </summary>
    Private Sub lstPackages_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstPackages.DrawItem
        If e.Index < 0 Then Return
        Dim package = DirectCast(lstPackages.Items(e.Index), StoredPackage)
        Dim installed = IsInstalled(package)
        Dim selected = (e.State And DrawItemState.Selected) = DrawItemState.Selected AndAlso Not installed

        Using back As New SolidBrush(If(selected, Color.FromArgb(28, 36, 22), Theme.Input))
            e.Graphics.FillRectangle(back, e.Bounds)
        End Using

        Dim left = e.Bounds.Left + Theme.Scale(10)
        Dim mark As String
        Dim markFont As Font
        If Theme.GlyphFontName IsNot Nothing Then
            mark = If(selected, Theme.Glyphs.RadioOn, Theme.Glyphs.RadioOff)
            markFont = New Font(Theme.GlyphFontName, 10.0F)
        Else
            mark = If(selected, Char.ConvertFromUtf32(&H25CF), Char.ConvertFromUtf32(&H25CB))
            markFont = New Font(Font, FontStyle.Regular)
        End If
        Using markFont
            Dim markColor = If(selected, Theme.AccentBright, If(installed, Theme.TextMuted, Theme.TextSecondary))
            Dim markBounds As New Rectangle(left, e.Bounds.Top, Theme.Scale(22), e.Bounds.Height)
            TextRenderer.DrawText(e.Graphics, mark, markFont, markBounds, markColor, TextFormatFlags.VerticalCenter Or TextFormatFlags.NoPadding)
        End Using

        Dim versionBounds As New Rectangle(left + Theme.Scale(26), e.Bounds.Top, e.Bounds.Width \ 2, e.Bounds.Height)
        TextRenderer.DrawText(e.Graphics, package.Version, Font, versionBounds, If(installed, Theme.TextMuted, Theme.Text),
                              TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)

        Dim rightBounds As New Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - Theme.Scale(12), e.Bounds.Height)
        Using small As New Font(Font.FontFamily, 8.25F)
            If installed Then
                Dim badgeText = "Installed"
                Dim badgeSize = TextRenderer.MeasureText(badgeText, small)
                Dim badge As New Rectangle(rightBounds.Right - badgeSize.Width - Theme.Scale(8), e.Bounds.Top + (e.Bounds.Height - badgeSize.Height - Theme.Scale(4)) \ 2,
                                           badgeSize.Width + Theme.Scale(8), badgeSize.Height + Theme.Scale(4))
                Theme.DrawRoundedBox(e.Graphics, badge, Theme.Scale(4), Color.FromArgb(31, 46, 20), Color.Empty)
                TextRenderer.DrawText(e.Graphics, badgeText, small, badge, Color.FromArgb(156, 198, 107), TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            Else
                Dim channel = If(package.Channel = UpdateChannel.Release, "Release", "Snapshot")
                Dim built = If(package.BuildDate.HasValue, $" · Built {ForgeInstallation.FormatBuildDate(package.BuildDate.Value)}", String.Empty)
                TextRenderer.DrawText(e.Graphics, channel & built, small, rightBounds, Theme.TextSecondary, TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            End If
        End Using
    End Sub
End Class
