/*
This script is used by each Mark when it is played, moving smoothly and updating the GameController
that it reached its destination.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    Vector3 last_played_pos;

    private float speed = 10f;

    bool has_reached_target;

    private GameController GC;
    // Start is called before the first frame update
    void Start()
    {
        GC = FindObjectOfType<GameController>();
        last_played_pos = new Vector3(transform.position.x,transform.position.y-8f,0f );
    }

    // Update is called once per frame
      void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,last_played_pos,speed*Time.deltaTime);
        
        Update_GC_On_Status();

        Check_for_Destruction();
    }

    void Check_for_Destruction()
    {
        if(GC.game_board.board[(int)transform.position.x, (int)transform.position.y].status==Cell_status.Neutral)
            Destroy(this);

        if((int)transform.position.x>1 && GC.game_board.board[(int)transform.position.x-1, (int)transform.position.y].status==Cell_status.Neutral)
        {

            var new_row = GC.game_board.Find_bottom_of_col((int)transform.position.x,(int)transform.position.y);

            last_played_pos = new Vector3((float)new_row, transform.position.y,0f);
            has_reached_target = false;
        }
        
    }


    void Update_GC_On_Status()
    {
        if(transform.position == last_played_pos && !has_reached_target)
        {
            GC.is_dropping=false;
            has_reached_target = true;
        }
        else if(!has_reached_target)
            GC.is_dropping=true;
    }
}
