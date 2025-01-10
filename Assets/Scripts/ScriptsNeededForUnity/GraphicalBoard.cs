using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalBoard : MonoBehaviour
{

    [SerializeField] GameObject[] pieces;
    [SerializeField] Transform pieceParent;
    [SerializeField] Vector2 testing;
    [SerializeField] Move moveTEST = new Move(0, 1);

    Board board = new Board();

    public Board Board
    {
        get{return board;}
    }

    // Start is called before the first frame update
    void Start()
    {
        board.SetUpBoard();
        //Rank is 0 on the first line of black.
        for (int rank = 0; rank < 8; rank++) 
        {
            for(int file = 0; file < 8; file++)
            {
                SpawnInPiece(rank, file);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TESTINGMakeMove();
        }
    }

    private void SpawnInPiece(int rank, int file)
    {
        //Finds where to place the pices
        float x = Mathf.Lerp(-3.5f, 3.5f, file / 7f);
        float y = Mathf.Lerp(3.5f, -3.5f, rank / 7f);

        Vector2 pos = new Vector2(x, y);

        Quaternion rotation = pieces[0].transform.rotation;

        PlacePiece(rank, file, pos, rotation);
    }

    

    private void PlacePiece(int rank, int file, Vector2 pos, Quaternion rotation)
    {
        switch (board.squares[file + rank * 8])
        {
            case 0:
                
                break;
            case Piece.king | Piece.white:
                Instantiate(pieces[0], pos, rotation, pieceParent);
                break;
            case Piece.pawn | Piece.white:
                Instantiate(pieces[1], pos, rotation, pieceParent);
                break;
            case Piece.knight | Piece.white:
                Instantiate(pieces[2], pos, rotation, pieceParent);
                break;
            case Piece.bishop | Piece.white:
                Instantiate(pieces[3], pos, rotation, pieceParent);
                break;
            case Piece.rook | Piece.white:
                Instantiate(pieces[4], pos, rotation, pieceParent);
                break;
            case Piece.queen | Piece.white:
                Instantiate(pieces[5], pos, rotation, pieceParent);
                break;
            // Black pieces
            case Piece.king | Piece.black:
                Instantiate(pieces[6], pos, rotation, pieceParent);
                break;
            case Piece.pawn | Piece.black:
                Instantiate(pieces[7], pos, rotation, pieceParent);
                break;
            case Piece.knight | Piece.black:
                Instantiate(pieces[8], pos, rotation, pieceParent);
                break;
            case Piece.bishop | Piece.black:
                Instantiate(pieces[9], pos, rotation, pieceParent);
                break;
            case Piece.rook | Piece.black:
                Instantiate(pieces[10], pos, rotation, pieceParent);
                break;
            case Piece.queen | Piece.black:
                Instantiate(pieces[11], pos, rotation, pieceParent);
                break;
        }
    }
    
    private void TESTINGMakeMove()
    {
        //Move[] moves = LegalMovesGenerator.GenerateMoves(board);



        moveTEST = new Move((byte)testing.x, (byte)testing.y);
        
        

        
    }

    public void MakeMove(Move move)
    {
        board.MakeMove(move);
        string colorStr = (board.ColorToMove == Piece.white) ? "white" : "black";
        
        Debug.Log($"Color to move next:{colorStr}");
        
        for (int i = 0; i < pieceParent.childCount; i++)
        {
            Destroy(pieceParent.GetChild(i).gameObject);
        }


        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                SpawnInPiece(rank, file);
            }
        }
    }
}
