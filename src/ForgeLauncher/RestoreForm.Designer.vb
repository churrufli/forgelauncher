<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RestoreForm
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
        Me.lblIntro = New System.Windows.Forms.Label()
        Me.pnlList = New ForgeLauncher.BorderPanel()
        Me.lstPackages = New System.Windows.Forms.ListBox()
        Me.lblNote = New System.Windows.Forms.Label()
        Me.tglSkip = New ForgeLauncher.ToggleSwitch()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnCancel = New ForgeLauncher.DarkButton()
        Me.btnRestore = New ForgeLauncher.DarkButton()
        Me.pnlList.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.SuspendLayout()

        Me.lblIntro.AutoSize = True
        Me.lblIntro.Location = New System.Drawing.Point(16, 14)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(213, 15)
        Me.lblIntro.TabIndex = 0
        Me.lblIntro.Text = "Choose the Forge version to restore."

        Me.pnlList.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer))
        Me.pnlList.Controls.Add(Me.lstPackages)
        Me.pnlList.Location = New System.Drawing.Point(16, 40)
        Me.pnlList.Name = "pnlList"
        Me.pnlList.Padding = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.pnlList.Size = New System.Drawing.Size(428, 136)
        Me.pnlList.TabIndex = 1

        Me.lstPackages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstPackages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstPackages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lstPackages.IntegralHeight = False
        Me.lstPackages.ItemHeight = 34
        Me.lstPackages.Location = New System.Drawing.Point(2, 4)
        Me.lstPackages.Name = "lstPackages"
        Me.lstPackages.Size = New System.Drawing.Size(424, 128)
        Me.lstPackages.TabIndex = 0

        Me.lblNote.ForeColor = System.Drawing.Color.FromArgb(CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.lblNote.Location = New System.Drawing.Point(16, 186)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(428, 34)
        Me.lblNote.TabIndex = 2
        Me.lblNote.Text = "Your decks, preferences and card images are kept. Files that only exist in the version you leave are removed."

        Me.tglSkip.Location = New System.Drawing.Point(16, 224)
        Me.tglSkip.Name = "tglSkip"
        Me.tglSkip.Size = New System.Drawing.Size(428, 32)
        Me.tglSkip.TabIndex = 3

        Me.pnlFooter.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
        Me.pnlFooter.Controls.Add(Me.btnCancel)
        Me.pnlFooter.Controls.Add(Me.btnRestore)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 268)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(460, 52)
        Me.pnlFooter.TabIndex = 4

        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(210, 11)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(88, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"

        Me.btnRestore.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnRestore.Location = New System.Drawing.Point(304, 11)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(140, 30)
        Me.btnRestore.TabIndex = 0
        Me.btnRestore.Text = "Restore version"
        Me.btnRestore.Variant = ForgeLauncher.ButtonVariant.Primary

        Me.AcceptButton = Me.btnRestore
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(460, 320)
        Me.Controls.Add(Me.lblIntro)
        Me.Controls.Add(Me.pnlList)
        Me.Controls.Add(Me.lblNote)
        Me.Controls.Add(Me.tglSkip)
        Me.Controls.Add(Me.pnlFooter)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RestoreForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Restore a previous version"
        Me.pnlList.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents pnlList As ForgeLauncher.BorderPanel
    Friend WithEvents lstPackages As System.Windows.Forms.ListBox
    Friend WithEvents lblNote As System.Windows.Forms.Label
    Friend WithEvents tglSkip As ForgeLauncher.ToggleSwitch
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As ForgeLauncher.DarkButton
    Friend WithEvents btnRestore As ForgeLauncher.DarkButton
End Class
