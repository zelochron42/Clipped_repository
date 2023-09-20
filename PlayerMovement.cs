using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code to control the player's movement
/// Written by Joshua Cashmore, 9/15/2023
/// Last updated 9/17/2023
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //"Busy" means the player is in the middle of dashing or attacking and cannot perform normal inputs.
    //"Free" means they are not busy, and are free to move normally.
    enum state { free, busy };
    state playerState = state.free;
    [SerializeField] float horizontalSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float slowFallSpeed;
    [SerializeField] float slideFallSpeed;
    [SerializeField] float raycastMargin; //how far raycasts extend outside of the player's collider

    [SerializeField] float movementControl = 1f;
    [SerializeField] float controlRecoveryRate;
    [SerializeField] float totalControlThreshold;

    LayerMask ground;
    
    Rigidbody2D rb2d;
    
    Collider2D col2d;

    [SerializeField] Vector2 forward = Vector2.right; //horizontal direction the player is facing
    [SerializeField] bool isWalking = false;
    [SerializeField] bool isSliding = false;

    [SerializeField] int maxDashes;
    [SerializeField] int remainingDashes = 0;

    private void Awake() {
        ground = LayerMask.GetMask("Ground");
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        remainingDashes = maxDashes;
    }
    void Start()
    {
        
    }
    void Update()
    {
        switch (playerState) {
            case state.free:
                FreeUpdate();
                break;
            case state.busy:
                break;
        }
    }
    private void FixedUpdate() {
        switch (playerState) {
            case state.free:
                FreeFixedUpdate();
                break;
            case state.busy:
                break;
        }
    }

    //update method that runs when the player is able to move freely and not busy with a move
    bool jumpQueued = false;
    private void FreeUpdate() {
        if (Input.GetButtonDown("Jump")) {
            jumpQueued = true;
        }
    }
    private void FreeFixedUpdate() {
        if (OnGround()) {
            isSliding = false;
            movementControl = 1.1f;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(0f, rb2d.velocity.y));
            ResetJumps();
            if (jumpQueued) {
                Jump();
            }
        }
        else {
            FreeFallUpdate();
        }
        isWalking = false;
        if (!isSliding)
            FreeMovement();
        jumpQueued = false;
    }

    //read player inputs to determine horizontal speed
    private void FreeMovement() {
        float inputX = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(inputX) > 0.1f) {
            forward = new Vector2(inputX, 0f).normalized;
            isWalking = true;
        } else {
            inputX = 0f;
            isWalking = false;
        }
        float newSpeed = ApplyInputMovement(inputX);
        rb2d.velocity = new Vector2(newSpeed, rb2d.velocity.y);
    }
    private float ApplyInputMovement(float inputX) {
        float newSpeed = inputX * horizontalSpeed;
        if (movementControl < 1f) {
            //if the player does not have 100% control of their movement, their inputs have less influence over the actual speed of the character
            float controlledSpeed = newSpeed * movementControl;
            float existingSpeed = rb2d.velocity.x * (1f - movementControl);
            newSpeed = existingSpeed + controlledSpeed;
        }
        return newSpeed;
    }

    //runs when the player is in the air and not busy
    private void FreeFallUpdate() {
        ControlUpdate();
        if (!Input.GetButton("Jump") && rb2d.velocity.y > 0f) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        }

        if (isSliding && Input.GetAxisRaw("Vertical") <= -0.1f)
            isSliding = false;

        if (rb2d.velocity.y <= -slideFallSpeed && isSliding)
            rb2d.velocity = new Vector2(0f, -slideFallSpeed);
        else if (!isSliding && Input.GetButton("Jump") && rb2d.velocity.y <= -slowFallSpeed)
            rb2d.velocity = new Vector2(rb2d.velocity.x, -slowFallSpeed);


        bool facingWall = FacingWall();
        if (!facingWall) {
            isSliding = false;
        }
        else if (isWalking) {
            isSliding = true;
        }
        if (jumpQueued) {
            if (isSliding == true) {
                WallJump();
                ResetJumps();
            }
            else {
                Jump();
            }
        }
    }

    //cause the player to jump
    private void Jump() {
        jumpQueued = false;
        movementControl = 1f;
        if (OnGround()) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
        else if (remainingDashes > 0) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            remainingDashes--;
        }
    }

    //wall jump can only be performed while sliding
    private void WallJump() {
        jumpQueued = false;
        isSliding = false;
        rb2d.velocity = new Vector2(-forward.x * horizontalSpeed, jumpForce);
        movementControl = 0f;
    }
    private void ResetJumps() {
        remainingDashes = maxDashes;
    }
    private void ControlUpdate() {
        movementControl += controlRecoveryRate;
        if (movementControl >= totalControlThreshold)
            movementControl = 1.1f;
    }
    private bool OnGround() {
        RaycastHit2D groundCheck = Physics2D.BoxCast(transform.position, (Vector2)col2d.bounds.size - new Vector2(raycastMargin, raycastMargin), 0f, Vector2.down, raycastMargin * 1.5f, ground);
        //RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, col2d.bounds.extents.y + raycastMargin, ground);
        return groundCheck;
    }
    private bool FacingWall() {
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, forward, col2d.bounds.extents.x + raycastMargin, ground);
        return wallCheck;
    }
}
