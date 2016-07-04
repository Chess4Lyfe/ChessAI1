' Bow before your God object

Public Class cMove


    Private HasNotMoved As New Dictionary(Of String, Boolean)

    Private b As Chessboard

    Sub New(board As Chessboard)

        b = board

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
                AllPossibleMovements(i, j) = CalculateMoves(New iVector2(i, j))
            Next
        Next

    End Sub

    Public Function VerifyMove(pos As iVector2, target As iVector2, White As Boolean) As Boolean
        Return AllPossibleMovements(pos.x, pos.y).Contains(target)
    End Function

    Public Function GetMoves(pos As iVector2) As Movements
        Return AllPossibleMovements(pos.x, pos.y)
    End Function

    Public Function GetMoves(x As Integer, y As Integer) As Movements
        Return AllPossibleMovements(x, y)
    End Function

    Private Function CalculateMoves(pos As iVector2) As Movements
        Dim retval As New Movements(b)
        Dim type As Integer = b.at(pos)


        Select Case Math.Abs(type)
            Case 1
                'pawn
                If type > 0 AndAlso pos.y > 0 Then
                    If pos.y = 6 Then
                        'white start
                        retval.Add(pos.x, 4)

                    End If
                    ' Standard movement
                    If b.at(pos.x, pos.y - 1) = 0 Then
                        retval.Add(pos.x, pos.y - 1)
                    End If



                    If pos.x > 0 AndAlso b.at(pos.x - 1, pos.y - 1) < 0 Then

                        retval.Add(pos.x - 1, pos.y - 1)

                    ElseIf pos.x < 7 AndAlso b.at(pos.x + 1, pos.y - 1) < 0 Then
                        retval.Add(pos.x + 1, pos.y - 1)

                    End If


                ElseIf pos.y < 7 Then
                    'Black
                    If pos.y = 1 Then
                        'black start
                        retval.Add(pos.x, 3)

                    End If
                    ' Standard movement
                    If b.at(pos.x, pos.y + 1) = 0 Then
                        retval.Add(pos.x, pos.y + 1)
                    End If



                    'capture
                    If pos.x > 0 AndAlso b.at(pos.x - 1, pos.y + 1) > 0 Then

                        retval.Add(pos.x - 1, pos.y + 1)


                    ElseIf pos.x < 7 AndAlso b.at(pos.x + 1, pos.y + 1) > 0 Then

                        retval.Add(pos.x + 1, pos.y + 1)

                    End If

                End If


            Case 2
                'horsey
                Dim positions(,) As Integer = New Integer(7, 1) {{1, 2}, {2, 1}, {-1, 2}, {2, -1}, {-1, -2}, {-2, -1}, {1, -2}, {-2, 1}}
                For i As Integer = 0 To 7
                    Dim v = New iVector2(positions(i, 0), positions(i, 1))
                    v.Addition(pos)
                    If (b.at(v) * type <= 0) Then
                        retval.Add(v)
                    End If
                Next


            Case 3, 5
                'bishop or queen
                For i = 1 To 7
                    'up and left
                    If pos.x - i <= -1 Or pos.y - i <= -1 Or b.at(pos.Plus(-i, -i)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else

                        retval.Add(pos.x - i, pos.y - i)

                        If b.at(pos.Plus(-i, -i)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If


                    End If
                Next

                For i = 1 To 7
                    'up and right
                    If pos.x + i >= 8 Or pos.y - i <= -1 Or b.at(pos.Plus(i, -i)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For


                    Else
                        retval.Add(pos.x + i, pos.y - i)

                        If b.at(pos.Plus(i, -i)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If

                    End If
                Next

                For i = 1 To 7
                    'down and left
                    If pos.x - i <= -1 Or pos.y + i >= 8 Or b.at(pos.Plus(-i, i)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x - i, pos.y + i)

                        If b.at(pos.Plus(-i, i)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If
                Next

                For i = 1 To 7
                    'down and right
                    If pos.x + i >= 8 Or pos.y + i >= 8 Or b.at(pos.Plus(i, i)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x + i, pos.y + i)
                        If b.at(pos.Plus(i, i)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If
                Next

            'Case 4
            '    ' rook castling
            '    If pos.isAt(7, 7) AndAlso b.at(6, 7) = 0 AndAlso HasNotMoved("WK") Then
            '        ' White Kingside Castle
            '        retval.Add(5, 7)
            '    ElseIf pos.isAt(0, 7) AndAlso b.at(1, 7) = 0 AndAlso b.at(2, 7) = 0 AndAlso HasNotMoved("WQ") Then
            '        ' White Queenside Castle
            '        retval.Add(3, 7)
            '    ElseIf pos.isAt(0, 0) AndAlso b.at(6, 0) = 0 AndAlso HasNotMoved("BK") Then
            '        'Black Queenside Castle
            '        retval.Add(5, 0)
            '    ElseIf pos.isAt(7, 0) AndAlso b.at(1, 0) = 0 AndAlso b.at(2, 0) = 0 AndAlso HasNotMoved("BQ") Then
            '        ' Black Queenside Castle
            '        retval.Add(3, 0)
            '    End If

            Case 4, 5
                'rook or queen
                For i = 1 To 7
                    ' up
                    If pos.y - i <= -1 Or b.at(pos.Plus(0, -i)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else

                        retval.Add(pos.x, pos.y - i)

                        If b.at(pos.Plus(0, -i)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If


                    End If
                Next

                For i = 1 To 7
                    ' right
                    If pos.x + i >= 8 Or b.at(pos.Plus(i, 0)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For


                    Else
                        retval.Add(pos.x + i, pos.y)

                        If b.at(pos.Plus(i, 0)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If

                    End If
                Next

                For i = 1 To 7
                    ' left
                    If pos.x - i <= -1 Or b.at(pos.Plus(-i, 0)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x - i, pos.y)

                        If b.at(pos.Plus(-i, 0)) * type < 0 Then
                            ' different colour, save this spot then leave
                            Exit For
                        End If
                    End If
                Next

                For i = 1 To 7
                    'down
                    If pos.y + i >= 8 Or b.at(pos.Plus(0, i)) * type > 0 Then
                        'same colour, can't go any further
                        Exit For

                    Else
                        retval.Add(pos.x, pos.y + i)
                        If b.at(pos.Plus(0, i)) * type < 0 Then
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
                    If HasNotMoved("WK") AndAlso b.at(5, 7) = 0 AndAlso b.at(6, 7) = 0 Then
                        'White king-side caslting valid
                        retval.Add(6, 7)
                    End If
                    If HasNotMoved("WQ") AndAlso b.at(1, 7) = 0 AndAlso b.at(2, 7) = 0 AndAlso b.at(3, 7) = 0 Then
                        'White queen-side caslting valid
                        retval.Add(2, 7)
                    End If
                Else
                    ' Black castling
                    If HasNotMoved("BK") AndAlso b.at(1, 0) = 0 AndAlso b.at(2, 0) = 0 Then
                        'Black king-side caslting valid
                        retval.Add(1, 0)
                    End If
                    If HasNotMoved("BQ") AndAlso b.at(6, 0) = 0 AndAlso b.at(5, 0) = 0 AndAlso b.at(4, 0) = 0 Then
                        'Black queen-side caslting valid
                        retval.Add(5, 0)
                    End If
                End If

        End Select
        Return retval
    End Function


End Class