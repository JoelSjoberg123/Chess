using System;
using System.Collections;
using System.Collections.Generic;

public static class LegalMovesGenerator
{
    private static readonly int[] allDirs = new int[] { -9, -8, -7, -1, 1, 7, 8, 9 };
    private static readonly int[] queenMoveDirs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
    private static readonly int[] rookMoveDirs = new int[] { 1, 3, 4, 6 };
    private static readonly int[] bishopMoveDirs = new int[] { 0, 2, 5, 7 };

    private static List<Move> movesList;

    public static Move[] GenerateMoves(Board board)
    {
        movesList = new List<Move>();

        for (int startSquare = 0; startSquare < 64; startSquare++)
        {
            int targetSquare = startSquare;

            int movingColor = board.ColorToMove;
            int oppositeColor = (movingColor == Piece.white) ? Piece.black : Piece.white;


            int[] dirs = new int[0];
            int piece = board.Squares[startSquare];
            //Debug.Log($"Color to move:{board.ColorToMove}  piececolor:{piece & 0b11000}  piece is color to move:{Piece.IsColor(piece, board.ColorToMove)}");
            if(piece == Piece.none || !Piece.IsColor(piece, board.ColorToMove)) continue;
            else if(Piece.IsPieceType(piece, Piece.pawn  )) PawnMovement(board, startSquare, movingColor, oppositeColor);
            else if(Piece.IsPieceType(piece, Piece.knight)) KnightMovement(board, startSquare, movingColor, oppositeColor);
            else if(Piece.IsPieceType(piece, Piece.bishop)) dirs = bishopMoveDirs;
            else if(Piece.IsPieceType(piece, Piece.rook  )) dirs = rookMoveDirs;
            else if(Piece.IsPieceType(piece, Piece.queen ) 
                || Piece.IsPieceType(piece, Piece.king   )) dirs = queenMoveDirs;

            for(int direction = 0; direction < dirs.Length; direction++, targetSquare = startSquare)
            {
                while(true)
                {
                    if (CanNotMoveFurther(targetSquare, allDirs[dirs[direction]])) break;
                    
                    targetSquare += allDirs[dirs[direction]];
                    
                    if(!(targetSquare >= 0 && targetSquare <= 63)) break;
                    
                    int pieceOnSquare = board.Squares[targetSquare];

                    if(Piece.IsColor(pieceOnSquare, board.ColorToMove)) break;

                    movesList.Add(new Move(startSquare, targetSquare));

                    if(Piece.IsPieceType(piece, Piece.king)) break;

                    if(Piece.IsColor(pieceOnSquare, oppositeColor) && pieceOnSquare != 0) break;
                }
            }
        }
        return movesList.ToArray();
    }

    private static void KnightMovement(Board board, int startSquare, int movingColor, int oppositeColor)
    {
        int rank = Board.SquareIndexToRank(startSquare);
        int file = Board.SquareIndexToFile(startSquare);

        bool canMoveUpwardsAllWay   = rank > 1;
        bool canMoveDownwardsAllWay = rank < 6;
        bool canMoveLeftAllWay      = file > 1;
        bool canMoveRightAllWay     = file < 6;



        if(canMoveUpwardsAllWay)
        {
            if((canMoveRightAllWay || file != 7) && !Piece.IsColor(board.Squares[startSquare - 15], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare - 15));
            }

            if((canMoveLeftAllWay || file != 0) && !Piece.IsColor(board.Squares[startSquare - 17], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare - 17));
            }
        }

        if(canMoveDownwardsAllWay)
        {
            if((canMoveRightAllWay || file != 7) && !Piece.IsColor(board.Squares[startSquare + 15], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare + 15));
            }

            if((canMoveLeftAllWay || file != 0) && !Piece.IsColor(board.Squares[startSquare + 17], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare + 17));
            }
        }

        if(canMoveLeftAllWay)
        {
            if((canMoveDownwardsAllWay || rank != 7) && !Piece.IsColor(board.Squares[startSquare + 6], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare + 6));
            }

            if((canMoveUpwardsAllWay || rank != 0) && !Piece.IsColor(board.Squares[startSquare - 10], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare - 10));
            }
        }

        if(canMoveRightAllWay)
        {
            if((canMoveDownwardsAllWay || rank != 7) && !Piece.IsColor(board.Squares[startSquare + 10], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare + 10));
            }

            if((canMoveUpwardsAllWay || rank != 0) && !Piece.IsColor(board.Squares[startSquare - 6], movingColor))
            {
                movesList.Add(new Move(startSquare, startSquare - 6));
            }
        }
    }

    private static void PawnMovement(Board board, int startSquare, int movingColor, int oppositeColor)
    {
        if(movingColor == Piece.white)
        {
            if(board.Squares[startSquare - 8] == Piece.none)
            {
                movesList.Add(new Move(startSquare, (startSquare - 8)));

                if(Board.SquareIndexToRank(startSquare) == 6)
                    if(board.Squares[startSquare - 16] == Piece.none)
                        movesList.Add(new Move(startSquare, startSquare - 16, true));
            }
            if(Piece.IsColor(board.Squares[startSquare - 7], oppositeColor) || (startSquare - 7 == board.EnPeasentSquare && board.EnPeasentSquare != 0))
                movesList.Add(new Move(startSquare, (startSquare - 7)));
            if(Piece.IsColor(board.Squares[startSquare - 9], oppositeColor) || (startSquare - 9 == board.EnPeasentSquare && board.EnPeasentSquare != 0))
                movesList.Add(new Move(startSquare, (startSquare - 9)));
        }
        else
        {
            if(board.Squares[startSquare + 8] == Piece.none)
            {
                movesList.Add(new Move(startSquare, (startSquare + 8)));

                if (Board.SquareIndexToRank(startSquare) == 1)
                    if (board.Squares[startSquare + 16] == Piece.none)
                        movesList.Add(new Move(startSquare, startSquare + 16, true));
            }
            if(Piece.IsColor(board.Squares[startSquare + 7], oppositeColor) || (startSquare + 7 == board.EnPeasentSquare && board.EnPeasentSquare != 0))
                movesList.Add(new Move(startSquare, (startSquare + 7)));
            if(Piece.IsColor(board.Squares[startSquare + 9], oppositeColor) || (startSquare + 9 == board.EnPeasentSquare && board.EnPeasentSquare != 0))
                movesList.Add(new Move(startSquare, (startSquare + 9)));
        }
    }

    private static bool CanNotMoveFurther(int square, int dir) 
    {
        
        if(dir <= -9 && dir >= -7 && Board.SquareIndexToRank(square) == 0)
        {
            return true;
        }else if(dir >= 7 && dir <= 9 && Board.SquareIndexToRank(square) == 7)
        {
            return true;
        }else if((dir == -7 || dir ==  1 || dir == 9) && Board.SquareIndexToFile(square) == 7)
        {
            return true;
        }else if((dir == -9 || dir == -1 || dir == 7) && Board.SquareIndexToFile(square) == 0)
        {
            return true;
        }


        return false;
    }
}