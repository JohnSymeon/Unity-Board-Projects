using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid_guider : MonoBehaviour
{
    public Vector3 target_pos;
    float step;
    
    void Start()
    {
        step = Mathf.Abs(Vector3.Distance(target_pos, transform.position));
        Destroy(gameObject,1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target_pos, Time.deltaTime * step);
    }
}
