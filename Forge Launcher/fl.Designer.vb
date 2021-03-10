<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fl))
        Me.txlog = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
        Me.launchforge = New System.Windows.Forms.Button()
        Me.rbt_normal = New System.Windows.Forms.RadioButton()
        Me.rbt_properties = New System.Windows.Forms.RadioButton()
        Me.group_install = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.GroupForgeOptions = New System.Windows.Forms.GroupBox()
        Me.chkenableprompt = New System.Windows.Forms.CheckBox()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.btnlaunchmode = New System.Windows.Forms.Button()
        Me.chklaunchforgeafterupdate = New System.Windows.Forms.CheckBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.typeofupdate = New System.Windows.Forms.ComboBox()
        Me.vtoupdate = New System.Windows.Forms.Label()
        Me.MenuGeneral = New System.Windows.Forms.MenuStrip()
        Me.OpenDecksFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestartForgeLauncherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckForForgeLauncherUpdatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForzeUpdateForgeLauncherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReadForgeLogFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreForgePreferencesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutForgeToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForgeForumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForgeWikiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForgeDiscordChannelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WhatsNewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupExtras = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lbgauntletformat = New System.Windows.Forms.ListBox()
        Me.lbgauntletyear = New System.Windows.Forms.ListBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Other_Formats = New System.Windows.Forms.TabPage()
        Me.metag2 = New System.Windows.Forms.ComboBox()
        Me.howmuch2 = New System.Windows.Forms.ComboBox()
        Me.extract3 = New System.Windows.Forms.Button()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.By_Tournament = New System.Windows.Forms.TabPage()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.extract4 = New System.Windows.Forms.Button()
        Me.maxtournm = New System.Windows.Forms.ComboBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.fromweb = New System.Windows.Forms.ComboBox()
        Me.maxtournamentsdecks = New System.Windows.Forms.ComboBox()
        Me.by_metagame = New System.Windows.Forms.TabPage()
        Me.metagame = New System.Windows.Forms.ComboBox()
        Me.howmuch = New System.Windows.Forms.ComboBox()
        Me.chktopnumber = New System.Windows.Forms.CheckBox()
        Me.extract1 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.group_install.SuspendLayout()
        Me.GroupForgeOptions.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuGeneral.SuspendLayout()
        Me.GroupExtras.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Other_Formats.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.By_Tournament.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.by_metagame.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txlog
        '
        Me.txlog.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txlog.Location = New System.Drawing.Point(16, 284)
        Me.txlog.Multiline = True
        Me.txlog.Name = "txlog"
        Me.txlog.ReadOnly = True
        Me.txlog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txlog.Size = New System.Drawing.Size(698, 217)
        Me.txlog.TabIndex = 24
        '
        'launchforge
        '
        Me.launchforge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold)
        Me.launchforge.ForeColor = System.Drawing.Color.Black
        Me.launchforge.Location = New System.Drawing.Point(211, 22)
        Me.launchforge.Name = "launchforge"
        Me.launchforge.Size = New System.Drawing.Size(197, 28)
        Me.launchforge.TabIndex = 28
        Me.launchforge.Text = "Launch Forge"
        Me.ToolTip1.SetToolTip(Me.launchforge, "Launch Forge")
        Me.launchforge.UseVisualStyleBackColor = True
        '
        'rbt_normal
        '
        Me.rbt_normal.AutoSize = True
        Me.rbt_normal.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbt_normal.ForeColor = System.Drawing.Color.Black
        Me.rbt_normal.Location = New System.Drawing.Point(124, 22)
        Me.rbt_normal.Name = "rbt_normal"
        Me.rbt_normal.Size = New System.Drawing.Size(104, 19)
        Me.rbt_normal.TabIndex = 33
        Me.rbt_normal.Text = "Normal Install"
        Me.rbt_normal.UseVisualStyleBackColor = True
        '
        'rbt_properties
        '
        Me.rbt_properties.AutoSize = True
        Me.rbt_properties.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbt_properties.ForeColor = System.Drawing.Color.Black
        Me.rbt_properties.Location = New System.Drawing.Point(214, 22)
        Me.rbt_properties.Name = "rbt_properties"
        Me.rbt_properties.Size = New System.Drawing.Size(397, 19)
        Me.rbt_properties.TabIndex = 34
        Me.rbt_properties.Text = "Install All In The Same Folder Creating Forge.Profiles.Properties File"
        Me.rbt_properties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbt_properties.UseVisualStyleBackColor = True
        '
        'group_install
        '
        Me.group_install.BackColor = System.Drawing.Color.Transparent
        Me.group_install.Controls.Add(Me.Button2)
        Me.group_install.Controls.Add(Me.rbt_normal)
        Me.group_install.Controls.Add(Me.rbt_properties)
        Me.group_install.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.group_install.ForeColor = System.Drawing.Color.Black
        Me.group_install.Location = New System.Drawing.Point(16, 35)
        Me.group_install.Margin = New System.Windows.Forms.Padding(1)
        Me.group_install.Name = "group_install"
        Me.group_install.Size = New System.Drawing.Size(698, 59)
        Me.group_install.TabIndex = 35
        Me.group_install.TabStop = False
        Me.group_install.Text = "Install Options"
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(6, 20)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 24)
        Me.Button2.TabIndex = 36
        Me.Button2.Text = "Install Forge"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(16, 511)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(697, 25)
        Me.ProgressBar1.TabIndex = 36
        Me.ProgressBar1.Visible = False
        '
        'GroupForgeOptions
        '
        Me.GroupForgeOptions.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupForgeOptions.Controls.Add(Me.chkenableprompt)
        Me.GroupForgeOptions.Controls.Add(Me.btnupdate)
        Me.GroupForgeOptions.Controls.Add(Me.btnlaunchmode)
        Me.GroupForgeOptions.Controls.Add(Me.chklaunchforgeafterupdate)
        Me.GroupForgeOptions.Controls.Add(Me.PictureBox3)
        Me.GroupForgeOptions.Controls.Add(Me.typeofupdate)
        Me.GroupForgeOptions.Controls.Add(Me.vtoupdate)
        Me.GroupForgeOptions.Controls.Add(Me.launchforge)
        Me.GroupForgeOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupForgeOptions.Location = New System.Drawing.Point(16, 35)
        Me.GroupForgeOptions.Name = "GroupForgeOptions"
        Me.GroupForgeOptions.Size = New System.Drawing.Size(698, 70)
        Me.GroupForgeOptions.TabIndex = 39
        Me.GroupForgeOptions.TabStop = False
        Me.GroupForgeOptions.Text = "Update Options"
        '
        'chkenableprompt
        '
        Me.chkenableprompt.AutoSize = True
        Me.chkenableprompt.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.chkenableprompt.Location = New System.Drawing.Point(476, 20)
        Me.chkenableprompt.Name = "chkenableprompt"
        Me.chkenableprompt.Size = New System.Drawing.Size(195, 19)
        Me.chkenableprompt.TabIndex = 53
        Me.chkenableprompt.Text = "Enable prompt for new version"
        Me.ToolTip1.SetToolTip(Me.chkenableprompt, "If this is checked, when Launcher loads will alert you when a new version is avai" &
        "lable")
        Me.chkenableprompt.UseVisualStyleBackColor = True
        '
        'btnupdate
        '
        Me.btnupdate.Image = CType(resources.GetObject("btnupdate.Image"), System.Drawing.Image)
        Me.btnupdate.Location = New System.Drawing.Point(7, 24)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(32, 26)
        Me.btnupdate.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.btnupdate, "Check for Forge Updates")
        Me.btnupdate.UseVisualStyleBackColor = True
        '
        'btnlaunchmode
        '
        Me.btnlaunchmode.BackColor = System.Drawing.Color.Gainsboro
        Me.btnlaunchmode.FlatAppearance.BorderSize = 0
        Me.btnlaunchmode.Image = CType(resources.GetObject("btnlaunchmode.Image"), System.Drawing.Image)
        Me.btnlaunchmode.Location = New System.Drawing.Point(417, 22)
        Me.btnlaunchmode.Name = "btnlaunchmode"
        Me.btnlaunchmode.Size = New System.Drawing.Size(43, 29)
        Me.btnlaunchmode.TabIndex = 51
        Me.ToolTip1.SetToolTip(Me.btnlaunchmode, "Launch Options")
        Me.btnlaunchmode.UseVisualStyleBackColor = False
        '
        'chklaunchforgeafterupdate
        '
        Me.chklaunchforgeafterupdate.AutoSize = True
        Me.chklaunchforgeafterupdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.chklaunchforgeafterupdate.Location = New System.Drawing.Point(476, 38)
        Me.chklaunchforgeafterupdate.Name = "chklaunchforgeafterupdate"
        Me.chklaunchforgeafterupdate.Size = New System.Drawing.Size(175, 19)
        Me.chklaunchforgeafterupdate.TabIndex = 50
        Me.chklaunchforgeafterupdate.Text = "Launch Forge after Update"
        Me.ToolTip1.SetToolTip(Me.chklaunchforgeafterupdate, "Launch Forge after Update")
        Me.chklaunchforgeafterupdate.UseVisualStyleBackColor = True
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(161, 25)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(25, 24)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 49
        Me.PictureBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox3, "snapshot: You get new cards and features earlier, but you also have a higher chan" &
        "ce to encounter some bugs" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "release: Last stable version.")
        '
        'typeofupdate
        '
        Me.typeofupdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.typeofupdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.typeofupdate.FormattingEnabled = True
        Me.typeofupdate.Items.AddRange(New Object() {"release", "snapshot"})
        Me.typeofupdate.Location = New System.Drawing.Point(45, 26)
        Me.typeofupdate.Name = "typeofupdate"
        Me.typeofupdate.Size = New System.Drawing.Size(110, 23)
        Me.typeofupdate.TabIndex = 35
        Me.ToolTip1.SetToolTip(Me.typeofupdate, "Type of Update")
        '
        'vtoupdate
        '
        Me.vtoupdate.AutoSize = True
        Me.vtoupdate.Location = New System.Drawing.Point(557, 30)
        Me.vtoupdate.Name = "vtoupdate"
        Me.vtoupdate.Size = New System.Drawing.Size(134, 15)
        Me.vtoupdate.TabIndex = 34
        Me.vtoupdate.Text = "Version a actualizar"
        Me.vtoupdate.Visible = False
        '
        'MenuGeneral
        '
        Me.MenuGeneral.BackColor = System.Drawing.Color.Silver
        Me.MenuGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.MenuGeneral.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuGeneral.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenDecksFolderToolStripMenuItem, Me.SettingsToolStripMenuItem1, Me.ToolsToolStripMenuItem, Me.ToolStripMenuItem1, Me.AboutForgeToolStripMenuItem1, Me.WhatsNewToolStripMenuItem})
        Me.MenuGeneral.Location = New System.Drawing.Point(0, 0)
        Me.MenuGeneral.Name = "MenuGeneral"
        Me.MenuGeneral.Size = New System.Drawing.Size(729, 25)
        Me.MenuGeneral.TabIndex = 41
        Me.MenuGeneral.Text = "MenuStrip1"
        '
        'OpenDecksFolderToolStripMenuItem
        '
        Me.OpenDecksFolderToolStripMenuItem.Name = "OpenDecksFolderToolStripMenuItem"
        Me.OpenDecksFolderToolStripMenuItem.Size = New System.Drawing.Size(144, 21)
        Me.OpenDecksFolderToolStripMenuItem.Text = "Open Decks Folder"
        '
        'SettingsToolStripMenuItem1
        '
        Me.SettingsToolStripMenuItem1.Name = "SettingsToolStripMenuItem1"
        Me.SettingsToolStripMenuItem1.Size = New System.Drawing.Size(73, 21)
        Me.SettingsToolStripMenuItem1.Text = "Settings"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestartForgeLauncherToolStripMenuItem, Me.CheckForForgeLauncherUpdatesToolStripMenuItem, Me.ForzeUpdateForgeLauncherToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(121, 21)
        Me.ToolsToolStripMenuItem.Text = "Launcher Tools"
        '
        'RestartForgeLauncherToolStripMenuItem
        '
        Me.RestartForgeLauncherToolStripMenuItem.Name = "RestartForgeLauncherToolStripMenuItem"
        Me.RestartForgeLauncherToolStripMenuItem.Size = New System.Drawing.Size(446, 26)
        Me.RestartForgeLauncherToolStripMenuItem.Text = "Restart Forge Launcher"
        '
        'CheckForForgeLauncherUpdatesToolStripMenuItem
        '
        Me.CheckForForgeLauncherUpdatesToolStripMenuItem.Name = "CheckForForgeLauncherUpdatesToolStripMenuItem"
        Me.CheckForForgeLauncherUpdatesToolStripMenuItem.Size = New System.Drawing.Size(446, 26)
        Me.CheckForForgeLauncherUpdatesToolStripMenuItem.Text = "Check for Forge Launcher Updates"
        '
        'ForzeUpdateForgeLauncherToolStripMenuItem
        '
        Me.ForzeUpdateForgeLauncherToolStripMenuItem.Name = "ForzeUpdateForgeLauncherToolStripMenuItem"
        Me.ForzeUpdateForgeLauncherToolStripMenuItem.Size = New System.Drawing.Size(446, 26)
        Me.ForzeUpdateForgeLauncherToolStripMenuItem.Text = "Force to Install Last Forge Launcher Version from server"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReadForgeLogFileToolStripMenuItem, Me.RestoreForgePreferencesToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(98, 21)
        Me.ToolStripMenuItem1.Text = "Forge Tools"
        '
        'ReadForgeLogFileToolStripMenuItem
        '
        Me.ReadForgeLogFileToolStripMenuItem.Name = "ReadForgeLogFileToolStripMenuItem"
        Me.ReadForgeLogFileToolStripMenuItem.Size = New System.Drawing.Size(262, 26)
        Me.ReadForgeLogFileToolStripMenuItem.Text = "Read Forge Log File"
        '
        'RestoreForgePreferencesToolStripMenuItem
        '
        Me.RestoreForgePreferencesToolStripMenuItem.Name = "RestoreForgePreferencesToolStripMenuItem"
        Me.RestoreForgePreferencesToolStripMenuItem.Size = New System.Drawing.Size(262, 26)
        Me.RestoreForgePreferencesToolStripMenuItem.Text = "Restore Forge Preferences"
        '
        'AboutForgeToolStripMenuItem1
        '
        Me.AboutForgeToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ForgeForumToolStripMenuItem, Me.ForgeWikiToolStripMenuItem, Me.ForgeDiscordChannelToolStripMenuItem})
        Me.AboutForgeToolStripMenuItem1.Name = "AboutForgeToolStripMenuItem1"
        Me.AboutForgeToolStripMenuItem1.Size = New System.Drawing.Size(100, 21)
        Me.AboutForgeToolStripMenuItem1.Text = "About Forge"
        '
        'ForgeForumToolStripMenuItem
        '
        Me.ForgeForumToolStripMenuItem.Name = "ForgeForumToolStripMenuItem"
        Me.ForgeForumToolStripMenuItem.Size = New System.Drawing.Size(235, 26)
        Me.ForgeForumToolStripMenuItem.Text = "Forge Forum"
        '
        'ForgeWikiToolStripMenuItem
        '
        Me.ForgeWikiToolStripMenuItem.Name = "ForgeWikiToolStripMenuItem"
        Me.ForgeWikiToolStripMenuItem.Size = New System.Drawing.Size(235, 26)
        Me.ForgeWikiToolStripMenuItem.Text = "Forge Wiki"
        '
        'ForgeDiscordChannelToolStripMenuItem
        '
        Me.ForgeDiscordChannelToolStripMenuItem.Name = "ForgeDiscordChannelToolStripMenuItem"
        Me.ForgeDiscordChannelToolStripMenuItem.Size = New System.Drawing.Size(235, 26)
        Me.ForgeDiscordChannelToolStripMenuItem.Text = "Forge Discord Channel"
        '
        'WhatsNewToolStripMenuItem
        '
        Me.WhatsNewToolStripMenuItem.Name = "WhatsNewToolStripMenuItem"
        Me.WhatsNewToolStripMenuItem.Size = New System.Drawing.Size(104, 21)
        Me.WhatsNewToolStripMenuItem.Text = "What's New?"
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 200
        Me.ToolTip1.ReshowDelay = 100
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!)
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.Location = New System.Drawing.Point(641, 88)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(35, 27)
        Me.Button3.TabIndex = 56
        Me.ToolTip1.SetToolTip(Me.Button3, "Delete all downloaded Gauntlets")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 455)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 15)
        Me.Label1.TabIndex = 45
        '
        'GroupExtras
        '
        Me.GroupExtras.Controls.Add(Me.TabPage1)
        Me.GroupExtras.Controls.Add(Me.TabPage2)
        Me.GroupExtras.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupExtras.Location = New System.Drawing.Point(15, 113)
        Me.GroupExtras.Name = "GroupExtras"
        Me.GroupExtras.SelectedIndex = 0
        Me.GroupExtras.Size = New System.Drawing.Size(702, 157)
        Me.GroupExtras.TabIndex = 46
        '
        'TabPage1
        '
        Me.TabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.TabPage1.Controls.Add(Me.TabControl1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(694, 129)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Decks Extractor"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Controls.Add(Me.TextBox2)
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Controls.Add(Me.lbgauntletformat)
        Me.TabPage2.Controls.Add(Me.lbgauntletyear)
        Me.TabPage2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(694, 129)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Gauntlets Extractor"
        '
        'TextBox2
        '
        Me.TextBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar
        Me.TextBox2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.TextBox2.Location = New System.Drawing.Point(6, 16)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(414, 105)
        Me.TextBox2.TabIndex = 55
        Me.TextBox2.TabStop = False
        Me.TextBox2.Text = resources.GetString("TextBox2.Text")
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(448, 89)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(187, 26)
        Me.Button1.TabIndex = 52
        Me.Button1.Text = "Extract Gauntlets"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lbgauntletformat
        '
        Me.lbgauntletformat.FormattingEnabled = True
        Me.lbgauntletformat.ItemHeight = 15
        Me.lbgauntletformat.Items.AddRange(New Object() {"Standard", "Modern", "Legacy", "Vintage"})
        Me.lbgauntletformat.Location = New System.Drawing.Point(574, 16)
        Me.lbgauntletformat.Name = "lbgauntletformat"
        Me.lbgauntletformat.Size = New System.Drawing.Size(102, 49)
        Me.lbgauntletformat.TabIndex = 3
        '
        'lbgauntletyear
        '
        Me.lbgauntletyear.FormattingEnabled = True
        Me.lbgauntletyear.ItemHeight = 15
        Me.lbgauntletyear.Items.AddRange(New Object() {"2020", "2019", "2018", "2017", "2016", "2015", "2014"})
        Me.lbgauntletyear.Location = New System.Drawing.Point(448, 16)
        Me.lbgauntletyear.Name = "lbgauntletyear"
        Me.lbgauntletyear.Size = New System.Drawing.Size(120, 49)
        Me.lbgauntletyear.TabIndex = 2
        '
        'Timer1
        '
        '
        'Other_Formats
        '
        Me.Other_Formats.BackColor = System.Drawing.Color.Transparent
        Me.Other_Formats.Controls.Add(Me.PictureBox6)
        Me.Other_Formats.Controls.Add(Me.extract3)
        Me.Other_Formats.Controls.Add(Me.howmuch2)
        Me.Other_Formats.Controls.Add(Me.metag2)
        Me.Other_Formats.Location = New System.Drawing.Point(4, 24)
        Me.Other_Formats.Name = "Other_Formats"
        Me.Other_Formats.Padding = New System.Windows.Forms.Padding(3)
        Me.Other_Formats.Size = New System.Drawing.Size(671, 75)
        Me.Other_Formats.TabIndex = 5
        Me.Other_Formats.Text = "Other Formats"
        '
        'metag2
        '
        Me.metag2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.metag2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.metag2.FormattingEnabled = True
        Me.metag2.Items.AddRange(New Object() {"Budget Standard", "Budget Modern", "Budget Commander", "Duel Commander", "Arena Singleton", "Historic Brawl", "Artisan Historic", "Cascade", "Oathbreaker", "Canadian Highlander", "Old School", "No Banned List Modern", "Frontier", "Tiny Leaders", "Limited", "Block", "Free Form"})
        Me.metag2.Location = New System.Drawing.Point(47, 23)
        Me.metag2.Name = "metag2"
        Me.metag2.Size = New System.Drawing.Size(231, 24)
        Me.metag2.TabIndex = 49
        '
        'howmuch2
        '
        Me.howmuch2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.howmuch2.FormattingEnabled = True
        Me.howmuch2.Items.AddRange(New Object() {"last 8", "last 16", "last 25", "last 50", "all"})
        Me.howmuch2.Location = New System.Drawing.Point(288, 23)
        Me.howmuch2.Name = "howmuch2"
        Me.howmuch2.Size = New System.Drawing.Size(71, 23)
        Me.howmuch2.TabIndex = 50
        '
        'extract3
        '
        Me.extract3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.extract3.ForeColor = System.Drawing.Color.Black
        Me.extract3.Location = New System.Drawing.Point(387, 21)
        Me.extract3.Name = "extract3"
        Me.extract3.Size = New System.Drawing.Size(268, 26)
        Me.extract3.TabIndex = 51
        Me.extract3.Text = "Extract Decks"
        Me.extract3.UseVisualStyleBackColor = True
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(16, 23)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(25, 24)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 52
        Me.PictureBox6.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox6, "Other formats. User submmited decks. No top available.")
        '
        'By_Tournament
        '
        Me.By_Tournament.BackColor = System.Drawing.Color.Transparent
        Me.By_Tournament.Controls.Add(Me.maxtournamentsdecks)
        Me.By_Tournament.Controls.Add(Me.fromweb)
        Me.By_Tournament.Controls.Add(Me.PictureBox5)
        Me.By_Tournament.Controls.Add(Me.maxtournm)
        Me.By_Tournament.Controls.Add(Me.extract4)
        Me.By_Tournament.Controls.Add(Me.ComboBox2)
        Me.By_Tournament.Controls.Add(Me.LinkLabel1)
        Me.By_Tournament.Controls.Add(Me.Label5)
        Me.By_Tournament.Location = New System.Drawing.Point(4, 24)
        Me.By_Tournament.Name = "By_Tournament"
        Me.By_Tournament.Padding = New System.Windows.Forms.Padding(3)
        Me.By_Tournament.Size = New System.Drawing.Size(671, 75)
        Me.By_Tournament.TabIndex = 1
        Me.By_Tournament.Text = "By Tournament"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(39, 144)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(0, 15)
        Me.Label5.TabIndex = 12
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(795, 249)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(0, 15)
        Me.LinkLabel1.TabIndex = 13
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"Standard", "Modern", "Pioneer", "Pauper", "Legacy", "Vintage"})
        Me.ComboBox2.Location = New System.Drawing.Point(133, 22)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(142, 24)
        Me.ComboBox2.TabIndex = 54
        '
        'extract4
        '
        Me.extract4.Location = New System.Drawing.Point(442, 21)
        Me.extract4.Name = "extract4"
        Me.extract4.Size = New System.Drawing.Size(211, 26)
        Me.extract4.TabIndex = 55
        Me.extract4.Text = "Extract Decks"
        Me.extract4.UseVisualStyleBackColor = True
        '
        'maxtournm
        '
        Me.maxtournm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.maxtournm.FormattingEnabled = True
        Me.maxtournm.Items.AddRange(New Object() {"Last One", "Last 2", "Last 3 ", "Last 4", "Last 5", "Last 6", "Last 7", "Last 8", "Last 9", "Last 10"})
        Me.maxtournm.Location = New System.Drawing.Point(281, 22)
        Me.maxtournm.Name = "maxtournm"
        Me.maxtournm.Size = New System.Drawing.Size(80, 23)
        Me.maxtournm.TabIndex = 56
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(16, 21)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(25, 24)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 57
        Me.PictureBox5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox5, "Autoextract last tournament decks by format.")
        '
        'fromweb
        '
        Me.fromweb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.fromweb.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!)
        Me.fromweb.FormattingEnabled = True
        Me.fromweb.Items.AddRange(New Object() {"mtgtop8", "mtggoldfish"})
        Me.fromweb.Location = New System.Drawing.Point(47, 22)
        Me.fromweb.Name = "fromweb"
        Me.fromweb.Size = New System.Drawing.Size(80, 24)
        Me.fromweb.TabIndex = 58
        '
        'maxtournamentsdecks
        '
        Me.maxtournamentsdecks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.maxtournamentsdecks.FormattingEnabled = True
        Me.maxtournamentsdecks.Items.AddRange(New Object() {"Limit 8", "Limit 12", "Limit 16", "Limit 25", "Limit 50"})
        Me.maxtournamentsdecks.Location = New System.Drawing.Point(367, 23)
        Me.maxtournamentsdecks.Name = "maxtournamentsdecks"
        Me.maxtournamentsdecks.Size = New System.Drawing.Size(69, 23)
        Me.maxtournamentsdecks.TabIndex = 59
        '
        'by_metagame
        '
        Me.by_metagame.BackColor = System.Drawing.Color.Transparent
        Me.by_metagame.Controls.Add(Me.PictureBox1)
        Me.by_metagame.Controls.Add(Me.extract1)
        Me.by_metagame.Controls.Add(Me.chktopnumber)
        Me.by_metagame.Controls.Add(Me.howmuch)
        Me.by_metagame.Controls.Add(Me.metagame)
        Me.by_metagame.Location = New System.Drawing.Point(4, 24)
        Me.by_metagame.Name = "by_metagame"
        Me.by_metagame.Padding = New System.Windows.Forms.Padding(3)
        Me.by_metagame.Size = New System.Drawing.Size(671, 75)
        Me.by_metagame.TabIndex = 2
        Me.by_metagame.Text = "Top Formats"
        '
        'metagame
        '
        Me.metagame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.metagame.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.metagame.FormattingEnabled = True
        Me.metagame.Items.AddRange(New Object() {"Standard", "Modern", "Pioneer", "Pauper", "Legacy", "Vintage", "Historic", "Penny Dreadful", "Commander 1v1", "Commander", "Brawl"})
        Me.metagame.Location = New System.Drawing.Point(47, 23)
        Me.metagame.Name = "metagame"
        Me.metagame.Size = New System.Drawing.Size(123, 24)
        Me.metagame.TabIndex = 41
        '
        'howmuch
        '
        Me.howmuch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.howmuch.FormattingEnabled = True
        Me.howmuch.Location = New System.Drawing.Point(178, 23)
        Me.howmuch.Name = "howmuch"
        Me.howmuch.Size = New System.Drawing.Size(65, 23)
        Me.howmuch.TabIndex = 43
        '
        'chktopnumber
        '
        Me.chktopnumber.AutoSize = True
        Me.chktopnumber.Checked = True
        Me.chktopnumber.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chktopnumber.Location = New System.Drawing.Point(249, 25)
        Me.chktopnumber.Name = "chktopnumber"
        Me.chktopnumber.Size = New System.Drawing.Size(99, 19)
        Me.chktopnumber.TabIndex = 44
        Me.chktopnumber.Text = "#top number"
        Me.chktopnumber.UseVisualStyleBackColor = True
        '
        'extract1
        '
        Me.extract1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.extract1.ForeColor = System.Drawing.Color.Black
        Me.extract1.Location = New System.Drawing.Point(373, 21)
        Me.extract1.Name = "extract1"
        Me.extract1.Size = New System.Drawing.Size(278, 26)
        Me.extract1.TabIndex = 45
        Me.extract1.Text = "Extract Decks"
        Me.extract1.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(16, 21)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(25, 24)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 48
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "Select metagame and number of decks to download. Put #top number (by default) for" &
        " get the number in the top in the deck file.")
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.by_metagame)
        Me.TabControl1.Controls.Add(Me.By_Tournament)
        Me.TabControl1.Controls.Add(Me.Other_Formats)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.TabControl1.Location = New System.Drawing.Point(9, 10)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(679, 103)
        Me.TabControl1.TabIndex = 42
        Me.TabControl1.Tag = ""
        '
        'fl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(729, 549)
        Me.Controls.Add(Me.GroupForgeOptions)
        Me.Controls.Add(Me.GroupExtras)
        Me.Controls.Add(Me.group_install)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.MenuGeneral)
        Me.Controls.Add(Me.txlog)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuGeneral
        Me.MaximizeBox = False
        Me.Name = "fl"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forge Launcher"
        Me.group_install.ResumeLayout(False)
        Me.group_install.PerformLayout()
        Me.GroupForgeOptions.ResumeLayout(False)
        Me.GroupForgeOptions.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuGeneral.ResumeLayout(False)
        Me.MenuGeneral.PerformLayout()
        Me.GroupExtras.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.Other_Formats.ResumeLayout(False)
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.By_Tournament.ResumeLayout(False)
        Me.By_Tournament.PerformLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.by_metagame.ResumeLayout(False)
        Me.by_metagame.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txlog As System.Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog2 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents launchforge As Button
    Friend WithEvents rbt_normal As RadioButton
    Friend WithEvents rbt_properties As RadioButton
    Friend WithEvents group_install As GroupBox
    Friend WithEvents Button2 As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents GroupForgeOptions As GroupBox
    Friend WithEvents MenuGeneral As MenuStrip
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents AboutForgeToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ForgeDiscordChannelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ForgeForumToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ForgeWikiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestartForgeLauncherToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WhatsNewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ForzeUpdateForgeLauncherToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupExtras As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents vtoupdate As Label
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RestoreForgePreferencesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReadForgeLogFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenDecksFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents typeofupdate As ComboBox
    Friend WithEvents chklaunchforgeafterupdate As CheckBox
    Friend WithEvents btnlaunchmode As Button
    Friend WithEvents SettingsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents btnupdate As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Button1 As Button
    Friend WithEvents lbgauntletformat As ListBox
    Friend WithEvents lbgauntletyear As ListBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Private WithEvents TextBox2 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents CheckForForgeLauncherUpdatesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkenableprompt As CheckBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents by_metagame As TabPage
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents extract1 As Button
    Friend WithEvents chktopnumber As CheckBox
    Friend WithEvents howmuch As ComboBox
    Friend WithEvents metagame As ComboBox
    Friend WithEvents By_Tournament As TabPage
    Friend WithEvents maxtournamentsdecks As ComboBox
    Friend WithEvents fromweb As ComboBox
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents maxtournm As ComboBox
    Friend WithEvents extract4 As Button
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents Label5 As Label
    Friend WithEvents Other_Formats As TabPage
    Friend WithEvents PictureBox6 As PictureBox
    Friend WithEvents extract3 As Button
    Friend WithEvents howmuch2 As ComboBox
    Friend WithEvents metag2 As ComboBox
End Class
