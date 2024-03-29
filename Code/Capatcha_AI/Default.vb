﻿Public Class Form1
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
        InduvidualChar(Group, currentchar:=0)
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


    Function YesNoBox(ByVal Message As String)
        Dim result As String = MsgBox(Message, vbYesNo)
        If result = vbYes Then
            Return True
        End If
    End Function


  Sub InduvidualChar(ByVal Group(,) As Integer, ByVal currentchar As Integer)
        Dim counter As Integer = 0
        Dim Max_X As Integer = 0
        Dim Max_Y As Integer = 0
        Dim Min_X As Integer = 10000
        Dim Min_Y As Integer = 10000
        Dim ScaleFactor(1, 1) As Integer
        Do Until (Group(currentchar, counter) = 0 And Group(currentchar + 1, counter) = 0) Or counter - 1 = Group.GetLength(currentchar)
            If Group(currentchar, counter) > Max_X Then
                Max_X = Group(currentchar, counter)
            ElseIf Group(currentchar, counter) < Min_X Then
                Min_X = Group(currentchar, counter)
            End If
            If Group(currentchar + 1, counter) > Max_Y Then
                Max_Y = Group(currentchar, counter)
            ElseIf Group(currentchar + 1, counter) < Min_Y Then
                Min_Y = Group(currentchar, counter)
            End If
            counter += 1
        Loop

        ScaleFactor(0, 0) = (Max_X - Min_X) \ 20
        ScaleFactor(1, 0) = (Max_Y - Min_Y) \ 30
ScaleFactor(0, 1) = (Max_X - Min_X) mod 20
ScaleFactor(1, 1) = (Max_Y - Min_Y) mod 30
Dim localavg As boolean = false
        For YA As Integer = 0 To (Max_Y - Min_Y) Step ScaleFactor(1, 0)
            For XA As Integer = 0 To (Max_X - Min_X) Step ScaleFactor(0, 0)
                For y As Integer = 0 To ScaleFactor(1, 0)
For x As Integer = 0 To ScaleFactor(0, 1)'Coding Here
                        Group(XA + x, YA + y)
                    Next
                Next
                If localavg / (ScaleFactor(1, 0) * ScaleFactor(0, 0)) Then

                End If
            Next
        Next


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
