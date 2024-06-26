using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRootMotion : MonoBehaviour
{
    private Animator animator;

    public bool Jump;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Movement();
    }

    public void Movement()
    {
        animator.SetFloat("X", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetFloat("Y", Mathf.Abs(Input.GetAxis("Vertical")));

        if (Mathf.Abs(Input.GetAxis("Horizontal")) != 0f)
        {
            animator.SetBool("IsMoving", true);
        }

        RaycastHit hitGround;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitGround, 0.6f))
        {
            if (hitGround.transform.CompareTag("Ground"))
            {
                animator.SetBool("IsGrounded", true);
            }
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        animator.SetBool("Crouch", true);

        animator.SetBool("Falling", true);
        animator.SetBool("Landing", true);

        animator.SetTrigger("TurnAround");
        animator.SetTrigger("Jump");
        animator.SetTrigger("Climb");
        animator.SetTrigger("Vanne");
        animator.SetTrigger("LadderStart");
    }
}
