' Look at my sparkling comment-free code

' Jesus Object, son of cMove, the God object
Public Class Movements


    ' this subclass descrives a move completely
    Public Class MoveData
        Implements IComparable

        Public target As iVector2
        Public Property virtue As Integer
        Public castle As Byte



        Sub New(place As iVector2, value As Integer, Optional castles As Byte = 0)
            target = place
            castle = castles
            virtue = Me.value(place)
        End Sub

        ' Weird comparison function thing for storing in BST
        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim otherMove As MoveData = TryCast(obj, MoveData)
            If otherMove Is Nothing Then
                Return 1
            ElseIf castle = 0 Then
                Return Me.target.Index() - otherMove.target.Index()
            Else
                Return Me.target.Index() - otherMove.target.Index() +
                    64 + castle
            End If
        End Function


        Private pieceValue As Integer() = {0, 1, 3, 3, 5, 9, 9001}
        ' hash function for sorting by value of move
        ' this will get huuuuuuuuuge later
        Private Function value(pos As iVector2) As Integer
            If castle = 0 Then
                ' Not a castle move
                Dim tmp = pos.deref()
                If tmp = iVector2.OFF_BOARD Then
                    Return -9001
                Else
                    Return pieceValue(Math.Abs(pos.deref()))
                End If

            Else
                Return 6 ' Arbitrary as fuck
            End If
        End Function

    End Class

    Class CompareMoves : Implements IComparer

        ' Alternate comparison object
        Public Function Compare(ByVal A As Object, ByVal B As Object) As Integer Implements IComparer.Compare
            Dim M As MoveData = TryCast(A, MoveData)
            Dim N As MoveData = TryCast(B, MoveData)
            Return M.virtue - N.virtue ' this may need to be swapped
        End Function
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
        Dim tmp = vec.deref()
        If (tmp <> iVector2.OFF_BOARD) Then
            movements.Add(New MoveData(vec, tmp))
        End If

    End Sub

    Sub New()
        movements = New SortedSet(Of MoveData)
    End Sub




    Public Sub Print()
        For Each v In movements
            Debug.Print("{0}{1}", Form1.xmaps(v.target.x), Form1.ymaps(v.target.y))
        Next

    End Sub

    Public Function Contains(vec As iVector2) As Integer
        Return movements.Contains(New MoveData(vec, 0))
    End Function

    Public Function ListByValue() As MoveData()
        Dim GoodMoves(movements.Count()) As MoveData
        movements.CopyTo(GoodMoves)
        Array.Sort(Of MoveData)(GoodMoves, New CompareMoves)
        Return GoodMoves
    End Function
End Class
