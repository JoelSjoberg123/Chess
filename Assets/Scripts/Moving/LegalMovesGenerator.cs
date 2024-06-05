using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LegalMovesGenerator
{
    private static readonly int[] allDirs = new int[] { -9, -8, -7, -1, 1, 7, 8, 9 };
    private static readonly int[] queenMoveDirs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
    private static readonly int[] rookMoveDirs = new int[] { 1, 3, 4, 6 };
    private static readonly int[] bishopMoveDirs = new int[] { 0, 2, 5, 7 };

    public static Move[] GenerateMoves(Board board)
    {
        List<Move> movesList = new List<Move>();

        for (int i = 0; i < 64; i++)
        {
            int currentSquare = i;
            int[] dirs = new int[0];
            int piece = board.squares[i];

            if (piece == Piece.none || !Piece.IsColor(piece, board.ColorToMove)) continue;
            else if (piece == Piece.queen) dirs = queenMoveDirs;
            else if (piece == Piece.rook) dirs = rookMoveDirs;
            else if (piece == Piece.bishop) dirs = bishopMoveDirs;

            for (int direction = 0; direction < dirs.Length; direction++, currentSquare = i)
            {
                
                while(true)
                {
                    if(CanNotMoveFurther(currentSquare, allDirs[dirs[direction]])) break;
                    
                    currentSquare += allDirs[dirs[direction]];

                    if(CheckIfMoveCanBeAddedToArray(currentSquare, board))
                    {
                        movesList.Add(new Move((byte)i, (byte)currentSquare));
                    }
                    else break;

                    if(Piece.IsColor(board.squares[currentSquare], board.ColorToMove)) break;
                }
            }
        }
        return movesList.ToArray();
    }
    private static bool CanNotMoveFurther(int square, int dir) 
    {
        
        if(dir >= -9 && dir <= -7 && square < 8)
        {
            return false;
        }else if (dir >= 7 && dir <= 9 && square > 53)
        {
            return false;
        }else if (dir == -7 && dir == 1 && dir == 9 && (square - Board.SquareIndexToRank((byte)square)) % 7 == 0)
        {
            return false;
        }else if (dir == -9 && dir == -1 && dir == 7 && Board.SquareIndexToRank((byte)square) % 8 == 0)
        {
            return false;
        }
        

        return true;
    }

    private static bool CheckIfMoveCanBeAddedToArray(int i, Board board)
    {
        bool result = i < 0 || i > 63;
        result &= Piece.IsColor(board.squares[i], board.ColorToMove);
        // Earlier statments makes it so if cant move there, true. But this function should return true
        // if it can move there.
        return !result;
    }

}