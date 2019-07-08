Public Class Form1
    Private Sub Btn_SearchForImage_Click(sender As Object, e As EventArgs) Handles btn_SearchForImage.Click
        'allows user to selet a file and checks its a vaild filepath
        If OFD_ImageInsert.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            'sets Picture box to image user selected
            pb_Input.Image = Image.FromFile(OFD_ImageInsert.FileName)
        End If
        MsgBox("Continue to Black and White ?")
        'calls Black and White function
        ToBlackWhite()

    End Sub
    Sub DetectPixelGroups()
        Dim pic As Bitmap = New Bitmap(pb_Input.Image)
        Dim col As Color
        Dim BinaryImageMatrix(pic.Width - 1, pic.Height - 1) As Integer
        'transfers BW into Binary Matrixm, a 1 being where image has a black pixel
        For x As Integer = 0 To pic.Width - 1
            For y As Integer = 0 To pic.Height - 1 'cycles through image pixel by pixel
                col = pic.GetPixel(x, y)
                If col.R > 0 Then 'if r value is ">  0" then it cant be black so must be white 
                    BinaryImageMatrix(x, y) = 0
                Else
                    BinaryImageMatrix(x, y) = 1
                End If
            Next y
        Next x
        Dim Group(1, 0) As Integer
        Dim counter As Integer = 0
        For x As Integer = 0 To pic.Width - 1
            For y As Integer = 0 To pic.Height - 1 'cycles through image pixel by pixel
                col = pic.GetPixel(x, y)
                If col.R < 0 Then
                    Group(0, counter) = x
                    Group(1, counter) = y
                    counter = counter + 1
                    Dim ToCheckX As Queue
                    Dim ToCheckY As Queue
                    ToCheckX.Enqueue(x)
                    ToCheckY.Enqueue(y)
                    For Each element In ToCheckX
                        Dim Temp(,) As Integer = SurroundingPixels(ToCheckX.Dequeue, ToCheckY.Dequeue, BinaryImageMatrix)
                        For i As Integer = 0 To Temp.GetLength(0) - 1
                            ReDim Preserve Group(0, counter)
                            Group(0, counter) = Temp(0, i)
                            Group(1, counter) = Temp(1, i)
                            ToCheckX.Enqueue(Temp(0, i))
                            ToCheckY.Enqueue(Temp(1, i))
                            counter += 1
                        Next
                    Next
                End If
            Next y
        Next x
    End Sub
    Function SurroundingPixels(ByRef Xpixel As Integer, ByRef Ypixel As Integer, ByRef BinaryImageMatrix(,) As Integer) As Integer(,)
        Dim counter As Integer = 0
        Dim Pixels(1, counter) As Integer
        For x As Integer = Xpixel - 1 To Xpixel + 1
            For y As Integer = Ypixel - 1 To Ypixel + 1
                If x >= 0 And y >= 0 And (Xpixel <> x Xor Ypixel <> y) And BinaryImageMatrix(x, y) <> 0 Then
                    ReDim Preserve Pixels(1, counter)
                    BinaryImageMatrix(x, y) = 0
                    Pixels(0, counter) = x
                    Pixels(1, counter) = y
                    counter = counter + 1
                End If
            Next
        Next
        Return Pixels
    End Function
    Function YesNoBox(ByVal Message As String)
        Dim result As String = MsgBox(Message, vbYesNo)
        If result = vbYes Then
            Return True
        End If
    End Function
    Function ToBlackWhite()
        'Turns Bitmap images as input into Black and white
        Dim pic As Bitmap = New Bitmap(pb_Input.Image) 'uses image that user uploaded to picture box 
        Dim gem, r, g, b As Integer
        Dim col As Color
        'loops through each pixel 
        For x As Integer = 0 To pic.Width - 1
            For y As Integer = 0 To pic.Height - 1
                col = pic.GetPixel(x, y) 'retrieves current pixel colour values 
                r = col.R 'retrieves colour values for RBG of current pixel
                g = col.G
                b = col.B
                gem = (r + g + b) / 3 'averages RGB colour values
                If gem > 128 Then 'if more than half then ixel is set to white,ViceVersa
                    pic.SetPixel(x, y, Color.White)
                Else
                    pic.SetPixel(x, y, Color.Black)
                End If
            Next y
        Next x
        pb_Input.Image = pic 'updates Picture Box to display new Image(BW)
    End Function
End Class
