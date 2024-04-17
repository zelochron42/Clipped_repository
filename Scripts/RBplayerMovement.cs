using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBplayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;
    public float jumpForce;
    public float JumpCool;
    public float airMultiplier;
    [SerializeField] bool ReadyToJump;
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
   [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatisGround;
    [SerializeField] private bool grounded;
    RaycastHit hit;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f, whatisGround))
        {
            Debug.Log("OnGround;");
            Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.red);
            grounded = true;
        }
        else
            grounded = false;
        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded)
        {
            rb.drag = groundDrag;
            ReadyToJump = true;
        }
        else
            rb.drag = 0;
    }
    void FixedUpdate()
    {
        MovePLayer();
    }
    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpKey) && ReadyToJump && grounded)
        {
            ReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), JumpCool);
        }

    }
    private void StateHandler()
    {
        if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }
    void MovePLayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
       // rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        ReadyToJump = true;
    }
  
}

