Imports System.Drawing.Drawing2D

Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private F_Piece As Font = New Font("Segoe UI Symbol", 37)

    Public Board As New Chessboard(False)

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

            'Draw tiles
            For i_x = 0 To 7
                For i_y = 0 To 7

                    ' Using board coordinates
                    r = toRect(i_x, i_y)

                    If highlights(i_x, i_y) Then
                        .FillRectangle(b_highlight, r)
                    ElseIf (i_x + 9 * i_y) Mod 2 = 0 Then
                        .FillRectangle(b_white, r)
                    Else
                        .FillRectangle(b_black, r)
                    End If



                    'check if mouse is hovering over rendered square
                    If r.Contains(PointToClient(MousePosition)) Then

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

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
		Dim r As Rectangle
		Dim vec As iVector2
		Dim sgn As Integer

		For i_x = 0 To 7
			For i_y = 0 To 7
				highlights(i_x, i_y) = False
			Next
		Next

		For i_x = 0 To 7
			For i_y = 0 To 7

				vec = Board.display(i_x, i_y)
				r = toRect(vec)

				If r.Contains(PointToClient(MousePosition)) Then

					Dim possibleMoves As New Movements(Board)

					possibleMoves = MoveGen.GetMoves(i_x, i_y)

					highlights(vec.x, vec.y) = True

					If Board.WhiteBottom Then
						sgn = 1
					Else
						sgn = -1
					End If

					If Board.at(i_x, i_y) * sgn > 0 Then
						For Each move As Movements.MoveData In possibleMoves

							Debug.Print(move.target.ToString())
							vec = Board.display(move.target)
							highlights(vec.x, vec.y) = True

						Next
					End If
				End If

			Next
		Next
		Refresh()
	End Sub



    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
		'Dim r As Rectangle
		'Dim vec As iVector2
		'Dim sgn As Integer

		'For i_x = 0 To 7
		'	For i_y = 0 To 7
		'		highlights(i_x, i_y) = False
		'	Next
		'Next

		'For i_x = 0 To 7
		'	For i_y = 0 To 7

		'		vec = Board.display(i_x, i_y)
		'		r = toRect(vec)

		'		If r.Contains(PointToClient(MousePosition)) Then

		'			Dim possibleMoves As New Movements(Board)

		'			possibleMoves = MoveGen.GetMoves(i_x, i_y)

		'			highlights(vec.x, vec.y) = True

		'			If Board.WhiteBottom Then
		'				sgn = 1
		'			Else
		'				sgn = -1
		'			End If

		'			If Board.at(i_x, i_y) * sgn > 0 Then
		'				For Each move As Movements.MoveData In possibleMoves

		'					Debug.Print(move.target.ToString())
		'					vec = Board.display(move.target)
		'					highlights(vec.x, vec.y) = True

		'				Next
		'			End If
		'		End If

		'	Next
		'Next
		Refresh()
    End Sub
End Class
