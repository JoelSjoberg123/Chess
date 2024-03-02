using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Piece
{
    public const int none = 0b000;
    public const int king = 0b001;
    public const int pawn = 0b010;
    public const int knight = 0b011;
    public const int bishop = 0b100;
    public const int rook = 0b101;
    public const int queen = 0b110;

    public const int white = 0b01000;
    public const int black = 0b10000;

    public static bool IsColor(int piece, int color = white)
    {
        return (piece & color) != 0;
    }

    
}

