<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class preferences
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
        Me.group_forgeoptions = New System.Windows.Forms.GroupBox()
        Me.decksdirbutton = New System.Windows.Forms.Button()
        Me.gauntletsdirbutton = New System.Windows.Forms.Button()
        Me.decks_dir = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gauntlet_dir = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.picsdirbutton = New System.Windows.Forms.Button()
        Me.pics_dir = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.preservedecksnumber = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tournamentsdecks_dir = New System.Windows.Forms.TextBox()
        Me.chk_preservedecks = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.downloadeddecks_dir = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.removepreviousjarfiles = New System.Windows.Forms.CheckBox()
        Me.UFL = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog3 = New System.Windows.Forms.FolderBrowserDialog()
        Me.flpref = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.group_forgeoptions.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.flpref.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'group_forgeoptions
        '
        Me.group_forgeoptions.BackColor = System.Drawing.Color.Transparent
        Me.group_forgeoptions.Controls.Add(Me.decksdirbutton)
        Me.group_forgeoptions.Controls.Add(Me.gauntletsdirbutton)
        Me.group_forgeoptions.Controls.Add(Me.decks_dir)
        Me.group_forgeoptions.Controls.Add(Me.Label1)
        Me.group_forgeoptions.Controls.Add(Me.gauntlet_dir)
        Me.group_forgeoptions.Controls.Add(Me.Label2)
        Me.group_forgeoptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.group_forgeoptions.ForeColor = System.Drawing.Color.Black
        Me.group_forgeoptions.Location = New System.Drawing.Point(20, 68)
        Me.group_forgeoptions.Name = "group_forgeoptions"
        Me.group_forgeoptions.Size = New System.Drawing.Size(706, 107)
        Me.group_forgeoptions.TabIndex = 40
        Me.group_forgeoptions.TabStop = False
        Me.group_forgeoptions.Text = "Forge Directories"
        '
        'decksdirbutton
        '
        Me.decksdirbutton.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.decksdirbutton.Location = New System.Drawing.Point(615, 27)
        Me.decksdirbutton.Name = "decksdirbutton"
        Me.decksdirbutton.Size = New System.Drawing.Size(66, 22)
        Me.decksdirbutton.TabIndex = 2
        Me.decksdirbutton.Text = "Browse"
        Me.decksdirbutton.UseVisualStyleBackColor = True
        '
        'gauntletsdirbutton
        '
        Me.gauntletsdirbutton.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.gauntletsdirbutton.Location = New System.Drawing.Point(615, 66)
        Me.gauntletsdirbutton.Name = "gauntletsdirbutton"
        Me.gauntletsdirbutton.Size = New System.Drawing.Size(66, 22)
        Me.gauntletsdirbutton.TabIndex = 5
        Me.gauntletsdirbutton.Text = "Browse"
        Me.gauntletsdirbutton.UseVisualStyleBackColor = True
        '
        'decks_dir
        '
        Me.decks_dir.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.decks_dir.Location = New System.Drawing.Point(142, 31)
        Me.decks_dir.Name = "decks_dir"
        Me.decks_dir.Size = New System.Drawing.Size(467, 21)
        Me.decks_dir.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(73, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Decks Dir:"
        '
        'gauntlet_dir
        '
        Me.gauntlet_dir.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.gauntlet_dir.Location = New System.Drawing.Point(147, 67)
        Me.gauntlet_dir.Name = "gauntlet_dir"
        Me.gauntlet_dir.Size = New System.Drawing.Size(462, 21)
        Me.gauntlet_dir.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.Label2.Location = New System.Drawing.Point(16, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(125, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Gauntlet Contests Dir:"
        '
        'picsdirbutton
        '
        Me.picsdirbutton.Location = New System.Drawing.Point(605, 546)
        Me.picsdirbutton.Name = "picsdirbutton"
        Me.picsdirbutton.Size = New System.Drawing.Size(66, 22)
        Me.picsdirbutton.TabIndex = 8
        Me.picsdirbutton.Text = "Brownse"
        Me.picsdirbutton.UseVisualStyleBackColor = True
        '
        'pics_dir
        '
        Me.pics_dir.Location = New System.Drawing.Point(138, 546)
        Me.pics_dir.Name = "pics_dir"
        Me.pics_dir.Size = New System.Drawing.Size(462, 21)
        Me.pics_dir.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(57, 546)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 15)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Pics Dir:"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.preservedecksnumber)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.tournamentsdecks_dir)
        Me.GroupBox1.Controls.Add(Me.chk_preservedecks)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Black
        Me.GroupBox1.Location = New System.Drawing.Point(20, 193)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(706, 71)
        Me.GroupBox1.TabIndex = 40
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Decks Extractor Options"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(255, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(94, 15)
        Me.Label6.TabIndex = 56
        Me.Label6.Text = "Extracted Packs"
        '
        'preservedecksnumber
        '
        Me.preservedecksnumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.preservedecksnumber.FormattingEnabled = True
        Me.preservedecksnumber.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"})
        Me.preservedecksnumber.Location = New System.Drawing.Point(213, 31)
        Me.preservedecksnumber.Name = "preservedecksnumber"
        Me.preservedecksnumber.Size = New System.Drawing.Size(39, 23)
        Me.preservedecksnumber.TabIndex = 55
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(430, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(155, 15)
        Me.Label4.TabIndex = 53
        Me.Label4.Text = "Tournaments Folder Name"
        '
        'tournamentsdecks_dir
        '
        Me.tournamentsdecks_dir.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tournamentsdecks_dir.Location = New System.Drawing.Point(593, 33)
        Me.tournamentsdecks_dir.Name = "tournamentsdecks_dir"
        Me.tournamentsdecks_dir.Size = New System.Drawing.Size(88, 21)
        Me.tournamentsdecks_dir.TabIndex = 52
        '
        'chk_preservedecks
        '
        Me.chk_preservedecks.AutoSize = True
        Me.chk_preservedecks.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_preservedecks.Location = New System.Drawing.Point(13, 33)
        Me.chk_preservedecks.Name = "chk_preservedecks"
        Me.chk_preservedecks.Size = New System.Drawing.Size(174, 19)
        Me.chk_preservedecks.TabIndex = 0
        Me.chk_preservedecks.Text = "Preserve Decks saving last" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTip1.SetToolTip(Me.chk_preservedecks, "Metagame Decks Folder is saved with the date info and the decks are not overwritt" &
        "en")
        Me.chk_preservedecks.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(-2, 561)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(218, 15)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "Downloaded Decks Root Folder Name"
        Me.Label3.Visible = False
        '
        'downloadeddecks_dir
        '
        Me.downloadeddecks_dir.Location = New System.Drawing.Point(185, 561)
        Me.downloadeddecks_dir.Name = "downloadeddecks_dir"
        Me.downloadeddecks_dir.Size = New System.Drawing.Size(88, 21)
        Me.downloadeddecks_dir.TabIndex = 50
        Me.downloadeddecks_dir.Visible = False
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.Button2.Location = New System.Drawing.Point(156, 29)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(166, 27)
        Me.Button2.TabIndex = 56
        Me.Button2.Text = "Reset Forge Launcher settings"
        Me.ToolTip1.SetToolTip(Me.Button2, "Reset Forge Launcher will delete your launcher setting files")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'removepreviousjarfiles
        '
        Me.removepreviousjarfiles.AutoSize = True
        Me.removepreviousjarfiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.removepreviousjarfiles.Location = New System.Drawing.Point(13, 22)
        Me.removepreviousjarfiles.Name = "removepreviousjarfiles"
        Me.removepreviousjarfiles.Size = New System.Drawing.Size(237, 19)
        Me.removepreviousjarfiles.TabIndex = 58
        Me.removepreviousjarfiles.Text = "Delete previous Jar file in each update"
        Me.ToolTip1.SetToolTip(Me.removepreviousjarfiles, "only if you use releases (temporarily unavailable). Not necessary if you use snap" &
        "shots. ")
        Me.removepreviousjarfiles.UseVisualStyleBackColor = True
        '
        'UFL
        '
        Me.UFL.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.UFL.Location = New System.Drawing.Point(370, 28)
        Me.UFL.Name = "UFL"
        Me.UFL.Size = New System.Drawing.Size(153, 28)
        Me.UFL.TabIndex = 58
        Me.UFL.Text = "Uninstall Forge Launcher"
        Me.ToolTip1.SetToolTip(Me.UFL, "Uninstall and delete all files used by Forge Launcher")
        Me.UFL.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.Button1.Location = New System.Drawing.Point(176, 371)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(367, 27)
        Me.Button1.TabIndex = 55
        Me.Button1.Text = "Save Settings (restart needed)"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'flpref
        '
        Me.flpref.Controls.Add(Me.removepreviousjarfiles)
        Me.flpref.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.flpref.Location = New System.Drawing.Point(20, 12)
        Me.flpref.Name = "flpref"
        Me.flpref.Size = New System.Drawing.Size(706, 51)
        Me.flpref.TabIndex = 57
        Me.flpref.TabStop = False
        Me.flpref.Text = "Launcher Preferences"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.UFL)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.GroupBox2.Location = New System.Drawing.Point(20, 279)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(714, 74)
        Me.GroupBox2.TabIndex = 59
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Forge Launcher Settings"
        '
        'preferences
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 416)
        Me.Controls.Add(Me.picsdirbutton)
        Me.Controls.Add(Me.pics_dir)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.flpref)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.downloadeddecks_dir)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.group_forgeoptions)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "preferences"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forge Launcher Settings"
        Me.TopMost = True
        Me.group_forgeoptions.ResumeLayout(False)
        Me.group_forgeoptions.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.flpref.ResumeLayout(False)
        Me.flpref.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents group_forgeoptions As GroupBox
    Friend WithEvents decksdirbutton As Button
    Friend WithEvents decks_dir As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents gauntletsdirbutton As Button
    Friend WithEvents gauntlet_dir As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents FolderBrowserDialog2 As FolderBrowserDialog
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents chk_preservedecks As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents downloadeddecks_dir As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tournamentsdecks_dir As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents picsdirbutton As Button
    Friend WithEvents pics_dir As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents FolderBrowserDialog3 As FolderBrowserDialog
    Friend WithEvents preservedecksnumber As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents flpref As GroupBox
    Friend WithEvents removepreviousjarfiles As CheckBox
    Friend WithEvents UFL As Button
    Friend WithEvents GroupBox2 As GroupBox
End Class
