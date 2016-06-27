Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private theBoard As Board

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub



    Class cMove
        Public Function Check(pos() As Integer, type As Integer) As List(Of Integer())
            Dim retval As New List(Of Integer())
            Dim finish(1) As Integer
            Dim i, j As Integer

            Select Case Math.Abs(type)
                Case 1
                    'pawn
                    If type > 0 Then
                        If pos(1) = 6 Then
                            'white start
                            finish(0) = pos(0)
                            finish(1) = 4
                            retval.Add(finish)
                        End If


                    Else
                        'black start
                        If pos(1) = 2 Then

                        End If
                    End If
                Case 2
                    'knight
                Case 3
                    'bishop
                Case 4
                    'rook
                Case 5
                    'queen

            End Select
        End Function

    End Class





    Class Board
        ' 1 = pawn, 2=kinght, 3=bishop, 4=rook, 5=queen, 6=king

        Public board(7, 7) As Integer

        Sub setup()
            Dim i, j As Integer
            For i = 0 To 7
                For j = 0 To 7
                    board(i, j) = 0
                Next
            Next

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
            board(0, 7) = -4
            board(7, 0) = 4
            board(7, 7) = 4

            ' Kings and Queens
            board(3, 0) = -6
            board(4, 0) = -5
            board(3, 7) = 6
            board(4, 7) = 5


        End Sub

        Sub New()
            setup()
        End Sub

    End Class

    Public brd As New Board

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        With e.Graphics
            'Constant declerations
            Const SQR As Integer = 60
            Const H_DISP As Integer = 30
            Const V_DISP As Integer = 30
            Const B_THICKNESS As Integer = 30

            'Draw border
            Using b_brown As New SolidBrush(Color.FromArgb(61, 36, 17))
                .FillRectangle(b_brown, New Rectangle(H_DISP - B_THICKNESS, V_DISP - B_THICKNESS, SQR * 8 + (2 * B_THICKNESS), SQR * 8 + (2 * B_THICKNESS)))
            End Using

            'Draw white tiles
            Using b_white As New SolidBrush(Color.FromArgb(254, 222, 137))
                .FillRectangle(b_white, New Rectangle(H_DISP, V_DISP, SQR * 8, SQR * 8))
            End Using

            'Draw black tiles
            Using b_black As New SolidBrush(Color.FromArgb(142, 81, 33))
                For v = 1 To 8
                    For b = 0 To 3
                        .FillRectangle(b_black, New Rectangle((2 * (SQR * b)) + (SQR * (v Mod 2)) + H_DISP, SQR * (v - 1) + V_DISP, SQR, SQR))
                    Next
                Next
            End Using

            'Draw column/row labels
            Using b_gold As New SolidBrush(Color.FromArgb(212, 175, 55))
                For v = 0 To 7
                    Dim sz_n As Size = .MeasureString(v + 1, F).ToSize
                    Dim sz_l As Size = .MeasureString(Convert.ToChar(Convert.ToInt32("A"c) + v - 1).ToString().ToLower, F).ToSize
                    .DrawString(v + 1, F, b_gold, H_DISP - (B_THICKNESS / 2) - (sz_n.Width / 2), V_DISP + (SQR * v) + (SQR / 2) - (sz_n.Height / 2))
                    .DrawString(v + 1, F, b_gold, H_DISP + (8 * SQR) + (B_THICKNESS / 2) - (sz_n.Width / 2), V_DISP + (SQR * v) + (SQR / 2) - (sz_n.Height / 2))

                    .DrawString(Convert.ToChar(Convert.ToInt32("A"c) + v).ToString().ToLower, F, b_gold, H_DISP + (SQR * v) + (SQR / 2) - (sz_l.Width / 2), V_DISP + (SQR * 8) + (B_THICKNESS / 2) - (sz_l.Height / 2))
                    .DrawString(Convert.ToChar(Convert.ToInt32("A"c) + v).ToString().ToLower, F, b_gold, H_DISP + (SQR * v) + (SQR / 2) - (sz_l.Width / 2), V_DISP - (B_THICKNESS / 2) - (sz_l.Height / 2))
                Next
            End Using

            Me.Size = New Size((H_DISP) + (8 * SQR) + B_THICKNESS + 16, (2 * V_DISP) + (8 * SQR) + B_THICKNESS + 9)
        End With

    End Sub
End Class
