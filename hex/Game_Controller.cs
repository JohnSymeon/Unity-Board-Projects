using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    Board_Generator BG;

    Status whos_turn;
    bool intermediate_state;

    AI ai;

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        BG = GetComponent<Board_Generator>();
        whos_turn = Status.Blue;
        ai = GetComponent<AI>();
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
    }

    void Player_Turn()
    {
        canvas.SetActive(true);
        //activate player buttons
    }

    void Computer_Turn()
    {
        canvas.SetActive(false);
        int ai_play = ai.AI_Chooses(whos_turn);
        PlacedTile(ai_play);
        BG.board[ai_play/BG.size,ai_play%BG.size].go.GetComponent<Hex_Generator>().CloseButton();
        //activate computer buttons
    }

    public void PlacedTile(int id)
    {
        BG.Place_tile(id, whos_turn);
        intermediate_state = true;
    }

}
