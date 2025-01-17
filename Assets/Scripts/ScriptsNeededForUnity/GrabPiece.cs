using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrabPiece : MonoBehaviour
{
    [SerializeField]Camera cam;
    [SerializeField]Transform squareTarget;
    [SerializeField]Transform targetsParent;
    List<Transform> squareTargets;

    GameObject pieceHolding;
    SpriteRenderer pieceHoldingRenderer;
    Vector2 pieceWorldStartSquare;

    int startSquare;
    GraphicalBoard graphicalBoard;



    List<Move> pieceMoves = new List<Move>();

    private void Awake()
    {
        graphicalBoard = GetComponent<GraphicalBoard>();
        squareTargets = new List<Transform>();
    }
    void Update()
    {
        Vector2 mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log($"x{mousePos.x}   y{mousePos.y}");
        RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.05f, Vector2.up, 0.01f);
        
        if (Input.GetMouseButtonDown(0)) 
        {
            if(hit.collider != null) 
            {
                pieceHolding = hit.collider.gameObject;
                pieceHoldingRenderer = pieceHolding.GetComponent<SpriteRenderer>();
                pieceHoldingRenderer.sortingOrder = 3;
                startSquare = FromGlobalPosToIndex((Vector2)pieceHolding.transform.position);

                pieceWorldStartSquare = (Vector2)pieceHolding.transform.position;

                Board board = graphicalBoard.Board;

                if(!Piece.IsColor(board.Squares[startSquare], board.ColorToMove)){
                    pieceHolding = null;
                    return;
                }
                
                Move[] moves = LegalMovesGenerator.GenerateMoves(graphicalBoard.Board);
                Debug.Log(moves.Length);
                for(int i = 0; i < moves.Length; i++) 
                {
                    //Debug.Log("Before");
                    if(moves[i].StartSquare != startSquare) continue;
                    //Debug.Log("After");
                    Vector2 pos = FromIndexToGlobalPos(moves[i].TargetSquare);
                    quaternion rotation = squareTarget.rotation;
                    //Debug.Log($"Pos:({pos.x}, {pos.y})");
                    squareTargets.Add(Instantiate(squareTarget, pos, rotation, targetsParent));
                    pieceMoves.Add(moves[i]);
                }
            }
        }
        
        if (pieceHolding == null) return;
        
        pieceHolding.transform.position = mousePos;
        
        if(Input.GetMouseButtonUp(0))
        {
            int targetSquare = FromGlobalPosToIndex((Vector2)pieceHolding.transform.position);

            int correctMoveIndex = -1;

            Move move = new Move((byte)startSquare, (byte)targetSquare);
            for(int i = 0; i < pieceMoves.Count; i++)
            {
                if(move.Equals(pieceMoves[i])){
                    correctMoveIndex = i;
                    break;
                }
            }
            
            for(int i = squareTargets.Count - 1; i >= 0; i--)
            {
                Destroy(squareTargets[i].gameObject);
                squareTargets.Remove(squareTargets[i]);
            }

            if(correctMoveIndex == -1)
            {
                pieceHolding.transform.position = pieceWorldStartSquare;
                pieceHolding = null;
                return;
            }

            graphicalBoard.MakeMove(pieceMoves[correctMoveIndex]);

            pieceHolding = null;
            
            pieceMoves.Clear();
        }
    }

    private static int FromGlobalPosToIndex(Vector2 pos)
    {
        //Fixes pos for lerping. Explanation: Makes it range from 0 to 1.
        pos = new Vector2(pos.x + 3.5f, pos.y + 3.5f);
        pos /= 7;

        //Rank 7 to 0 needed, as black starts close to index 0, but is far away from down(which becomes zero)
        int file = Mathf.RoundToInt(Mathf.Lerp(0, 7, pos.x));
        //Debug.Log($"File: {file}");

        int rank = Mathf.RoundToInt(Mathf.Lerp(7, 0, pos.y));
        
        //Debug.Log($"Rank:{rank}");
        
        return rank * 8 + file;
    }

    private static Vector2 FromIndexToGlobalPos(int index)
    {
        int file = Board.SquareIndexToFile((byte)index); 
        int rank = Board.SquareIndexToRank((byte)index);
        
        Vector2 result = new Vector2(Mathf.InverseLerp(0, 7, file) * 7 - 3.5f,
                                     Mathf.InverseLerp(7, 0, rank) * 7 - 3.5f);

        return result;
    }

}
