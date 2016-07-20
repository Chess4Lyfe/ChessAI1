Public Class Chessboard
    Public WhiteBottom As Boolean

    ' Stores all the piece IDs 0 = nothing, 1=pawn...
    Private board(7, 7) As Integer

    ' Stores the identities of the squares to highlight (used for rendering)
    Public toHighlight(7, 7) As Boolean

    ' Get set to false on castle moves 0       WQ    WK    BQ   BK
    Public HasNotMoved() As Boolean = {False, True, True, True, True}

    ' A constant error value
    Public Const OFF_BOARD As Integer = 1000


    ' Dereference function, returns ID of piece at position (x,y)
    Public Function at(x As Integer, y As Integer) As Integer
        If x < 0 OrElse x > 7 OrElse y < 0 OrElse y > 7 Then
            Return OFF_BOARD
        Else
            Return board(x, y)
        End If
    End Function

    Public Function at(v As iVector2) As Integer
        Return at(v.x, v.y)
    End Function

    ' Rotates (x,y) on the board - converts absolute coords to display and back
    Public Function display(x As Integer, y As Integer) As iVector2
        If WhiteBottom Then
            Return New iVector2(x, y)
        Else
            Return New iVector2(7 - x, 7 - y)
        End If
    End Function

    Public Function display(v As iVector2) As iVector2
        Return display(v.x, v.y)
    End Function

    ' Move Maker
    Public Sub movePiece(x_i As Integer, y_i As Integer, x_f As Integer, y_f As Integer)
        board(x_f, y_f) = Me.at(x_i, y_i)
        board(x_i, y_i) = 0

        If y_i = 7 Then
            'white back row
            Select Case x_i
                Case 0, 4
                    HasNotMoved(1) = False
                Case 7, 4
                    HasNotMoved(2) = False
            End Select
        ElseIf y_i = 0 Then
            ' black back row
            Select Case x_i
                Case 0, 4
                    HasNotMoved(3) = False
                Case 7, 4
                    HasNotMoved(4) = False
            End Select
        End If
    End Sub

    Public Sub movePiece(v_i As iVector2, v_f As iVector2)
        movePiece(v_i.x, v_i.y, v_f.x, v_f.y)
    End Sub

    Public Sub Castle(castleType As Integer)
        Select Case castleType
            Case 1
                ' white queenside
                board(3, 7) = Me.at(0, 7)
                board(0, 7) = 0
                HasNotMoved(1) = False
                HasNotMoved(2) = False
            Case 2
                ' white kingside
                board(5, 7) = Me.at(7, 7)
                board(7, 7) = 0
                HasNotMoved(1) = False
                HasNotMoved(2) = False
            Case 3
                ' black queenside
                board(3, 0) = Me.at(0, 0)
                board(0, 0) = 0
                HasNotMoved(3) = False
                HasNotMoved(4) = False
            Case 4
                'black kingside
                board(5, 0) = Me.at(7, 0)
                board(7, 0) = 0
                HasNotMoved(3) = False
                HasNotMoved(4) = False

        End Select
    End Sub



    ' Constructor
    Public Sub New(wab As Boolean)
        Dim i, j As Integer
        WhiteBottom = wab

        For i = 0 To 7
            For j = 0 To 7
                board(i, j) = 0
            Next
        Next

        ' Setting up the board...

        ' Pawns
        For i = 0 To 7
            board(i, 1) = -1
            board(i, 6) = 1
        Next

        ' Knights
        board(1, 0) = -2
        board(6, 0) = -2
        board(1, 7) = 2
        board(6, 7) = 2

        ' Bishops
        board(2, 0) = -3
        board(5, 0) = -3
        board(2, 7) = 3
        board(5, 7) = 3

        ' Rooks
        board(0, 0) = -4
        board(7, 0) = -4
        board(0, 7) = 4
        board(7, 7) = 4

        ' Queens
        board(3, 0) = -5
        board(3, 7) = 5

        ' Kings
        board(4, 7) = 6
        board(4, 0) = -6

    End Sub

End Class
