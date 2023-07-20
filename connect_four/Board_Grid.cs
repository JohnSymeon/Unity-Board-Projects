using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Grid
{
    private int width;
    private int height;
    public Cell[,] board;
    

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
                if(i==9)
                    return true;
            }
                
        }
        return false;
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
                if(board[i,j].status==who && Check_Four(i,j,who))
                    return true;
            }
        }
        return false;
    }

    private bool Check_Four(int i, int j, Cell_status who)
    {
        //horizontal check
        if(i<board.GetLength(0)-4 &&
        board[i+1,j].status==who && board[i+2,j].status==who && board[i+3,j].status==who)
            return true;

        //vertical check
        if(j<board.GetLength(1)-4 &&
        board[i,j+1].status==who && board[i,j+2].status==who && board[i,j+3].status==who)
            return true;

        //diagonal check upper
        if(i<board.GetLength(0)-4 && j<board.GetLength(1)-4 &&
        board[i+1,j+1].status==who &&  board[i+2,j+2].status==who && board[i+3,j+3].status==who)
            return true;
        
        //diagonal check lower
        if(i>3 && j<board.GetLength(1)-4 &&
        board[i-1,j+1].status==who &&  board[i-2,j+2].status==who && board[i-3,j+3].status==who)
            return true;
        
        return false;
        
    }
}
