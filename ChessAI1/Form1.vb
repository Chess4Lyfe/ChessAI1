Imports System.Drawing.Drawing2D

Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private F_Piece As Font = New Font("Segoe UI Symbol", 37)

    Public Board As New Chessboard(True)

    Private MoveGen As New cMove(Board)


    'Constant declerations
    Const SQR As Integer = 60
    Const H_DISP As Integer = 30
    Const V_DISP As Integer = 30
    Const B_THICKNESS As Integer = 30

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''DEBUG CODE''''''''''''''''''''''''''''



    '''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ' 1 = pawn, 2=kinght, 3=bishop, 4=rook, 5=queen, 6=king




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        'Fix form size
        Me.Size = New Size((H_DISP) + (8 * SQR) + B_THICKNESS + 16, (2 * V_DISP) + (8 * SQR) + B_THICKNESS + 9)

        ' Initialise everthing
        DoubleBuffered = True


        '''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''' DEBUG CODE ''''''''''''''''''' 

        Debug.Print("======== DEBUG ========")




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

    Private Function toRect(x As Integer, y As Integer) As Rectangle
        Return New Rectangle(SQR * x + H_DISP, SQR * y + V_DISP, SQR, SQR)
    End Function

    Private Function toRect(vec As iVector2) As Rectangle
        Return toRect(vec.x, vec.y)
    End Function


    Private highlights(7, 7) As Boolean
    Private Chighlights(7, 7) As Boolean
    Private selectedPiece As New iVector2

    Private Sub rectangle_Click(sender As Object, e As MouseEventArgs)

    End Sub



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
            Dim b_chighlight As New SolidBrush(Color.FromArgb(153, 255, 51))

            Dim r As Rectangle

            'Draw tiles
            For i_x = 0 To 7
                For i_y = 0 To 7

                    ' Using board coordinates
                    r = toRect(i_x, i_y)
                    If Chighlights(i_x, i_y) Then
                        .FillRectangle(b_chighlight, r)
                    ElseIf highlights(i_x, i_y) Then
                        .FillRectangle(b_highlight, r)

                    ElseIf (i_x + 9 * i_y) Mod 2 = 0 Then
                        .FillRectangle(b_white, r)
                    Else
                        .FillRectangle(b_black, r)
                    End If
                Next
            Next

            ' Clean up
            b_white.Dispose()
            b_black.Dispose()
            b_highlight.Dispose()
            b_chighlight.Dispose()

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

                    Dim i As Integer = Board.at(h, v)

                    If i <> Board.OFF_BOARD Then
                        pos = Board.display(h, v)
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
                    End If
                Next
            Next
        End With

    End Sub


    Private isSelected As Boolean = False   ' Boolean for whether piece is selected on board



    Private Sub SelectPiece(position As iVector2)
        ' position is in display coordinates

        ' Check valid side
        Dim sgn As Integer
        If Board.WhiteBottom Then
            sgn = 1
        Else
            sgn = -1
        End If

        ' Check if valid piece is at position
        Dim piece = Board.at(Board.display(position))
        If (piece * sgn) > 0 Then
            selectedPiece.store(position.x, position.y)
            isSelected = True
        Else
            isSelected = False
        End If

    End Sub

    Private Sub clear_highlight()
        ' Clears all highlights
        For i_x = 0 To 7
            For i_y = 0 To 7
                highlights(i_x, i_y) = False
                Chighlights(i_x, i_y) = False
            Next
        Next
    End Sub

    Shared king_spots(,) As Integer = {{2, 0}, {6, 0}, {2, 7}, {6, 7}}



    Private Sub highlight_piece(isClick As Boolean)



        Dim vec As iVector2
        Dim r As Rectangle
        Dim sgn As Integer

        For i_x = 0 To 7
            For i_y = 0 To 7

                vec = Board.display(i_x, i_y)
                r = toRect(vec)

                If r.Contains(PointToClient(MousePosition)) Then
                    Dim possiblemoves As New Movements(Board)
                    Debug.Print("At (" + i_x.ToString + ", " + i_y.ToString + ")")
                    possiblemoves = MoveGen.GetMoves(i_x, i_y)

                    If isClick Then
                        ' Change selected variable if valid
                        SelectPiece(vec)
                    End If

                    ' Highlights actual space
                    highlights(vec.x, vec.y) = True

                    'Black/White check
                    If Board.WhiteBottom Then
                        sgn = 1
                    Else
                        sgn = -1
                    End If

                    'Drawing in highlights for possible moves of piece
                    If Board.at(i_x, i_y) * sgn > 0 Then
                        If possiblemoves.Count = 0 Then
                            isSelected = False
                            Refresh()
                            Exit Sub
                        End If
                        ' Deal with normal moves
                        For Each move As Movements.MoveData In possiblemoves

                            Debug.Print(move.target.ToString())
                            vec = Board.display(move.target)
                            highlights(vec.x, vec.y) = True

                        Next
                        ' Deal with castles because the medieval peasants were ASSHOLES that introduced
                        ' overcomplicated shitty rules like en passant
                        For Each castler As Movements.CastleData In possiblemoves.castles
                            Debug.Print("Castling " + castler.castle.ToString)
                            vec = Board.display(king_spots(castler.castle + 1, 0), king_spots(castler.castle + 1, 1))
                            chighlights(vec.x, vec.y) = True
                        Next

                    End If
                End If
            Next
        Next
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        'Highlights where mouse is

        ' Piece selected, stopping normal highlighting
        If isSelected Then
            Refresh()
            Exit Sub
        End If

        clear_highlight()

        highlight_piece(False)

        Refresh()
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Dim i_X As Integer = -1
        Dim i_Y As Integer = -1


        If New Rectangle(H_DISP, V_DISP, H_DISP + 8 * SQR, V_DISP + 8 * SQR).Contains(e.Location) Then
            i_X = (e.Location.X - H_DISP) \ SQR
            i_Y = (e.Location.Y - V_DISP) \ SQR
        End If

        Dim here = New iVector2(i_X, i_Y)





        ' Rightclick feature to de-select peice
        If e.Button = MouseButtons.Right Then
            clear_highlight()
            isSelected = False
            Refresh()
            Exit Sub
        End If

        ' Piece already selected, stop everything
        If isSelected Then
            'Check if clicked on piece is also valid piece
            If Not selectedPiece.Equals(here) Then
                If highlights(i_X, i_Y) Then

                    ' Make the Move ay
                    Board.movePiece(selectedPiece, here)
                    MoveGen.UpdateMoves()
                    isSelected = False
                ElseIf Chighlights(i_X, i_Y) Then
                    Dim v As iVector2 = Board.display(i_X, i_Y)

                    If v.y = 0 Then
                        ' black
                        If v.x = 2 Then
                            ' queenside
                            Board.Castle(1)

                        ElseIf v.x = 6 Then
                            ' kingside
                            Board.Castle(2)

                        End If
                    ElseIf v.x = 7 Then
                        ' white
                        If v.x = 2 Then
                            ' queenside
                            Board.Castle(3)

                        ElseIf v.x = 6 Then
                            ' kingside
                            Board.Castle(4)

                        End If
                    End If
                    MoveGen.UpdateMoves()
                    isSelected = False
                End If
            Else
                clear_highlight()
                highlight_piece(True)
                Refresh()
            End If

            Exit Sub
        End If

        clear_highlight()
        selectedPiece.store(-1, -1)
        highlight_piece(True)

        Refresh()
    End Sub
End Class
