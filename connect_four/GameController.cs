/*
    This script essentially acts as the game handler during the actual connect four gameplay scene, 
    initiallizing the board and controlling which player has an active turn.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject cell_object;
    public Board_Grid game_board;

    public GameObject player_mark;
    public GameObject computer_mark;

    public bool is_dropping;

    Cell_status who_plays =Cell_status.Player;

    public Buttons[] buttons;

    public GameObject canvas;

    public bool CPU_is_thinking;

    public GameObject CPU_won;
    public GameObject player_won;
    public GameObject Red_won;
    public GameObject Yellow_won;

    public TMP_Text whos_turn_txt;

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
        Debug.Log(MenuScript.MONTE_NUMBER);
        Create_Board(6,7);

        for(int i=0;i<buttons.Length;i++)
        {
            buttons[i].isActive = true;
            buttons[i].reachedMax = false;
        }

        canvas.SetActive(true);

    }

    void Update()
    {
        
        if(who_plays== Cell_status.Player && !is_dropping)
        {
            Check_who_won();
            if(!MenuScript.PVP_mode && whos_turn_txt.text!="")
            {
                whos_turn_txt.text = "Your Turn!";
            }
            else if(whos_turn_txt.text!="")
            {
                whos_turn_txt.text = "Red's Turn!";
            }
            is_dropping = true;
            foreach(Buttons but in buttons)
            {
                if(!but.reachedMax)
                    but.button.SetActive(true);
            }
        }
        else if(who_plays== Cell_status.Computer && !is_dropping && !CPU_is_thinking)
        {
            Check_who_won();
            if(!MenuScript.PVP_mode)
            {   
                if(whos_turn_txt.text!="")
                    whos_turn_txt.text = "CPU's turn";
                CPU_is_thinking = true;
                StartCoroutine(Computer_Played());
            }
            else
            {
                if(whos_turn_txt.text!="")
                    whos_turn_txt.text = "Yellow's Turn!";
                is_dropping = true;
                foreach(Buttons but in buttons)
                {
                    if(!but.reachedMax)
                        but.button.SetActive(true);
                }

            }
        }
        

    }
//use to check for victory after the mark reaches its destination 

    private void Check_who_won()
    {
        if(!MenuScript.PVP_mode)
        {
            if(game_board.Check_for_Victory(Cell_status.Computer))
            {
                whos_turn_txt.text="";
                Debug.Log("computer wins");
                CPU_won.SetActive(true);
                Time.timeScale =0;
                canvas.SetActive(false);
            }
            else if(game_board.Check_for_Victory(Cell_status.Player))
            {
                whos_turn_txt.text="";
                Debug.Log("player wins");
                player_won.SetActive(true);
                Time.timeScale=0;
                canvas.SetActive(false);
            }
        }
        else
        {
            if(game_board.Check_for_Victory(Cell_status.Computer))
            {
                whos_turn_txt.text="";
                Debug.Log("Yellow wins");
                Yellow_won.SetActive(true);
                Time.timeScale =0;
                canvas.SetActive(false);  
            }
            else if(game_board.Check_for_Victory(Cell_status.Player))
            {
                whos_turn_txt.text="";
                Debug.Log("Red wins");
                Red_won.SetActive(true);
                Time.timeScale=0;
                canvas.SetActive(false);    
            }

        }


    }

    public void Restart_Game()
    {
        Time.timeScale =1;
        SceneManager.LoadScene(1);
    }
    public void Exit_Menu()
    {
        Time.timeScale =1;
        SceneManager.LoadScene(0);
    }

    //use for the computer's turn
    IEnumerator Computer_Played()
    {
        is_dropping = true;
        game_board.AI_Plays(Cell_status.Computer, MenuScript.MONTE_NUMBER);
        yield return new WaitForSeconds(MenuScript.MONTE_NUMBER/10000f);
        Instantiate(computer_mark, new Vector3(game_board.last_played_position.x,game_board.last_played_position.y+8f,0f ) ,transform.rotation);
            
        Debug.Log("comp plays");
        who_plays = Cell_status.Player;
        CPU_is_thinking = false;
        yield return null;
    }
    //use for player's turn
    public void Player_Played(int col)
    {
        if(who_plays==Cell_status.Player)
        {
            buttons[col].reachedMax = game_board.Who_Plays_And_Return_If_Full(Cell_status.Player, col);
            Instantiate(player_mark, new Vector3(game_board.last_played_position.x,game_board.last_played_position.y+8f,0f ) ,transform.rotation);
            Debug.Log("Player 1 played");
            for(int i=0;i<buttons.Length;i++)
            {
                buttons[i].isActive = false;
                buttons[i].button.SetActive(false);
            }
            who_plays=Cell_status.Computer;
        }
        else
        {
            buttons[col].reachedMax = game_board.Who_Plays_And_Return_If_Full(Cell_status.Computer, col);
            Instantiate(computer_mark, new Vector3(game_board.last_played_position.x,game_board.last_played_position.y+8f,0f ) ,transform.rotation);
            Debug.Log("Player 2 played");
            for(int i=0;i<buttons.Length;i++)
            {
                buttons[i].isActive = false;
                buttons[i].button.SetActive(false);
            }
            who_plays=Cell_status.Player;
        }
        
    }

     //use to initialize the game
    public void Create_Board(int N,int M)
    {
        game_board = new Board_Grid(N,M);
        
        for(int i =0;i<N;i++)
        {
            for(int j=0;j<M;j++)
            {
                GameObject obj = Instantiate(cell_object, new Vector3((float)game_board.board[i,j].y_pos-Board_Grid.y_offset ,(float)game_board.board[i,j].x_pos-Board_Grid.x_offset ,0 ), transform.rotation);
                game_board.Set_Object_to_Cell( i , j, obj );

            }
        }

    }

}
