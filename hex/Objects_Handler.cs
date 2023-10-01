using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects_Handler : MonoBehaviour
{
    public GameObject player_station_1;
    public GameObject player_station_2;
    public GameObject alien_station_1;
    public GameObject alien_station_2;
    public Board_Generator BG;

    public float w;
    public float t;

    public GameObject patrol_position_go_1;
    private Vector3 PP1;
    private Vector3 PP2;
    private bool reached_PP2;

    public void Idle_Turret(GameObject turret)
    {
        turret.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(Mathf.Cos(w*t),Mathf.Sin(w*t),0f ) );
    }

    public void Idle_Station(GameObject station)
    {
        if(!reached_PP2 && station.transform.position!=PP2)
        {
            station.transform.position = Vector3.MoveTowards(station.transform.position, PP2 , 20*Time.fixedDeltaTime );
        }
        else if(station.transform.position== PP2)
        {
            reached_PP2 = true;
        }
        else if(reached_PP2 && station.transform.position!=PP1)
        {
            station.transform.position = Vector3.MoveTowards(station.transform.position, PP1 , 20*Time.fixedDeltaTime );
        }
        else if(station.transform.position== PP1)
        {
            reached_PP2 = false;
        }
        
    }

    void Start()
    {
        PP1 = player_station_1.transform.position;
        PP2 = patrol_position_go_1.transform.position;
        w=1f;
        t=0f;
        BG = GetComponent<Board_Generator>();
        
    }

    void FixedUpdate()
    {
        t+=Time.fixedDeltaTime;
        Idle_Turret(player_station_1.transform.GetChild(0).gameObject);
        Idle_Station(player_station_1);
        
    }
}
