Public Class Chessboard
    Public WhiteBottom As Boolean


    Private board(7, 7) As Integer

    Public toHighlight(7, 7) As Boolean

    Public Const OFF_BOARD As Integer = 1000

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

    Public Sub New(wab As Boolean)
        Dim i, j As Integer
        WhiteBottom = wab

        For i = 0 To 7
            For j = 0 To 7
                board(i, j) = 0
            Next
        Next

        'Initialisation conditions

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
