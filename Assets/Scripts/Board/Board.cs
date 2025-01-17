using System;
using System.Collections;
using System.Collections.Generic;
public class Board
{
    const string startFenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w";
    
    private int[] squares;

    private int enPeasentSquare;

    public int EnPeasentSquare
    {
        get{ return enPeasentSquare;}
    }

    int colorToMove = Piece.white;
    
    public Board()
    {
        squares = new int[64];
    }
    
    public int[] Squares { get { return (int[])squares.Clone(); } }
    public int ColorToMove
    {
        get { return colorToMove; }
    }
    public static int SquareIndexToRank(int index)
    {
        return index / 8;
    }
    public static int SquareIndexToFile(int index)
    {
        return index % 8;
    }
    /// <summary>
    /// Plays a move on the board.
    /// </summary>
    /// <param name="move">The move that will be played.</param>
    public void MakeMove(Move move)
    {
        
        byte startSquare  = move.StartSquare;
        byte targetSquare = move.TargetSquare;
        
        if(targetSquare == enPeasentSquare && Piece.IsPieceType(squares[startSquare], Piece.pawn) && enPeasentSquare != 0)
        {
            //If the piece that was moved is white, then the black pawn is towards the white side,
            //aka the side that gets bigger. If the piece that was moved is black, then the white 
            //pawn is moving towards the black side, aka the side that gets smaller.
            int squareWherePawnThatDoubleMovedIs = targetSquare + ((colorToMove == Piece.white) ? 8 : -8);
            squares[squareWherePawnThatDoubleMovedIs] = Piece.none;
        }

        if(move.IsDoublePawnMove){
            if(colorToMove == Piece.white)enPeasentSquare = targetSquare + 8;
            else enPeasentSquare = targetSquare - 8;
        }
        else enPeasentSquare = 0;

        if (startSquare == targetSquare) throw new ArgumentException("startSquare can not be equal to targetSquare.");
        
        colorToMove = (colorToMove == Piece.white)? Piece.black : Piece.white; 

        squares[targetSquare] = squares[startSquare];
        squares[startSquare] = Piece.none;
    }

    public void SetUpBoard(string fen = startFenString) 
    {
        char[] fenChar = fen.ToCharArray();
        int index = 0;
        for (int i = 0; i < fenChar.Length; i++)
        {
            switch (fenChar[i])
            {
                case 'p':
                    squares[index] = Piece.pawn | Piece.black;
                    index++;
                    break;
                case 'n':
                    squares[index] = Piece.knight | Piece.black;
                    index++;
                    break;
                case 'b':
                    squares[index] = Piece.bishop | Piece.black;
                    index++;
                    break;
                case 'r':
                    squares[index] = Piece.rook | Piece.black;
                    index++;
                    break;
                case 'q':
                    squares[index] = Piece.queen | Piece.black;
                    index++;
                    break;
                case 'k':
                    squares[index] = Piece.king | Piece.black;
                    index++;
                    break;
                case 'P':
                    squares[index] = Piece.pawn | Piece.white;
                    index++;
                    break;
                case 'N':
                    squares[index] = Piece.knight | Piece.white;
                    index++;
                    break;
                case 'B':
                    squares[index] = Piece.bishop | Piece.white;
                    index++;
                    break;
                case 'R':
                    squares[index] = Piece.rook | Piece.white;
                    index++;
                    break;
                case 'Q':
                    squares[index] = Piece.queen | Piece.white;
                    index++;
                    break;
                case 'K':
                    squares[index] = Piece.king | Piece.white;
                    index++;
                    break;
            }


            if (char.IsNumber(fenChar[i]))
            {
                int num = Convert.ToInt32(fenChar[i].ToString());
                index += num;
            }
            else if (fenChar[i] == 'w')
            {
                colorToMove = Piece.white;
            }
            else if (fenChar[i] == 'b')
            {
                colorToMove = Piece.black;
            }
        }
        string colorStr = (colorToMove == Piece.white) ? "white" : "black";

        UnityEngine.Debug.Log($"Color to move next:{colorStr}");
    }
}
