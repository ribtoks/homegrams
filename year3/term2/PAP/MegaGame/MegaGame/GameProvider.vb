Public Class GameProvider
    Private list1 As List(Of Mosquito)
    Private list2 As List(Of Mosquito)
    Private list3 As List(Of Mosquito)
    Private bloodPlaces As List(Of DeadPoint)

    Private gameImage As Bitmap
    Private mosquitoImage As Bitmap
    Private bloodImage As Bitmap

    Private _currCount As Integer
    Private rand As Random
    Private delta As Integer

    Private rect As Rectangle

    Private tempP As Point


    Public Sub New(ByVal pictureWidth As Integer, ByVal pictureHeight As Integer, ByVal MosquitoImage As Image, ByVal BloodImage As Image)
        Me.rand = New Random(DateTime.Now.Millisecond)
        Me.delta = 45
        Me.list1 = New List(Of Mosquito)
        Me.list2 = New List(Of Mosquito)
        Me.list3 = New List(Of Mosquito)

        Me.gameImage = New Bitmap(pictureWidth, pictureHeight)
        Me.mosquitoImage = New Bitmap(MosquitoImage)
        Me.bloodImage = New Bitmap(BloodImage)
        rect = New Rectangle(0, 0, 0, 0)

        Me.bloodPlaces = New List(Of DeadPoint)
    End Sub


    Public Sub AddMosquito1(ByVal p As Point)
        Dim temp As New Mosquito(p)
        temp.DeltaX = (Me.rand.Next(2, 5) * Math.Sign(CInt((Me.rand.Next - Me.rand.Next))))
        temp.DeltaY = (Me.rand.Next(2, 5) * Math.Sign(CInt((Me.rand.Next - Me.rand.Next))))

        Me.list1.Add(temp)
        Me._currCount += 1
    End Sub

    Public Sub AddMosquito2(ByVal p As Point)
        Dim temp As New Mosquito(p)
        temp.DeltaX = (Me.rand.Next(5, 8) * Math.Sign(CInt((Me.rand.Next - Me.rand.Next))))
        temp.DeltaY = (Me.rand.Next(5, 8) * Math.Sign(CInt((Me.rand.Next - Me.rand.Next))))
        Me.list2.Add(temp)
        Me._currCount += 1
    End Sub

    Public Sub AddMosquito3(ByVal p As Point)
        Dim temp As New Mosquito(p)
        temp.DeltaX = (Me.rand.Next(8, 11) * Math.Sign(CInt((Me.rand.Next - Me.rand.Next))))
        temp.DeltaY = (Me.rand.Next(8, 11) * Math.Sign(CInt((Me.rand.Next - Me.rand.Next))))
        Me.list3.Add(temp)
        Me._currCount += 1
    End Sub

    Private Sub DrawMosquitos(ByRef gc As Graphics, ByRef list As List(Of Mosquito))
        Dim i As Integer
        For i = 0 To list.Count - 1
            rect.X = list.Item(i).CurrPoint.X
            rect.Y = list.Item(i).CurrPoint.Y
            rect.Width = Me.mosquitoImage.Width + Me.rand.Next(10)
            rect.Height = Me.mosquitoImage.Height + Me.rand.Next(10)
            gc.DrawImage(Me.mosquitoImage, rect)
        Next i
    End Sub

    ''' <summary>
    ''' Updates the field image
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdatePicture()
        Dim gc As Graphics = Graphics.FromImage(Me.gameImage)
        gc.Clear(Color.White)
        Dim i As Integer = 0
        For i = 0 To Me.bloodPlaces.Count - 1
            rect.X = Me.bloodPlaces.Item(i).P.X
            rect.Y = Me.bloodPlaces.Item(i).P.Y

            rect.Width = bloodImage.Width + i
            rect.Height = bloodImage.Height + i

            gc.DrawImage(Me.bloodImage, rect)
        Next i
        Me.DrawMosquitos((gc), (Me.list1))
        Me.DrawMosquitos((gc), (Me.list2))
        Me.DrawMosquitos((gc), (Me.list3))
    End Sub

    ''' <summary>
    ''' Gets the image of field
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGameImage() As Image
        Me.UpdatePicture()
        Return Me.gameImage
    End Function

    Private Function ToDelete(ByVal mosq As Mosquito) As Boolean
        If (mosq.IsOnDelta(tempP, delta)) Then
            Return True
        End If
        Return False
    End Function

    Public Function HitPoint(ByVal p As Point) As Boolean
        Dim i As Integer
        Dim state As Boolean = False
        'save temp local variable for RemoveAll
        tempP = New Point(p.X, p.Y)
        i = 0
        For i = 0 To Me.list1.Count - 1
            If Me.list1.Item(i).IsOnDelta(p, Me.delta) Then
                'Me.list1.RemoveAt(i)
                Me._currCount -= 1
                tempP.X = p.X + rand.Next(29, 32)
                tempP.Y = p.Y + rand.Next(26, 29)
                Me.bloodPlaces.Add(New DeadPoint(tempP))
                state = True
            End If
        Next i

        tempP.X = p.X
        tempP.Y = p.Y
        list1.RemoveAll(AddressOf ToDelete)

        i = 0
        For i = 0 To Me.list2.Count - 1
            If Me.list2.Item(i).IsOnDelta(p, Me.delta) Then
                'Me.list2.RemoveAt(i)
                Me._currCount -= 1
                tempP.X = p.X + rand.Next(29, 32)
                tempP.Y = p.Y + rand.Next(26, 29)
                Me.bloodPlaces.Add(New DeadPoint(tempP))
                state = True
            End If
        Next i

        tempP.X = p.X
        tempP.Y = p.Y
        list2.RemoveAll(AddressOf ToDelete)

        i = 0
        For i = 0 To Me.list3.Count - 1
            If Me.list3.Item(i).IsOnDelta(p, Me.delta) Then
                'Me.list3.RemoveAt(i)
                Me._currCount -= 1
                tempP.X = p.X + rand.Next(29, 32)
                tempP.Y = p.Y + rand.Next(26, 29)
                Me.bloodPlaces.Add(New DeadPoint(tempP))
                state = True
            End If
        Next i

        tempP.X = p.X
        tempP.Y = p.Y
        list3.RemoveAll(AddressOf ToDelete)

        Return state
    End Function

    Public Sub RefreshDeadPoints()
        Dim i As Integer
        For i = 0 To Me.bloodPlaces.Count - 1
            Dim local1 As DeadPoint = Me.bloodPlaces.Item(i)
            local1.TimeOut -= 1
        Next i
        Dim howMany As Integer = Me.bloodPlaces.RemoveAll(AddressOf OutTimeOut)
    End Sub

    Private Shared Function OutTimeOut(ByVal place As DeadPoint) As Boolean
        If place.TimeOut <= 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Changes pictures of all mosquitos
    ''' </summary>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <remarks></remarks>
    Public Sub RefreshLists(ByVal width As Integer, ByVal height As Integer)
        Dim i As Integer = 0
        i = 0
        For i = 0 To Me.list1.Count - 1
            Me.list1.Item(i).ProvideIteration(width, height)
        Next i

        For i = 0 To Me.list2.Count - 1
            Me.list2.Item(i).ProvideIteration(width, height)
        Next i

        For i = 0 To Me.list3.Count - 1
            Me.list3.Item(i).ProvideIteration(width, height)
        Next i
    End Sub


    ' Properties
    Public ReadOnly Property CurrCount() As Integer
        Get
            Return Me._currCount
        End Get
    End Property


End Class
