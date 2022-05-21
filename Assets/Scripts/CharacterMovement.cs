using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crouchMoveSpeed;

    //Gravity(Add later if there's time or I want to expand with jump)
    //====================================================
    //[SerializeField] private bool isGrounded;
    //[SerializeField] private float checkGround;
    //[SerializeField]  private LayerMask groundMask;
    //[SerializeField] private float gravity;

    //private Vector3 velocity;

    private Vector3 moveDirection;

    CharacterController controller;
    Animator animate;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animate = GetComponent<Animator>();
    }

    private void Update()
    {
        animate.SetInteger("Stance", 0);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            animate.SetInteger("Stance", 1);
        }
        else
        {
            animate.SetInteger("Stance", 0);
        }
        Move();
    }

    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
        else if (moveDirection == Vector3.zero)
        {
            Idle();
        }

        if (moveDirection == Vector3.zero && Input.GetKey(KeyCode.LeftControl))
        {
            CrouchIdle();
        }
        if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftControl))
        {
            CrouchWalk();
        }

        moveDirection *= moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animate.SetFloat("Blend", 0.5f, 0.15f, Time.deltaTime);
    }

    private void Idle()
    {
        animate.SetFloat("Blend", 0, 0.15f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animate.SetFloat("Blend", 1, 0.15f, Time.deltaTime);
    }

    private void CrouchIdle()
    {
        animate.SetFloat("CBlend", 0, 0.15f, Time.deltaTime);

    }

    private void CrouchWalk()
    {
        moveSpeed = crouchMoveSpeed;
        animate.SetFloat("CBlend", 1, 0.15f, Time.deltaTime);
    }
}
