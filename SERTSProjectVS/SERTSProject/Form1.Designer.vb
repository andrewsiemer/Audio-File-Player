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
        Me.components = New System.ComponentModel.Container()
        Me.playBtn = New System.Windows.Forms.Button()
        Me.stopBtn = New System.Windows.Forms.Button()
        Me.Show_Files = New System.Windows.Forms.Button()
        Me.fileNameLabel = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.List_File = New System.Windows.Forms.ListBox()
        Me.pauseBtn = New System.Windows.Forms.Button()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.Send_File = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'playBtn
        '
        Me.playBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.playBtn.Location = New System.Drawing.Point(12, 133)
        Me.playBtn.Name = "playBtn"
        Me.playBtn.Size = New System.Drawing.Size(249, 68)
        Me.playBtn.TabIndex = 0
        Me.playBtn.Text = "Play"
        Me.playBtn.UseVisualStyleBackColor = True
        '
        'stopBtn
        '
        Me.stopBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stopBtn.Location = New System.Drawing.Point(537, 133)
        Me.stopBtn.Name = "stopBtn"
        Me.stopBtn.Size = New System.Drawing.Size(249, 68)
        Me.stopBtn.TabIndex = 1
        Me.stopBtn.Text = "Stop"
        Me.stopBtn.UseVisualStyleBackColor = True
        '
        'Show_Files
        '
        Me.Show_Files.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Show_Files.Location = New System.Drawing.Point(12, 394)
        Me.Show_Files.Name = "Show_Files"
        Me.Show_Files.Size = New System.Drawing.Size(373, 32)
        Me.Show_Files.TabIndex = 2
        Me.Show_Files.Text = "Show Files"
        Me.Show_Files.UseVisualStyleBackColor = True
        '
        'fileNameLabel
        '
        Me.fileNameLabel.AutoSize = True
        Me.fileNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fileNameLabel.Location = New System.Drawing.Point(327, 9)
        Me.fileNameLabel.Name = "fileNameLabel"
        Me.fileNameLabel.Size = New System.Drawing.Size(142, 42)
        Me.fileNameLabel.TabIndex = 3
        Me.fileNameLabel.Text = "file.wav"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 54)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(776, 20)
        Me.ProgressBar1.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(314, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 33)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "0:45"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(391, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 33)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "/"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(420, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 33)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "1:00"
        '
        'List_File
        '
        Me.List_File.FormattingEnabled = True
        Me.List_File.Location = New System.Drawing.Point(12, 212)
        Me.List_File.Name = "List_File"
        Me.List_File.Size = New System.Drawing.Size(776, 173)
        Me.List_File.TabIndex = 8
        '
        'pauseBtn
        '
        Me.pauseBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pauseBtn.Location = New System.Drawing.Point(274, 134)
        Me.pauseBtn.Name = "pauseBtn"
        Me.pauseBtn.Size = New System.Drawing.Size(249, 68)
        Me.pauseBtn.TabIndex = 9
        Me.pauseBtn.Text = "Pause"
        Me.pauseBtn.UseVisualStyleBackColor = True
        '
        'SerialPort1
        '
        Me.SerialPort1.PortName = "COM6"
        '
        'Send_File
        '
        Me.Send_File.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Send_File.Location = New System.Drawing.Point(397, 394)
        Me.Send_File.Name = "Send_File"
        Me.Send_File.Size = New System.Drawing.Size(373, 32)
        Me.Send_File.TabIndex = 10
        Me.Send_File.Text = "Send File"
        Me.Send_File.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 440)
        Me.Controls.Add(Me.Send_File)
        Me.Controls.Add(Me.pauseBtn)
        Me.Controls.Add(Me.List_File)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.fileNameLabel)
        Me.Controls.Add(Me.Show_Files)
        Me.Controls.Add(Me.stopBtn)
        Me.Controls.Add(Me.playBtn)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents playBtn As Button
    Friend WithEvents stopBtn As Button
    Friend WithEvents Show_Files As Button
    Friend WithEvents fileNameLabel As Label
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents List_File As ListBox
    Friend WithEvents pauseBtn As Button
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents Send_File As Button
End Class
