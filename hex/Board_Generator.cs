using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Generator : MonoBehaviour
{

    public GameObject hex_tile_prefab;

    public int size;

    public Tile[,] board;

    public bool[,] connections_matrix;

    // Start is called before the first frame update
    void Start()
    {
        board = new Tile[size,size];
        float j_offset=0f;
        float i_offset = 0f;
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                GameObject o = Instantiate(hex_tile_prefab,new Vector3(j-j_offset,i - i_offset,0),transform.rotation, gameObject.transform);
                o.transform.parent = gameObject.transform;
                board[i,j] = new Tile(i*size+j,o);
            }
            j_offset += 0.5f;
            i_offset += 0.11f;
        }

        connections_matrix = new bool[size*size, size*size];


        transform.position = new Vector3(3.86f,1.77f,0f);
        transform.localScale = new Vector3(0.7f,0.7f,0.7f);
        
    }

    void Initialise_CM()
    {
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                //upper left corner
                if( i==0 && j==0)
                {
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
