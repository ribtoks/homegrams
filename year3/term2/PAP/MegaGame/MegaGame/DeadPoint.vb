Public Class DeadPoint
    Dim point As Point
    Dim _timeOut As Integer

    Public Sub New(ByVal tpoint As Point)
        Me._timeOut = 20
        Me.point = New Point(tpoint.X, tpoint.Y)
    End Sub



    Public Property P() As Point
        Get
            Return Me.point
        End Get
        Set(ByVal value As Point)
            Me.point = value
        End Set
    End Property

    Public Property TimeOut() As Integer
        Get
            Return Me._timeOut
        End Get
        Set(ByVal value As Integer)
            Me._timeOut = value
        End Set
    End Property

End Class
