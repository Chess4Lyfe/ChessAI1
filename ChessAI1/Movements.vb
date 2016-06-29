' Look at my sparkling comment-free code


Public Class Movements
    ' this subclass descrives a move completely
    Public Class MoveData
        Implements IComparable

        Public target As iVector2
        Public takes As Integer
        Public target2 As iVector2

        ' Weird hash function thingy
        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim otherMove As MoveData = TryCast(obj, MoveData)
            If otherMove Is Nothing Then
                Return 1
            ElseIf target2 Is Nothing Then
                Return Me.target.Index() - otherMove.target.Index()
            Else
                Return Me.target.Index() - otherMove.target.Index() +
                    Me.target2.Index() - otherMove.target2.Index()
            End If
        End Function

        Private pieceValue As Integer() = {0, 1, 3, 3, 5, 9, 9001}
        ' hash function for sorting by value of move
        ' this will get huuuuuuuuuge later
        Public Function Virtue() As Integer
            If target2 Is Nothing Then
                ' Not a castle move
                Return Math.Abs(pieceValue(takes))
            Else
                Return 4 ' Arbitrary as fuck
            End If
        End Function

        Sub New(place As iVector2, value As Integer, Optional place2 As iVector2 = Nothing)
            target = place
            takes = value
            target2 = place2
        End Sub
    End Class

    Private movements As SortedSet(Of MoveData)

    Public Sub Add(x As Integer, y As Integer)
        Dim vec = New iVector2(x, y)
        Dim tmp = vec.deref()
        If (tmp <> iVector2.OFF_BOARD) Then
            movements.Add(New MoveData(vec, tmp))
        End If
    End Sub

    Public Sub Castle(x As Integer, y As Integer)
        Dim vec = New iVector2(x, y)


    End Sub

    Sub New()
        movements = New SortedSet(Of MoveData)
    End Sub

    Public Sub Print()
        For Each v In movements
            Debug.Print("(" + v.target.x.ToString() + ", " + v.target.y.ToString() + ")")
        Next

    End Sub

    Public Function Contains(vec As iVector2) As Integer
        Return movements.Contains(New MoveData(vec, 0))
    End Function

    Public Function ListByValue() As MoveData()
        Dim GoodMoves(movements.Count()) As MoveData
        movements.CopyTo(GoodMoves)
        movements.OrderBy(Of MoveData)(MoveData.cmp)
        Return GoodMoves
    End Function
End Class
