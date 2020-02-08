using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator Anim;
    public Surroundings SR;
    public bool Walking;
    void Start()
    {
        Anim = GetComponent<Animator>();
        SR = GetComponent<Surroundings>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    public void UpdateAnimator() 
    {
        Walking = SR.Walking;
        Anim.SetBool("IsWalking", Walking);
    }
}
