using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB;
    public Surroundings SR;

    public Vector2 Velocity;
    public Vector2 WallHopDirection;
    public Vector2 WallJumpDirection;

    public float MovementInputDirection;
    public float MaxSpeed;
    public float Acceleration;
    public float Deceleration;
    public float JumpForce;
    public float WallSlideSpeed;
    public float WallHopForce;
    public float WallJumpForce;

    public int JumpsAmount;
    public int JumpsAmountLeft;

    void Start()
    {
        JumpsAmountLeft = JumpsAmount;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<Surroundings>();

        WallHopDirection.Normalize();
        WallJumpDirection.Normalize();
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();   
        }
    }


    public void Movement()
    {
        Move();
        MoveCap();
        JumbCap();
        WallSlide();
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
        if (SR.CanJump) 
        {
            RB.velocity = new Vector2(RB.velocity.x, JumpForce);
            JumpsAmountLeft--;
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y + (-JumpForce * Time.deltaTime));
        }
    }

    public void WallSlide()
    {
        if (SR.WallSliding)
        {
            if (RB.velocity.y < -WallSlideSpeed)
            {
                RB.velocity = new Vector2(RB.velocity.x, -WallSlideSpeed);
            }
        }
    }
}
