Public Class Mosquito
    'x-axis speed of current mosquito
    Private _currPoint As Point

    'y-axis speed of current mosquito
    Private _deltaX As Integer

    'current coordinates
    Private _deltaY As Integer

    Public Sub New(ByVal location As Point)
        Me._currPoint = New Point(location.X, location.Y)
    End Sub

    Public Function IsOnDelta(ByVal p As Point, ByVal delta As Integer) As Boolean
        Dim distance As Double
        distance = ((Me._currPoint.X - 30) - p.X) * ((Me._currPoint.X - 30) - p.X)
        distance = distance + ((Me._currPoint.Y - 28) - p.Y) * ((Me._currPoint.Y - 28) - p.Y)
        distance = Math.Sqrt(distance)
        If distance < delta Then
            Return True
        End If
        Return False
    End Function

    Public Property CurrPoint() As Point
        Get
            Return Me._currPoint
        End Get
        Set(ByVal value As Point)
            Me._currPoint = value
        End Set
    End Property

    Public Property DeltaX() As Integer
        Get
            Return Me._deltaX
        End Get
        Set(ByVal value As Integer)
            Me._deltaX = value
        End Set
    End Property

    Public Property DeltaY() As Integer
        Get
            Return Me._deltaY
        End Get
        Set(ByVal value As Integer)
            Me._deltaY = value
        End Set
    End Property

    Public Sub ProvideIteration(ByVal width As Integer, ByVal height As Integer)
        If ((Me._currPoint.X + Me._deltaX) < 0) Then
            Me._deltaX = (Me._deltaX * -1)
        End If
        If ((Me._currPoint.Y + Me.DeltaY) < 0) Then
            Me._deltaY = (Me._deltaY * -1)
        End If
        Me._currPoint.X = (Math.Abs(CInt((Me._currPoint.X + Me._deltaX))) Mod width)
        Me._currPoint.Y = (Math.Abs(CInt((Me._currPoint.Y + Me._deltaY))) Mod height)
    End Sub

End Class
