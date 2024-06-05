using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Move
{
	private ushort theMove;
	public ushort TheMove
	{
		get {  return theMove; }
	}

	public Move(byte startSquare, byte targetSquare)
	{
		theMove = 0;
		//0b 000000        000000         0000
		//   start square  target square  extra
		theMove = (ushort)(startSquare << 10 | targetSquare << 4);
	}
	public byte StartSquare()
	{
		return (byte)(TheMove >> 10);
	}
	public byte TargetSquare()
	{
		return (byte)((TheMove >> 4) & 0b111111);
	}

}