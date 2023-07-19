using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell
{
    
    [SerializeField] public GameObject cell_obj;
    public int cell_status;
    public int x_pos;
    public int y_pos;

    public Cell()
    {
        cell_status = 0;
    }

    public void SetCoordinate(int x_pos, int y_pos)
    {
        this.x_pos = x_pos;
        this.y_pos = y_pos;
       // cell_obj.transform.position = new Vector3(x_pos, y_pos,0);
    }


}
