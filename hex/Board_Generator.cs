using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Generator : MonoBehaviour
{

    public GameObject hex_tile_prefab;

    public int size;

    public Tile[,] board;

    public bool[,] connections_matrix;

    public GameObject test_ids;

    public GameObject Red_Line;
    public GameObject Blue_Line;
    public GameObject Conflict_Line;

    public bool[,] line_matrix;
    

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
                GameObject o = Instantiate(hex_tile_prefab,new Vector3(j+j_offset,-i + i_offset,0),transform.rotation, gameObject.transform);
                o.transform.parent = gameObject.transform;
                board[i,j] = new Tile(i*size+j,o);
                o.GetComponent<Hex_Generator>().id = i*size+j;     
            }
            j_offset += 0.5f;
            i_offset += 0.11f;
        }

        connections_matrix = new bool[size*size, size*size];
        Initialise_CM();
        line_matrix = new bool[size*size, size*size];

        transform.position = new Vector3(1.5f,3.38f,0f);
        transform.localScale = new Vector3(0.9423107f,0.9423107f,0.9423107f);
        
    }

    public void Place_tile(int id_played, Status who_played)
    {
        board[id_played/size, id_played%size].Change_Status(who_played);
    }

    public void Check_Connect_Lines()
    {
        
        for(int i=0;i<size*size;i++)
        {
            for(int j=0;j<size*size;j++)
            {
                if(connections_matrix[i,j] && !line_matrix[i,j] && board[i/size,i%size].status!=Status.Neutral && board[j/size,j%size].status!=Status.Neutral )
                {
                    line_matrix[i,j] = true;
                    Vector3 vec = board[i/size,i%size].go.transform.position -  board[j/size,j%size].go.transform.position;
                    Vector3 offset = board[i/size,i%size].go.transform.position;
                    Create_Line(i,j,vec,offset);
                }
            }
        }
    }

    void Create_Line(int i, int j, Vector3 vec, Vector3 offset)
    {
        if( board[i/size,i%size].status==Status.Red && board[j/size,j%size].status==Status.Red )
        {
            Instantiate(Red_Line,offset- vec, Quaternion.Euler( 0,0,Mathf.Rad2Deg * Mathf.Atan2(vec.normalized.y,vec.normalized.x)));
        }
        else if(board[i/size,i%size].status==Status.Blue && board[j/size,j%size].status==Status.Blue)
        {
            Instantiate(Blue_Line,offset- vec, Quaternion.Euler( 0,0,Mathf.Rad2Deg * Mathf.Atan2(vec.normalized.y,vec.normalized.x)));
        }
        else
        {
            Instantiate(Conflict_Line,offset- vec, Quaternion.Euler( 0,0,Mathf.Rad2Deg * Mathf.Atan2(vec.normalized.y,vec.normalized.x)));
        }
    }


    TextMesh[,] arr;
    public void test_BoardtoWorld()
    {
        if(arr==null)
        {
            arr = new TextMesh[size,size];
            
        }
            
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                Color color;
                string text;

                text = board[i,j].id.ToString();
                color =Color.yellow;
                if(board[i,j].status== Status.Blue)
                {
                    color =Color.blue;
                }
                else if(board[i,j].status== Status.Red)
                {
                    color = Color.red;
                }

                if(arr[i,j]==null)
                    arr[i,j] = CreateWorldText(text,color, TextAnchor.MiddleCenter, TextAlignment.Center,board[i,j].go.transform.position,100,test_ids.transform);
                else
                {
                    arr[i,j].text = text;
                    arr[i,j].color = color;
                }
                    
            }
        }
       
    }

    public TextMesh CreateWorldText( string text, Color color, TextAnchor textAnchor, TextAlignment textAlignment,Vector3  localPosition , int sortingOrder,Transform parent = null, int fontSize = 50)
    {
        GameObject gameObject = new GameObject("world_text",typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent,true);
        transform.localPosition = localPosition;
        transform.eulerAngles = new Vector3(0,0,0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text= text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        gameObject.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;

    }

    void Initialise_CM()
    {
        // upper left = 0,0
        for(int j=0;j<size;j++)
        {
            for(int i=0;i<size;i++)
            {
                //upper left corner//LOWER LEFT
                if( i==0 && j==0)
                {
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==size-1 && j==size-1)//lower right corner//UPPER RIGHT
                {
                    connections_matrix[board[i,j].id, i*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                }
                else if(i==0 && j == size-1)//upper right corner
                {
                    connections_matrix[board[i,j].id, i*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==size-1 && j ==0)//lower left corner
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                }
                else if(j==0)//left column
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                }
                else if(j==size-1)//right column
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==0)//upper row
                {
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==size-1)//lower row
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j+1] = true;
                }
                else if(i>0 && j>0 && (i<size-1) && (j<size-1))//rest
                {
                    connections_matrix[board[i,j].id, (i)*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                }
            }
        }
    }


    public bool Check_for_Victory( Tile[,] test_board , Status who_played )
    {
        bool[] reached_nodes = new bool[size*size];

        for(int j=0;j<size;j++)
        {
            if((test_board[0,j].status== Status.Red && who_played==Status.Red )
            || (test_board[j,0].status== Status.Blue && who_played==Status.Blue ) )
            {
                
                bool found_reached=true;

                while(found_reached)
                {               
                    found_reached=false;                 
                    if(who_played==Status.Red)
                        reached_nodes[j] = true;
                    else
                        reached_nodes[j*size] = true;
                    
                    for(int k=0;k<size*size;k++)
                    {
                        for(int w=0; w<size*size;w++)
                        {
                            if(test_board[k/size,k%size].status == who_played 
                            && connections_matrix[k,w] 
                            && test_board[w/size, w%size].status == who_played
                            && reached_nodes[w] && !reached_nodes[k])
                            {
                                reached_nodes[k] = true;
                                found_reached = true; 
                            }
                        }
                    }
                }
            }
        }

        if(who_played == Status.Red)
        {
            for(int k=size*size-size;k<size*size;k++)
            {
                if(reached_nodes[k])
                    return true;
            }
        }
        else if(who_played == Status.Blue)
        {
            for(int k= size-1; k<size*size; k=k+size)
            {
                if(reached_nodes[k])
                    return true;
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //test_BoardtoWorld();
    }
}
