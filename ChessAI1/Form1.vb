Imports System.Drawing.Drawing2D

Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private F_Piece As Font = New Font("Segoe UI Symbol", 37)

    Private MoveGen As New cMove
    Public WhiteBottom As Boolean

    'Constant declerations
    Const SQR As Integer = 60
    Const H_DISP As Integer = 30
    Const V_DISP As Integer = 30
    Const B_THICKNESS As Integer = 30

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''DEBUG CODE''''''''''''''''''''''''''''

    Public xmaps As String() = {"a", "b", "c", "d", "e", "f", "g", "h"}
    Public ymaps As String() = {"1", "2", "3", "4", "5", "6", "7", "8"}

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''



    ' 1 = pawn, 2=kinght, 3=bishop, 4=rook, 5=queen, 6=king

    Public board(7, 7) As Integer
    Const OFF_BOARD As Integer = 1000


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Fix form size
        Me.Size = New Size((H_DISP) + (8 * SQR) + B_THICKNESS + 16, (2 * V_DISP) + (8 * SQR) + B_THICKNESS + 9)

        ' Initialise everthing
        DoubleBuffered = True
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

        ' Queens
        board(3, 0) = -5
        board(3, 7) = 5

        ' Kings
        board(4, 7) = 6
        board(4, 0) = -6

        '''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''' DEBUG CODE ''''''''''''''''''' 

        Debug.Print("======== DEBUG ========")
        WhiteBottom = True ' assume black at the bottom


        Dim tmp As Movements

        MoveGen.UpdateMoves()

        'For i = 0 To 7
        '    For j = 0 To 7
        '        ' Find all moves for every square
        '        Dim checker = New iVector2(j, i)
        '        checker.ChangeCoords()
        '        Debug.Print("Checking move {0}{1}", xmaps(checker.x), ymaps(checker.y))
        '        tmp = MoveGen.GetMoves(checker)
        '        tmp.Print()
        '    Next
        'Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''' END DEBUG CODE ''''''''''''''''''' 


    End Sub



    Private Function toRect(vec As iVector2) As Rectangle
        Return New Rectangle(SQR * vec.x + H_DISP, SQR * vec.y + V_DISP, SQR, SQR)
    End Function

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        With e.Graphics

            'Draw border
            Using b_brown As New SolidBrush(Color.FromArgb(21, 38, 41))
                .FillRectangle(b_brown, New Rectangle(H_DISP - B_THICKNESS, V_DISP - B_THICKNESS, SQR * 8 + (2 * B_THICKNESS), SQR * 8 + (2 * B_THICKNESS)))
            End Using

            Dim b_here As New iVector2
            'b_here is here in board coords


            ' I'm not using Using, sue me 
            Dim b_white As New SolidBrush(Color.FromArgb(189, 204, 222))
            Dim b_black As New SolidBrush(Color.FromArgb(82, 128, 139))
            Dim b_highlight As New SolidBrush(Color.FromArgb(64, 201, 222))

            Dim r As Rectangle
            Dim typ As Integer

            'Draw tiles
            For i_x = 0 To 7
                For i_y = 0 To 7
                    b_here.store(i_x, i_y)


                    r = toRect(b_here)



                    'check if mouse is hovering over rendered square
                    If r.Contains(PointToClient(MousePosition)) Then
                        Dim possibleMoves As New Movements
                        Dim here = New iVector2(i_x, i_y)
                        here.ChangeCoords()
                        typ = here.deref()

                        If WhiteBottom Then
                            If typ > 0 Then
                                'playing as white
                                possibleMoves = MoveGen.GetMoves(here)

                            End If
                        Else
                            If typ < 0 Then
                                'playing as black
                                possibleMoves = MoveGen.GetMoves(here)
                            End If

                            .FillRectangle(b_highlight, r)
                        End If

                        Debug.Print(possibleMoves.toString())


                        For Each move As Movements.MoveData In possibleMoves
                            move.target.ChangeCoords()

                            .FillRectangle(b_highlight, toRect(move.target))
                        Next


                    Else

                        If (b_here.Index() + b_here.y) Mod 2 = 0 Then
                            .FillRectangle(b_white, r)
                        Else
                            .FillRectangle(b_black, r)
                        End If


                    End If

                Next
            Next

            ' Clean up
            b_white.Dispose()
            b_black.Dispose()
            b_highlight.Dispose()

            'Draw column/row labels
            Using b_gold As New SolidBrush(Color.FromArgb(82, 129, 142))
                For v = 0 To 7
                    Dim sz_n As Size = .MeasureString(v + 1, F).ToSize
                    Dim sz_l As Size = .MeasureString(Convert.ToChar(Convert.ToInt32("A"c) + v - 1).ToString().ToLower, F).ToSize
                    .DrawString(v + 1, F, b_gold, H_DISP - (B_THICKNESS / 2) - (sz_n.Width / 2), V_DISP + (SQR * v) + (SQR / 2) - (sz_n.Height / 2))
                    .DrawString(v + 1, F, b_gold, H_DISP + (8 * SQR) + (B_THICKNESS / 2) - (sz_n.Width / 2), V_DISP + (SQR * v) + (SQR / 2) - (sz_n.Height / 2))

                    .DrawString(Convert.ToChar(Convert.ToInt32("A"c) + v).ToString().ToLower, F, b_gold, H_DISP + (SQR * v) + (SQR / 2) - (sz_l.Width / 2), V_DISP + (SQR * 8) + (B_THICKNESS / 2) - (sz_l.Height / 2))
                    .DrawString(Convert.ToChar(Convert.ToInt32("A"c) + v).ToString().ToLower, F, b_gold, H_DISP + (SQR * v) + (SQR / 2) - (sz_l.Width / 2), V_DISP - (B_THICKNESS / 2) - (sz_l.Height / 2))
                Next
            End Using

            'Draw pieces
            Dim piece_array() As String = {"", "♟", "♞", "♝", "♜", "♛", "♚"}
            Dim piece_char As String

            Dim pos As iVector2

            For v = 0 To 7
                For h = 0 To 7

                    pos = New iVector2(h, v)

                    Dim i As Integer = pos.deref()

                    pos.ChangeCoords()

                    .SmoothingMode = SmoothingMode.AntiAlias
                    piece_char = piece_array(Math.Abs(i))
                    If i < 0 Then
                        Using gp As New GraphicsPath()
                            gp.AddString(piece_char, F_Piece.FontFamily, F_Piece.Style, F_Piece.Size + 3, New Point((pos.x * SQR) + B_THICKNESS + 9, (pos.y * SQR) + B_THICKNESS + 3), StringFormat.GenericTypographic)
                            .FillPath(Brushes.Black, gp)
                        End Using
                    Else
                        Using gp As New GraphicsPath, p As New Pen(Brushes.Black, 3)
                            gp.AddString(piece_char, F_Piece.FontFamily, F_Piece.Style, F_Piece.Size + 3, New Point((pos.x * SQR) + B_THICKNESS + 9, (pos.y * SQR) + B_THICKNESS + 3), StringFormat.GenericTypographic)
                            .DrawPath(p, gp)
                            .FillPath(Brushes.White, gp)
                        End Using
                    End If
                Next
            Next
        End With

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Refresh()
    End Sub
End Class
