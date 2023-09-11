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
            Destroy(obj.GetComponent<asteroid_guider>());
            if(Random.Range(0f,1f)>0.5f)
                rend.sortingOrder= -20;
            else
            {
                rend.sortingOrder= 10;
            }
        }
    }

    public void Spawn_Roid(int x, int y)
    {
        float x_spawn;
        float y_spawn;
        float prob = Random.Range(0f,1f);
        if(prob<0.25f)
        {
            y_spawn = 9f;
            x_spawn = Random.Range(-5f,11f);
        }
        else if(prob<0.5f)
        {
            x_spawn = 13f;
            y_spawn =  Random.Range(-2f,7.5f);
        }
        else if(prob<0.75)
        {
            y_spawn = -4.4f;
            x_spawn = Random.Range(-5f,11f);
        }
        else
        {
            x_spawn = -6f;
            y_spawn =  Random.Range(-2f,7.5f);
        }

        var asteroid = Instantiate(asteroids[Random.Range(0,asteroids.GetLength(0))], new Vector3(x_spawn,y_spawn,0f),transform.rotation);
        asteroid.GetComponent<asteroid_guider>().target_pos = new Vector3(x,y,0f);
        Destroy(asteroid.GetComponent<asteroid_BG>());
        asteroid.GetComponent<SpriteRenderer>().sortingOrder= 10;
    }

}
