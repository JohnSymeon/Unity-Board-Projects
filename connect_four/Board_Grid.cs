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

    public void SetObject(GameObject obj)
    {
        
    }
}
