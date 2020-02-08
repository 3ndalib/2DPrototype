using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator Anim;
    public Surroundings SR;
    public PlayerController PC;

    public bool Walking;
    public bool Grounded;
    public bool WallSliding;

    public float YVelocity;

    void Start()
    {
        Anim = GetComponent<Animator>();
        SR = GetComponent<Surroundings>();
        PC = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    public void UpdateAnimator() 
    {
        Walking = SR.Walking;
        Grounded = SR.Grounded;
        WallSliding = SR.WallSliding;

        YVelocity = PC.RB.velocity.y;
        
        Anim.SetBool("IsWalking", Walking);
        Anim.SetBool("IsGrounded", Grounded);
        Anim.SetBool("IsWallSliding", WallSliding);

        Anim.SetFloat("YVelocity", YVelocity);
    }
}
