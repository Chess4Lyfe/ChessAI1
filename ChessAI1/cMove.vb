' Bow before your God object

Public Class cMove


    Private b As Chessboard

    Sub New(board As Chessboard)

        b = board

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
        Dim v As New iVector2

        Dim typ = Math.Abs(type)
        If typ = 1 Then
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


        ElseIf typ = 2 Then
            'horsey
            Dim positions(,) As Integer = New Integer(7, 1) {{1, 2}, {2, 1}, {-1, 2}, {2, -1}, {-1, -2}, {-2, -1}, {1, -2}, {-2, 1}}
            For i As Integer = 0 To 7
                v.store(positions(i, 0), positions(i, 1))
                v.Addition(pos)
                If (b.at(v) * type <= 0) Then
                    retval.Add(v)
                End If
            Next


        ElseIf typ = 3 Or typ = 5 Then
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

        ElseIf typ = 6 Then
            ' King

            ' standard moves
            Dim positions(,) As Integer = New Integer(7, 1) {{1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}, {0, -1}, {1, -1}}
            For i As Integer = 0 To 7
                v.store(positions(i, 0), positions(i, 1))
                v.Addition(pos)
                If (b.at(v) * type <= 0) Then
                    retval.Add(v)
                End If
            Next

            ' King Castling
            ' 0 = no castle, 1 = w queenside, 2 = w kingside, 3 = b queenside, 4 = b kingside


            If type > 0 AndAlso pos.isAt(4, 7) Then
                ' white
                If b.HasNotMoved(1) AndAlso b.at(1, 7) = 0 AndAlso b.at(2, 7) = 0 AndAlso b.at(3, 7) = 0 Then
                    ' white queenside
                    retval.Castle(1)
                End If
                If b.HasNotMoved(2) AndAlso b.at(5, 7) = 0 AndAlso b.at(6, 7) = 0 Then
                    ' white kingside
                    retval.Castle(2)
                End If
            ElseIf pos.isAt(4, 0) Then
                ' black
                If b.HasNotMoved(3) AndAlso b.at(1, 0) = 0 AndAlso b.at(2, 0) = 0 AndAlso b.at(3, 0) = 0 Then
                    ' black queenside
                    retval.Castle(3)
                End If
                If b.HasNotMoved(4) AndAlso b.at(5, 0) = 0 AndAlso b.at(6, 0) = 0 Then
                    ' black kingside
                    retval.Castle(4)
                End If
            End If



        End If


        If typ = 4 Or typ = 5 Then

            'If Math.Abs(type) = 4 Then
            '    ' rook castling
            '    ' 0 = no castle, 1 = w queenside, 2 = w kingside, 3 = b queenside, 4 = b kingside

            '    If type <> 0 Then
            '        If type > 0 Then
            '            ' white
            '            If pos.isAt(0, 7) AndAlso b.HasNotMoved(1) AndAlso b.at(1, 7) = 0 AndAlso b.at(2, 7) = 0 AndAlso b.at(3, 7) = 0 Then
            '                ' white queenside
            '                retval.Castle(1)
            '            ElseIf pos.isAt(7, 7) AndAlso b.HasNotMoved(2) AndAlso b.at(5, 7) = 0 AndAlso b.at(6, 7) = 0 Then
            '                ' white kingside
            '                retval.Castle(2)
            '            End If
            '        Else
            '            ' black
            '            If pos.isAt(0, 0) AndAlso b.HasNotMoved(3) AndAlso b.at(1, 0) = 0 AndAlso b.at(2, 0) = 0 AndAlso b.at(3, 0) = 0 Then
            '                ' black queenside
            '                retval.Castle(3)
            '            ElseIf pos.isAt(7, 0) AndAlso b.HasNotMoved(4) AndAlso b.at(5, 0) = 0 AndAlso b.at(6, 0) = 0 Then
            '                ' black kingside
            '                retval.Castle(4)
            '            End If
            '        End If

            '    End If
            'End If




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
        End If

        Return retval
    End Function


End Class