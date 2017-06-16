Public Class Form1

    Private currCursor As Point
    Private currGameImage As Bitmap
    Private gameProvider As GameProvider
    Private isNowSmallZapper As Boolean
    Private lastMouse As Point
    Private zapperImage As Bitmap
    Private count As Integer = 0
    Private rect As Rectangle
    Private nilPoint

    Private Sub DrawZapper(ByRef gc As Graphics)
        If Not Me.isNowSmallZapper Then
            gc.DrawImage(Me.zapperImage, Me.currCursor)
        Else
            rect.Y = currCursor.Y
            rect.X = currCursor.X
            gc.DrawImage(Me.zapperImage, rect)
        End If
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.gameProvider = New GameProvider(MyBase.Width, MyBase.Height, Global.MegaGame.My.Resources.ResourceManager.GetObject("Mosquito"), _
            Global.MegaGame.My.Resources.ResourceManager.GetObject("bloodItem"))
        Me.currGameImage = New Bitmap(Me.gameProvider.GetGameImage)
        currCursor = New Point()
        nilPoint = New Point()
        Me.zapperImage = Global.MegaGame.My.Resources.ResourceManager.GetObject("Zapper")
        Dim rand As New Random(DateTime.Now.Millisecond)

        Me.gameProvider.AddMosquito1(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
        Dim i As Integer
        For i = 0 To 2
            Me.gameProvider.AddMosquito2(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
            Me.gameProvider.AddMosquito3(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
        Next i
        rect = New Rectangle(Me.currCursor, New Size((Me.zapperImage.Width - 25), (Me.zapperImage.Height - 30)))
    End Sub

    Private Sub Form1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        If (Not Me.gameProvider Is Nothing) Then
            e.Graphics.DrawImage(Me.currGameImage, nilPoint)
        End If

    End Sub

    Private Sub Form1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If (Not Me.gameProvider Is Nothing) Then
            Me.lastMouse.X = Me.currCursor.X
            Me.lastMouse.Y = Me.currCursor.Y

            Me.currCursor.X = e.X - 40
            Me.currCursor.Y = e.Y - 50
            If ((Math.Abs(CInt((Me.currCursor.X - Me.lastMouse.X))) > 5) OrElse (Math.Abs(CInt((Me.currCursor.Y - Me.lastMouse.Y))) > 5)) Then
                Dim gc As Graphics = MyBase.CreateGraphics
                gc.DrawImage(Me.currGameImage, nilPoint)
                Me.DrawZapper((gc))
            End If
        End If

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If (Me.gameProvider.CurrCount > 0) Then
            Me.gameProvider.RefreshLists(MyBase.Width, MyBase.Height)
            Dim gc As Graphics = MyBase.CreateGraphics
            Me.currGameImage = Me.gameProvider.GetGameImage
            gc.DrawImage(Me.currGameImage, nilPoint)
            Me.DrawZapper((gc))
        Else
            Dim time1 As Boolean = Me.Timer1.Enabled
            Dim time2 As Boolean = Me.Timer2.Enabled
            Dim time3 As Boolean = Me.Timer3.Enabled
            Dim time4 As Boolean = Me.Timer4.Enabled
            Me.Timer1.Enabled = False
            Me.Timer2.Enabled = False
            Me.Timer3.Enabled = False
            Me.Timer4.Enabled = False
            If (MessageBox.Show("You is a winner!!!" & Environment.NewLine & "You killed " & count.ToString & " mosquitos!" + Environment.NewLine & _
                                 "Game over." & Environment.NewLine & "Play one more time?", "MegaGame", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
                Dim rand As New Random(DateTime.Now.Millisecond)
                Dim i As Integer = 0

                For i = 0 To rand.Next(5) - 1
                    Me.gameProvider.AddMosquito1(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
                Next i

                For i = 0 To rand.Next(4) - 1
                    Me.gameProvider.AddMosquito2(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
                Next i

                For i = 0 To rand.Next(3) - 1
                    Me.gameProvider.AddMosquito3(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
                Next i
                Me.Timer1.Enabled = time1
                Me.Timer2.Enabled = time2
                Me.Timer3.Enabled = time3
                Me.Timer4.Enabled = time4
            Else
                Me.BackColor = Color.FromKnownColor(KnownColor.Control)
                Me.gameProvider = Nothing
            End If
        End If

    End Sub

    Private Sub Form1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseClick
        If (Not Me.gameProvider Is Nothing) Then
            Dim res As Boolean
            res = Me.gameProvider.HitPoint(Me.currCursor)
            If (res = True) Then
                count = count + 1
            End If
            Me.isNowSmallZapper = True

            Dim gc As Graphics = MyBase.CreateGraphics
            Me.currGameImage = Me.gameProvider.GetGameImage
            gc.DrawImage(Me.currGameImage, nilPoint)
            Me.DrawZapper((gc))

            Me.Timer4.Enabled = True
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Me.gameProvider.RefreshDeadPoints()
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Dim rand As New Random(DateTime.Now.Millisecond)
        Dim i As Integer = 0

        For i = 0 To rand.Next(5) - 1
            Me.gameProvider.AddMosquito1(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
        Next i

        For i = 0 To rand.Next(4) - 1
            Me.gameProvider.AddMosquito2(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
        Next i

        For i = 0 To rand.Next(3) - 1
            Me.gameProvider.AddMosquito3(New Point(rand.Next(MyBase.Width), rand.Next(MyBase.Height)))
        Next i

    End Sub

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        Me.isNowSmallZapper = False
        Me.Timer4.Enabled = False
    End Sub

    Private Sub Form1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDoubleClick
        If (Not Me.gameProvider Is Nothing) Then
            Dim res As Boolean
            res = Me.gameProvider.HitPoint(Me.currCursor)
            If (res = True) Then
                count = count + 1
            End If
            Me.isNowSmallZapper = True

            Dim gc As Graphics = MyBase.CreateGraphics
            Me.currGameImage = Me.gameProvider.GetGameImage
            gc.DrawImage(Me.currGameImage, nilPoint)
            Me.DrawZapper((gc))

            Me.Timer4.Enabled = True
        End If
    End Sub
End Class

