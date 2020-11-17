<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.playBtn = New System.Windows.Forms.Button()
        Me.stopBtn = New System.Windows.Forms.Button()
        Me.Show_Files = New System.Windows.Forms.Button()
        Me.fileNameLabel = New System.Windows.Forms.Label()
        Me.progress_bar = New System.Windows.Forms.ProgressBar()
        Me.file_position = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.file_length = New System.Windows.Forms.Label()
        Me.List_File = New System.Windows.Forms.ListBox()
        Me.pauseBtn = New System.Windows.Forms.Button()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.SuspendLayout()
        '
        'playBtn
        '
        Me.playBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.playBtn.Location = New System.Drawing.Point(24, 256)
        Me.playBtn.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.playBtn.Name = "playBtn"
        Me.playBtn.Size = New System.Drawing.Size(499, 131)
        Me.playBtn.TabIndex = 0
        Me.playBtn.Text = "Play"
        Me.playBtn.UseVisualStyleBackColor = True
        '
        'stopBtn
        '
        Me.stopBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stopBtn.Location = New System.Drawing.Point(1075, 256)
        Me.stopBtn.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.stopBtn.Name = "stopBtn"
        Me.stopBtn.Size = New System.Drawing.Size(499, 131)
        Me.stopBtn.TabIndex = 1
        Me.stopBtn.Text = "Stop"
        Me.stopBtn.UseVisualStyleBackColor = True
        '
        'Show_Files
        '
        Me.Show_Files.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Show_Files.Location = New System.Drawing.Point(24, 758)
        Me.Show_Files.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.Show_Files.Name = "Show_Files"
        Me.Show_Files.Size = New System.Drawing.Size(1548, 61)
        Me.Show_Files.TabIndex = 2
        Me.Show_Files.Text = "Show Files"
        Me.Show_Files.UseVisualStyleBackColor = True
        '
        'fileNameLabel
        '
        Me.fileNameLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fileNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fileNameLabel.Location = New System.Drawing.Point(24, 11)
        Me.fileNameLabel.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.fileNameLabel.Name = "fileNameLabel"
        Me.fileNameLabel.Size = New System.Drawing.Size(1552, 80)
        Me.fileNameLabel.TabIndex = 3
        Me.fileNameLabel.Text = "file.wav"
        Me.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'progress_bar
        '
        Me.progress_bar.Location = New System.Drawing.Point(24, 104)
        Me.progress_bar.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.progress_bar.Name = "progress_bar"
        Me.progress_bar.Size = New System.Drawing.Size(1552, 39)
        Me.progress_bar.TabIndex = 4
        '
        'file_position
        '
        Me.file_position.AutoSize = True
        Me.file_position.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.file_position.Location = New System.Drawing.Point(628, 168)
        Me.file_position.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.file_position.Name = "file_position"
        Me.file_position.Size = New System.Drawing.Size(141, 67)
        Me.file_position.TabIndex = 5
        Me.file_position.Text = "0:45"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(783, 168)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 67)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "/"
        '
        'file_length
        '
        Me.file_length.AutoSize = True
        Me.file_length.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.file_length.Location = New System.Drawing.Point(840, 168)
        Me.file_length.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.file_length.Name = "file_length"
        Me.file_length.Size = New System.Drawing.Size(141, 67)
        Me.file_length.TabIndex = 7
        Me.file_length.Text = "1:00"
        '
        'List_File
        '
        Me.List_File.FormattingEnabled = True
        Me.List_File.ItemHeight = 25
        Me.List_File.Location = New System.Drawing.Point(24, 408)
        Me.List_File.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.List_File.Name = "List_File"
        Me.List_File.Size = New System.Drawing.Size(1548, 329)
        Me.List_File.TabIndex = 8
        '
        'pauseBtn
        '
        Me.pauseBtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pauseBtn.Location = New System.Drawing.Point(548, 258)
        Me.pauseBtn.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.pauseBtn.Name = "pauseBtn"
        Me.pauseBtn.Size = New System.Drawing.Size(499, 131)
        Me.pauseBtn.TabIndex = 9
        Me.pauseBtn.Text = "Pause"
        Me.pauseBtn.UseVisualStyleBackColor = True
        '
        'SerialPort1
        '
        Me.SerialPort1.PortName = "COM4"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1600, 846)
        Me.Controls.Add(Me.pauseBtn)
        Me.Controls.Add(Me.List_File)
        Me.Controls.Add(Me.file_length)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.file_position)
        Me.Controls.Add(Me.progress_bar)
        Me.Controls.Add(Me.fileNameLabel)
        Me.Controls.Add(Me.Show_Files)
        Me.Controls.Add(Me.stopBtn)
        Me.Controls.Add(Me.playBtn)
        Me.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents playBtn As Button
    Friend WithEvents stopBtn As Button
    Friend WithEvents Show_Files As Button
    Friend WithEvents fileNameLabel As Label
    Friend WithEvents progress_bar As ProgressBar
    Friend WithEvents file_position As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents file_length As Label
    Friend WithEvents List_File As ListBox
    Friend WithEvents pauseBtn As Button
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
End Class
