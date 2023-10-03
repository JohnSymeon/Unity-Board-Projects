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
    public bool reached_PP2;

    public GameObject bullet;

    public bool idle;

    public Vector3 turret_rotation;
    public Vector3 Target;

    public void Idle_Turret(GameObject turret)
    {
        turret.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(Mathf.Cos(w*t),Mathf.Sin(w*t),0f ) );
    }

    public IEnumerator Lock_Shoot(GameObject turret, Vector3 target, Status who)
    {
        Target = target;
        idle = false;
        //turret.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
        yield return new WaitForSeconds(1f);
        var b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);

        b.GetComponent<Bullet>().target= target;
        yield return new WaitForSeconds(0.5f);
        idle = true;

        //turret_rotation = target - transform.position;

        Vector3 vector = (target-turret.transform.position).normalized;
        Debug.Log(vector);
        float x_variable = Mathf.Acos(vector.x)/w;
        Debug.Log(x_variable);
        var y_variable = Mathf.Asin(vector.y)/w;
        t = x_variable;

    }

    public void Idle_Station(GameObject station)
    {
        if(!reached_PP2 && station.transform.position!=PP2)
        {
            station.transform.position = Vector3.MoveTowards(station.transform.position, PP2 , 0.5f*Time.fixedDeltaTime );
            station.transform.rotation = Quaternion.RotateTowards(station.transform.rotation, Quaternion.Euler(0f,0f, -160f ), 100* Time.fixedDeltaTime);
            
        }
        else if(station.transform.position== PP2)
        {
            reached_PP2 = true;
        }
        if(reached_PP2 && station.transform.position!=PP1)
        {
            station.transform.position = Vector3.MoveTowards(station.transform.position, PP1 , 0.5f*Time.fixedDeltaTime );

            station.transform.rotation = Quaternion.RotateTowards(station.transform.rotation, Quaternion.Euler(0f,0f, 20f ), 100* Time.fixedDeltaTime);
            //station.transform.rotation = Quaternion.LookRotation(Vector3.forward, PP2-PP1);
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
        idle = true;

        turret_rotation = Vector3.forward;
        
    }

    void FixedUpdate()
    {
        t+=Time.fixedDeltaTime;
        //Debug.Log(t);
        if(idle)
        {
            Idle_Turret(player_station_1.transform.GetChild(0).gameObject);
        }   
        else if(!idle)
        {
            player_station_1.transform.GetChild(0).gameObject.transform.rotation = Quaternion.RotateTowards(
                player_station_1.transform.GetChild(0).gameObject.transform.rotation, Quaternion.Euler( 0,0,
                 Mathf.Rad2Deg * Mathf.Atan2((Target-player_station_1.transform.GetChild(0).gameObject.transform.position).normalized.y,(Target-player_station_1.transform.GetChild(0).gameObject.transform.position).normalized.x)-90f), 
                 250* Time.fixedDeltaTime);
        }
        Idle_Station(player_station_1);
        
    }
}
