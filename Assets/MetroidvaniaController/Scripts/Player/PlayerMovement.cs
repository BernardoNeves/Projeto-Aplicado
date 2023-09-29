using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;
    public bool isAttacking;
    public bool isGrounded;
    public bool canDoubleJump;

    // Update is called once per frame
    void Update()
    {
        canDoubleJump = GetComponent<CharacterController2D>().canDoubleJump;

        isGrounded = GetComponent<CharacterController2D>().m_Grounded;

        isAttacking = GetComponent<Attack>().isAttacking;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = true;
        }

        //if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
        //{
        //    if (dashAxis == false)
        //    {
        //        dashAxis = true;
        //        dash = true;
        //    }
        //}
        //else
        //{
        //    dashAxis = false;
        //}


    }

    public void OnFall()
    {
        animator.SetBool("IsJumping", true);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
            jump = false;
            dash = false;
            animator.SetBool("CanWalk", true);
        }
        else if(isAttacking && !isGrounded)
        {
            dash = false;
            jump = false;
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
            jump = false;
            canDoubleJump = false;
            animator.SetBool("CanWalk", false);
        }
        else if (isAttacking && isGrounded)
        {
            dash = false;
            jump = false;
            controller.Move(0, jump, dash);
            jump = false;
            animator.SetBool("CanWalk", false);
        }

    }
}
