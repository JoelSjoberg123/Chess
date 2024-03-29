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

    public Move(byte startSquare, byte endSquare)
    {
        theMove = 0;
        theMove = (ushort)(startSquare << 10 | endSquare << 4);
    }
}
