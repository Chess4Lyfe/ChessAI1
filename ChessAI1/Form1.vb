Imports System.Drawing.Drawing2D

Public Class Form1

    Private F As Font = New Font("Segoe UI", 9)
    Private F_Piece As Font = New Font("Segoe UI Symbol", 37)
    Private MoveGen As New cMove

    ' 1 = pawn, 2=kinght, 3=bishop, 4=rook, 5=queen, 6=king

    Public board(7, 7) As Integer




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialise everthing
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



        Debug.Print("======== DEBUG ========")

        For i = 0 To 7
            MoveGen.Check(New iVector2(i, 1))
            board(i, 6) = 1
        Next



    End Sub


    'Integer 2D vector
    Class iVector2
        Public x As Integer
        Public y As Integer

        Public Sub store(newX As Integer, newY As Integer)
            x = newX
            y = newY
        End Sub

        Public Sub New(Optional newX As Integer = 1000, Optional newY As Integer = 1000)
            If newX <> 1000 And newY <> 1000 Then
                store(newX, newY)
            End If
        End Sub

        Public Function deref() As Integer
            If 0 <= x <= 7 And 0 <= y <= 7 Then
                Return Form1.board(x, y)
            Else
                Return 1000 ' Out of range
            End If

        End Function

    End Class

    ' A single 'move'
    Structure Movement
        Public Target As iVector2
        Public taking As Integer

        Sub New(trg As iVector2)
            Target = trg
            taking = trg.deref()
        End Sub
    End Structure


    Class Movements
        Private movements As List(Of Tuple(Of iVector2, Integer))
        Sub Add(x As Integer, y As Integer)
            Dim tmp = New iVector2(x, y)
            movements.Add(New Tuple(Of iVector2, Integer)(tmp, tmp.deref()))
        End Sub

        Sub New()
            movements = New List(Of Tuple(Of iVector2, Integer))
        End Sub
    End Class


    Class cMove

        Public Function Check(pos As iVector2) As Movements
            Dim retval As New Movements
            Dim type As Integer = pos.deref()

            Select Case Math.Abs(type)
                Case 1
                    'pawn
                    If type > 0 Then
                        If pos.y = 6 Then
                            'white start
                            retval.Add(pos.x, 4)


                        Else
                            ' Standard movement
                            If pos.y < 7 And Form1.board(pos.x, pos.y - 1) = 0 Then
                                retval.Add(pos.x, pos.y - 1)

                            End If

                        End If

                        If Form1.board(pos.x - 1, pos.y - 1) < 0 Then

                            retval.Add(pos.x - 1, pos.y - 1)

                        ElseIf Form1.board(pos.x + 1, pos.y - 1) < 0 Then
                            retval.Add(pos.x + 1, pos.y - 1)

                        End If


                    Else
                        'Black
                        If pos.y = 1 Then
                            'black start

                            retval.Add(pos.x, 3)


                        Else
                            ' Standard movement
                            If pos.y > 0 And Form1.board(pos.x, pos.y - 1) = 0 Then

                                retval.Add(pos.x, pos.y - 1)


                            End If

                        End If

                        'capture
                        If Form1.board(pos.x - 1, pos.y + 1) > 0 Then

                            retval.Add(pos.x - 1, pos.y + 1)


                        ElseIf Form1.board(pos.x + 1, pos.y + 1) > 0 Then

                            retval.Add(pos.x + 1, pos.y + 1)

                        End If

                    End If

                Case 2
                    'knight
                    Dim positions(,) As Integer = New Integer(7, 1) {{1, 2}, {2, 1}, {-1, 2}, {2, -1}, {-1, -2}, {-2, -1}, {1, -2}, {-2, 1}}
                    For i As Integer = 0 To 7
                        retval.Add(positions(i, 0), positions(i, 1))
                    Next


                Case 3, 5
                    'bishop or queen
                    For i = 1 To 7
                        'up and left
                        If pos.x - i <= -1 Or pos.y - i <= -1 Or pos.deref() * type > 0 Then
                            'same colour, can't go any further
                            Exit For

                        ElseIf pos.deref() * type < 0 Then
                            ' different colour, save this spot then leave
                        Else

                            retval.Add(pos.x - i, pos.y - i)

                        End If

                        'up and right
                        If pos.x + i >= 8 Or pos.y - i <= -1 Then

                        Else
                            retval.Add(pos.x + i, pos.y - i)


                        End If

                        'down and left
                        If pos.x - i <= -1 Or pos.y + i >= 8 Then

                        Else
                            retval.Add(pos.x - i, pos.y + i)

                        End If

                        'down and right
                        If pos.x + i >= 8 Or pos.y + i >= 8 Then

                        Else
                            retval.Add(pos.x + i, pos.y + i)

                        End If
                    Next
                Case 4, 5
                    'rook or queen


            End Select
            Return retval
        End Function


    End Class




    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        With e.Graphics

            'Constant declerations
            Const SQR As Integer = 60
            Const H_DISP As Integer = 30
            Const V_DISP As Integer = 30
            Const B_THICKNESS As Integer = 30

            'Draw border
            Using b_brown As New SolidBrush(Color.FromArgb(21, 38, 41))
                .FillRectangle(b_brown, New Rectangle(H_DISP - B_THICKNESS, V_DISP - B_THICKNESS, SQR * 8 + (2 * B_THICKNESS), SQR * 8 + (2 * B_THICKNESS)))
            End Using

            'Draw white tiles
            Using b_white As New SolidBrush(Color.FromArgb(189, 204, 222))
                .FillRectangle(b_white, New Rectangle(H_DISP, V_DISP, SQR * 8, SQR * 8))
            End Using

            'Draw black tiles
            Using b_black As New SolidBrush(Color.FromArgb(82, 128, 139))
                For v = 1 To 8
                    For b = 0 To 3
                        .FillRectangle(b_black, New Rectangle((2 * (SQR * b)) + (SQR * (v Mod 2)) + H_DISP, SQR * (v - 1) + V_DISP, SQR, SQR))
                    Next
                Next
            End Using

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

            'Fix form size
            Me.Size = New Size((H_DISP) + (8 * SQR) + B_THICKNESS + 16, (2 * V_DISP) + (8 * SQR) + B_THICKNESS + 9)


            'Draw pieces
            Dim piece_array() As String = {"♟", "♞", "♝", "♜", "♛", "♚"}
            Dim piece_char As String

            For v = 0 To 7
                For h = 0 To 7

                    Debug.Print(v)

                    Dim i As Integer = Me.board(h, v)
                    .SmoothingMode = SmoothingMode.AntiAlias
                    If i <> 0 Then piece_char = piece_array(Math.Abs(i) - 1)
                    If i < 0 Then
                        Using gp As New GraphicsPath()
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
