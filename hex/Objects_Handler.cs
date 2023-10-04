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

    public GameObject turret_1;
    public GameObject turret_2;

    public bool idle_turret_1;

    public void Order_turret_to_shoot(Status who, Vector3 target )
    {
        if(who == Status.Blue)
        {
            StartCoroutine(turret_1.GetComponent<Turret_Script>().Lock_Shoot(target));
        }
        else
        {
            StartCoroutine(turret_2.GetComponent<Turret_Script>().Lock_Shoot(target));
        }
    }

    void Start()
    {
        turret_1 = player_station_1.transform.GetChild(0).gameObject;
        turret_2 = alien_station_1.transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
       player_station_1.GetComponent<Ship_Script>().Idle_Station();
       alien_station_1.GetComponent<Ship_Script>().Idle_Station();
       /*if(idle_turret_1)
       {
            turret_1.GetComponent<Turret_Script>().idle = true;
       }*/
       
       
        
    }
}
