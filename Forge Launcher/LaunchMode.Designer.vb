<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LaunchMode
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LaunchMode))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblram = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tb_launchline = New System.Windows.Forms.TextBox()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.rbt_normalmode = New System.Windows.Forms.RadioButton()
        Me.rbt_advancedmode = New System.Windows.Forms.RadioButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTip2 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblram)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.tb_launchline)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 47)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(425, 205)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Launch Mode"
        '
        'lblram
        '
        Me.lblram.AutoSize = True
        Me.lblram.Location = New System.Drawing.Point(224, 180)
        Me.lblram.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblram.Name = "lblram"
        Me.lblram.Size = New System.Drawing.Size(68, 15)
        Me.lblram.TabIndex = 4
        Me.lblram.Text = "TOTAL RAM"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(135, 180)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 15)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "RAM available:"
        '
        'tb_launchline
        '
        Me.tb_launchline.Location = New System.Drawing.Point(15, 31)
        Me.tb_launchline.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.tb_launchline.Multiline = True
        Me.tb_launchline.Name = "tb_launchline"
        Me.tb_launchline.Size = New System.Drawing.Size(396, 141)
        Me.tb_launchline.TabIndex = 0
        '
        'rbt_normalmode
        '
        Me.rbt_normalmode.AutoSize = True
        Me.rbt_normalmode.Location = New System.Drawing.Point(10, 13)
        Me.rbt_normalmode.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.rbt_normalmode.Name = "rbt_normalmode"
        Me.rbt_normalmode.Size = New System.Drawing.Size(99, 19)
        Me.rbt_normalmode.TabIndex = 1
        Me.rbt_normalmode.TabStop = True
        Me.rbt_normalmode.Text = "Normal Mode"
        Me.rbt_normalmode.UseVisualStyleBackColor = True
        '
        'rbt_advancedmode
        '
        Me.rbt_advancedmode.AutoSize = True
        Me.rbt_advancedmode.Location = New System.Drawing.Point(159, 13)
        Me.rbt_advancedmode.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.rbt_advancedmode.Name = "rbt_advancedmode"
        Me.rbt_advancedmode.Size = New System.Drawing.Size(113, 19)
        Me.rbt_advancedmode.TabIndex = 2
        Me.rbt_advancedmode.TabStop = True
        Me.rbt_advancedmode.Text = "Advanded Mode"
        Me.ToolTip1.SetToolTip(Me.rbt_advancedmode, "Advanced: Create a bat.file with a custom launch for more RAM assignation.")
        Me.rbt_advancedmode.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(108, 12)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(22, 23)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 49
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "Normal: Forge goes up to 1GB RAM by default (leave blank)")
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(279, 12)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(22, 23)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 50
        Me.PictureBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox2, "Advanced: Create a bat.file with a custom launch for more RAM assignation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CLICK" &
        " TO WRITE AN EXAMPLE.")
        '
        'LaunchMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(442, 264)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.rbt_advancedmode)
        Me.Controls.Add(Me.rbt_normalmode)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LaunchMode"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forge Launcher - Launch Mode"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents rbt_normalmode As RadioButton
    Friend WithEvents rbt_advancedmode As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents lblram As Label
    Friend WithEvents tb_launchline As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ToolTip2 As ToolTip
End Class
