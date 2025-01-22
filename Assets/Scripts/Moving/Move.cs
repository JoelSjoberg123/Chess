using System;
using System.Collections;
using System.Collections.Generic;


public struct Move
{
	private ushort theMove;
	public ushort TheMove
	{
		get {  return theMove; }
	}


    override public bool Equals(object obj)
    {
		Move m = (Move)obj;
		return m.StartSquare == this.StartSquare && m.TargetSquare == this.TargetSquare;
	}
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public Move(int startSquare, int targetSquare, bool isDoublePawnMove = false, bool isCastling = false)
	{
		if(startSquare > 63 || targetSquare > 63)
		{
			throw new ArgumentException("startSquare or targetSquare can not be bigger than 63. " +
				$"startSquare was {startSquare}. targetSquare was {targetSquare}");
		}else if(startSquare < 0 || targetSquare < 0)
		{
            throw new ArgumentException("startSquare or targetSquare can not be smaller than 0. " +
                $"startSquare was {startSquare}. targetSquare was {targetSquare}");
        }
		theMove = 0;
		//0b 000000        000000                  0                 0              00
		//   start square  target square  is double pawn move        is castling    extra
		
		theMove = (ushort)(startSquare << 10 | targetSquare << 4 |
			              (isDoublePawnMove ? 1:0) << 3 | (isCastling ? 1 : 0) << 2);
	}
	public byte StartSquare
	{
		get{return (byte)(theMove >> 10); }
	}
	public byte TargetSquare
	{
		get{return (byte)((theMove >> 4) & 0b111111); }
	}

	public bool IsDoublePawnMove
	{
		get {return ((theMove >> 3) & 0b0000000000001) == 1;}
	}
    public bool IsCastlingMove
    {
        get { return ((theMove >> 2) & 0b0000000000001) == 1; }
    }

}