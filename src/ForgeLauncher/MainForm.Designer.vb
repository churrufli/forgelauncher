<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.menuMain = New System.Windows.Forms.MenuStrip()
        Me.mnuForge = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOpenForgeFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOpenUserFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOpenForgeLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuForgeSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRestore = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuResetPreferences = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReportProblem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuForgeWebsite = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuForgeReleases = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuForgeDiscord = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLauncherWebsite = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCheckLauncherUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.picForge = New System.Windows.Forms.PictureBox()
        Me.lblForgeVersion = New System.Windows.Forms.Label()
        Me.dotState = New ForgeLauncher.StatusDot()
        Me.lblForgeState = New System.Windows.Forms.Label()
        Me.segChannel = New ForgeLauncher.SegmentedSelector()
        Me.cmbTarget = New System.Windows.Forms.ComboBox()
        Me.chkPortable = New ForgeLauncher.ToggleSwitch()
        Me.btnSettings = New ForgeLauncher.DarkButton()
        Me.btnCheck = New ForgeLauncher.DarkButton()
        Me.btnLaunch = New ForgeLauncher.DarkButton()
        Me.pnlLog = New ForgeLauncher.BorderPanel()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.barProgress = New ForgeLauncher.ThinProgressBar()
        Me.btnStatusAction = New ForgeLauncher.DarkButton()
        Me.tipMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.menuMain.SuspendLayout()
        CType(Me.picForge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLog.SuspendLayout()
        Me.SuspendLayout()

        Me.menuMain.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(13, Byte), Integer), CType(CType(13, Byte), Integer))
        Me.menuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuForge, Me.mnuSettings, Me.mnuHelp})
        Me.menuMain.Location = New System.Drawing.Point(0, 0)
        Me.menuMain.Name = "menuMain"
        Me.menuMain.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.menuMain.Size = New System.Drawing.Size(620, 24)
        Me.menuMain.TabIndex = 0

        Me.mnuForge.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOpenForgeFolder, Me.mnuOpenUserFolder, Me.mnuOpenForgeLog, Me.mnuForgeSeparator, Me.mnuRestore, Me.mnuResetPreferences})
        Me.mnuForge.Name = "mnuForge"
        Me.mnuForge.Size = New System.Drawing.Size(51, 20)
        Me.mnuForge.Text = "&Forge"

        Me.mnuOpenForgeFolder.Name = "mnuOpenForgeFolder"
        Me.mnuOpenForgeFolder.Size = New System.Drawing.Size(240, 22)
        Me.mnuOpenForgeFolder.Text = "Open Forge folder"

        Me.mnuOpenUserFolder.Name = "mnuOpenUserFolder"
        Me.mnuOpenUserFolder.Size = New System.Drawing.Size(240, 22)
        Me.mnuOpenUserFolder.Text = "Open user data folder"

        Me.mnuOpenForgeLog.Name = "mnuOpenForgeLog"
        Me.mnuOpenForgeLog.Size = New System.Drawing.Size(240, 22)
        Me.mnuOpenForgeLog.Text = "Open forge.log"

        Me.mnuForgeSeparator.Name = "mnuForgeSeparator"
        Me.mnuForgeSeparator.Size = New System.Drawing.Size(237, 6)

        Me.mnuRestore.Name = "mnuRestore"
        Me.mnuRestore.Size = New System.Drawing.Size(240, 22)
        Me.mnuRestore.Text = "Restore a previous version..."

        Me.mnuResetPreferences.Name = "mnuResetPreferences"
        Me.mnuResetPreferences.Size = New System.Drawing.Size(240, 22)
        Me.mnuResetPreferences.Text = "Reset Forge preferences..."

        Me.mnuSettings.Name = "mnuSettings"
        Me.mnuSettings.Size = New System.Drawing.Size(61, 20)
        Me.mnuSettings.Text = "&Settings"

        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportProblem, Me.mnuHelpSeparator1, Me.mnuForgeWebsite, Me.mnuForgeReleases, Me.mnuForgeDiscord, Me.mnuHelpSeparator2, Me.mnuLauncherWebsite, Me.mnuCheckLauncherUpdates, Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "&Help"

        Me.mnuReportProblem.Name = "mnuReportProblem"
        Me.mnuReportProblem.Size = New System.Drawing.Size(270, 22)
        Me.mnuReportProblem.Text = "Report a problem..."

        Me.mnuHelpSeparator1.Name = "mnuHelpSeparator1"
        Me.mnuHelpSeparator1.Size = New System.Drawing.Size(267, 6)

        Me.mnuForgeWebsite.Name = "mnuForgeWebsite"
        Me.mnuForgeWebsite.Size = New System.Drawing.Size(270, 22)
        Me.mnuForgeWebsite.Text = "Forge website"

        Me.mnuForgeReleases.Name = "mnuForgeReleases"
        Me.mnuForgeReleases.Size = New System.Drawing.Size(270, 22)
        Me.mnuForgeReleases.Text = "Forge releases on GitHub"

        Me.mnuForgeDiscord.Name = "mnuForgeDiscord"
        Me.mnuForgeDiscord.Size = New System.Drawing.Size(270, 22)
        Me.mnuForgeDiscord.Text = "Forge Discord"

        Me.mnuHelpSeparator2.Name = "mnuHelpSeparator2"
        Me.mnuHelpSeparator2.Size = New System.Drawing.Size(267, 6)

        Me.mnuLauncherWebsite.Name = "mnuLauncherWebsite"
        Me.mnuLauncherWebsite.Size = New System.Drawing.Size(270, 22)
        Me.mnuLauncherWebsite.Text = "Forge Launcher on GitHub"

        Me.mnuCheckLauncherUpdates.Name = "mnuCheckLauncherUpdates"
        Me.mnuCheckLauncherUpdates.Size = New System.Drawing.Size(270, 22)
        Me.mnuCheckLauncherUpdates.Text = "Check for Forge Launcher updates"

        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(270, 22)
        Me.mnuAbout.Text = "About Forge Launcher"

        Me.picForge.Location = New System.Drawing.Point(16, 34)
        Me.picForge.Name = "picForge"
        Me.picForge.Size = New System.Drawing.Size(40, 40)
        Me.picForge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picForge.TabIndex = 1
        Me.picForge.TabStop = False

        Me.lblForgeVersion.AutoSize = True
        Me.lblForgeVersion.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblForgeVersion.Location = New System.Drawing.Point(64, 31)
        Me.lblForgeVersion.Name = "lblForgeVersion"
        Me.lblForgeVersion.Size = New System.Drawing.Size(50, 21)
        Me.lblForgeVersion.TabIndex = 2
        Me.lblForgeVersion.Text = "Forge"

        Me.dotState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(115, Byte), Integer), CType(CType(115, Byte), Integer), CType(CType(115, Byte), Integer))
        Me.dotState.Location = New System.Drawing.Point(66, 60)
        Me.dotState.Name = "dotState"
        Me.dotState.Size = New System.Drawing.Size(8, 8)
        Me.dotState.TabIndex = 3

        Me.lblForgeState.AutoSize = True
        Me.lblForgeState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblForgeState.Location = New System.Drawing.Point(78, 55)
        Me.lblForgeState.Name = "lblForgeState"
        Me.lblForgeState.Size = New System.Drawing.Size(0, 15)
        Me.lblForgeState.TabIndex = 4

        Me.segChannel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.segChannel.Items = New String() {"Snapshot", "Release"}
        Me.segChannel.Location = New System.Drawing.Point(434, 39)
        Me.segChannel.Name = "segChannel"
        Me.segChannel.Size = New System.Drawing.Size(170, 28)
        Me.segChannel.TabIndex = 5

        Me.cmbTarget.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTarget.FormattingEnabled = True
        Me.cmbTarget.Location = New System.Drawing.Point(16, 96)
        Me.cmbTarget.Name = "cmbTarget"
        Me.cmbTarget.Size = New System.Drawing.Size(232, 23)
        Me.cmbTarget.TabIndex = 6

        Me.chkPortable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkPortable.Location = New System.Drawing.Point(16, 92)
        Me.chkPortable.Name = "chkPortable"
        Me.chkPortable.Size = New System.Drawing.Size(232, 30)
        Me.chkPortable.TabIndex = 7
        Me.chkPortable.Text = "Portable install"
        Me.chkPortable.Visible = False

        Me.btnSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSettings.Location = New System.Drawing.Point(254, 92)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(32, 30)
        Me.btnSettings.TabIndex = 8

        Me.btnCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheck.Location = New System.Drawing.Point(292, 92)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(156, 30)
        Me.btnCheck.TabIndex = 9
        Me.btnCheck.Text = "Check for updates"

        Me.btnLaunch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLaunch.Location = New System.Drawing.Point(454, 92)
        Me.btnLaunch.Name = "btnLaunch"
        Me.btnLaunch.Size = New System.Drawing.Size(150, 30)
        Me.btnLaunch.TabIndex = 10
        Me.btnLaunch.Text = "Launch Forge"
        Me.btnLaunch.Variant = ForgeLauncher.ButtonVariant.Primary

        Me.pnlLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLog.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer))
        Me.pnlLog.Controls.Add(Me.txtLog)
        Me.pnlLog.Location = New System.Drawing.Point(16, 134)
        Me.pnlLog.Name = "pnlLog"
        Me.pnlLog.Padding = New System.Windows.Forms.Padding(10, 8, 4, 8)
        Me.pnlLog.Size = New System.Drawing.Size(588, 252)
        Me.pnlLog.TabIndex = 11

        Me.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(10, 8)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(574, 236)
        Me.txtLog.TabIndex = 0
        Me.txtLog.TabStop = False

        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoEllipsis = True
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblStatus.Location = New System.Drawing.Point(16, 397)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(320, 30)
        Me.lblStatus.TabIndex = 12
        Me.lblStatus.Text = "Ready"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.barProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.barProgress.Location = New System.Drawing.Point(344, 410)
        Me.barProgress.Name = "barProgress"
        Me.barProgress.Size = New System.Drawing.Size(160, 4)
        Me.barProgress.TabIndex = 13
        Me.barProgress.Visible = False

        Me.btnStatusAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStatusAction.Location = New System.Drawing.Point(512, 397)
        Me.btnStatusAction.Name = "btnStatusAction"
        Me.btnStatusAction.Size = New System.Drawing.Size(92, 30)
        Me.btnStatusAction.TabIndex = 14
        Me.btnStatusAction.Text = "Cancel"
        Me.btnStatusAction.Variant = ForgeLauncher.ButtonVariant.Subtle
        Me.btnStatusAction.Visible = False

        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(620, 440)
        Me.Controls.Add(Me.picForge)
        Me.Controls.Add(Me.lblForgeVersion)
        Me.Controls.Add(Me.dotState)
        Me.Controls.Add(Me.lblForgeState)
        Me.Controls.Add(Me.segChannel)
        Me.Controls.Add(Me.cmbTarget)
        Me.Controls.Add(Me.chkPortable)
        Me.Controls.Add(Me.btnSettings)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.btnLaunch)
        Me.Controls.Add(Me.pnlLog)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.barProgress)
        Me.Controls.Add(Me.btnStatusAction)
        Me.Controls.Add(Me.menuMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.menuMain
        Me.MinimumSize = New System.Drawing.Size(560, 380)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forge Launcher"
        Me.menuMain.ResumeLayout(False)
        Me.menuMain.PerformLayout()
        CType(Me.picForge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLog.ResumeLayout(False)
        Me.pnlLog.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents menuMain As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuForge As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenForgeFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenUserFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenForgeLog As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuForgeSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRestore As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuResetPreferences As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportProblem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuForgeWebsite As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuForgeReleases As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuForgeDiscord As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuLauncherWebsite As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCheckLauncherUpdates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents picForge As System.Windows.Forms.PictureBox
    Friend WithEvents lblForgeVersion As System.Windows.Forms.Label
    Friend WithEvents dotState As ForgeLauncher.StatusDot
    Friend WithEvents lblForgeState As System.Windows.Forms.Label
    Friend WithEvents segChannel As ForgeLauncher.SegmentedSelector
    Friend WithEvents cmbTarget As System.Windows.Forms.ComboBox
    Friend WithEvents chkPortable As ForgeLauncher.ToggleSwitch
    Friend WithEvents btnSettings As ForgeLauncher.DarkButton
    Friend WithEvents btnCheck As ForgeLauncher.DarkButton
    Friend WithEvents btnLaunch As ForgeLauncher.DarkButton
    Friend WithEvents pnlLog As ForgeLauncher.BorderPanel
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents barProgress As ForgeLauncher.ThinProgressBar
    Friend WithEvents btnStatusAction As ForgeLauncher.DarkButton
    Friend WithEvents tipMain As System.Windows.Forms.ToolTip
End Class
