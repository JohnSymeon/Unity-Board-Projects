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
    //public GameObject attached_mark;
    
    [SerializeField] public GameObject cell_obj;
    
    public Cell_status status;
    public int x_pos;
    public int y_pos;

    public bool kill_switch;

    public bool marked_for_explosion;

    public Cell()
    {
        status = Cell_status.Neutral;
    }

    public void Set_Switch()
    {
        if(status != Cell_status.Neutral)
        {
            kill_switch = true;
            status = Cell_status.Neutral;
        }
        else
            marked_for_explosion = true;
    }

    public void SetCoordinate(int x_pos, int y_pos)
    {
        this.x_pos = x_pos;
        this.y_pos = y_pos;
    }


}
