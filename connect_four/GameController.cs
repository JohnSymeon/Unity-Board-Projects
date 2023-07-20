using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public GameObject cell_object;
    public Board_Grid game_board;

    Cell_status who_plays =Cell_status.Player;

    public Buttons[] buttons;

    [Serializable]
    public struct Buttons
    {
        [SerializeField]
        public GameObject button;
        public bool reachedMax;
        public bool isActive;
    }

    void Start()
    {
        Create_Board(10);

        for(int i=0;i<buttons.Length;i++)
        {
            buttons[i].isActive = true;
            buttons[i].reachedMax = false;
        }
    }

    void Update()
    {

        if(who_plays== Cell_status.Player)
        {

            foreach(Buttons but in buttons)
            {
                if(!but.reachedMax)
                    but.button.SetActive(true);
            }

        }
        else
        {
            Debug.Log("comp plays");
            who_plays = Cell_status.Player;
        }

    }

    public void Player_Played(int col)
    {
        buttons[col].reachedMax = game_board.Who_Plays_And_Return_If_Full(Cell_status.Player, col);

        game_board.Check_for_Victory(Cell_status.Player);

        Debug.Log("Player played");

        for(int i=0;i<buttons.Length;i++)
        {
            buttons[i].isActive = false;
            buttons[i].button.SetActive(false);
        }
            

        who_plays=Cell_status.Computer;
    }

    public void Create_Board(int N)
    {
        game_board = new Board_Grid(N,N);
        
        for(int i =0;i<N;i++)
        {
            for(int j=0;j<N;j++)
            {
                GameObject obj = Instantiate(cell_object, new Vector3( (float)game_board.board[i,j].x_pos-5f, (float)game_board.board[i,j].y_pos-4.5f,0 ), transform.rotation);
                game_board.Set_Object_to_Cell( i , j, obj );

            }
        }

    }

}
