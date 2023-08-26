/*
    The connect four board class providing the board, the player's and computer's turn
    and an AI that uses the monte carlo method to find the best move by determining which
    play brings the biggest wins to losses ratio.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Board_Grid
{
    public int width;
    public int height;
    public Cell[,] board;
    public Vector3 last_played_position;
    public static float x_offset = 0f;
    public static float y_offset = 0f;

    public bool MODE_Tetris;
    public bool MODE_Roids;
    public bool explosion;
    public bool check_for_chain;

    public Cell_status who_won;



    public Board_Grid(int height, int width)
    {
        who_won = Cell_status.Neutral;
        this.width=width;
        this.height = height;
        board = new Cell[height,width];
        for(int i=0;i<board.GetLength(0);i++)
        {
            for(int j=0;j<board.GetLength(1);j++)
            {
                board[i,j] = new Cell();
                board[i,j].SetCoordinate(j,i);

            }
        }
    }
    //Used by player to play a tile
    public bool Who_Plays_And_Return_If_Full(Cell_status who, int col)
    {
        for(int i=0;i<board.GetLength(0);i++)
        {
            if(board[i,col].status==Cell_status.Neutral)
            {
                board[i,col].status = who;
                //last_played_position = board[i,col].cell_obj.transform.position;
                last_played_position = new Vector3((float)col,(float)i,0f);

                if(i==height-1)
                    return true;
                else
                    return false;
            }  
        }
        return false;
    }
    //Used by computer to play a tile
    public bool Who_Plays_And_Return_If_Full(Cell_status who, int col, Cell[,] board )
    {
        for(int i=0;i<height;i++)
        {
            if(board[i,col].status==Cell_status.Neutral)
            {
                board[i,col].status = who;
                return true;
            }
        }
        return false;
    }

    public void AI_Plays(Cell_status who, int MONTE_NUMBER)
    {
        int[] check_for_wins = new int[width];
        int[] check_for_losses = new int[width];
        int[] arr1 = {0,1,2,3,4,5};
        int[] arr2 = {0,1,2,3,4,5,6};
        for(int i=0;i<width;i++)
        {
            check_for_wins[i]=0;
            check_for_losses[i]=0;
        }
            
        

        for(int k=0;k<MONTE_NUMBER;k++)
        {
            Cell[,] board_monte = new Cell[height, width];

            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    board_monte[i,j] = new Cell();
                    board_monte[i,j].status = board[i,j].status;
                }
            }

            System.Random rnd = new System.Random();
            arr1 = arr1.OrderBy(x=>rnd.Next()).ToArray();
            System.Random rnd2 = new System.Random();
            arr2 = arr2.OrderBy(x=>rnd2.Next()).ToArray();
            
            int[] comp_played = new int[width];
            for(int i=0;i<width;i++)
                comp_played[i] = -1;

            bool exited = false;
            var rand = new System.Random();
            foreach(int i in arr1)
            {
                foreach(int j in arr2)
                {
                    if(board_monte[height-1,j].status==Cell_status.Neutral)
                    {
                        
                        //Debug.Log((float)rand.NextDouble());
                        //if(UnityEngine.Random.Range(0f,1f)>=0.5f)
                        if(((float)rand.NextDouble())>0.5f)
                        {
                            
                            Who_Plays_And_Return_If_Full(who,j,board_monte);
                            if(Check_for_Victory(who, board_monte))
                            {
                                exited = true;
                                break;
                            }
                        }
                        else
                        {
                            Who_Plays_And_Return_If_Full(Cell_status.Player,j,board_monte);
                            if(Check_for_Victory(Cell_status.Player, board_monte))
                            {
                                exited = true;
                                break;
                            }
                        }  
                    }   
                }
                if(exited)
                    break;
            }
            
            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    if(board[i,j].status==Cell_status.Neutral && board_monte[i,j].status==Cell_status.Computer && comp_played[j]==-1 && 
                    ( i==0 || board[i-1,j].status!=Cell_status.Neutral ) )
                    {
                        comp_played[j] = i;
                    }
                }
            }

    
            if(Check_for_Victory(Cell_status.Player, board_monte))
            {
                for(int i=0;i<width;i++)
                {
                    if(comp_played[i]>-1)
                        check_for_losses[i]+=1;
                }
                
            }
            else if(Check_for_Victory(who, board_monte))
            {
                for(int i=0;i<width;i++)
                {
                    if(comp_played[i]>-1)
                        check_for_wins[i]+=1;
                }

            }
        }
       
        float max =0f;
        int pos=0;
        for(int i=0;i<width;i++)
        {
            if(( (float)check_for_wins[i] )/( (float)check_for_losses[i] )>max)
            {
                pos = i;
                max = ( (float)check_for_wins[i] )/( (float)check_for_losses[i] );
            }
        }

        Debug.Log(max);
        Who_Plays_And_Return_If_Full(who,pos);
    }


    //initialization of objects
    public void Set_Object_to_Cell(int i, int j, GameObject obj)
    {
        board[i,j].cell_obj = obj;
    }

    //check if anybody wins at board
    public bool Check_for_Victory(Cell_status who)
    {
        for(int i=0;i<board.GetLength(0);i++)
        {
            for(int j=0; j<board.GetLength(1);j++)
            {
                if(board[i,j].status==who && Check_Four(i,j,who,board,false))
                    return true;
            }
        }
        return false;
    }

    //check if anybody wins at the monte carlo board
    public bool Check_for_Victory(Cell_status who, Cell[,] game_board)
    {
        for(int i=0;i<game_board.GetLength(0);i++)
        {
            for(int j=0; j<game_board.GetLength(1);j++)
            {
                if(game_board[i,j].status==who && Check_Four(i,j,who,game_board,true))
                    return true;
            }
        }
        return false;
    }

    
    //method used to determine victor
    private bool Check_Four(int i, int j, Cell_status who, Cell[,] test_board, bool isMonte)
    {
        //vertical check
        if(i<height-3 &&
        test_board[i+1,j].status==who && test_board[i+2,j].status==who && test_board[i+3,j].status==who)
        {
            if(MODE_Tetris && !isMonte)
                MODE_Tetris_method(board[i,j], board[i+1,j], board[i+2,j], board[i+3,j]);
            return true;
        }
            
        //horizontal check
        if(j<width-3 && 
        test_board[i,j+1].status==who && test_board[i,j+2].status==who && test_board[i,j+3].status==who)
        {
            if(MODE_Tetris && !isMonte)
                MODE_Tetris_method(board[i,j], board[i,j+1], board[i,j+2], board[i,j+3]);
            return true;
        }

        //diagonal check upper
        if(i<height-3 && j<width-3 &&
        test_board[i+1,j+1].status==who &&  test_board[i+2,j+2].status==who && test_board[i+3,j+3].status==who)
        {
            if(MODE_Tetris && !isMonte)
                MODE_Tetris_method(board[i,j], board[i+1,j+1], board[i+2,j+2], board[i+3,j+3]);
            return true;
        }
        //diagonal check lower
        if(i>2 && j<width-3 &&
        test_board[i-1,j+1].status==who &&  test_board[i-2,j+2].status==who && test_board[i-3,j+3].status==who)
        {
            if(MODE_Tetris && !isMonte)
                MODE_Tetris_method(board[i,j], board[i-1,j+1], board[i-2,j+2], board[i-3,j+3]);
            return true;
        }
        return false;
    }

    private void MODE_Tetris_method(Cell c1, Cell c2, Cell c3, Cell c4)
    {
        Debug.Log("Entered mode tetris method");
        Cell[] cells = {c1,c2,c3,c4};
        who_won = c1.status;
        for(int i=0;i<4;i++)
        {
            Debug.Log("activate kill switch for loop");
            cells[i].status = Cell_status.Neutral;
            cells[i].kill_switch = true;
            var j = cells[i].x_pos;
            var k = cells[i].y_pos;
            if(k<height-1 && board[k+1,j].status!=Cell_status.Neutral)
            {
                Debug.Log("activate kill switch");
                board[k+1,j].status = Cell_status.Neutral;
                board[k+1,j].kill_switch = true;
            }
            if(k>0 && board[k-1,j].status != Cell_status.Neutral)
            {
                    board[k-1,j].status = Cell_status.Neutral;
                    board[k-1,j].kill_switch = true;
            }
            if(j<width-1 && board[k,j+1].status != Cell_status.Neutral)
            {
                board[k,j+1].status = Cell_status.Neutral;
                board[k,j+1].kill_switch = true;
            }
            if(j>0 && board[k,j-1].status != Cell_status.Neutral)
            {
                board[k,j-1].status = Cell_status.Neutral;
                board[k,j-1].kill_switch = true;
            }
                
        }


        for(int i=1;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                var bot = Find_bottom_of_col(i,j);
                if(bot!=i-1)
                {
                    board[bot+1,j].status = board[i,j].status;
                    board[i,j].status = Cell_status.Neutral;
                }

                
            }
        }
        BoardtoWorld();
        explosion = true;
        check_for_chain =true;
    }

    public int Find_bottom_of_col(int curr_height, int col)
    {
        for(int j=curr_height-1;j>-1;j--)
        {
            if(board[j, col].status!=Cell_status.Neutral)
            {
                return j;
            }
        }
        return -1;
    }

    public void Roids(int row, int col)
    {
        float probability = Random.Range(0f,0.6f);
        if(probability<0.1f)//single cell
        {
            board[row,col].Set_Switch();
           // if(board[row,col].status!=Cell_status.Neutral)
           // {
           //     board[row,col].kill_switch=true;
            //    board[row,col].status=Cell_status.Neutral;
            //}
        }
        else if(probability<0.2f)//straight cross single
        {
            board[row,col].Set_Switch();
            if(row<height-1 )//up
            {
                board[row+1,col].Set_Switch();
            }
            if(row>0)//down
            {
                board[row-1,col].Set_Switch();
            }
            if(col<width-1)//right
            {
                board[row,col+1].Set_Switch();
            }
            if(col>0)//left
            {
                board[row,col-1].Set_Switch();
            }
        }
        else if(probability<0.3f)//skewd cross
        {
            board[row,col].Set_Switch();
            if(row<height-1 && col>0)//top left
            {
                board[row+1,col-1].Set_Switch();
            }
            if(row<height-1 && col<width-1)//top right
            {
                board[row+1,col+1].Set_Switch();
            }
            if(col>0 && row>0)//down left
            {
                board[row-1,col-1].Set_Switch();
            }
            if(col<width-1 && row>0)//down right
            {
                board[row-1,col+1].Set_Switch();
            }
        }
        else if(probability<0.4f)//entire row
        {
            for(int i=0;i<width;i++)
            {
                board[row,i].Set_Switch();
            }
        }
        else if(probability<0.5f)//entire column
        {
            for(int i=0;i<height;i++)
            {
                board[i,col].Set_Switch();
            }
        }
        else if(probability<0.6f)//huge straight cross
        {
            for(int i=0;i<width;i++)
            {
                board[row,i].Set_Switch();
            }
            for(int i=0;i<height;i++)
            {
                board[i,col].Set_Switch();
            }
        }

        for(int i=1;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                var bot = Find_bottom_of_col(i,j);
                if(bot!=i-1)
                {
                    board[bot+1,j].status = board[i,j].status;
                    board[i,j].status = Cell_status.Neutral;
                }    
            }
        }
        BoardtoWorld();
        explosion = true;
        check_for_chain =true;
    }

    TextMesh[,] arr;

    public void BoardtoWorld()
    {
        if(arr==null)
            arr = new TextMesh[height,width];
        for(int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                Color color;
                string text;
                if(board[i,j].status== Cell_status.Neutral)
                {
                    text = "N";
                    color =Color.white;
                }
                    
                else if(board[i,j].status== Cell_status.Player)
                {
                    color = Color.red;
                    text = "R";
                }
                else
                {
                    color = Color.yellow;
                    text = "Y";
                }
                    
                if(arr[i,j]==null)
                    arr[i,j] = CreateWorldText(text,color, TextAnchor.MiddleCenter, TextAlignment.Center,new Vector3(j,i),100);
                else
                {
                    arr[i,j].text = text;
                    arr[i,j].color = color;
                }
                    
            }
        }
        

       
    }

    public TextMesh CreateWorldText( string text, Color color, TextAnchor textAnchor, TextAlignment textAlignment,Vector3  localPosition , int sortingOrder,Transform parent = null, int fontSize = 10)
    {
        GameObject gameObject = new GameObject("world_text",typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent,false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text= text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;

    }

}
