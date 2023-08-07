/*
The script of asteroids as background elements
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid_BG : MonoBehaviour
{
    private Vector3 pos;

    private float offsety, offsetx;


    void Start()
    {
        offsety = Random.Range(0f, 10f);
        offsetx = Random.Range(0f, 10f);
        pos = transform.position;

    }


    void Update()
    {
        transform.position = pos + new Vector3(Mathf.Sin(0.1f*Time.time + offsetx), Mathf.Sin(0.1f * Time.time + offsety), 0);
        //transform.Translate(Vector3.forward * 10 * Time.fixedDeltaTime);
    }

}
