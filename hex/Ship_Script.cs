using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Script : MonoBehaviour
{
    GameObject station;
    public GameObject patrol_position_go_1;
    private Vector3 PP1;
    private Vector3 PP2;
    public bool reached_PP2;
    float t=0f;

    public float angle_a;
    public float angle_b;

    public void Idle_Station()
    {
        if(!reached_PP2 && station.transform.position!=PP2)
        {
            station.transform.position = Vector3.MoveTowards(station.transform.position, PP2 , 0.5f*Time.fixedDeltaTime );
            station.transform.rotation = Quaternion.RotateTowards(station.transform.rotation, Quaternion.Euler(0f,0f, angle_a ), 100* Time.fixedDeltaTime);
            
        }
        else if(station.transform.position== PP2)
        {
            reached_PP2 = true;
        }
        if(reached_PP2 && station.transform.position!=PP1)
        {
            station.transform.position = Vector3.MoveTowards(station.transform.position, PP1 , 0.5f*Time.fixedDeltaTime );

            station.transform.rotation = Quaternion.RotateTowards(station.transform.rotation, Quaternion.Euler(0f,0f, angle_b ), 100* Time.fixedDeltaTime);
            //station.transform.rotation = Quaternion.LookRotation(Vector3.forward, PP2-PP1);
        }
        else if(station.transform.position== PP1)
        {
            reached_PP2 = false;
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        PP1 = transform.position;
        PP2 = patrol_position_go_1.transform.position;
        t=0f;
        station = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t+=Time.fixedDeltaTime;

    }
}
