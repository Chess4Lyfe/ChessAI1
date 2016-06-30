Public Class CoordUnfucker
    Public WhiteBottom As Boolean

    Public Function Swap(x As Integer, y As Integer) As iVector2
        If WhiteBottom Then
            Return New iVector2(x, y)
        Else
            Return New iVector2(7 - x, 7 - y)
        End If
    End Function

    Public Function Swap(ByRef v As iVector2) As iVector2
        If WhiteBottom Then
            Return v
        Else
            Return New iVector2(7 - v.x, 7 - v.y)
        End If
    End Function

End Class
