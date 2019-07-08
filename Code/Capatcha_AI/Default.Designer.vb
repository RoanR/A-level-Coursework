<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pb_Input = New System.Windows.Forms.PictureBox()
        Me.btn_SearchForImage = New System.Windows.Forms.Button()
        Me.OFD_ImageInsert = New System.Windows.Forms.OpenFileDialog()
        CType(Me.pb_Input, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pb_Input
        '
        Me.pb_Input.Location = New System.Drawing.Point(12, 12)
        Me.pb_Input.Name = "pb_Input"
        Me.pb_Input.Size = New System.Drawing.Size(782, 596)
        Me.pb_Input.TabIndex = 1
        Me.pb_Input.TabStop = False
        '
        'btn_SearchForImage
        '
        Me.btn_SearchForImage.Location = New System.Drawing.Point(12, 12)
        Me.btn_SearchForImage.Name = "btn_SearchForImage"
        Me.btn_SearchForImage.Size = New System.Drawing.Size(149, 23)
        Me.btn_SearchForImage.TabIndex = 2
        Me.btn_SearchForImage.Text = "View"
        Me.btn_SearchForImage.UseVisualStyleBackColor = True
        '
        'OFD_ImageInsert
        '
        Me.OFD_ImageInsert.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(806, 620)
        Me.Controls.Add(Me.btn_SearchForImage)
        Me.Controls.Add(Me.pb_Input)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.pb_Input, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pb_Input As PictureBox
    Friend WithEvents btn_SearchForImage As Button
    Friend WithEvents OFD_ImageInsert As OpenFileDialog
End Class
