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
using UnityEngine.UI;
using System.Threading.Tasks;

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
    public GameObject pause;

    public GameObject pause_button;

    public TMP_Text whos_turn_txt;

    //[Serializable]
    public Sprite[] player_sprites;
    //[Serializable]
    public Sprite[] enemy_sprites;

    public Image UI_turn_image_obj;

    Sprite player_sprite;
    Sprite p2_or_CPU_sprite;

    public Image player_portrait;
    public Image enemy_portrait;

    public GameObject particle_enemy;
    public GameObject particle_player;

    public GameObject players_health;
    public GameObject computers_health;
    int players_HP=3;
    int computers_HP=3;

    public GameObject explosion_effect;

    bool allow_play_coroutine = true;

    Vector3 original_pos_player;
    Vector3 original_pos_enemy;

    bool shake_player;
    bool shake_enemy;


    public GameObject[] UI_player;
    public GameObject[] UI_computer;

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
        SetSprites(MenuScript.MONTE_NUMBER, MenuScript.PVP_mode);

        Debug.Log(MenuScript.MONTE_NUMBER);
        Create_Board(6,7);

        for(int i=0;i<buttons.Length;i++)
        {
            buttons[i].isActive = true;
            buttons[i].reachedMax = false;
        }

        canvas.SetActive(true);

        if(game_board.MODE_Tetris)
        {
            players_health.SetActive(true);
            computers_health.SetActive(true);
        }
        else
        {
            players_health.SetActive(false);
            computers_health.SetActive(false);
        }

        original_pos_player = player_portrait.transform.position;
        original_pos_enemy = enemy_portrait.transform.position;
    }

    void Update()
    {
        game_board.BoardtoWorld();
        if(!is_dropping)
        {
            game_board.Check_for_Victory(Cell_status.Computer);
            game_board.Check_for_Victory(Cell_status.Player);
        }
               
        if(who_plays== Cell_status.Player && !is_dropping && allow_play_coroutine)
        {
            StartCoroutine(Wait_for_asteroid(who_plays));
            //Player_Is_Playing();
        }
        else if(who_plays== Cell_status.Computer && !is_dropping && !CPU_is_thinking && allow_play_coroutine)
        {
            StartCoroutine(Wait_for_asteroid(who_plays));
        }
        
        if(game_board.explosion)
        {
            FindObjectOfType<AudioManager>().Play("kaboom"); 
            game_board.explosion = false;

            GameObject[] marks = GameObject.FindGameObjectsWithTag("Marks");
            foreach(var mark in marks)
            {
                StartCoroutine(mark.GetComponent<Mark>().Delay(0.001f));
            }

            for(int i=0;i<game_board.height;i++)
            {
                for(int j=0;j<game_board.width;j++)
                {
                    if(game_board.board[i,j].marked_for_explosion)
                    {
                        Instantiate(explosion_effect,new Vector3((float)j,(float)i,0f), transform.rotation);
                        game_board.board[i,j].marked_for_explosion = false;
                    }
                }
            }

        }

        if(game_board.MODE_Tetris)
            Mode_Tetris_Check_Who_Won();
        
        if(shake_player)
            Hurt_Shake(player_portrait.gameObject);
        else if(shake_enemy)
            Hurt_Shake(enemy_portrait.gameObject);
        
    }

    void UI_Illuminate_Portrait(Cell_status who)
    {
        GameObject[] UI_player_array;
        UI_player_array = new GameObject[20];
        int k=0;
        foreach(var element in UI_player)
        {
            foreach(Transform trans in element.transform)
            {
                UI_player_array[k] = trans.gameObject;
                k++;
            }
        }
        UI_player_array[k] = UI_player[0];
        Array.Resize( ref UI_player_array, k+1);
        GameObject[] UI_computer_array;
        UI_computer_array = new GameObject[20];
        k=0;
        foreach(var element in UI_computer)
        {
            foreach(Transform trans in element.transform)
            {
                UI_computer_array[k] = trans.gameObject;
                k++;
            }
        }
        UI_computer_array[k] = UI_computer[0];
        Array.Resize( ref UI_computer_array, k+1);

        if(who==Cell_status.Player)
        {
            foreach(var elem in UI_computer_array)
            {
                elem.GetComponent<Image>().color = new Color32(135,135,135,255);
            }
            foreach(var elem in UI_player_array)
            {
                elem.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
        }
        else
        {
            foreach(var elem in UI_player_array)
            {
                elem.GetComponent<Image>().color = new Color32(135,135,135,255);
            }
            foreach(var elem in UI_computer_array)
            {
                elem.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
        }
    }

    IEnumerator Allow_Shake(GameObject go)
    {
        go.GetComponent<Image>().color = new Color32(255,50,50,255);
        if(go == player_portrait.gameObject)
            shake_player = true;
        else
            shake_enemy = true;
        yield return new WaitForSeconds(0.5f);
        go.GetComponent<Image>().color = new Color32(255,255,255,255);
        shake_player = false;
        shake_enemy = false;
    }

    void Hurt_Shake(GameObject go)
    {
        Vector3 v3 = UnityEngine.Random.insideUnitCircle * (Time.deltaTime * 1000f);
        if(go==player_portrait.gameObject)
            go.transform.position= original_pos_player + v3;
        else
            go.transform.position= original_pos_enemy + v3;
        
    }



    IEnumerator Wait_for_asteroid(Cell_status who)
    {
        allow_play_coroutine=false;
        float prob = UnityEngine.Random.Range(0f,1f);
        if(game_board.MODE_Roids && (prob<0.08f))
        {
            yield return new WaitForSeconds(1f);
            game_board.Roids(UnityEngine.Random.Range(0,game_board.height-1),UnityEngine.Random.Range(0,game_board.width-1));
            yield return new WaitForSeconds(0.3f);
        }
        if(who==Cell_status.Player)
            Player_Is_Playing();
        else
            P2_Or_CPU_Is_Playing();
        
        yield return null;
    }

    public void Rest_Roids()
    {
        game_board.Roids(UnityEngine.Random.Range(0,game_board.height-1),UnityEngine.Random.Range(0,game_board.width-1));
    }

    private void Mode_Tetris_Check_Who_Won()
    {
        if(game_board.who_won==Cell_status.Player)
        {
            if(computers_HP>1)
            {
                Destroy(computers_health.transform.GetChild(0).gameObject);
                computers_HP--;
                StartCoroutine(Allow_Shake(enemy_portrait.gameObject));
            }
            else
            {
                Destroy(computers_health.transform.GetChild(0).gameObject);
                UI_turn_image_obj.transform.parent.gameObject.SetActive(false);
                whos_turn_txt.text="";
                Debug.Log("Red wins");
                Red_won.SetActive(true);
                Time.timeScale=0;
                canvas.SetActive(false);
                FindObjectOfType<AudioManager>().Play("won"); 
            }
            game_board.who_won = Cell_status.Neutral;
        }
        else if(game_board.who_won==Cell_status.Computer)
        {
            if(players_HP>1)
            {
                Destroy(players_health.transform.GetChild(0).gameObject);
                players_HP--;
                StartCoroutine(Allow_Shake(player_portrait.gameObject));
            }
            else
            {
                Destroy(computers_health.transform.GetChild(0).gameObject);
                UI_turn_image_obj.transform.parent.gameObject.SetActive(false);
                whos_turn_txt.text="";
                Debug.Log("Yellow wins");
                Yellow_won.SetActive(true);
                Time.timeScale=0;
                canvas.SetActive(false);
                FindObjectOfType<AudioManager>().Play("won"); 
            }
            game_board.who_won = Cell_status.Neutral;
            
        }

    }

    private void SetSprites(int N, bool PVP)
    {
        player_sprite = player_sprites[0];
        if(PVP)
        {
            p2_or_CPU_sprite = player_sprites[1];
        }
        else
        {
            if(N==50)
                p2_or_CPU_sprite = enemy_sprites[0];
            else if(N==250)
                p2_or_CPU_sprite = enemy_sprites[1];
            else if(N==1000)
                p2_or_CPU_sprite = enemy_sprites[2];
            else if(N==10000)
                p2_or_CPU_sprite = enemy_sprites[3];
            else if(N==100000)
                p2_or_CPU_sprite = enemy_sprites[4];
        }

        player_portrait.sprite = player_sprite;
        enemy_portrait.sprite = p2_or_CPU_sprite;

    }


    private void P2_Or_CPU_Is_Playing()
    {
        UI_Illuminate_Portrait(Cell_status.Computer);
        if(!game_board.MODE_Tetris)
            Check_who_won();
        if(!MenuScript.PVP_mode)
        {   
            if(whos_turn_txt.text!="")
                whos_turn_txt.text = "      turn";
            CPU_is_thinking = true;
            //StartCoroutine(Computer_Played());
            Computer_Played_async();
        }
        else
        {
            if(whos_turn_txt.text!="")
                whos_turn_txt.text = "      Turn!";
            is_dropping = true;
            foreach(Buttons but in buttons)
            {
                if(!but.reachedMax)
                    but.button.SetActive(true);
            }

        }
        particle_player.SetActive(false);
        particle_enemy.SetActive(true);

    }

    private void Player_Is_Playing()
    {
        
        UI_Illuminate_Portrait(Cell_status.Player);

        if(!game_board.MODE_Tetris)
            Check_who_won();
        if(!MenuScript.PVP_mode && whos_turn_txt.text!="")
        {
            whos_turn_txt.text = "      Turn!";
        }
        else if(whos_turn_txt.text!="")
        {
            whos_turn_txt.text = "      Turn!";
        }
        is_dropping = true;
        foreach(Buttons but in buttons)
        {
            if(!but.reachedMax)
                but.button.SetActive(true);
        }

        UI_turn_image_obj.sprite = player_sprite;
        particle_player.SetActive(true);
        particle_enemy.SetActive(false);
    }

    private void Check_who_won()
    {
        game_board.BoardtoWorld();

        UI_turn_image_obj.sprite = p2_or_CPU_sprite;
        if(!MenuScript.PVP_mode)
        {
            if(game_board.Check_for_Victory(Cell_status.Computer))
            {
                UI_turn_image_obj.transform.parent.gameObject.SetActive(false);
                whos_turn_txt.text="";
                Debug.Log("computer wins");
                CPU_won.SetActive(true);
                Time.timeScale =0;
                canvas.SetActive(false);
                FindObjectOfType<AudioManager>().Play("lost");
                
            }
            else if(game_board.Check_for_Victory(Cell_status.Player))
            {
                UI_turn_image_obj.transform.parent.gameObject.SetActive(false);
                whos_turn_txt.text="";
                Debug.Log("player wins");
                player_won.SetActive(true);
                Time.timeScale=0;
                canvas.SetActive(false);
                FindObjectOfType<AudioManager>().Play("won");
            }
        }
        else
        {
            if(game_board.Check_for_Victory(Cell_status.Computer))
            {
                UI_turn_image_obj.transform.parent.gameObject.SetActive(false);
                whos_turn_txt.text="";
                Debug.Log("Yellow wins");
                Yellow_won.SetActive(true);
                Time.timeScale =0;
                canvas.SetActive(false);
                FindObjectOfType<AudioManager>().Play("won");
            }
            else if(game_board.Check_for_Victory(Cell_status.Player))
            {
                UI_turn_image_obj.transform.parent.gameObject.SetActive(false);
                whos_turn_txt.text="";
                Debug.Log("Red wins");
                Red_won.SetActive(true);
                Time.timeScale=0;
                canvas.SetActive(false);
                FindObjectOfType<AudioManager>().Play("won"); 
            }

        }


    }
    public void Pause_game()
    {
        pause.SetActive(true);
        Time.timeScale=0;
        canvas.SetActive(false);
        pause_button.SetActive(false);
        FindObjectOfType<AudioManager>().Play("pause");
    }

    public void Continue_game()
    {
        pause.SetActive(false);
        Time.timeScale=1;
        canvas.SetActive(true);
        pause_button.SetActive(true);
        FindObjectOfType<AudioManager>().Play("pause");
    }

    public void Restart_Game()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        Time.timeScale =1;
        SceneManager.LoadScene(1);
    }
    public void Exit_Menu()
    {
        FindObjectOfType<AudioManager>().Play("UI_button");
        Time.timeScale =1;
        SceneManager.LoadScene(0);
    }

    //use for the computer's turn
    IEnumerator Computer_Played()
    {
        
        is_dropping = true;
        game_board.AI_Plays(Cell_status.Computer, MenuScript.MONTE_NUMBER);
        yield return new WaitForSeconds(MenuScript.MONTE_NUMBER/10000f);
        Instantiate(computer_mark, new Vector3(game_board.last_played_position.x,7f,0f ) ,transform.rotation);
        FindObjectOfType<AudioManager>().Play("cpu_plays"); 
        Debug.Log("comp plays");
        who_plays = Cell_status.Player;
        CPU_is_thinking = false;
        allow_play_coroutine = true;
        //game_board.Roids(UnityEngine.Random.Range(0,game_board.height-1),UnityEngine.Random.Range(0,game_board.width-1));
        yield return null;
    }

    async void Computer_Played_async()
    {
        //var rand = new System.Random();
        //Debug.Log((float)rand.NextDouble());
        is_dropping = true;
        await Task.Run(()=>{
            //await Task.Yield();
            game_board.AI_Plays(Cell_status.Computer, MenuScript.MONTE_NUMBER);
            });
            //Debug.Log("comp plays");
            Instantiate(computer_mark, new Vector3(game_board.last_played_position.x,8f,0f ) ,transform.rotation);
            FindObjectOfType<AudioManager>().Play("cpu_plays"); 
            who_plays = Cell_status.Player;
            CPU_is_thinking = false;
            allow_play_coroutine = true;
            //await Task.Delay(5000);
            
        
    }


    //use for player's turn
    public void Player_Played(int col)
    {
        if(who_plays==Cell_status.Player)
        {
            buttons[col].reachedMax = game_board.Who_Plays_And_Return_If_Full(Cell_status.Player, col);
            Instantiate(player_mark, new Vector3(game_board.last_played_position.x,8f,0f ) ,transform.rotation);
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
            Instantiate(computer_mark, new Vector3(game_board.last_played_position.x,8f,0f ) ,transform.rotation);
            Debug.Log("Player 2 played");
            for(int i=0;i<buttons.Length;i++)
            {
                buttons[i].isActive = false;
                buttons[i].button.SetActive(false);
            }
            who_plays=Cell_status.Player;
        }
        FindObjectOfType<AudioManager>().Play("player_plays");
        allow_play_coroutine = true;
        //game_board.Roids(UnityEngine.Random.Range(0,game_board.height-1),UnityEngine.Random.Range(0,game_board.width-1));
    }

     //use to initialize the game
    public void Create_Board(int N,int M)
    {
        game_board = new Board_Grid(N,M);
        game_board.MODE_Tetris = MenuScript.MODE_Tetris;
        game_board.MODE_Roids = MenuScript.MODE_Roids;
        
        for(int i =0;i<N;i++)
        {
            for(int j=0;j<M;j++)
            {
                GameObject obj = Instantiate(cell_object, new Vector3((float)game_board.board[i,j].x_pos ,(float)game_board.board[i,j].y_pos ,0 ), transform.rotation);
                game_board.Set_Object_to_Cell( i , j, obj );

            }
        }

    }

}
