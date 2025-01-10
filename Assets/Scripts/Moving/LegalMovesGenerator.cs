using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LegalMovesGenerator
{
    private static readonly int[] allDirs = new int[] { -9, -8, -7, -1, 1, 7, 8, 9 };
    private static readonly int[] queenMoveDirs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
    private static readonly int[] rookMoveDirs = new int[] { 1, 3, 4, 6 };
    private static readonly int[] bishopMoveDirs = new int[] { 0, 2, 5, 7 };

    private static List<Move> movesList;

    public static Move[] GenerateMoves(Board board, GameObject obj)
    {
        movesList = new List<Move>();

        for (int i = 0; i < 64; i++)
        {
            int currentSquare = i;

            int movingColor = board.ColorToMove;
            int oppositeColor = (movingColor == Piece.white) ? Piece.black : Piece.white;


            int[] dirs = new int[0];
            int piece = board.squares[i];
            //Debug.Log($"Color to move:{board.ColorToMove}  piececolor:{piece & 0b11000}  piece is color to move:{Piece.IsColor(piece, board.ColorToMove)}");
            if(piece == Piece.none || !Piece.IsColor(piece, board.ColorToMove)) continue;
            else if(Piece.IsPieceType(piece, Piece.pawn  )) PawnMovement(board, i, currentSquare, movingColor, oppositeColor);
            else if(Piece.IsPieceType(piece, Piece.bishop)) dirs = bishopMoveDirs;
            else if(Piece.IsPieceType(piece, Piece.rook  )) dirs = rookMoveDirs;
            else if(Piece.IsPieceType(piece, Piece.queen ) 
                || Piece.IsPieceType(piece, Piece.king   )) dirs = queenMoveDirs;

            for(int direction = 0; direction < dirs.Length; direction++, currentSquare = i)
            {
                while(true)
                {
                    if (CanNotMoveFurther(currentSquare, allDirs[dirs[direction]])) break;
                    
                    currentSquare += allDirs[dirs[direction]];
                    
                    if(!(currentSquare >= 0 && currentSquare <= 63)) break;
                    
                    int pieceOnSquare = board.squares[currentSquare];

                    if (Piece.IsPieceType(pieceOnSquare, Piece.king)) Debug.Log("Hello", obj);

                    if(Piece.IsColor(pieceOnSquare, board.ColorToMove)) break;

                    movesList.Add(new Move(i, currentSquare));

                    if(Piece.IsPieceType(piece, Piece.king)) break;

                    if(Piece.IsColor(pieceOnSquare, oppositeColor) && pieceOnSquare != 0) break;
                }
            }
        }
        return movesList.ToArray();
    }

    private static void PawnMovement(Board board, int i, int currentSquare, int movingColor, int oppositeColor)
    {
        if (movingColor == Piece.white)
        {
            if (board.squares[i - 8] == Piece.none)
            {
                movesList.Add(new Move(currentSquare, (currentSquare - 8)));

                if (Board.SquareIndexToRank(currentSquare) == 6)
                    if (board.squares[i - 16] == Piece.none)
                        movesList.Add(new Move(currentSquare, (currentSquare - 16)));
            }
            if (Piece.IsColor(board.squares[i - 7], oppositeColor))
                movesList.Add(new Move(currentSquare, (currentSquare - 7)));
            if (Piece.IsColor(board.squares[i - 9], oppositeColor))
                movesList.Add(new Move(currentSquare, (currentSquare - 9)));
        }
        else
        {
            if (board.squares[i + 8] == Piece.none)
            {
                movesList.Add(new Move(currentSquare, (currentSquare + 8)));

                if (Board.SquareIndexToRank(currentSquare) == 1)
                    if (board.squares[i + 16] == Piece.none)
                        movesList.Add(new Move(currentSquare, (currentSquare + 16)));
            }
            if (Piece.IsColor(board.squares[i + 7], oppositeColor))
                movesList.Add(new Move(currentSquare, (currentSquare + 7)));
            if (Piece.IsColor(board.squares[i + 9], oppositeColor))
                movesList.Add(new Move(currentSquare, (currentSquare + 9)));
        }
    }

    private static bool CanNotMoveFurther(int square, int dir) 
    {
        
        if(dir <= -9 && dir >= -7 && Board.SquareIndexToRank(square) == 0)
        {
            return true;
        }else if (dir >= 7 && dir <= 9 && Board.SquareIndexToRank(square) == 7)
        {
            return true;
        }else if ((dir == -7 || dir ==  1 || dir == 9) && Board.SquareIndexToFile(square) == 7)
        {
            return true;
        }else if ((dir == -9 || dir == -1 || dir == 7) && Board.SquareIndexToFile(square) == 0)
        {
            return true;
        }
        

        return false;
    }
}