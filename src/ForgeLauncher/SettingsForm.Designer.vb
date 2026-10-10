<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SettingsForm
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
        Me.lblUpdatesHeader = New System.Windows.Forms.Label()
        Me.pnlUpdates = New ForgeLauncher.BorderPanel()
        Me.tglCheckForge = New ForgeLauncher.ToggleSwitch()
        Me.tglCheckLauncher = New ForgeLauncher.ToggleSwitch()
        Me.tglAskToLaunch = New ForgeLauncher.ToggleSwitch()
        Me.lblVersionsHeader = New System.Windows.Forms.Label()
        Me.pnlVersions = New ForgeLauncher.BorderPanel()
        Me.lblKeep = New System.Windows.Forms.Label()
        Me.lblKeepInfo = New System.Windows.Forms.Label()
        Me.segKeep = New ForgeLauncher.SegmentedSelector()
        Me.lblRestoreHint = New System.Windows.Forms.Label()
        Me.btnRestore = New ForgeLauncher.DarkButton()
        Me.lblLaunchHeader = New System.Windows.Forms.Label()
        Me.pnlLaunch = New ForgeLauncher.BorderPanel()
        Me.lblMode = New System.Windows.Forms.Label()
        Me.segMode = New ForgeLauncher.SegmentedSelector()
        Me.lblApp = New System.Windows.Forms.Label()
        Me.cmbApp = New System.Windows.Forms.ComboBox()
        Me.lblJava = New System.Windows.Forms.Label()
        Me.txtJava = New System.Windows.Forms.TextBox()
        Me.btnBrowseJava = New ForgeLauncher.DarkButton()
        Me.lblMemory = New System.Windows.Forms.Label()
        Me.numMemory = New System.Windows.Forms.NumericUpDown()
        Me.lblRam = New System.Windows.Forms.Label()
        Me.lblArgs = New System.Windows.Forms.Label()
        Me.txtArgs = New System.Windows.Forms.TextBox()
        Me.lblMaintenanceHeader = New System.Windows.Forms.Label()
        Me.btnOpenData = New ForgeLauncher.DarkButton()
        Me.btnOpenLog = New ForgeLauncher.DarkButton()
        Me.btnUninstall = New ForgeLauncher.DarkButton()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnCancel = New ForgeLauncher.DarkButton()
        Me.btnSave = New ForgeLauncher.DarkButton()
        Me.pnlUpdates.SuspendLayout()
        Me.pnlVersions.SuspendLayout()
        Me.pnlLaunch.SuspendLayout()
        CType(Me.numMemory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.SuspendLayout()

        Me.lblUpdatesHeader.AutoSize = True
        Me.lblUpdatesHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdatesHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.lblUpdatesHeader.Location = New System.Drawing.Point(16, 12)
        Me.lblUpdatesHeader.Name = "lblUpdatesHeader"
        Me.lblUpdatesHeader.Size = New System.Drawing.Size(58, 13)
        Me.lblUpdatesHeader.TabIndex = 0
        Me.lblUpdatesHeader.Text = "UPDATES"

        Me.pnlUpdates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlUpdates.Controls.Add(Me.tglCheckForge)
        Me.pnlUpdates.Controls.Add(Me.tglCheckLauncher)
        Me.pnlUpdates.Controls.Add(Me.tglAskToLaunch)
        Me.pnlUpdates.Location = New System.Drawing.Point(16, 32)
        Me.pnlUpdates.Name = "pnlUpdates"
        Me.pnlUpdates.Size = New System.Drawing.Size(488, 112)
        Me.pnlUpdates.TabIndex = 1

        Me.tglCheckForge.Location = New System.Drawing.Point(12, 6)
        Me.tglCheckForge.Name = "tglCheckForge"
        Me.tglCheckForge.Size = New System.Drawing.Size(464, 32)
        Me.tglCheckForge.TabIndex = 0
        Me.tglCheckForge.Text = "Check for Forge updates on startup"

        Me.tglCheckLauncher.Location = New System.Drawing.Point(12, 40)
        Me.tglCheckLauncher.Name = "tglCheckLauncher"
        Me.tglCheckLauncher.Size = New System.Drawing.Size(464, 32)
        Me.tglCheckLauncher.TabIndex = 1
        Me.tglCheckLauncher.Text = "Check for Forge Launcher updates on startup"

        Me.tglAskToLaunch.Location = New System.Drawing.Point(12, 74)
        Me.tglAskToLaunch.Name = "tglAskToLaunch"
        Me.tglAskToLaunch.Size = New System.Drawing.Size(464, 32)
        Me.tglAskToLaunch.TabIndex = 2
        Me.tglAskToLaunch.Text = "Offer to start Forge when it is up to date"

        Me.lblVersionsHeader.AutoSize = True
        Me.lblVersionsHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersionsHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.lblVersionsHeader.Location = New System.Drawing.Point(16, 156)
        Me.lblVersionsHeader.Name = "lblVersionsHeader"
        Me.lblVersionsHeader.Size = New System.Drawing.Size(115, 13)
        Me.lblVersionsHeader.TabIndex = 2
        Me.lblVersionsHeader.Text = "PREVIOUS VERSIONS"

        Me.pnlVersions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlVersions.Controls.Add(Me.lblKeep)
        Me.pnlVersions.Controls.Add(Me.lblKeepInfo)
        Me.pnlVersions.Controls.Add(Me.segKeep)
        Me.pnlVersions.Controls.Add(Me.lblRestoreHint)
        Me.pnlVersions.Controls.Add(Me.btnRestore)
        Me.pnlVersions.Location = New System.Drawing.Point(16, 176)
        Me.pnlVersions.Name = "pnlVersions"
        Me.pnlVersions.Size = New System.Drawing.Size(488, 90)
        Me.pnlVersions.TabIndex = 3

        Me.lblKeep.AutoSize = True
        Me.lblKeep.Location = New System.Drawing.Point(12, 10)
        Me.lblKeep.Name = "lblKeep"
        Me.lblKeep.Size = New System.Drawing.Size(164, 15)
        Me.lblKeep.TabIndex = 0
        Me.lblKeep.Text = "Keep previous Forge versions"

        Me.lblKeepInfo.AutoEllipsis = True
        Me.lblKeepInfo.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblKeepInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblKeepInfo.Location = New System.Drawing.Point(12, 29)
        Me.lblKeepInfo.Name = "lblKeepInfo"
        Me.lblKeepInfo.Size = New System.Drawing.Size(286, 15)
        Me.lblKeepInfo.TabIndex = 1

        Me.segKeep.Items = New String() {"None", "1", "2", "3"}
        Me.segKeep.Location = New System.Drawing.Point(306, 12)
        Me.segKeep.Name = "segKeep"
        Me.segKeep.Size = New System.Drawing.Size(170, 28)
        Me.segKeep.TabIndex = 2

        Me.lblRestoreHint.AutoSize = True
        Me.lblRestoreHint.Location = New System.Drawing.Point(12, 60)
        Me.lblRestoreHint.Name = "lblRestoreHint"
        Me.lblRestoreHint.Size = New System.Drawing.Size(150, 15)
        Me.lblRestoreHint.TabIndex = 3
        Me.lblRestoreHint.Text = "Return to a saved version"

        Me.btnRestore.Location = New System.Drawing.Point(346, 53)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(130, 28)
        Me.btnRestore.TabIndex = 4
        Me.btnRestore.Text = "Restore..."

        Me.lblLaunchHeader.AutoSize = True
        Me.lblLaunchHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLaunchHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.lblLaunchHeader.Location = New System.Drawing.Point(16, 278)
        Me.lblLaunchHeader.Name = "lblLaunchHeader"
        Me.lblLaunchHeader.Size = New System.Drawing.Size(48, 13)
        Me.lblLaunchHeader.TabIndex = 4
        Me.lblLaunchHeader.Text = "LAUNCH"

        Me.pnlLaunch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLaunch.Controls.Add(Me.lblMode)
        Me.pnlLaunch.Controls.Add(Me.segMode)
        Me.pnlLaunch.Controls.Add(Me.lblApp)
        Me.pnlLaunch.Controls.Add(Me.cmbApp)
        Me.pnlLaunch.Controls.Add(Me.lblJava)
        Me.pnlLaunch.Controls.Add(Me.txtJava)
        Me.pnlLaunch.Controls.Add(Me.btnBrowseJava)
        Me.pnlLaunch.Controls.Add(Me.lblMemory)
        Me.pnlLaunch.Controls.Add(Me.numMemory)
        Me.pnlLaunch.Controls.Add(Me.lblRam)
        Me.pnlLaunch.Controls.Add(Me.lblArgs)
        Me.pnlLaunch.Controls.Add(Me.txtArgs)
        Me.pnlLaunch.Location = New System.Drawing.Point(16, 298)
        Me.pnlLaunch.Name = "pnlLaunch"
        Me.pnlLaunch.Size = New System.Drawing.Size(488, 200)
        Me.pnlLaunch.TabIndex = 5

        Me.lblMode.AutoSize = True
        Me.lblMode.Location = New System.Drawing.Point(12, 16)
        Me.lblMode.Name = "lblMode"
        Me.lblMode.Size = New System.Drawing.Size(76, 15)
        Me.lblMode.TabIndex = 0
        Me.lblMode.Text = "Launch mode"

        Me.segMode.Items = New String() {"Normal", "Custom Java"}
        Me.segMode.Location = New System.Drawing.Point(276, 10)
        Me.segMode.Name = "segMode"
        Me.segMode.Size = New System.Drawing.Size(200, 28)
        Me.segMode.TabIndex = 1

        Me.lblApp.AutoSize = True
        Me.lblApp.Location = New System.Drawing.Point(12, 54)
        Me.lblApp.Name = "lblApp"
        Me.lblApp.Size = New System.Drawing.Size(68, 15)
        Me.lblApp.TabIndex = 2
        Me.lblApp.Text = "Application"

        Me.cmbApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbApp.FormattingEnabled = True
        Me.cmbApp.Items.AddRange(New Object() {"Forge", "Forge Adventure"})
        Me.cmbApp.Location = New System.Drawing.Point(136, 50)
        Me.cmbApp.Name = "cmbApp"
        Me.cmbApp.Size = New System.Drawing.Size(340, 23)
        Me.cmbApp.TabIndex = 3

        Me.lblJava.AutoSize = True
        Me.lblJava.Location = New System.Drawing.Point(12, 86)
        Me.lblJava.Name = "lblJava"
        Me.lblJava.Size = New System.Drawing.Size(91, 15)
        Me.lblJava.TabIndex = 4
        Me.lblJava.Text = "Java executable"

        Me.txtJava.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtJava.Location = New System.Drawing.Point(136, 83)
        Me.txtJava.Name = "txtJava"
        Me.txtJava.Size = New System.Drawing.Size(302, 23)
        Me.txtJava.TabIndex = 5

        Me.btnBrowseJava.Location = New System.Drawing.Point(444, 81)
        Me.btnBrowseJava.Name = "btnBrowseJava"
        Me.btnBrowseJava.Size = New System.Drawing.Size(32, 27)
        Me.btnBrowseJava.TabIndex = 6

        Me.lblMemory.AutoSize = True
        Me.lblMemory.Location = New System.Drawing.Point(12, 118)
        Me.lblMemory.Name = "lblMemory"
        Me.lblMemory.Size = New System.Drawing.Size(101, 15)
        Me.lblMemory.TabIndex = 7
        Me.lblMemory.Text = "Maximum memory"

        Me.numMemory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numMemory.Increment = New Decimal(New Integer() {512, 0, 0, 0})
        Me.numMemory.Location = New System.Drawing.Point(136, 115)
        Me.numMemory.Maximum = New Decimal(New Integer() {65536, 0, 0, 0})
        Me.numMemory.Minimum = New Decimal(New Integer() {512, 0, 0, 0})
        Me.numMemory.Name = "numMemory"
        Me.numMemory.Size = New System.Drawing.Size(90, 23)
        Me.numMemory.TabIndex = 8
        Me.numMemory.ThousandsSeparator = True
        Me.numMemory.Value = New Decimal(New Integer() {4096, 0, 0, 0})

        Me.lblRam.AutoSize = True
        Me.lblRam.ForeColor = System.Drawing.Color.FromArgb(CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblRam.Location = New System.Drawing.Point(234, 118)
        Me.lblRam.Name = "lblRam"
        Me.lblRam.Size = New System.Drawing.Size(0, 15)
        Me.lblRam.TabIndex = 9

        Me.lblArgs.AutoSize = True
        Me.lblArgs.Location = New System.Drawing.Point(12, 150)
        Me.lblArgs.Name = "lblArgs"
        Me.lblArgs.Size = New System.Drawing.Size(88, 15)
        Me.lblArgs.TabIndex = 10
        Me.lblArgs.Text = "JVM arguments"

        Me.txtArgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtArgs.Location = New System.Drawing.Point(136, 147)
        Me.txtArgs.Multiline = True
        Me.txtArgs.Name = "txtArgs"
        Me.txtArgs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtArgs.Size = New System.Drawing.Size(340, 44)
        Me.txtArgs.TabIndex = 11

        Me.lblMaintenanceHeader.AutoSize = True
        Me.lblMaintenanceHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaintenanceHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.lblMaintenanceHeader.Location = New System.Drawing.Point(16, 510)
        Me.lblMaintenanceHeader.Name = "lblMaintenanceHeader"
        Me.lblMaintenanceHeader.Size = New System.Drawing.Size(83, 13)
        Me.lblMaintenanceHeader.TabIndex = 6
        Me.lblMaintenanceHeader.Text = "MAINTENANCE"

        Me.btnOpenData.Location = New System.Drawing.Point(16, 530)
        Me.btnOpenData.Name = "btnOpenData"
        Me.btnOpenData.Size = New System.Drawing.Size(120, 30)
        Me.btnOpenData.TabIndex = 7
        Me.btnOpenData.Text = "Open fldata"

        Me.btnOpenLog.Location = New System.Drawing.Point(142, 530)
        Me.btnOpenLog.Name = "btnOpenLog"
        Me.btnOpenLog.Size = New System.Drawing.Size(150, 30)
        Me.btnOpenLog.TabIndex = 8
        Me.btnOpenLog.Text = "Open launcher.log"

        Me.btnUninstall.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUninstall.Location = New System.Drawing.Point(304, 530)
        Me.btnUninstall.Name = "btnUninstall"
        Me.btnUninstall.Size = New System.Drawing.Size(200, 30)
        Me.btnUninstall.TabIndex = 9
        Me.btnUninstall.Text = "Uninstall Forge Launcher..."
        Me.btnUninstall.Variant = ForgeLauncher.ButtonVariant.Danger

        Me.pnlFooter.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
        Me.pnlFooter.Controls.Add(Me.btnCancel)
        Me.pnlFooter.Controls.Add(Me.btnSave)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 576)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(520, 52)
        Me.pnlFooter.TabIndex = 10

        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(322, 11)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(88, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"

        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(416, 11)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(88, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        Me.btnSave.Variant = ForgeLauncher.ButtonVariant.Primary

        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(520, 628)
        Me.Controls.Add(Me.lblUpdatesHeader)
        Me.Controls.Add(Me.pnlUpdates)
        Me.Controls.Add(Me.lblVersionsHeader)
        Me.Controls.Add(Me.pnlVersions)
        Me.Controls.Add(Me.lblLaunchHeader)
        Me.Controls.Add(Me.pnlLaunch)
        Me.Controls.Add(Me.lblMaintenanceHeader)
        Me.Controls.Add(Me.btnOpenData)
        Me.Controls.Add(Me.btnOpenLog)
        Me.Controls.Add(Me.btnUninstall)
        Me.Controls.Add(Me.pnlFooter)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SettingsForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        Me.pnlUpdates.ResumeLayout(False)
        Me.pnlVersions.ResumeLayout(False)
        Me.pnlVersions.PerformLayout()
        Me.pnlLaunch.ResumeLayout(False)
        Me.pnlLaunch.PerformLayout()
        CType(Me.numMemory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblUpdatesHeader As System.Windows.Forms.Label
    Friend WithEvents pnlUpdates As ForgeLauncher.BorderPanel
    Friend WithEvents tglCheckForge As ForgeLauncher.ToggleSwitch
    Friend WithEvents tglCheckLauncher As ForgeLauncher.ToggleSwitch
    Friend WithEvents tglAskToLaunch As ForgeLauncher.ToggleSwitch
    Friend WithEvents lblVersionsHeader As System.Windows.Forms.Label
    Friend WithEvents pnlVersions As ForgeLauncher.BorderPanel
    Friend WithEvents lblKeep As System.Windows.Forms.Label
    Friend WithEvents lblKeepInfo As System.Windows.Forms.Label
    Friend WithEvents segKeep As ForgeLauncher.SegmentedSelector
    Friend WithEvents lblRestoreHint As System.Windows.Forms.Label
    Friend WithEvents btnRestore As ForgeLauncher.DarkButton
    Friend WithEvents lblLaunchHeader As System.Windows.Forms.Label
    Friend WithEvents pnlLaunch As ForgeLauncher.BorderPanel
    Friend WithEvents lblMode As System.Windows.Forms.Label
    Friend WithEvents segMode As ForgeLauncher.SegmentedSelector
    Friend WithEvents lblApp As System.Windows.Forms.Label
    Friend WithEvents cmbApp As System.Windows.Forms.ComboBox
    Friend WithEvents lblJava As System.Windows.Forms.Label
    Friend WithEvents txtJava As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseJava As ForgeLauncher.DarkButton
    Friend WithEvents lblMemory As System.Windows.Forms.Label
    Friend WithEvents numMemory As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRam As System.Windows.Forms.Label
    Friend WithEvents lblArgs As System.Windows.Forms.Label
    Friend WithEvents txtArgs As System.Windows.Forms.TextBox
    Friend WithEvents lblMaintenanceHeader As System.Windows.Forms.Label
    Friend WithEvents btnOpenData As ForgeLauncher.DarkButton
    Friend WithEvents btnOpenLog As ForgeLauncher.DarkButton
    Friend WithEvents btnUninstall As ForgeLauncher.DarkButton
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As ForgeLauncher.DarkButton
    Friend WithEvents btnSave As ForgeLauncher.DarkButton
End Class
