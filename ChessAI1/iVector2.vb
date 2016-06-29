' Integer 2D vector


Public Class iVector2
    Const OFF_BOARD = 1000

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

    Public Function deref() As Integer
        ' Returns the ID of the piece at the the specified place
        If 0 <= x AndAlso x <= 7 AndAlso 0 <= y AndAlso y <= 7 Then
            Return Form1.board(x, y)
        Else
            Return OFF_BOARD ' Out of range
        End If

    End Function

    Public Sub Addition(v As iVector2)
        x = x + v.x
        y = y + v.y
    End Sub

    Public Function Plus(vx As Integer, vy As Integer) As iVector2
        Return New iVector2(vx + x, vy + y)
    End Function

End Class
