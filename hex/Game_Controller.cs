using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Game_Controller : MonoBehaviour
{
    Board_Generator BG;

    Status whos_turn;
    bool intermediate_state;

    AI ai;

    public GameObject canvas;

    public bool allow_check_in_intermidiate;

    Objects_Handler OH;

    // Start is called before the first frame update
    void Start()
    {
        BG = GetComponent<Board_Generator>();
        whos_turn = Status.Blue;
        ai = GetComponent<AI>();
        OH = GetComponent<Objects_Handler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(intermediate_state)
        {
            Intermediate_State();
        }
        else if(whos_turn == Status.Blue)
        {
            Player_Turn();
        }
        else
        {
            Computer_Turn();
        }
    }

    void Intermediate_State()
    {
        Deactivate_All_Buttons(true);
        canvas.SetActive(false);
        if(allow_check_in_intermidiate)
        {
            if(BG.Check_for_Victory(BG.board, whos_turn))
            {
                Debug.Log(whos_turn);
                Debug.Log("has won");
            }
            else
                Debug.Log("no victor, game continues..");
            
            if(whos_turn==Status.Blue)
                whos_turn = Status.Red;
            else
                whos_turn = Status.Blue;
            intermediate_state = false;
            allow_check_in_intermidiate = false;
        }
        
    }

    void Player_Turn()
    {
        Deactivate_All_Buttons(false);
        canvas.SetActive(true);
        //activate player buttons
    }

    bool thinking = false;
    int ai_play = -1;
    void Computer_Turn()
    {   

        if(!thinking)
            Cpu_plays();
        if(ai_play!=-1)
        {
            PlacedTile(ai_play);
            BG.board[ai_play/BG.size,ai_play%BG.size].go.GetComponent<Hex_Generator>().CloseButton();
            thinking = false;
            ai_play = -1;
        }
        //activate computer buttons
    }
    async void Cpu_plays()
    {
        thinking = true;
        await Task.Run(()=>{
            ai_play = ai.AI_Chooses(whos_turn);
        });

    }

    public void PlacedTile(int id)
    {
        Debug.Log("placetile");
        OH.Order_turret_to_shoot(whos_turn, BG.board[id/BG.size, id%BG.size].go.transform.position);
        //StartCoroutine(OH.turret_1.GetComponent<Turret_Script>().Lock_Shoot(BG.board[id/BG.size, id%BG.size].go.transform.position)) ;
        BG.Place_tile(id, whos_turn);
        intermediate_state = true;
    }

    public void Deactivate_All_Buttons(bool set)
    {
        for(int i=0;i<BG.size*BG.size;i++)
        {
            if(BG.board[i/BG.size,i%BG.size].status==Status.Neutral)
                BG.board[i/BG.size,i%BG.size].go.GetComponent<Hex_Generator>().DeActivate_Button_While_Thinking(set);
        }
    }

}
