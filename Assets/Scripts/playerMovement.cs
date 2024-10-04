using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float groundDrag;

    public float jumpPower;
    public float jumpCooldown;
    public float airMultiplier;
    bool jumpReady;

    [Header("Keybinds")]
    public KeyCode jumpKey;

    [Header("Ground Checks")]
    public float playerHeight;
    public LayerMask Ground;
    bool isgrounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;
    Rigidbody rb;

    [Header("Raycast")]
    public float rayDistance = 1.5f;


    public Animator animator;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("rigidbody is not getting called"); //null check
        }

        rb.freezeRotation = true;
        jumpReady = true;
    }



    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.3f, Ground))
        {
            isgrounded = true;

            transform.SetParent(hit.transform); //Whatever hit the raycast and is of the layerMask "Ground" becomes the parent of the object and the player themselveves become a child to the object.
        }
        else
        {
            isgrounded = false;

            transform.SetParent(null); //the player will not be a child to anything.
        }

        PlayerInputs();
        SpeedController();

        if (isgrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }



    private void FixedUpdate()
    {
        playerMove();
    }



    private void PlayerInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jump logic.
        if (Input.GetKey(jumpKey) && jumpReady && isgrounded)
        {
            animator.SetTrigger("Jump");
            jumpReady = false;
            JumpAction();
            Invoke(nameof(JumpReset), jumpCooldown);
        }
    }



    void playerMove()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isgrounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);

            if(movementDirection.magnitude > 0)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

        }
        else if (!isgrounded) //air check.
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
            animator.SetBool("isWalking", false);
        }
    }



    void SpeedController()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVector = flatVelocity.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVector.x, rb.velocity.y, limitedVector.z);
        }
    }



    void JumpAction()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }



    void JumpReset()
    {
        jumpReady = true;
    }
}
