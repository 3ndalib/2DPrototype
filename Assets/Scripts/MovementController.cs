using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody2D RB;
    public Camera Cam;
    public Animator Anim;
    public BoxCollider2D CC;

    public float MaxSpeed;
    public float Acceleration;
    public float Deceleration;
    public float JumpForce;
    public float ExtraHeight;
    public float WallCheckDistance;
    public float WallSlideSpeed;

    public int ExtraJumbs;
    public int ExtraJumbsValue;

    public Vector2 Velocity;

    public bool IsMoving;
    public bool IsJumbing;
    public bool IsFalling;
    public bool Grounded;
    public bool TouchingWall;
    public bool WallSliding;

    public LayerMask PlatformLayerMask;

    public Color CollidingColor;
    public Color NotCollidingColor;

    private void Start()
    {
        ExtraJumbs = ExtraJumbsValue;
        RB = GetComponent<Rigidbody2D>();
        Cam = Camera.main;
        Anim = GetComponent<Animator>();
        CC = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        CheckingSurroundings();
        Velocity = RB.velocity;
        Movement();
    }


    public void CheckingSurroundings()
    {
        IsGrounded();
        IsTouchingWall();
        IsWallSliding();
        Grounded = IsGrounded();
        TouchingWall = IsTouchingWall();
        WallSliding = IsWallSliding();
    }

    void Movement()
    {
        Move();
        MoveCap();
        Jumb();
        JumbCap();
        MoveCheck();
        WallSlide();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RB.velocity = new Vector2(RB.velocity.x + (MaxSpeed * Acceleration * Time.deltaTime), RB.velocity.y);
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RB.velocity = new Vector2(RB.velocity.x + (-MaxSpeed * Acceleration * Time.deltaTime), RB.velocity.y);
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            if (RB.velocity.x > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x - (MaxSpeed * Deceleration * Time.deltaTime), RB.velocity.y);
                if (RB.velocity.x < 0)
                {
                    RB.velocity = new Vector2(0, RB.velocity.y);
                }
            }
            else if (RB.velocity.x < 0)
            {
                RB.velocity = new Vector2(RB.velocity.x + (MaxSpeed * Deceleration * Time.deltaTime), RB.velocity.y);
                if (RB.velocity.x > 0)
                {
                    RB.velocity = new Vector2(0, RB.velocity.y);
                }
            }
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

    void Jumb()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ExtraJumbs > 0 || Input.GetKeyDown(KeyCode.W) && ExtraJumbs > 0 || Input.GetKeyDown(KeyCode.UpArrow) && ExtraJumbs > 0)
        {
            //RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            RB.velocity = new Vector2(RB.velocity.x, JumpForce);
            ExtraJumbs--;
        }
        else if (Input.GetKey(KeyCode.Space) && ExtraJumbs == 0 && IsGrounded() || Input.GetKey(KeyCode.W) && ExtraJumbs == 0 && IsGrounded() || Input.GetKey(KeyCode.UpArrow) && ExtraJumbs == 0 && IsGrounded())
        {
            RB.velocity = new Vector2(RB.velocity.x, JumpForce);
            //RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y + (-JumpForce * Time.deltaTime));
        }
    }

    void JumbCap()
    {
        if (IsGrounded())
        {
            ExtraJumbs = ExtraJumbsValue;
        }
    }


    void MoveCheck()
    {
        if (RB.velocity.x > 0)
        {
            IsMoving = true;
        }
        else if (RB.velocity.x < 0)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }

        if (RB.velocity.y > 0)
        {
            IsJumbing = true;
            IsFalling = false;
        }
        else if (RB.velocity.y < 0)
        {
            IsFalling = true;
            IsJumbing = false;
        }
        else
        {
            IsJumbing = false;
            IsFalling = false;
        }
    }

    public void WallSlide() 
    {
        if (WallSliding) 
        {
            if (RB.velocity.y < -WallSlideSpeed) 
            {
                RB.velocity = new Vector2(RB.velocity.x, -WallSlideSpeed);
            }
        }
    } 

    public bool IsGrounded() 
    {
        RaycastHit2D Hit = Physics2D.BoxCast(CC.bounds.center, CC.bounds.size, 0f, Vector2.down, ExtraHeight, PlatformLayerMask);
        Color RayColor;
        if (Hit.collider != null)
        {
            RayColor = CollidingColor;
        }
        else 
        {
            RayColor = NotCollidingColor;
        }
        Debug.DrawRay(CC.bounds.center + new Vector3(CC.bounds.extents.x, 0), Vector2.down * (CC.bounds.extents.y + ExtraHeight), RayColor);
        Debug.DrawRay(CC.bounds.center - new Vector3(CC.bounds.extents.x, 0), Vector2.down * (CC.bounds.extents.y + ExtraHeight), RayColor);
        Debug.DrawRay(CC.bounds.center - new Vector3(CC.bounds.extents.x, CC.bounds.extents.y + ExtraHeight), Vector2.right * CC.bounds.extents.x, RayColor);
        return Hit.collider != null;
    }

    public bool IsTouchingWall() 
    {
        RaycastHit2D Hit = Physics2D.Raycast(CC.bounds.center, Vector2.right, WallCheckDistance, PlatformLayerMask);
        Color RayColor;
        if (Hit.collider != null)
        {
            RayColor = CollidingColor;
        }
        else
        {
            RayColor = NotCollidingColor;
        }
        Debug.DrawRay(CC.bounds.center, Vector2.right * WallCheckDistance, RayColor);
        return Hit.collider != null;
    }

    public bool IsWallSliding() 
    {
        if (IsTouchingWall() && !IsGrounded() && RB.velocity.y < 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
        
}
