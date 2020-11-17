Imports System.Threading
Public Class Form1

    Delegate Sub ListBoxDelegate(ByVal command As Integer, ByVal myStr As String)
    Dim ListBoxDel As New ListBoxDelegate(AddressOf ListBoxDelMethod)
    Delegate Sub FileLengthDelegate(ByVal myStr As String)
    Dim FileLengthDel As New FileLengthDelegate(AddressOf FileLengthDelMethod)
    Delegate Sub FilePositionDelegate(ByVal myStr As String)
    Dim FilePositionDel As New FilePositionDelegate(AddressOf FilePositionDelMethod)
    Delegate Sub ProgressDelegate(ByVal myStr As Int32)
    Dim ProgressDel As New ProgressDelegate(AddressOf ProgressDelMethod)

    Dim PC_Comm As New Thread(AddressOf PC_Comm_method)
    Dim StopFileStr As String = "1"
    Dim PlayFileStr As String = "2"
    Dim PauseFileStr As String = "3"
    Dim ShowFilesStr As String = "4"
    Dim StartFileListStr As String = "a"
    Dim EndTimerStr As String = "b"
    Dim EndFileListStr As String = "6"
    Dim SendFileNameStr As String = "7"
    Dim RestartSongStr As String = "8"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            SerialPort1.Open()
        Catch
            Console.WriteLine("Failed to open serial port")
        End Try

        SerialPort1.DiscardInBuffer()
        SerialPort1.Encoding = System.Text.Encoding.UTF8 ' this is needed so 8bit data trasmit correctly

        ' Make this a background thread so it automatically
        ' aborts when the main program stops.
        PC_Comm.IsBackground = True
        ' Set the thread priority to lowest
        PC_Comm.Priority = ThreadPriority.Lowest
        ' Start the thread
        PC_Comm.Start()
    End Sub

    ' Delegate function that accesses the ListBox object
    ' command = 2 - clear the contents of the ListBox
    ' command = 3 - add the string to the ListBox
    Public Sub ListBoxDelMethod(ByVal command As Integer, ByVal myStr As String)

        If command = StartFileList Then
            List_File.Items.Clear()
        ElseIf command = EndFileList Then
            List_File.Items.Add(myStr)
        End If
    End Sub
    Public Sub FileLengthDelMethod(ByVal myStr As String)
        file_length.Text = myStr
    End Sub
    Public Sub FilePositionDelMethod(ByVal myStr As String)
        file_position.Text = myStr
    End Sub
    Public Sub ProgressDelMethod(ByVal myInt As Int32)
        progress_bar.Value = myInt
    End Sub

    ' Thread that monitors the receive items on the serial port
    Private Sub PC_Comm_method()
        Dim str As String
        Dim c As Integer
        Dim rt As Int32 ' number of bytes read
        Dim bytes(4) As Byte
        Dim i16 As Int16
        Dim mins As String
        Dim secs As String
        Dim songLength As Int32
        Dim currPos As Int32
        While 1
            If SerialPort1.IsOpen Then
                Try
                    str = SerialPort1.ReadLine() ' Wait for start string
                    Debug.WriteLine(str)

                Catch ex As Exception
                End Try

                ' If command is the StartFileListStr
                If Equals(str, StartFileListStr) Then
                    ' Received a StartFileList response
                    ' clear the list
                    ' Use the delegate to access the ListBox
                    List_File.Invoke(ListBoxDel, StartFileList, "")
                    ' get next string
                    Try
                        str = SerialPort1.ReadLine() ' read file name
                    Catch ex As Exception
                    End Try
                    While (str <> EndFileListStr)
                        ' The read string is a file name and not the EndFileList
                        List_File.Invoke(ListBoxDel, EndFileList, str)
                        Try
                            str = SerialPort1.ReadLine() ' read file name
                        Catch ex As Exception
                        End Try
                    End While
                    ' While loop ends when a 6 is received
                ' If command is not the EndTimerStr
                ElseIf str <> EndTimerStr Then
                    ' format time for display
                    songLength = Convert.ToInt32(str)
                    mins = Convert.ToString(Math.Floor(songLength / 60))
                    secs = Convert.ToString(songLength Mod 60)
                    ' pad seconds to always be 2 digits
                    If secs < 10 Then
                        secs = "0" + secs
                    End If
                    ' display total length of song
                    file_length.Invoke(FileLengthDel, mins + ":" + secs)

                    Try
                        str = SerialPort1.ReadLine() ' read file name
                    Catch ex As Exception
                    End Try
                    ' while not receive EndTimerStr update current position
                    While (str <> EndTimerStr)
                        ' format time for display
                        currPos = Convert.ToInt32(str)
                        mins = Convert.ToString(Math.Floor(currPos / 60))
                        secs = Convert.ToString(currPos Mod 60)
                        ' pad seconds to always be 2 digits
                        If secs < 10 Then
                            secs = "0" + secs
                        End If
                        ' display current position in song
                        file_position.Invoke(FilePositionDel, mins + ":" + secs)
                        ' update progress bar
                        progress_bar.Invoke(ProgressDel, Convert.ToInt32((currPos / songLength) * 100))

                        Try
                            str = SerialPort1.ReadLine() ' read file name
                        Catch ex As Exception
                        End Try
                    End While
                End If
            Else
                Threading.Thread.Sleep(500)
            End If
        End While
    End Sub

    Private Sub Show_Files_Click(sender As Object, e As EventArgs) Handles Show_Files.Click
        If SerialPort1.IsOpen Then
            ' Send show files command
            SerialPort1.Write(ShowFilesStr, 0, 1)
        End If
    End Sub

    Private Sub playBtn_Click(sender As Object, e As EventArgs) Handles playBtn.Click
        Dim b(1) As Byte
        Dim str As String
        b(0) = 0
        ' A value of negative one (-1) is returned if no item is selected
        If Not (List_File.SelectedIndex = -1) Then
            If SerialPort1.IsOpen Then
                SerialPort1.Write(PlayFileStr, 0, 1)
            End If
            'change the label - determine if it is here
            SerialPort1.Write(List_File.SelectedItem)
            'change the label - or here
            SerialPort1.Write(b, 0, 1) ' New Line character at the end of the string
        End If
        'change name to selected song name
        fileNameLabel.Text = List_File.SelectedItem
    End Sub

    Private Sub pauseBtn_Click(sender As Object, e As EventArgs) Handles pauseBtn.Click
        If SerialPort1.IsOpen Then
            ' Send pause file command
            SerialPort1.Write(PauseFileStr, 0, 1)
        End If
    End Sub

    Private Sub stopBtn_Click(sender As Object, e As EventArgs) Handles stopBtn.Click
        If SerialPort1.IsOpen Then
            ' Send stop file command
            SerialPort1.Write(StopFileStr, 0, 1)
        End If
    End Sub
End Class
