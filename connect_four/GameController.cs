using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cell_object;
    public Board_Grid game_board;

    void Start()
    {
        game_board = new Board_Grid(10,10);
        
        for(int i =0;i<10;i++)
        {
            for(int j=0;j<10;j++)
            {
                Instantiate(cell_object, new Vector3( (float)game_board.board[i,j].x_pos-5f, (float)game_board.board[i,j].y_pos-4.5f,0 ), transform.rotation);
            }
        }

    }

}
