using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBplayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    public Transform cam;
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float GravityModifier;
    public float Rotationspeed;

    public float groundDrag;
    public float jumpForce;
    public float JumpCool;
    public float airMultiplier;
    [SerializeField] bool ReadyToJump;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
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

    Rigidbody PlayerRB;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
    }

    void Start()
    {

        PlayerRB = GetComponent<Rigidbody>();
        PlayerRB.freezeRotation = true;
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
            PlayerRB.drag = groundDrag;
            ReadyToJump = true;
            Physics.gravity = new Vector3(0, GravityModifier, 0);
        }
        else
            PlayerRB.drag = 0;
        Physics.gravity = new Vector3(0, GravityModifier, 0);
    }
    
        
    void FixedUpdate()
    {
        MovePLayer();
    }
    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, 0f).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z)* Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (grounded)
            PlayerRB.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);


        else if (!grounded)
            PlayerRB.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        

       
        
        } 
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(PlayerRB.velocity.x, 0f, PlayerRB.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            PlayerRB.velocity = new Vector3(limitedVel.x, PlayerRB.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        PlayerRB.velocity = new Vector3(PlayerRB.velocity.x, transform.up.y * jumpForce, PlayerRB.velocity.z);
        //PlayerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        ReadyToJump = true;
    }
  
}

