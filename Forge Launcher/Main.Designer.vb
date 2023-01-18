<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.txlog = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
        Me.launchforge = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.btnlaunchmode = New System.Windows.Forms.Button()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.typeofupdate = New System.Windows.Forms.ComboBox()
        Me.vtoupdate = New System.Windows.Forms.Label()
        Me.MenuGeneral = New System.Windows.Forms.MenuStrip()
        Me.SettingsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReadForgeLogFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreForgePreferencesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestartForgeLauncherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutForgeToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForgeForumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForgeWikiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForgeDiscordChannelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutLauncherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckForUpdatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GithubToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.listofexes = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.GroupForgeOptions = New System.Windows.Forms.GroupBox()
        Me.group_install = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.rbt_normal = New System.Windows.Forms.RadioButton()
        Me.rbt_properties = New System.Windows.Forms.RadioButton()
        CType(Me.PictureBox3,System.ComponentModel.ISupportInitialize).BeginInit
        Me.MenuGeneral.SuspendLayout
        Me.GroupForgeOptions.SuspendLayout
        Me.group_install.SuspendLayout
        Me.SuspendLayout
        '
        'txlog
        '
        Me.txlog.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txlog.Location = New System.Drawing.Point(9, 105)
        Me.txlog.Multiline = true
        Me.txlog.Name = "txlog"
        Me.txlog.ReadOnly = true
        Me.txlog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txlog.Size = New System.Drawing.Size(587, 240)
        Me.txlog.TabIndex = 8
        '
        'launchforge
        '
        Me.launchforge.BackColor = System.Drawing.Color.Gainsboro
        Me.launchforge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Bold)
        Me.launchforge.ForeColor = System.Drawing.Color.Black
        Me.launchforge.Location = New System.Drawing.Point(167, 20)
        Me.launchforge.Name = "launchforge"
        Me.launchforge.Size = New System.Drawing.Size(237, 30)
        Me.launchforge.TabIndex = 4
        Me.launchforge.Text = "Launch Forge"
        Me.ToolTip1.SetToolTip(Me.launchforge, "Launch Forge")
        Me.launchforge.UseVisualStyleBackColor = false
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 351)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(587, 20)
        Me.ProgressBar1.TabIndex = 36
        Me.ProgressBar1.Visible = false
        '
        'btnupdate
        '
        Me.btnupdate.BackColor = System.Drawing.Color.Gainsboro
        Me.btnupdate.Image = CType(resources.GetObject("btnupdate.Image"),System.Drawing.Image)
        Me.btnupdate.Location = New System.Drawing.Point(128, 20)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(33, 30)
        Me.btnupdate.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.btnupdate, "Check for Forge Updates")
        Me.btnupdate.UseVisualStyleBackColor = false
        '
        'btnlaunchmode
        '
        Me.btnlaunchmode.BackColor = System.Drawing.Color.Gainsboro
        Me.btnlaunchmode.FlatAppearance.BorderSize = 0
        Me.btnlaunchmode.Image = CType(resources.GetObject("btnlaunchmode.Image"),System.Drawing.Image)
        Me.btnlaunchmode.Location = New System.Drawing.Point(410, 20)
        Me.btnlaunchmode.Name = "btnlaunchmode"
        Me.btnlaunchmode.Size = New System.Drawing.Size(31, 28)
        Me.btnlaunchmode.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.btnlaunchmode, "Launch Options")
        Me.btnlaunchmode.UseVisualStyleBackColor = false
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"),System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(97, 24)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(25, 24)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 49
        Me.PictureBox3.TabStop = false
        Me.ToolTip1.SetToolTip(Me.PictureBox3, "snapshot: You get new cards and features earlier, but you also have a higher chan"& _ 
        "ce to encounter some bugs"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"release: Last stable version.")
        '
        'typeofupdate
        '
        Me.typeofupdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.typeofupdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Bold)
        Me.typeofupdate.FormattingEnabled = true
        Me.typeofupdate.Items.AddRange(New Object() {"release", "snapshot"})
        Me.typeofupdate.Location = New System.Drawing.Point(6, 26)
        Me.typeofupdate.Name = "typeofupdate"
        Me.typeofupdate.Size = New System.Drawing.Size(85, 21)
        Me.typeofupdate.TabIndex = 35
        Me.ToolTip1.SetToolTip(Me.typeofupdate, "Type of update")
        '
        'vtoupdate
        '
        Me.vtoupdate.AutoSize = true
        Me.vtoupdate.Location = New System.Drawing.Point(447, 29)
        Me.vtoupdate.Name = "vtoupdate"
        Me.vtoupdate.Size = New System.Drawing.Size(106, 13)
        Me.vtoupdate.TabIndex = 34
        Me.vtoupdate.Text = "version to update"
        Me.vtoupdate.Visible = false
        '
        'MenuGeneral
        '
        Me.MenuGeneral.BackColor = System.Drawing.Color.Silver
        Me.MenuGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!)
        Me.MenuGeneral.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuGeneral.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem1, Me.ToolStripMenuItem1, Me.ToolsToolStripMenuItem, Me.AboutForgeToolStripMenuItem1, Me.AboutLauncherToolStripMenuItem})
        Me.MenuGeneral.Location = New System.Drawing.Point(0, 0)
        Me.MenuGeneral.Name = "MenuGeneral"
        Me.MenuGeneral.Size = New System.Drawing.Size(606, 24)
        Me.MenuGeneral.TabIndex = 1
        Me.MenuGeneral.Text = "MenuStrip1"
        '
        'SettingsToolStripMenuItem1
        '
        Me.SettingsToolStripMenuItem1.Name = "SettingsToolStripMenuItem1"
        Me.SettingsToolStripMenuItem1.Size = New System.Drawing.Size(57, 20)
        Me.SettingsToolStripMenuItem1.Text = "Settings"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReadForgeLogFileToolStripMenuItem, Me.RestoreForgePreferencesToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(75, 20)
        Me.ToolStripMenuItem1.Text = "Forge Tools"
        '
        'ReadForgeLogFileToolStripMenuItem
        '
        Me.ReadForgeLogFileToolStripMenuItem.Name = "ReadForgeLogFileToolStripMenuItem"
        Me.ReadForgeLogFileToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.ReadForgeLogFileToolStripMenuItem.Text = "Read Forge Log File"
        '
        'RestoreForgePreferencesToolStripMenuItem
        '
        Me.RestoreForgePreferencesToolStripMenuItem.Name = "RestoreForgePreferencesToolStripMenuItem"
        Me.RestoreForgePreferencesToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.RestoreForgePreferencesToolStripMenuItem.Text = "Restore Forge Preferences"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestartForgeLauncherToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(93, 20)
        Me.ToolsToolStripMenuItem.Text = "Launcher Tools"
        '
        'RestartForgeLauncherToolStripMenuItem
        '
        Me.RestartForgeLauncherToolStripMenuItem.Name = "RestartForgeLauncherToolStripMenuItem"
        Me.RestartForgeLauncherToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.RestartForgeLauncherToolStripMenuItem.Text = "Restart Forge Launcher"
        '
        'AboutForgeToolStripMenuItem1
        '
        Me.AboutForgeToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ForgeForumToolStripMenuItem, Me.ForgeWikiToolStripMenuItem, Me.ForgeDiscordChannelToolStripMenuItem})
        Me.AboutForgeToolStripMenuItem1.Name = "AboutForgeToolStripMenuItem1"
        Me.AboutForgeToolStripMenuItem1.Size = New System.Drawing.Size(77, 20)
        Me.AboutForgeToolStripMenuItem1.Text = "About Forge"
        '
        'ForgeForumToolStripMenuItem
        '
        Me.ForgeForumToolStripMenuItem.Name = "ForgeForumToolStripMenuItem"
        Me.ForgeForumToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ForgeForumToolStripMenuItem.Text = "Forge Forum"
        '
        'ForgeWikiToolStripMenuItem
        '
        Me.ForgeWikiToolStripMenuItem.Name = "ForgeWikiToolStripMenuItem"
        Me.ForgeWikiToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ForgeWikiToolStripMenuItem.Text = "Forge Wiki"
        '
        'ForgeDiscordChannelToolStripMenuItem
        '
        Me.ForgeDiscordChannelToolStripMenuItem.Name = "ForgeDiscordChannelToolStripMenuItem"
        Me.ForgeDiscordChannelToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ForgeDiscordChannelToolStripMenuItem.Text = "Forge Discord Channel"
        '
        'AboutLauncherToolStripMenuItem
        '
        Me.AboutLauncherToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckForUpdatesToolStripMenuItem, Me.GithubToolStripMenuItem})
        Me.AboutLauncherToolStripMenuItem.Name = "AboutLauncherToolStripMenuItem"
        Me.AboutLauncherToolStripMenuItem.Size = New System.Drawing.Size(95, 20)
        Me.AboutLauncherToolStripMenuItem.Text = "About Launcher"
        '
        'CheckForUpdatesToolStripMenuItem
        '
        Me.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem"
        Me.CheckForUpdatesToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.CheckForUpdatesToolStripMenuItem.Text = "Check for Launcher Updates"
        '
        'GithubToolStripMenuItem
        '
        Me.GithubToolStripMenuItem.Name = "GithubToolStripMenuItem"
        Me.GithubToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.GithubToolStripMenuItem.Text = "Visit Project on GitHub"
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 200
        Me.ToolTip1.ReshowDelay = 100
        '
        'listofexes
        '
        Me.listofexes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.listofexes.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.listofexes.FormattingEnabled = true
        Me.listofexes.Items.AddRange(New Object() {"release", "snapshot"})
        Me.listofexes.Location = New System.Drawing.Point(453, 22)
        Me.listofexes.Name = "listofexes"
        Me.listofexes.Size = New System.Drawing.Size(128, 21)
        Me.listofexes.TabIndex = 51
        Me.ToolTip1.SetToolTip(Me.listofexes, "EXE or CMD to Launch (only in normal mode)"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Select an exe from the different exes"& _ 
        " listed, such as adventure mode and other exes for other Java versions.")
        Me.listofexes.Visible = false
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(12, 455)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 45
        '
        'Timer1
        '
        '
        'GroupForgeOptions
        '
        Me.GroupForgeOptions.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupForgeOptions.Controls.Add(Me.listofexes)
        Me.GroupForgeOptions.Controls.Add(Me.btnupdate)
        Me.GroupForgeOptions.Controls.Add(Me.btnlaunchmode)
        Me.GroupForgeOptions.Controls.Add(Me.PictureBox3)
        Me.GroupForgeOptions.Controls.Add(Me.typeofupdate)
        Me.GroupForgeOptions.Controls.Add(Me.vtoupdate)
        Me.GroupForgeOptions.Controls.Add(Me.launchforge)
        Me.GroupForgeOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.GroupForgeOptions.Location = New System.Drawing.Point(9, 35)
        Me.GroupForgeOptions.Name = "GroupForgeOptions"
        Me.GroupForgeOptions.Size = New System.Drawing.Size(587, 64)
        Me.GroupForgeOptions.TabIndex = 39
        Me.GroupForgeOptions.TabStop = false
        Me.GroupForgeOptions.Text = "Update Options"
        '
        'group_install
        '
        Me.group_install.BackColor = System.Drawing.Color.Transparent
        Me.group_install.Controls.Add(Me.Button2)
        Me.group_install.Controls.Add(Me.rbt_normal)
        Me.group_install.Controls.Add(Me.rbt_properties)
        Me.group_install.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.group_install.ForeColor = System.Drawing.Color.Black
        Me.group_install.Location = New System.Drawing.Point(9, 35)
        Me.group_install.Margin = New System.Windows.Forms.Padding(1)
        Me.group_install.Name = "group_install"
        Me.group_install.Size = New System.Drawing.Size(587, 64)
        Me.group_install.TabIndex = 35
        Me.group_install.TabStop = false
        Me.group_install.Text = "Install Options"
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(6, 20)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 24)
        Me.Button2.TabIndex = 36
        Me.Button2.Text = "Install Forge"
        Me.Button2.UseVisualStyleBackColor = true
        '
        'rbt_normal
        '
        Me.rbt_normal.AutoSize = true
        Me.rbt_normal.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbt_normal.ForeColor = System.Drawing.Color.Black
        Me.rbt_normal.Location = New System.Drawing.Point(124, 22)
        Me.rbt_normal.Name = "rbt_normal"
        Me.rbt_normal.Size = New System.Drawing.Size(87, 17)
        Me.rbt_normal.TabIndex = 33
        Me.rbt_normal.Text = "Normal Install"
        Me.rbt_normal.UseVisualStyleBackColor = true
        '
        'rbt_properties
        '
        Me.rbt_properties.AutoSize = true
        Me.rbt_properties.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbt_properties.ForeColor = System.Drawing.Color.Black
        Me.rbt_properties.Location = New System.Drawing.Point(214, 22)
        Me.rbt_properties.Name = "rbt_properties"
        Me.rbt_properties.Size = New System.Drawing.Size(338, 17)
        Me.rbt_properties.TabIndex = 34
        Me.rbt_properties.Text = "Install All In The Same Folder Creating Forge.Profiles.Properties File"
        Me.rbt_properties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbt_properties.UseVisualStyleBackColor = true
        '
        'Main
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(606, 376)
        Me.Controls.Add(Me.GroupForgeOptions)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.MenuGeneral)
        Me.Controls.Add(Me.txlog)
        Me.Controls.Add(Me.group_install)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximizeBox = false
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox3,System.ComponentModel.ISupportInitialize).EndInit
        Me.MenuGeneral.ResumeLayout(false)
        Me.MenuGeneral.PerformLayout
        Me.GroupForgeOptions.ResumeLayout(false)
        Me.GroupForgeOptions.PerformLayout
        Me.group_install.ResumeLayout(false)
        Me.group_install.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents txlog As System.Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog2 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents launchforge As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents MenuGeneral As MenuStrip
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents AboutForgeToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ForgeDiscordChannelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ForgeForumToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ForgeWikiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestartForgeLauncherToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents vtoupdate As Label
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RestoreForgePreferencesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReadForgeLogFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents typeofupdate As ComboBox
    Friend WithEvents btnlaunchmode As Button
    Friend WithEvents SettingsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents btnupdate As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents group_install As GroupBox
    Friend WithEvents Button2 As Button
    Friend WithEvents rbt_normal As RadioButton
    Friend WithEvents rbt_properties As RadioButton
    Friend WithEvents GroupForgeOptions As GroupBox
    Friend WithEvents AboutLauncherToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents listofexes As ComboBox
    Friend WithEvents GithubToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CheckForUpdatesToolStripMenuItem As ToolStripMenuItem
End Class
