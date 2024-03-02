using System.Collections;
using System.Collections.Generic;

public class Board
{
    public int[] squares;
    public Board()
    {
        squares = new int[64];
    }
    public void SetUpBoard() 
    {
        squares[0] = Piece.bishop | Piece.black;
        squares[8] = Piece.bishop | Piece.white;
        squares[56] = Piece.queen | Piece.black;
    }
}
