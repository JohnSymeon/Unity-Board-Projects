/*
This is the cell class that comprises the board having a neutral and two occupied states.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cell_status{
    Neutral,
    Player,
    Computer
};





[Serializable]
public class Cell
{
    
    [SerializeField] public GameObject cell_obj;

    public Cell_status status;
    public int x_pos;
    public int y_pos;

    public Cell()
    {
        status = Cell_status.Neutral;
    }

    public void SetCoordinate(int x_pos, int y_pos)
    {
        this.x_pos = x_pos;
        this.y_pos = y_pos;
    }


}
