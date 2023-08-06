using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_handler : MonoBehaviour
{
    public GameObject[] asteroids;

    void Start()
    {
        Create_Asteroids(6);
    }

    private void Create_Asteroids(int num)
    {
        for(int i=0;i<num;i++)
        {
            var obj = Instantiate(asteroids[Random.Range(0,asteroids.GetLength(0))], new Vector3(Random.Range(-5f,12f), Random.Range(-2.5f,8.3f),0f),  Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            var rend = obj.GetComponent<SpriteRenderer>();
            if(Random.Range(0f,1f)>0.5f)
                rend.sortingOrder= -20;
            else
            {
                rend.sortingOrder= 10;
            }
        }
    }

}
