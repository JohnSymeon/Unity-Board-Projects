using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Script : MonoBehaviour
{

    public float w;
    public float t;

    public GameObject bullet;

    public bool idle;

    public Vector3 turret_rotation;
    public Vector3 Target;

    public void Idle_Turret()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(Mathf.Cos(w*t),Mathf.Sin(w*t),0f ) );
    }

    public IEnumerator Lock_Shoot( Vector3 target)
    {
        Target = target;
        idle = false;
        //turret.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
        yield return new WaitForSeconds(1f);
        var b = Instantiate(bullet, transform.position, transform.rotation);

        b.GetComponent<Bullet>().target= target;
        yield return new WaitForSeconds(0.5f);
        idle = true;

        //turret_rotation = target - transform.position;

        Vector3 vector = (target-transform.position).normalized;
        float x_variable = Mathf.Acos(vector.x)/w;
        Debug.Log(x_variable);
        t = x_variable;

    }
    // Start is called before the first frame update
    void Start()
    {
        w=1f;
        t=0f;
        idle = true;

        turret_rotation = Vector3.forward;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t+=Time.fixedDeltaTime;
        //Debug.Log(t);
        if(idle)
        {
            Idle_Turret();
        }   
        else if(!idle)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.Euler( 0,0,
                 Mathf.Rad2Deg * Mathf.Atan2((Target-transform.position).normalized.y,(Target-transform.position).normalized.x)-90f), 
                 250* Time.fixedDeltaTime);
        }
        
    }
}
