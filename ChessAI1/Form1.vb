Imports System.Drawing.Drawing2D

Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private F_Piece As Font = New Font("Segoe UI Symbol", 37)
    Private theBoard As New Board
    Private Move As New cMove

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Debug.Print("======== DEBUG ========")
        Dim str As String = String.Join(" ", Move.Check({0, 1}, -1)(1))
        Debug.Print(str)
    End Sub



    Class cMove
        Public Function Check(pos() As Integer, type As Integer) As List(Of Integer())
            Dim retval As New List(Of Integer())
            Dim target(1) As Integer
            Dim i, j As Integer

            Select Case Math.Abs(type)
                Case 1
                    'pawn
                    If type > 0 Then
                        If pos(1) = 6 Then
                            'white start
                            target = {pos(0), 4}
                            retval.Add(target)
                            ReDim Preserve target(1)
                        End If
                        target = {pos(0), pos(1) + 1}
                        retval.Add(target)
                        ReDim Preserve target(1)
                        If theBoard(pos(0) - 1)(pos(1) - 1) Or theBoard(pos(0) + 1)(pos(1) - 1) Then

                        End If
                    Else
                            If pos(1) = 1 Then
                            'black start
                            target = {pos(0), 3}
                            retval.Add(target)
                            ReDim Preserve target(1)
                        End If
                        target = {pos(0), pos(1) + 1}
                        retval.Add(target)
                        ReDim Preserve target(1)
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
            Return retval
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
            board(7, 0) = -4
            board(0, 7) = 4
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

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias

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
            Using b_white As New SolidBrush(Color.FromArgb(237, 173, 123))
                .FillRectangle(b_white, New Rectangle(H_DISP, V_DISP, SQR * 8, SQR * 8))
            End Using

            'Draw black tiles
            Using b_black As New SolidBrush(Color.FromArgb(118, 69, 30))
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

            'Fix form size
            Me.Size = New Size((H_DISP) + (8 * SQR) + B_THICKNESS + 16, (2 * V_DISP) + (8 * SQR) + B_THICKNESS + 9)


            'Draw pieces
            For v = 0 To 7
                For h = 0 To 7

                    Dim piece_char As String
                    Dim i As Integer = theBoard.board(h, v)
                    If Math.Abs(i) = 1 Then
                        piece_char = "♟"
                    ElseIf Math.Abs(i) = 2 Then
                        piece_char = "♞"
                    ElseIf Math.Abs(i) = 3 Then
                        piece_char = "♝"
                    ElseIf Math.Abs(i) = 4 Then
                        piece_char = "♜"
                    ElseIf Math.Abs(i) = 5 Then
                        piece_char = "♛"
                    ElseIf Math.Abs(i) = 6 Then
                        piece_char = "♚"
                    End If
                    If i < 0 Then
                        Using gp As New GraphicsPath
                            gp.AddString(piece_char, F_Piece.FontFamily, F_Piece.Style, F_Piece.Size + 3, New Point((h * SQR) + B_THICKNESS + 10, (v * SQR) + B_THICKNESS + 3), StringFormat.GenericTypographic)
                            .FillPath(Brushes.Black, gp)
                        End Using
                    Else
                        Using gp As New GraphicsPath, p As New Pen(Brushes.Black, 3)
                            gp.AddString(piece_char, F_Piece.FontFamily, F_Piece.Style, F_Piece.Size + 3, New Point((h * SQR) + B_THICKNESS + 10, (v * SQR) + B_THICKNESS + 3), StringFormat.GenericTypographic)
                            .DrawPath(p, gp)
                            .FillPath(Brushes.White, gp)
                        End Using
                    End If
                    piece_char = ""
                Next
            Next
        End With

    End Sub
End Class
