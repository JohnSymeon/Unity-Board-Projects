using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    Board_Generator BG;

    public int MONTE_NUMBER=1000;

    int size;

    void Start()
    {
        BG = GetComponent<Board_Generator>();
        size = BG.size;

    }

    public int AI_Chooses(Status who)
    {
        Status who_player;
        if(who==Status.Red)
            who_player = Status.Blue;
        else
            who_player = Status.Red;

        bool[] tiles_played = new bool[size*size];
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                if(BG.board[i,j].status==who)
                    tiles_played[i*size+j] = true;
            }
        }
        bool[] tiles_played_player = new bool[size*size];
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                if(BG.board[i,j].status==who_player)
                    tiles_played_player[i*size+j] = true;
            }
        }

        int[] Victories = new int[size*size];
        int[] Losses = new int[size*size];
        System.Random rnd = new System.Random();

        for(int count=0;count<MONTE_NUMBER;count++)
        {
            Tile[,] board_copy = new Tile[size,size];
            for(int i=0;i<size;i++)
            {
                for(int j=0;j<size;j++)
                {
                    board_copy[i,j] = new Tile();
                }
            }
            for(int i=0;i<size;i++)
            {
                for(int j=0;j<size;j++)
                {
                    if(BG.board[i,j].status!=Status.Neutral)
                        board_copy[i,j].status = BG.board[i,j].status;
                    else
                    {
                        //float temp =  Random.Range(0f,1f);
                        
                        //float temp = (float)rnd.NextDouble();
                        if((float)rnd.NextDouble()>0.5f)
                            board_copy[i,j].status = who;
                        else
                            board_copy[i,j].status = who_player;
                    }
                         
                }
            }

            if(BG.Check_for_Victory(board_copy,who))
            {
                for(int i=0;i<size;i++)
                {
                    for(int j=0;j<size;j++)
                    {
                        if(board_copy[i,j].status==who &&
                        tiles_played[i*size+j]==false)
                        {
                            Victories[i*size+j] +=1;
                        }
                    }
                }
            }
            if(BG.Check_for_Victory(board_copy,who_player))
            {
                for(int i=0;i<size;i++)
                {
                    for(int j=0;j<size;j++)
                    {
                        if(board_copy[i,j].status==who_player &&
                        tiles_played[i*size+j]==false)
                        {
                            Losses[i*size+j] +=1;
                        }
                    }
                }
            }
        }

        int max = 0;
        int ID_temp = 0;
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                if(Losses[i*size+j]>0 && (Victories[i*size+j]/Losses[i*size+j])>max)
                {
                    max = (Victories[i*size+j]/Losses[i*size+j]);
                    ID_temp = i*size+j;
                }
                else if(Victories[i*size+j]>max)
                {
                    max = Victories[i*size+j];
                    ID_temp = i*size+j;
                }
            }
        }


        return ID_temp;
    }

}
