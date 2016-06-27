Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private theBoard As Board

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub



    Class Move
        Public Function Check(pos As Tuple(Of Integer), type As Integer) As Boolean
            If (type > 0) Then
        End Function

    End Class





    Class Board
        ' 1 = pawn, 2=kinght, 3=bishop, 4=rook, 5=queen, 6=king
        Private ornt As Integer ' 1= white on top, -1 = black on top

        Public board(7, 7) As Integer

        Sub setup(ornt As Integer)
            ' Pawns
            For i = 0 To 7
                board(i, 1) = ornt * 1
                board(i, 6) = ornt * -1
            Next

            ' Knights
            board(1, 0) = ornt * 2
            board(6, 0) = ornt * 2
            board(1, 7) = ornt * -2
            board(6, 7) = ornt * -2

            ' Bishops
            board(2, 0) = ornt * 3
            board(5, 0) = ornt * 3
            board(2, 7) = ornt * -3
            board(5, 7) = ornt * -3

            ' Rooks
            board(0, 0) = ornt * 4
            board(0, 7) = ornt * 4
            board(7, 0) = ornt * -4
            board(7, 7) = ornt * -4

            ' Kings and Queens
            If ornt = 1 Then
                board(3, 0) = 6
                board(4, 0) = 5
                board(3, 7) = -6
                board(4, 7) = -5
            Else
                board(0, 3) = -5
                board(0, 4) = -6
                board(7, 3) = 5
                board(7, 4) = 6
            End If

        End Sub

        Sub New(whiteatop As Boolean)
            If whiteatop Then
                ornt = 1
            Else
                ornt = -1
            End If

            setup(ornt)
        End Sub

    End Class

    Public brd As New Board(True)

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
