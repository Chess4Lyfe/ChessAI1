' Look at my sparkling comment-free code

' Jesus Object, son of cMove, the God of all objects
Public Class Movements
    Implements IEnumerable
    Implements ICollection

    Private Shared b As Chessboard

    ' this subclass describes a move completely
    Public Class MoveData
        Implements IComparable

        Public target As iVector2 ' the square to move to
        Public virtue As Integer

        Sub New(place As iVector2)
            target = place
            virtue = Me.value(place)
        End Sub


        ' Weird comparison function thing for storing in BST
        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim otherMove As MoveData = TryCast(obj, MoveData)
            If otherMove Is Nothing Then
                Return 1
            Else
                Return Me.target.Index() - otherMove.target.Index()

            End If
        End Function

        Private pieceValue As Integer() = {0, 1, 3, 3, 5, 9, 9001}
        ' The value of taking a given square
        Private Function value(pos As iVector2) As Integer
            ' Not a castle move
            Dim tmp = b.at(pos)
            Return pieceValue(Math.Abs(b.at(pos)))
        End Function


        Public Overrides Function ToString() As String
            Return target.ToString()
        End Function


    End Class


    ' Class for storing castle moves
    Public Class CastleData
        Implements IComparable

        Public castle As Integer ' the castle move type
        ' 0 = no castle, 1 = w queenside, 2 = w kingside, 3 = b queenside, 4 = b kingside
        Public virtue As Integer

        Public Sub New(type As Integer)
            castle = type
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim otherCastle As CastleData = TryCast(obj, CastleData)
            Debug.Print(otherCastle.castle.ToString + "] [" + castle.ToString)
            Return otherCastle.castle - castle
        End Function
    End Class



    Public Overrides Function toString() As String
        Dim str As String = ""
        For Each m In movements
            str = str + m.ToString()
        Next
        Return str
    End Function



    ' The main arrays for storing shit

    Private movements As SortedSet(Of MoveData)
    Public castles As SortedSet(Of CastleData)

    Sub New(theBoard As Chessboard)
        movements = New SortedSet(Of MoveData)
        castles = New SortedSet(Of CastleData)
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
        If b.OFF_BOARD <> b.at(x, y) Then
            movements.Add(New MoveData(vec))
        End If
    End Sub

    Public Sub Add(vec As iVector2)
        Add(vec.x, vec.y)
    End Sub

    Public Sub Castle(castleType As Integer)
        castles.Add(New CastleData(castleType))
    End Sub







    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            Return movements.Count + castles.Count()
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






    Private xmaps As String() = {"a", "b", "c", "d", "e", "f", "g", "h"}
    Private ymaps As String() = {"1", "2", "3", "4", "5", "6", "7", "8"}

    Public Sub Print()
        For Each v In movements
            Debug.Print("{0}{1}", xmaps(v.target.x), ymaps(v.target.y))
        Next

    End Sub

    Public Function Contains(vec As iVector2) As Integer
        Return movements.Contains(New MoveData(vec))
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
