using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Script : MonoBehaviour
{
    public Vector3 target;
    public Vector3 minScale;
    public Vector3 maxScale;
    bool maximize;

    // Start is called before the first frame update
    void Start()
    {
        minScale = transform.localScale;
        maxScale = minScale + new Vector3(0.2f,0.2f,0.2f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((target-transform.position).magnitude>0.01f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target-transform.position );
            transform.position = Vector3.MoveTowards(transform.position, target , 100f*Time.fixedDeltaTime );
        }
        else
        {
            if(transform.localScale== minScale)
            {
                maximize = true;
            }
            if(transform.localScale==maxScale)
            {
                maximize = false;
            }
            if(maximize)
            {
                float nig = Mathf.Lerp(transform.localScale.x, maxScale.x, Time.fixedDeltaTime *10f);
                transform.localScale = new Vector3(nig,nig,nig);
            }
            else
            {
                float nig = Mathf.Lerp(transform.localScale.x, minScale.x, Time.fixedDeltaTime *10f);
                transform.localScale = new Vector3(nig,nig,nig);
            }
            

        }

    }
}
