using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB;
    public Surroundings SR;

    public Vector2 Velocity;

    public float MovementInputDirection;
    public float MaxSpeed;
    public float Acceleration;
    public float Deceleration;
    public float JumpForce;

    public int ExtraJumpsValue;
    public int ExtraJumps;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<Surroundings>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckingInput();
    }

    private void FixedUpdate()
    {
        Movement();
        Velocity = new Vector2(RB.velocity.x, RB.velocity.y);
    }

    public void CheckingInput()
    {
        MovementInputDirection = Input.GetAxisRaw("Horizontal");
        Jump();
    }


    public void Movement()
    {
        Move();
        Jump();
        MoveCap();
        JumbCap();

    }

    public void Move()
    {
        if (MovementInputDirection > 0)
        {
            RB.velocity = new Vector2(RB.velocity.x + (MaxSpeed * Acceleration * Time.deltaTime), RB.velocity.y);
        }
        else if (MovementInputDirection < 0)
        {
            RB.velocity = new Vector2(RB.velocity.x + (-MaxSpeed * Acceleration * Time.deltaTime), RB.velocity.y);
        }
        else
        {
            if (RB.velocity.x > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x - (MaxSpeed * Deceleration * Time.deltaTime), RB.velocity.y);
                if (RB.velocity.x <= 0)
                {
                    RB.velocity = new Vector2(0, RB.velocity.y);
                }
            }
            else if (RB.velocity.x < 0)
            {
                RB.velocity = new Vector2(RB.velocity.x + (MaxSpeed * Deceleration * Time.deltaTime), RB.velocity.y);
                if (RB.velocity.x >= 0)
                {
                    RB.velocity = new Vector2(0, RB.velocity.y);
                }
            }

        }

    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (ExtraJumps > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, JumpForce);
                ExtraJumps--;
            }
            else if (ExtraJumps == 0 && SR.Grounded)
            {
                RB.velocity = new Vector2(RB.velocity.x, JumpForce);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y + (-JumpForce * Time.deltaTime));
        }
    }

    void MoveCap()
    {
        if (RB.velocity.x > MaxSpeed)
        {
            RB.velocity = new Vector2(MaxSpeed, RB.velocity.y);
        }
        else if (RB.velocity.x < -MaxSpeed)
        {
            RB.velocity = new Vector2(-MaxSpeed, RB.velocity.y);
        }
    }

    void JumbCap()
    {
        if (SR.Grounded)
        {
            ExtraJumps = ExtraJumpsValue;
        }
    }
}
