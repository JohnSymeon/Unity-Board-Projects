using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public Vector3 target;
    Game_Controller GC;
    public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {
        GC = FindObjectOfType<Game_Controller>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
        transform.position = Vector3.MoveTowards(transform.position, target , 15f*Time.fixedDeltaTime );
        if(transform.position== target)
        {
            //Send_Force.Invoke();
            FindObjectOfType<Objects_Handler>().Order_Fighter();
            GC.allow_check_in_intermidiate = true;
            Instantiate(explosion,transform.position,transform.rotation);
            Destroy(gameObject);
            
        }
    }

    //[SerializeField] public UnityEvent Send_Force;

    
}
