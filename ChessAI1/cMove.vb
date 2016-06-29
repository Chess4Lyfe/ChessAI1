﻿Public Class cMove


    Private HasNotMoved As New Dictionary(Of String, Boolean)

    Sub New()
        ' Set up all the castle flags
        HasNotMoved.Add("WQ", True)
        HasNotMoved.Add("WK", True)
        HasNotMoved.Add("BQ", True)
        HasNotMoved.Add("BK", True)
    End Sub

    Private AllPossibleMovements(7, 7) As Movements

    Public Sub UpdateMoves()
        For i = 0 To 7
            For j = 0 To 7
                ' Find all moves for every square
                AllPossibleMovements(i, j) = GetMoves(New iVector2(i, j))
            Next
        Next

    End Sub

    Public Function VerifyMove(pos As iVector2, target As iVector2, White As Boolean)

    End Function

    Public Function GetMoves(pos As iVector2) As Movements
        Dim retval As New Movements
        Dim type As Integer = pos.deref()
        Debug.Print("Checking move (" + pos.x.ToString() + ", " + pos.y.ToString() + ")")

        Select Case Math.Abs(type)
            Case 1
                'pawn
                If type > 0 AndAlso pos.y > 0 Then
                    If pos.y = 6 Then
                        'white start
                        retval.Add(pos.x, 4)

                    Else
                        ' Standard movement
                        If Form1.board(pos.x, pos.y - 1) = 0 Then
                            retval.Add(pos.x, pos.y - 1)

                        End If

                    End If

                    If pos.x > 0 AndAlso Form1.board(pos.x - 1, pos.y - 1) < 0 Then

                        retval.Add(pos.x - 1, pos.y - 1)

                    ElseIf pos.x < 7 AndAlso Form1.board(pos.x + 1, pos.y - 1) < 0 Then
                        retval.Add(pos.x + 1, pos.y - 1)

                    End If


                ElseIf pos.y < 7 Then
                    'Black
                    If pos.y = 1 Then
                        'black start
                        retval.Add(pos.x, 3)

                    Else
                        ' Standard movement
                        If Form1.board(pos.x, pos.y + 1) = 0 Then
                            retval.Add(pos.x, pos.y + 1)
                        End If

                    End If

                    'capture
                    If pos.x > 0 AndAlso Form1.board(pos.x - 1, pos.y + 1) > 0 Then

                        retval.Add(pos.x - 1, pos.y + 1)


                    ElseIf pos.x < 7 AndAlso Form1.board(pos.x + 1, pos.y + 1) > 0 Then

                        retval.Add(pos.x + 1, pos.y + 1)

                    End If

                End If


            Case 2
                'knight
                Dim positions(,) As Integer = New Integer(7, 1) {{1, 2}, {2, 1}, {-1, 2}, {2, -1}, {-1, -2}, {-2, -1}, {1, -2}, {-2, 1}}
                For i As Integer = 0 To 7
                    retval.Add(positions(i, 0) + pos.x, positions(i, 1) + pos.y)
                Next


            Case 3, 5
                'bishop or queen
                For i = 1 To 7
                    'up and left
                    If pos.x - i <= -1 Or pos.y - i <= -1 Or pos.Plus(-i, -i).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else

                        retval.Add(pos.x - i, pos.y - i)

                        If pos.Plus(-i, -i).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If


                    End If

                    'up and right
                    If pos.x + i >= 8 Or pos.y - i <= -1 Or pos.Plus(i, -i).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For


                    Else
                        retval.Add(pos.x + i, pos.y - i)

                        If pos.Plus(i, -i).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If

                    End If

                    'down and left
                    If pos.x - i <= -1 Or pos.y + i >= 8 Or pos.Plus(-i, i).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x - i, pos.y + i)

                        If pos.Plus(-i, i).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If

                    'down and right
                    If pos.x + i >= 8 Or pos.y + i >= 8 Or pos.Plus(i, i).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x + i, pos.y + i)
                        If pos.Plus(i, i).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If
                Next

            Case 4
                ' rook castling
                If pos.isAt(7, 7) AndAlso HasNotMoved("WK") Then
                    retval.Add(7, 7)
                ElseIf pos.isAt(0, 7) AndAlso HasNotMoved("WQ") Then
                    retval.Add(0, 7)
                ElseIf pos.isAt(0, 0) AndAlso HasNotMoved("BQ") Then
                    retval.Add(0, 0)
                ElseIf pos.isAt(7, 0) AndAlso HasNotMoved("BK") Then
                    retval.Add(7, 0)
                End If

            Case 4, 5
                'rook or queen
                For i = 1 To 7
                    ' up
                    If pos.y - i <= -1 Or pos.Plus(0, -i).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else

                        retval.Add(pos.x, pos.y - i)

                        If pos.Plus(0, -i).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If


                    End If

                    ' right
                    If pos.x + i >= 8 Or pos.Plus(i, 0).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For


                    Else
                        retval.Add(pos.x + i, pos.y)

                        If pos.Plus(i, 0).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If

                    End If

                    ' left
                    If pos.x - i <= -1 Or pos.Plus(-i, 0).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x - i, pos.y)

                        If pos.Plus(-i, 0).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If

                    'down
                    If pos.y + i >= 8 Or pos.Plus(0, i).deref() * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x, pos.y + i)
                        If pos.Plus(0, i).deref() * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If
                Next

            Case 6
                ' King

                ' standard moves
                Dim positions(,) As Integer = New Integer(7, 1) {{1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}, {0, -1}, {1, -1}}
                For i As Integer = 0 To 7
                    retval.Add(positions(i, 0) + pos.x, positions(i, 1) + pos.y)
                Next

				' King Castling
				If type > 0 Then
					' White castling
					If HasNotMoved("WK") AndAlso Form1.board(5, 7) = 0 AndAlso Form1.board(6, 7) = 0 Then
						'White king-side caslting valid
						retval.Add(6, 7)
					End If
					If HasNotMoved("WQ") AndAlso Form1.board(1, 7) = 0 AndAlso Form1.board(2, 7) = 0 AndAlso Form1.board(3, 7) = 0 Then
						'White queen-side caslting valid
						retval.Add(2, 7)
					End If
				Else
					' Black castling
					If HasNotMoved("BK") AndAlso Form1.board(1, 0) = 0 AndAlso Form1.board(2, 0) = 0 Then
						'Black king-side caslting valid
						retval.Add(1, 0)
					End If
					If HasNotMoved("BQ") AndAlso Form1.board(6, 0) = 0 AndAlso Form1.board(5, 0) = 0 AndAlso Form1.board(4, 0) = 0 Then
						'Black queen-side caslting valid
						retval.Add(5, 0)
					End If
				End If

		End Select
        Return retval
    End Function


End Class