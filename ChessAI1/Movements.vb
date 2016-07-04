' Look at my sparkling comment-free code

' Jesus Object, son of cMove, the God of all objects
Public Class Movements
    Implements IEnumerable
    Implements ICollection

    Private Shared b As Chessboard

    ' this subclass describes a move completely
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


        Public Overrides Function ToString() As String
            Return target.ToString()
        End Function

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
        ' "this will get huuuuuuuuuge later" - Donald Trump
        Private Function value(pos As iVector2) As Integer
            'TODO: castling properly
            If True Then
                ' Not a castle move
                Dim tmp = b.at(pos)
                Return pieceValue(Math.Abs(b.at(pos)))

            Else
                Return 6 ' Arbitrary as fuck
            End If
        End Function


    End Class




    Public Overrides Function toString() As String
        Dim str As String = ""
        For Each m In movements
            str = str + m.ToString()
        Next
        Return str
    End Function


    Private movements As SortedSet(Of MoveData)

    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            Return movements.Count
        End Get
    End Property

    Private ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return movements.GetEnumerator()
    End Function

    Sub New(theBoard As Chessboard)
        movements = New SortedSet(Of MoveData)
        b = theBoard
    End Sub



    Public Class CompareMoves : Implements IComparer

        ' Alternate comparison object
        Public Function Compare(ByVal A As Object, ByVal B As Object) As Integer Implements IComparer.Compare
            Dim M As MoveData = TryCast(A, MoveData)
            Dim N As MoveData = TryCast(B, MoveData)
            Return M.virtue - N.virtue ' this may need to be swapped
        End Function
    End Class

    Public Sub Add(x As Integer, y As Integer)
        Dim vec = New iVector2(x, y)
        Dim tmp = b.at(vec)
        If b.OFF_BOARD <> tmp Then
            movements.Add(New MoveData(vec, tmp))
        End If
    End Sub

    Public Sub Add(vec As iVector2)
        Add(vec.x, vec.y)
    End Sub




    Private xmaps As String() = {"a", "b", "c", "d", "e", "f", "g", "h"}
    Private ymaps As String() = {"1", "2", "3", "4", "5", "6", "7", "8"}

    Public Sub Print()
        For Each v In movements
            Debug.Print("{0}{1}", xmaps(v.target.x), ymaps(v.target.y))
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

    Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        movements.CopyTo(array, index)
    End Sub
End Class
