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
