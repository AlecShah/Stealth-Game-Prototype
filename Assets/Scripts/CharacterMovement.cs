using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crouchMoveSpeed;


    //====================================================
    [SerializeField] private bool isGrounded;
    [SerializeField] private float checkGround;
    [SerializeField]  private LayerMask groundMask;
    [SerializeField] private float gravity;

    private Vector3 velocity;

    private Vector3 moveDirection;

    private bool canUseSmokeSwitch;
    //private SmokeSwitch smokeDevice;
    private SmokeSwitch smokeDevice;


    CharacterController controller;
    Animator animate;

    void Start()
    {
        canUseSmokeSwitch = false;
        smokeDevice = null;

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

        RaycastCheck();

        if (Input.GetKey(KeyCode.P) && canUseSmokeSwitch == true && smokeDevice != null)
        {
            OperateSmokeSwitch();
        }

    }


    private void OperateSmokeSwitch()
    {
        Debug.Log("using smoke switch!");
        smokeDevice.ChangeSmokeState();
    }

    private void RaycastCheck()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 1;
        Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, 3) && hit.transform.tag == "smoke switch")
        {


            if (canUseSmokeSwitch == false)
            {
                canUseSmokeSwitch = true;
                smokeDevice = hit.transform.gameObject.GetComponent<SmokeSwitch>();
                //smokeDevice = hit.collider.gameObject.GetComponent<SmokeSwitch>();
                //smokeDevice = hit.gameObject.GetComponent<SmokeSwitch>();
            }


            //Debug.Log("hit");
        }
        else
        {
            if (canUseSmokeSwitch == true)
            {
                canUseSmokeSwitch = false;
                smokeDevice = null;
            }

            //Debug.Log("no hit");
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, checkGround, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

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

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
