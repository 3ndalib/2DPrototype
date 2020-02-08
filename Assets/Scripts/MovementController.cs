using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody2D RB;
    public Animator Anim;
    public BoxCollider2D CC;

    public float MaxSpeed;
    public float Acceleration;
    public float Deceleration;
    public float JumpForce;
    public float WallHopForce;
    public float WallJumbForce;
    public float ExtraHeight;
    public float WallCheckDistance;
    public float WallSlideSpeed;
    public float SkinWidth;

    public int ExtraJumbs;
    public int ExtraJumbsValue;

    public Vector2 Velocity;

    public bool Done = false;
    public bool Walking;
    public bool IsMoving;
    public bool IsJumbing;
    public bool IsFalling;
    public bool Grounded;
    public bool TouchingWall;
    public bool WallSliding;
    public bool FacingRight;
    public bool FacingLeft;

    public LayerMask PlatformLayerMask;

    public Color CollidingColor;
    public Color NotCollidingColor;

    private void Start()
    {
        ExtraJumbs = ExtraJumbsValue;
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        CC = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        CheckingSurroundings();
        Velocity = RB.velocity;
        Movement();
    }


    public void CheckingSurroundings()
    {
        IsWalking();
        IsGrounded();
        IsTouchingWall();
        IsWallSliding();
        Face();
        FacingDirection();
        Walking = IsWalking();
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
        WallJumb();
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
    void WallJumb() 
    {
        if (IsTouchingWall() && !IsGrounded()) 
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            {
                if (WallSliding)
                {
                    RB.AddForce(FacingDirection() * WallHopForce, ForceMode2D.Impulse);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    RB.velocity += new Vector2(-1, 1) * WallJumbForce;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    RB.velocity += new Vector2(1, 1) * WallJumbForce;
                }
            }                
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

        if (RB.velocity.y > 0 && !Grounded)
        {
            IsJumbing = true;
            IsFalling = false;
        }
        else if (RB.velocity.y < 0 && !Grounded)
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
        RaycastHit2D Hit = Physics2D.BoxCast(new Vector2(CC.bounds.center.x, CC.bounds.min.y),new Vector2(CC.bounds.size.x - SkinWidth , CC.bounds.size.y / 8), 0f, Vector2.down, ExtraHeight, PlatformLayerMask);
        Color RayColor;
        if (Hit.collider != null)
        {
            RayColor = CollidingColor;
        }
        else 
        {
            RayColor = NotCollidingColor;
        }
        Debug.DrawRay(new Vector3(CC.bounds.center.x, CC.bounds.min.y) + new Vector3(CC.bounds.extents.x , 0), Vector2.down * (CC.bounds.extents.y / 8 + ExtraHeight), RayColor);
        Debug.DrawRay(new Vector3(CC.bounds.center.x, CC.bounds.min.y) - new Vector3(CC.bounds.extents.x , 0), Vector2.down * (CC.bounds.extents.y / 8 + ExtraHeight), RayColor);
        Debug.DrawRay(new Vector3(CC.bounds.center.x, CC.bounds.min.y) - new Vector3(CC.bounds.extents.x , CC.bounds.extents.y / 8 + ExtraHeight),Vector2.right * CC.bounds.extents * 2, RayColor);
        return Hit.collider != null;
    }

    public bool IsTouchingWall() 
    {
        RaycastHit2D Hit;
        if (FacingRight)
        {
            Hit = Physics2D.Raycast(CC.bounds.center, Vector2.right, WallCheckDistance, PlatformLayerMask);
        }
        else 
        {
            Hit = Physics2D.Raycast(CC.bounds.center, Vector2.left, WallCheckDistance, PlatformLayerMask);
        }
        Color RayColor;
        if (Hit.collider != null)
        {
            RayColor = CollidingColor;
        }
        else
        {
            RayColor = NotCollidingColor;
        }
        if (FacingRight)
        {
            Debug.DrawRay(CC.bounds.center, Vector2.right * WallCheckDistance, RayColor);
        }
        else
        {
            Debug.DrawRay(CC.bounds.center, Vector2.left * WallCheckDistance, RayColor);
        }
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

    public void Face() 
    {
        if (transform.eulerAngles == new Vector3(transform.rotation.x, 180))
        {
            FacingLeft = true;
            FacingRight = false;
        }
        else 
        {
            FacingRight = true;
            FacingLeft = false;
        }
    }
    public Vector2 FacingDirection() 
    {
        if (FacingRight)
        {
            return new Vector2(-1f, 0);
        }
        else 
        {
            return new Vector2(1f, 0);
        }
    }

    public bool IsWalking() 
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Grounded || Input.GetKey(KeyCode.A)
            && Grounded || Input.GetKey(KeyCode.RightArrow) && Grounded || Input.GetKey(KeyCode.D) && Grounded)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
