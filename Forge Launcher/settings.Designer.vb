<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class settings
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
        Me.picsdirbutton = New System.Windows.Forms.Button()
        Me.pics_dir = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
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
        Me.flpref.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
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
        Me.Button2.Location = New System.Drawing.Point(6, 29)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(166, 27)
        Me.Button2.TabIndex = 56
        Me.Button2.Text = "Reset Forge Launcher settings"
        Me.ToolTip1.SetToolTip(Me.Button2, "Reset Forge Launcher will delete your launcher setting file")
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
        Me.ToolTip1.SetToolTip(Me.removepreviousjarfiles, "only if you use releases. Not necessary if you use snapshots. ")
        Me.removepreviousjarfiles.UseVisualStyleBackColor = True
        '
        'UFL
        '
        Me.UFL.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.UFL.Location = New System.Drawing.Point(203, 28)
        Me.UFL.Name = "UFL"
        Me.UFL.Size = New System.Drawing.Size(153, 28)
        Me.UFL.TabIndex = 58
        Me.UFL.Text = "Uninstall Forge Launcher"
        Me.ToolTip1.SetToolTip(Me.UFL, "Uninstall and delete files used by Forge Launcher")
        Me.UFL.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.Button1.Location = New System.Drawing.Point(20, 159)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(379, 27)
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
        Me.flpref.Size = New System.Drawing.Size(379, 51)
        Me.flpref.TabIndex = 57
        Me.flpref.TabStop = False
        Me.flpref.Text = "Launcher Preferences"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.UFL)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!, System.Drawing.FontStyle.Bold)
        Me.GroupBox2.Location = New System.Drawing.Point(20, 79)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(379, 74)
        Me.GroupBox2.TabIndex = 59
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Forge Launcher Settings"
        '
        'preferences
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(417, 198)
        Me.Controls.Add(Me.picsdirbutton)
        Me.Controls.Add(Me.pics_dir)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.flpref)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.downloadeddecks_dir)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.2!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "preferences"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forge Launcher Settings"
        Me.TopMost = True
        Me.flpref.ResumeLayout(False)
        Me.flpref.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog2 As FolderBrowserDialog
    Friend WithEvents Label3 As Label
    Friend WithEvents downloadeddecks_dir As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents picsdirbutton As Button
    Friend WithEvents pics_dir As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents FolderBrowserDialog3 As FolderBrowserDialog
    Friend WithEvents flpref As GroupBox
    Friend WithEvents removepreviousjarfiles As CheckBox
    Friend WithEvents UFL As Button
    Friend WithEvents GroupBox2 As GroupBox
End Class