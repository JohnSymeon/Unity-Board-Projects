/*
This script is used by each Mark when it is played, moving smoothly and updating the GameController
that it reached its destination.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mark : MonoBehaviour
{
    Vector3 last_played_pos;

    private float speed = 10f;

    bool has_reached_target;

    public Rigidbody2D rb;

    private GameController GC;

    private bool allow_to_status;

    public GameObject death_effect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GC = FindObjectOfType<GameController>();
        last_played_pos = new Vector3(transform.position.x,transform.position.y-8f,0f );

        StartCoroutine(Delay(0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position,last_played_pos,speed*Time.deltaTime);
        
        //if(rb.velocity.y>0f)
            //StartCoroutine(Delay(0.5f));

        if(allow_to_status)
            Update_GC_On_Status();
        Check_Destroy();
       // Check_for_Destruction();
    }

    void Check_Destroy()
    {
        if( transform.position.y<5f && GC.game_board.board[(int)Math.Round(transform.position.y,0), (int)Math.Round(transform.position.x,0)].kill_switch)
        {
            Debug.Log("entered check_destroy");
            GC.game_board.board[(int)Math.Round(transform.position.y,0), (int)Math.Round(transform.position.x,0)].kill_switch = false;
            Instantiate(death_effect,transform.position,transform.rotation);
            Destroy(gameObject);
        }
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

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        allow_to_status = true;
    }

    void Update_GC_On_Status()
    {
        if(rb.velocity.y==0f)
        {
            GC.is_dropping=false;
            Debug.Log("reached bot");
            allow_to_status = false;
        }
        else
            GC.is_dropping=true;
    }
}
