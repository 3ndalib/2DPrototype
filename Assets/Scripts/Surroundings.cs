using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surroundings : MonoBehaviour
{
    public PlayerController PC;

    public bool FacingRight = true;
    public bool FacingLeft = false;
    void Start()
    {
        PC = GetComponent<PlayerController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Flip();
    }
    public void CheckMovementDirection() 
    {
        if (PC.MovementInputDirection > 0)
        {
            FacingRight = true;
            FacingLeft = false;
        }
        else if (PC.MovementInputDirection < 0) 
        {
            FacingLeft = true;
            FacingRight = false;
        }
    }
    public void Flip() 
    {
        CheckMovementDirection();
        if (FacingRight)
        {
            transform.eulerAngles = new Vector3(0, 0);
        }
        else if (FacingLeft) 
        {
            transform.eulerAngles = new Vector3(0, 180);
        }
    }

}
