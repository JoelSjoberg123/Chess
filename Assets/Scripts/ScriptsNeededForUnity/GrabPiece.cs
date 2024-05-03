using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPiece : MonoBehaviour
{
    [SerializeField]Camera cam;
    GameObject pieceHolding;
    int startSquare;
    GraphicalBoard graphicalBoard;

    private void Awake()
    {
        graphicalBoard = GetComponent<GraphicalBoard>();
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
                startSquare = FromGlobalPosToIndex((Vector2)pieceHolding.transform.position);
            }
        }
        
        if (pieceHolding == null) return;
        
        pieceHolding.transform.position = mousePos;
        
        if(Input.GetMouseButtonUp(0))
        {
            int targetSquare = FromGlobalPosToIndex((Vector2)pieceHolding.transform.position);

            Move move = new Move((byte)startSquare, (byte)targetSquare);

            graphicalBoard.MakeMove(move);

            pieceHolding = null;
        }
    }

    private static int FromGlobalPosToIndex(Vector2 pos)
    {
        //Fixes pos for lerping. Explanation: Makes it range from 0 to 1.
        pos = new Vector2(pos.x + 3.5f, pos.y + 3.5f);
        pos /= 7;

        //Rank 7 to 0 needed, as black starts close to index 0, but is far away from down(which becomes zero)
        int file = Mathf.RoundToInt(Mathf.Lerp(0, 7, pos.x));
        Debug.Log("File: " + file + "  " + Mathf.RoundToInt(Mathf.Lerp(0, 7, pos.x)));

        int rank = Mathf.RoundToInt(Mathf.Lerp(7, 0, pos.y));
        
        Debug.Log("Rank: " + rank + "  " + Mathf.RoundToInt(Mathf.Lerp(7, 0, pos.y)));
        
        return rank * 8 + file;
    }

}
