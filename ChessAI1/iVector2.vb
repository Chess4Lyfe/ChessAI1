' Integer 2D vector


Public Class iVector2
    Public Const OFF_BOARD = 1000

    Public x As Integer
    Public y As Integer

    Public Sub store(newX As Integer, newY As Integer)
        x = newX
        y = newY
    End Sub

    Sub New(Optional newX As Integer = OFF_BOARD, Optional newY As Integer = OFF_BOARD)
        If newX <> OFF_BOARD AndAlso newY <> OFF_BOARD Then
            store(newX, newY)
        End If
    End Sub

    Sub New(v As iVector2)
        store(v.x, v.y)
    End Sub

    Public Function deref() As Integer
        ' Returns the ID of the piece at the the specified place
        If 0 <= x AndAlso x <= 7 AndAlso 0 <= y AndAlso y <= 7 Then
            Return Form1.board(x, y)
        Else
            Return OFF_BOARD ' Out of range
        End If

    End Function

    Public Sub Addition(vx As Integer, vy As Integer)
        x = x + vx
        y = y + vy
    End Sub

    Public Sub Addition(v As iVector2)
        x = x + v.x
        y = y + v.y
    End Sub

    Public Function Plus(vx As Integer, vy As Integer) As iVector2
        Return New iVector2(vx + x, vy + y)
    End Function

    Public Function Plus(v As iVector2) As iVector2
        Return New iVector2(v.x + x, v.y + y)
    End Function

    Public Sub Multiply(a As Integer)
        x = a * x
        y = a * y
    End Sub

    Public Function Times(a As Integer) As iVector2
        Return New iVector2(a * x, a * y)
    End Function

    Public Function Index() As Integer
        Return x + 8 * y
    End Function

    Public Function isAt(vx As Integer, vy As Integer)
        Return vx = x AndAlso vy = y
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("({0}, {1})", x, y)
    End Function

    Public Sub ChangeCoords()
        If Not Form1.WhiteBottom Then
            x = 7 - x
            y = 7 - y
        End If
    End Sub


End Class
