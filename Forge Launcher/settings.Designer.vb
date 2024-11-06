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
        Me.picsdirbutton.Location = New System.Drawing.Point(706, 630)
        Me.picsdirbutton.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.picsdirbutton.Name = "picsdirbutton"
        Me.picsdirbutton.Size = New System.Drawing.Size(77, 25)
        Me.picsdirbutton.TabIndex = 8
        Me.picsdirbutton.Text = "Brownse"
        Me.picsdirbutton.UseVisualStyleBackColor = True
        '
        'pics_dir
        '
        Me.pics_dir.Location = New System.Drawing.Point(161, 630)
        Me.pics_dir.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.pics_dir.Name = "pics_dir"
        Me.pics_dir.Size = New System.Drawing.Size(538, 23)
        Me.pics_dir.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(66, 630)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 15)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Pics Dir:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(-2, 647)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(207, 15)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "Downloaded Decks Root Folder Name"
        Me.Label3.Visible = False
        '
        'downloadeddecks_dir
        '
        Me.downloadeddecks_dir.Location = New System.Drawing.Point(216, 647)
        Me.downloadeddecks_dir.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.downloadeddecks_dir.Name = "downloadeddecks_dir"
        Me.downloadeddecks_dir.Size = New System.Drawing.Size(102, 23)
        Me.downloadeddecks_dir.TabIndex = 50
        Me.downloadeddecks_dir.Visible = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(24, 33)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(194, 31)
        Me.Button2.TabIndex = 56
        Me.Button2.Text = "Reset Forge Launcher settings"
        Me.ToolTip1.SetToolTip(Me.Button2, "Reset Forge Launcher will delete your launcher setting file")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'removepreviousjarfiles
        '
        Me.removepreviousjarfiles.AutoSize = True
        Me.removepreviousjarfiles.Location = New System.Drawing.Point(15, 25)
        Me.removepreviousjarfiles.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.removepreviousjarfiles.Name = "removepreviousjarfiles"
        Me.removepreviousjarfiles.Size = New System.Drawing.Size(224, 19)
        Me.removepreviousjarfiles.TabIndex = 58
        Me.removepreviousjarfiles.Text = "Delete previous Jar file in each update"
        Me.ToolTip1.SetToolTip(Me.removepreviousjarfiles, "only if you use releases. Not necessary if you use snapshots. ")
        Me.removepreviousjarfiles.UseVisualStyleBackColor = True
        '
        'UFL
        '
        Me.UFL.Location = New System.Drawing.Point(237, 32)
        Me.UFL.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.UFL.Name = "UFL"
        Me.UFL.Size = New System.Drawing.Size(178, 32)
        Me.UFL.TabIndex = 58
        Me.UFL.Text = "Uninstall Forge Launcher"
        Me.ToolTip1.SetToolTip(Me.UFL, "Uninstall and delete files used by Forge Launcher")
        Me.UFL.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(23, 183)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(442, 31)
        Me.Button1.TabIndex = 55
        Me.Button1.Text = "Save Settings (restart needed)"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'flpref
        '
        Me.flpref.Controls.Add(Me.removepreviousjarfiles)
        Me.flpref.Location = New System.Drawing.Point(23, 14)
        Me.flpref.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.flpref.Name = "flpref"
        Me.flpref.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.flpref.Size = New System.Drawing.Size(442, 59)
        Me.flpref.TabIndex = 57
        Me.flpref.TabStop = False
        Me.flpref.Text = "Launcher Preferences"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.UFL)
        Me.GroupBox2.Location = New System.Drawing.Point(23, 91)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox2.Size = New System.Drawing.Size(442, 85)
        Me.GroupBox2.TabIndex = 59
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Forge Launcher Settings"
        '
        'settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 228)
        Me.Controls.Add(Me.picsdirbutton)
        Me.Controls.Add(Me.pics_dir)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.flpref)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.downloadeddecks_dir)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "settings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forge Launcher - Settings"
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