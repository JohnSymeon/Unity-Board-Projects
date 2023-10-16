using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conflict : MonoBehaviour
{
    public GameObject explosion;
    bool allow_explosion;
    bool allow_coroutine;

    void Start()
    {
        allow_explosion = true;
        StartCoroutine(Wait_to_Start_Coroutine());
    }

    void FixedUpdate()
    {
        if(allow_explosion && allow_coroutine)
        {
            StartCoroutine(Create_explosion());
        }
        
    }
    IEnumerator Wait_to_Start_Coroutine()
    {
        yield return new WaitForSeconds(5f);
        allow_coroutine = true;
    }

    IEnumerator Create_explosion()
    {
        allow_explosion = false;

        float offset = Random.Range(-0.5f,0.5f);

        var go = Instantiate(explosion,transform.position + new Vector3(offset,offset,offset),transform.rotation);
        float seconds_wait = Random.Range(5f,10f);
        yield return new WaitForSeconds(seconds_wait);
        allow_explosion = true;
        yield return null;
    }
}
