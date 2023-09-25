using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{

    Status whos_turn;
    bool intermediate_state;

    // Start is called before the first frame update
    void Start()
    {
        whos_turn = Status.Blue;
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
        Debug.Log(whos_turn);
        if(whos_turn==Status.Blue)
            whos_turn = Status.Red;
        else
            whos_turn = Status.Blue;
        intermediate_state = false;
    }

    void Player_Turn()
    {
        //activate player buttons
    }

    void Computer_Turn()
    {
        //activate computer buttons
    }

    public void OnButtonClick()
    {
        //button logic
    }

}
