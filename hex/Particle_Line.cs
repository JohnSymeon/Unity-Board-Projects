using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Line : MonoBehaviour
{

    public Vector3 hex_1;
    public Vector3 hex_2;
    Board_Generator BG;

    // Start is called before the first frame update
    void Start()
    {   
        BG= FindObjectOfType<Board_Generator>();
        hex_1 = BG.board[5,6].go.transform.position;
        hex_2 = BG.board[6,6].go.transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, hex_1-hex_2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
