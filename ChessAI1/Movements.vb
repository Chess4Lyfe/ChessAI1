Public Class Movements
    Private movements As List(Of Tuple(Of iVector2, Integer))

    Public Sub Add(x As Integer, y As Integer)
        Dim tmp = New iVector2(x, y)
        movements.Add(New Tuple(Of iVector2, Integer)(tmp, tmp.deref()))
    End Sub

    Sub New()
        movements = New List(Of Tuple(Of iVector2, Integer))
    End Sub

    Public Sub Print()
        For Each v In movements
            Debug.Print("(" + v.Item1.x.ToString() + ", " + v.Item1.y.ToString() + ")")
        Next

    End Sub
End Class
