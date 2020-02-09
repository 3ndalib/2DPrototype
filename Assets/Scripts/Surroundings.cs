using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surroundings : MonoBehaviour
{
    public PlayerController PC;
    public BoxCollider2D BC;

    public bool FacingRight = true;
    public bool FacingLeft = false;
    public bool Grounded;
    public bool Walking;
    public bool CanJump;
    public bool TouchingWall;
    public bool WallSliding;

    public float ExtraHeight;
    public float SkinWidth;
    public float WallCheckDistance;

    public int FacingDirection = 1;

    public Color CollidingColor;
    public Color NotCollidingColor;

    public LayerMask PlatformLayerMask;
    void Start()
    {
        PC = GetComponent<PlayerController>();
        BC = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckingSurroundings();
    }

    public void CheckingSurroundings() 
    {
        Flip();
        IsWalking();
        CheckIfCanJump();
        IsTouchingWall();
        IsWallSliding();
        CheckIfCanJump();
        Grounded = IsGrounded();
        Walking = IsWalking();
        TouchingWall = IsTouchingWall();
        WallSliding = IsWallSliding();
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
        if (!WallSliding) 
        {
            FacingDirection *= -1;
        }
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

    public bool IsGrounded()
    {
        RaycastHit2D Hit = Physics2D.BoxCast(new Vector2(BC.bounds.center.x, BC.bounds.min.y), new Vector2(BC.bounds.size.x - SkinWidth, BC.bounds.size.y / 8), 0f, Vector2.down, ExtraHeight, PlatformLayerMask);
        Color RayColor;
        if (Hit.collider != null)
        {
            RayColor = CollidingColor;
        }
        else
        {
            RayColor = NotCollidingColor;
        }
        Debug.DrawRay(new Vector3(BC.bounds.center.x, BC.bounds.min.y) + new Vector3(BC.bounds.extents.x, 0), Vector2.down * (BC.bounds.extents.y / 8 + ExtraHeight), RayColor);
        Debug.DrawRay(new Vector3(BC.bounds.center.x, BC.bounds.min.y) - new Vector3(BC.bounds.extents.x, 0), Vector2.down * (BC.bounds.extents.y / 8 + ExtraHeight), RayColor);
        Debug.DrawRay(new Vector3(BC.bounds.center.x, BC.bounds.min.y) - new Vector3(BC.bounds.extents.x, BC.bounds.extents.y / 8 + ExtraHeight), Vector2.right * BC.bounds.extents * 2, RayColor);
        return Hit.collider != null;
    }

    public void CheckIfCanJump() 
    {
        if (Grounded && PC.RB.velocity.y <= 0)
        {
            PC.JumpsAmountLeft = PC.JumpsAmount;
        }
        if (PC.JumpsAmountLeft <= 0)
        {
            CanJump = false;
        }
        else 
        {
            CanJump = true;
        }
    }

    public bool IsTouchingWall()
    {
        RaycastHit2D Hit;
        if (FacingRight)
        {
            Hit = Physics2D.Raycast(BC.bounds.center, Vector2.right, WallCheckDistance, PlatformLayerMask);
        }
        else
        {
            Hit = Physics2D.Raycast(BC.bounds.center, Vector2.left, WallCheckDistance, PlatformLayerMask);
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
            Debug.DrawRay(BC.bounds.center, Vector2.right * WallCheckDistance, RayColor);
        }
        else
        {
            Debug.DrawRay(BC.bounds.center, Vector2.left * WallCheckDistance, RayColor);
        }
        return Hit.collider != null;
    }

    public bool IsWallSliding()
    {
        if (TouchingWall && !Grounded && PC.RB.velocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWalking()
    {
        if (PC.Velocity.x != 0 && Grounded && PC.MovementInputDirection !=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
