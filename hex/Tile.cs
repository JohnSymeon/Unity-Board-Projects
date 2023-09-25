using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    Neutral = 0,
    Blue,
    Red
    
}

public class Tile
{
    public int id;
    public GameObject go;
    public Status status;

    public Tile(int Id, GameObject Go)
    {
        id = Id;
        go =Go;
        status = Status.Neutral;
        go.GetComponent<Hex_Generator>().id = id;
    }
}
