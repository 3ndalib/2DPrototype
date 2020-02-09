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
    public float AirMovementForce;
    public float AirDragMultiplier;
    public float JumpHeightMultiplier = 0.5f;

    public int JumpsAmount;
    public int JumpsAmountLeft;

    void Start()
    {
        JumpsAmountLeft = JumpsAmount;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<Surroundings>();

        //WallHopDirection.Normalize();
        //WallJumpDirection.Normalize();
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
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * JumpHeightMultiplier);
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
        if (SR.Grounded) {
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
        else if (!SR.Grounded && !SR.WallSliding && MovementInputDirection != 0)
        {
            Vector2 ForceToAdd = new Vector2(AirMovementForce * MovementInputDirection, 0);
            RB.AddForce(ForceToAdd, ForceMode2D.Impulse);
        }
        else if (!SR.Grounded && !SR.WallSliding && MovementInputDirection == 0) 
        {
            RB.velocity = new Vector2(RB.velocity.x * AirDragMultiplier, RB.velocity.y);
        }
    }

    public void Jump()
    {
        if (SR.CanJump && !SR.WallSliding)
        {
            RB.velocity = new Vector2(RB.velocity.x, JumpForce);
            JumpsAmountLeft--;
        }
        else if (SR.WallSliding && MovementInputDirection == 0 && SR.CanJump)
        {
            SR.WallSliding = false;
            JumpsAmountLeft--;
            Vector2 ForceToAdd = new Vector2(WallHopForce * WallHopDirection.x * -SR.FacingDirection, WallHopForce * WallHopDirection.y);
            RB.AddForce(ForceToAdd, ForceMode2D.Impulse);
        }
        else if ((SR.WallSliding || SR.TouchingWall) && MovementInputDirection != 0 && SR.CanJump)
        {
            SR.WallSliding = false;
            JumpsAmountLeft--;
            Vector2 ForceToAdd = new Vector2(WallJumpForce * WallJumpDirection.x * MovementInputDirection, WallJumpForce * WallJumpDirection.y);
            RB.AddForce(ForceToAdd, ForceMode2D.Impulse);
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
