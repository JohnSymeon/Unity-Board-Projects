using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Script : MonoBehaviour
{
    public Vector3 target;
    public Vector3 minScale;
    public Vector3 maxScale;
    bool maximize;
    bool rotated;
    bool entered_coroutine;

    string my_tag;

    // Start is called before the first frame update
    void Start()
    {
        minScale = transform.localScale;
        maxScale = minScale + new Vector3(0.2f,0.2f,0.2f);
        my_tag = gameObject.tag;
        gameObject.tag =  "Untagged";
    }

    void Go_And_Idle()
    {
        if((target-transform.position).magnitude>0.01f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target-transform.position );
            transform.position = Vector3.MoveTowards(transform.position, target , 100f*Time.fixedDeltaTime );
        }
        else
        {
            gameObject.tag = my_tag;
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

    bool Search_And_Rotate()
    {
        string tag_to_be_found;

        if(gameObject.tag=="player_fighters")
        {
            tag_to_be_found = "alien_fighters";
        }
        else
        {
            tag_to_be_found = "player_fighters";
        }

        var list = GameObject.FindGameObjectsWithTag(tag_to_be_found);
        foreach(var obj in list)
        {
            if((obj.transform.position - transform.position).magnitude<1.5f)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward,obj.transform.position - transform.position);
                return true;
            }
        }
        return false;
    }

    IEnumerator Start_Searching()
    {
        entered_coroutine = true;
        if(!rotated)
        {
            rotated = Search_And_Rotate();
        }
        yield return new WaitForSeconds(5f);
        entered_coroutine = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Go_And_Idle();

        if(!entered_coroutine)
        {
            StartCoroutine(Start_Searching());
        }

    }
}
