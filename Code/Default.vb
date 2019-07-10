Public Class Form1
    Private Sub Btn_SearchForImage_Click(sender As Object, e As EventArgs) Handles btn_SearchForImage.Click
        'allows user to selet a file and checks its a vaild filepath
        If OFD_ImageInsert.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            'sets Picture box to image user selected
            pb_Input.Image = Image.FromFile(OFD_ImageInsert.FileName)
        End If
        ToBlackWhite()
        DetectPixelGroups()

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
        Dim Group(1, 1) As Integer
        Dim counter As Integer = 1
        Dim objectcounter As Integer = -1
        For x As Integer = 0 To pic.Width - 1
            For y As Integer = 0 To pic.Height - 1 'cycles through image pixel by pixel
                col = pic.GetPixel(x, y)
                If col.R = 0 Then
                    Dim localcounter As Integer = 1
                    objectcounter += 2
                    ReDim Group(objectcounter, counter)
                    Group(0, localcounter - 1) = x
                    Group(1, localcounter - 1) = y
                    localcounter = counter
                    Dim ToCheckX As Queue = New Queue()
                    Dim ToCheckY As Queue = New Queue()
                    ToCheckX.Enqueue(x)
                    ToCheckY.Enqueue(y)
                    Dim Temp(,) As Integer
                    While ToCheckX.Count > 0
                        Temp = SurroundingPixels(ToCheckX.Dequeue, ToCheckY.Dequeue, BinaryImageMatrix)
                        For i As Integer = 0 To ((Temp.Length / 2) - 1)
                            If Temp(0, i) > 0 Then
                                If localcounter = counter Then
                                    counter += 1
                                End If
                                ReDim Preserve Group(objectcounter, counter)
                                pic.SetPixel(Temp(0, i), Temp(1, i), Color.Red)
                                Group(objectcounter, localcounter - 1) = Temp(0, i)
                                Group(objectcounter - 1, localcounter - 1) = Temp(1, i)
                                ToCheckX.Enqueue(Temp(0, i))
                                ToCheckY.Enqueue(Temp(1, i))
                                localcounter += 1
                            End If
                        Next
                    End While
                    pb_Input.Image = pic 'updates Picture Box to display new Image(Red)
                End If
            Next y
        Next x
        IndividualChar(Group, currentchar:=0)
    End Sub
    Function SurroundingPixels(ByRef Xpixel As Integer, ByRef Ypixel As Integer, ByRef BinaryImageMatrix(,) As Integer) As Integer(,)
        Dim counter As Integer = 0
        Dim Pixels(1, counter) As Integer
        For x As Integer = Xpixel - 1 To Xpixel + 1
            For y As Integer = Ypixel - 1 To Ypixel + 1
                If x >= 0 And y >= 0 And ((Xpixel <> x) Xor (Ypixel <> y)) And BinaryImageMatrix(x, y) <> 0 Then
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
    Sub IndividualChar(ByVal Group(,) As Integer, ByVal currentchar As Integer)
        Dim pic As Bitmap = New Bitmap(pb_Input.Image)
        Dim col As Color
        Dim counter As Integer = 0
        Dim Max_X As Integer = 0
        Dim Max_Y As Integer = 0
        Dim Min_X As Integer = 10000
        Dim Min_Y As Integer = 10000
        Dim ScaleFactor(1, 1) As Integer
        Do Until counter = Group.GetLength(1) - 2
            If Group(currentchar, counter) > 0 Then
                If Group(currentchar, counter) > Max_X Then
                    Max_X = Group(currentchar, counter)
                End If
                If Group(currentchar, counter) < Min_X Then
                    Min_X = Group(currentchar, counter)
                End If
            End If
            If Group(currentchar + 1, counter) > 0 Then
                If Group(currentchar + 1, counter) > Max_Y Then
                    Max_Y = Group(currentchar + 1, counter)
                End If
                If Group(currentchar + 1, counter) < Min_Y Then
                    Min_Y = Group(currentchar + 1, counter)
                End If
            End If
            counter += 1
        Loop
        Dim CharMatrix(19, 29) As Integer
        ScaleFactor(0, 0) = (Max_X - Min_X) \ 20
        ScaleFactor(1, 0) = (Max_Y - Min_Y) \ 30
        ScaleFactor(0, 1) = (Max_X - Min_X) Mod 20
        ScaleFactor(1, 1) = (Max_Y - Min_Y) Mod 30
        Dim Significant As Boolean = False
        For YA As Integer = 0 To (Max_Y - Min_Y - ScaleFactor(1, 1) - 1) Step ScaleFactor(1, 0)
            For XA As Integer = 0 To (Max_X - Min_X - ScaleFactor(0, 1) - 1) Step ScaleFactor(0, 0)
                Significant = False
                For y As Integer = 0 To ScaleFactor(1, 0)
                    For x As Integer = 0 To ScaleFactor(0, 1)
                        col = pic.getpixel(XA + x + Min_X, YA + y + Min_Y)
                        If col.B = 0 Then
                            Significant = True
                        End If
                    Next
                Next
                If Significant = True Then
                    CharMatrix(XA / ScaleFactor(0, 0), YA / ScaleFactor(1, 0)) = 1
                Else
                    CharMatrix(XA / ScaleFactor(0, 0), YA / ScaleFactor(1, 0)) = 0
                End If
            Next
        Next
        pb_Input.Image = Image.FromFile("20x30.BMP")
        pic = pb_Input.Image
        For x As Integer = 0 To 19
            For y As Integer = 0 To 29
                If CharMatrix(x, y) = 1 Then
                    pic.SetPixel(x, y, Color.Black)
                End If
            Next
        Next
        pb_Input.Image = pic

        'NEED TO DISPLAY IMAGE IN PICTURE BOX AFTER DETECTING BOUNDARIES TO THEN BE ABLE TO PERFORM AI
    End Sub
    Function YesNoBox(ByVal Message As String)
        Dim result As String = MsgBox(Message, vbYesNo)
        If result = vbYes Then
            Return True
        End If
    End Function
    Sub ToBlackWhite()
        'Turns Bitmap images as input into Black and white
        Dim pic As Bitmap = New Bitmap(pb_Input.Image) 'uses image that user uploaded to picture box 
        Dim gem, total As Integer
        Dim col As Color
        'loops through each pixel 
        For x As Integer = 0 To pic.Width - 1
            For y As Integer = 0 To pic.Height - 1
                col = pic.GetPixel(x, y) 'retrieves current pixel colour values 
                total += col.R 'retrieves colour values for RBG of current pixel
                total += col.G
                total += col.B
            Next y
        Next x
        total = total \ 3
        total = total \ (pic.Width * pic.Height)
        For x As Integer = 0 To pic.Width - 1
            For y As Integer = 0 To pic.Height - 1
                col = pic.GetPixel(x, y)
                'retrieves current pixel colour values 
                gem = 0
                gem += (col.B)
                gem += (col.R)
                gem += (col.G)
                gem = gem / 3
                'averages RGB colour values
                If gem > total Then 'if more than half then ixel is set to white,ViceVersa
                    pic.SetPixel(x, y, Color.White)
                Else
                    pic.SetPixel(x, y, Color.Black)
                End If
            Next y
        Next x
        pb_Input.Image = pic 'updates Picture Box to display new Image(BW)
    End Sub
End Class
