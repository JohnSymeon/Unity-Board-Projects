using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Script : MonoBehaviour
{
    public Vector3 target;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((target-transform.position).magnitude>0.01f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target-transform.position );
            transform.position = Vector3.MoveTowards(transform.position, target , 100f*Time.fixedDeltaTime );
        }

    }
}
