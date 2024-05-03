using System;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    const string startFenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w";
    
    public int[] squares;

    bool whiteToMove = true;
    
    public Board()
    {
        squares = new int[64];
    }
    
    public bool WhiteToMove 
    {
        get { return whiteToMove;}
    }
    public static int SquareIndexToRank(byte index)
    {
        return index / 8;
    }
    public static int SquareIndexToFile(byte index)
    {
        return index % 8;
    }
    public void MakeMove(Move move)
    {
        
        byte startSquare  = move.StartSquare();
        byte targetSquare = move.TargetSquare();
        if (startSquare == targetSquare) return;
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
                whiteToMove = true;
            }
            else if (fenChar[i] == 'b')
            {
                whiteToMove = false;
            }
        }
    }
}
