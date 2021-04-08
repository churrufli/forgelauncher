<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class whatsnew
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Release_Notes = New System.Windows.Forms.TabPage()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Release_Notes.SuspendLayout
        Me.TabControl2.SuspendLayout
        Me.TabPage1.SuspendLayout
        Me.SuspendLayout
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "What's New?"
        '
        'Release_Notes
        '
        Me.Release_Notes.BackColor = System.Drawing.Color.Transparent
        Me.Release_Notes.Controls.Add(Me.TextBox1)
        Me.Release_Notes.Location = New System.Drawing.Point(4, 22)
        Me.Release_Notes.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Release_Notes.Name = "Release_Notes"
        Me.Release_Notes.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Release_Notes.Size = New System.Drawing.Size(452, 202)
        Me.Release_Notes.TabIndex = 0
        Me.Release_Notes.Text = "Launcher Release Notes"
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Location = New System.Drawing.Point(2, 5)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox1.Multiline = true
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = true
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(449, 195)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.TabStop = false
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.Release_Notes)
        Me.TabControl2.Controls.Add(Me.TabPage1)
        Me.TabControl2.Location = New System.Drawing.Point(1, 35)
        Me.TabControl2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(460, 228)
        Me.TabControl2.TabIndex = 44
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.Transparent
        Me.TabPage1.Controls.Add(Me.LinkLabel4)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.LinkLabel3)
        Me.TabPage1.Controls.Add(Me.LinkLabel2)
        Me.TabPage1.Controls.Add(Me.LinkLabel1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TabPage1.Size = New System.Drawing.Size(452, 202)
        Me.TabPage1.TabIndex = 1
        Me.TabPage1.Text = "About this tool"
        '
        'LinkLabel4
        '
        Me.LinkLabel4.AutoSize = true
        Me.LinkLabel4.Location = New System.Drawing.Point(178, 109)
        Me.LinkLabel4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(98, 13)
        Me.LinkLabel4.TabIndex = 6
        Me.LinkLabel4.TabStop = true
        Me.LinkLabel4.Text = "My GitHub Projects"
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.Location = New System.Drawing.Point(163, 40)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Leave your suggestion at:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Location = New System.Drawing.Point(29, 150)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(399, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "PLEASE DON'T ABUSE EXTRACTING DECKS AND SUPPORT THE WEBSITES"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = true
        Me.LinkLabel3.Location = New System.Drawing.Point(231, 174)
        Me.LinkLabel3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(45, 13)
        Me.LinkLabel3.TabIndex = 3
        Me.LinkLabel3.TabStop = true
        Me.LinkLabel3.Text = "mtgtop8"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = true
        Me.LinkLabel2.Location = New System.Drawing.Point(166, 174)
        Me.LinkLabel2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(60, 13)
        Me.LinkLabel2.TabIndex = 2
        Me.LinkLabel2.TabStop = true
        Me.LinkLabel2.Text = "mtggoldfish"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = true
        Me.LinkLabel1.Location = New System.Drawing.Point(80, 62)
        Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(311, 13)
        Me.LinkLabel1.TabIndex = 0
        Me.LinkLabel1.TabStop = true
        Me.LinkLabel1.Text = "Tools for Forge Users in Windows - Topic in Slighty Magic Forum"
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(94, 19)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(288, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "A Windows tool for Forge created by a collaborator and fan."
        '
        'whatsnew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(470, 273)
        Me.Controls.Add(Me.TabControl2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "whatsnew"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = true
        Me.Release_Notes.ResumeLayout(false)
        Me.Release_Notes.PerformLayout
        Me.TabControl2.ResumeLayout(false)
        Me.TabPage1.ResumeLayout(false)
        Me.TabPage1.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Release_Notes As TabPage
    Private WithEvents TextBox1 As TextBox
    Friend WithEvents TabControl2 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents Label2 As Label
    Friend WithEvents LinkLabel3 As LinkLabel
    Friend WithEvents LinkLabel2 As LinkLabel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LinkLabel4 As LinkLabel
End Class
