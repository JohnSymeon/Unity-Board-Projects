using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Board_Grid
{
    private int width;
    private int height;
    public Cell[,] board;

    
    public Vector3 last_played_position;


    public static float x_offset = 0f;
    public static float y_offset = 0f;

    public Board_Grid(int width, int height)
    {
        
        this.width=width;
        this.height = height;

        board = new Cell[width,height];
        for(int i=0;i<board.GetLength(0);i++)
        {
            for(int j=0;j<board.GetLength(1);j++)
            {
                board[i,j] = new Cell();
                board[i,j].SetCoordinate(i,j);

            }
        }
    }

    public bool Who_Plays_And_Return_If_Full(Cell_status who, int col)
    {
        for(int i=0;i<board.GetLength(1);i++)
        {
            if(board[i,col].status==Cell_status.Neutral)
            {
                board[i,col].status = who;

                //last_played_position = new Vector3(i-x_offset, col - y_offset,0f);

                last_played_position = board[i,col].cell_obj.transform.position;

                if(i==9)
                    return true;

                return false;
            }
                
        }
        return false;
    }

    public bool Who_Plays_And_Return_If_Full(Cell_status who, int col, Cell[,] board )
    {
        for(int i=0;i<board.GetLength(1);i++)
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
        int[] arr1 = {0,1,2,3,4,5,6,7,8,9};
        int[] arr2 = {0,1,2,3,4,5,6,7,8,9};
        for(int i=0;i<width;i++)
        {
            check_for_wins[i]=0;
            check_for_losses[i]=0;
        }
            
        

        for(int k=0;k<MONTE_NUMBER;k++)
        {
            Cell[,] board_monte = new Cell[width, height];

            for(int i=0;i<width;i++)
            {
                for(int j=0;j<height;j++)
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
            foreach(int i in arr1)
            {
                foreach(int j in arr2)
                {
                    if(board_monte[9,j].status==Cell_status.Neutral)
                    {
                        if(Random.Range(0f,1f)>=0.5f)
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




            
            for(int i=0;i<width;i++)
            {
                for(int j=0;j<height;j++)
                {
                    if(board[i,j].status==Cell_status.Neutral && board_monte[i,j].status==Cell_status.Computer && comp_played[j]==-1 && 
                    ( i==0 || board[i-1,j].status!=Cell_status.Neutral ) )
                    {
                        comp_played[j] = i;
                    }
                }
            }

    
            if(Check_for_Victory(who, board_monte))
            {
                for(int i=0;i<width;i++)
                {
                    if(comp_played[i]>-1)
                        check_for_wins[i]+=1;
                }
            }
            else
            {
                for(int i=0;i<width;i++)
                {
                    if(comp_played[i]>-1)
                        check_for_losses[i]+=1;
                }

            }
            //if(Check_for_Victory(Cell_status.Player, board_monte))
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
        //Debug.Log(pos);
        Who_Plays_And_Return_If_Full(who,pos);
    }



    public void Set_Object_to_Cell(int i, int j, GameObject obj)
    {
        board[i,j].cell_obj = obj;
    }

    public bool Check_for_Victory(Cell_status who)
    {
        for(int i=0;i<board.GetLength(0);i++)
        {
            for(int j=0; j<board.GetLength(1);j++)
            {
                if(board[i,j].status==who && Check_Four(i,j,who,board))
                    return true;
            }
        }
        return false;
    }

    public bool Check_for_Victory(Cell_status who, Cell[,] game_board)
    {
        for(int i=0;i<game_board.GetLength(0);i++)
        {
            for(int j=0; j<game_board.GetLength(1);j++)
            {
                if(game_board[i,j].status==who && Check_Four(i,j,who,game_board))
                    return true;
            }
        }
        return false;
    }

    

    private bool Check_Four(int i, int j, Cell_status who, Cell[,] board)
    {
        //horizontal check
        if(i<board.GetLength(0)-3 &&
        board[i+1,j].status==who && board[i+2,j].status==who && board[i+3,j].status==who)
            return true;

        //vertical check
        if(j<board.GetLength(1)-3 &&
        board[i,j+1].status==who && board[i,j+2].status==who && board[i,j+3].status==who)
            return true;

        //diagonal check upper
        if(i<board.GetLength(0)-3 && j<board.GetLength(1)-3 &&
        board[i+1,j+1].status==who &&  board[i+2,j+2].status==who && board[i+3,j+3].status==who)
            return true;
        
        //diagonal check lower
        if(i>2 && j<board.GetLength(1)-3 &&
        board[i-1,j+1].status==who &&  board[i-2,j+2].status==who && board[i-3,j+3].status==who)
            return true;
        
        return false;
        
    }
}
