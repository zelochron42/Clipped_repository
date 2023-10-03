using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Code to control the player's movement
/// Written by Joshua Cashmore,
/// Last updated 10/3/2023
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //"Busy" means the player is in the middle of dashing or attacking and cannot perform normal inputs.
    //"Free" means they are not busy, and are free to move normally.
    enum state { free, busy };
    state playerState = state.free;

    [Header("Movement stats")]

    [Tooltip("Walking speed on ground and in air")]
    [SerializeField] float horizontalSpeed;
    [Tooltip("Upward velocity set at the start of a jump")]
    [SerializeField] float jumpForce;
    [Tooltip("Capped downward velocity when holding jump")]
    [SerializeField] float slowFallSpeed;
    [Tooltip("Capped downward velocity while falling")]
    [SerializeField] float slideFallSpeed;
    [Tooltip("Omnidirectional velocity during a dash")]
    [SerializeField] float dashForce;
    [Tooltip("Duration of dash, during which player is unaffected by gravity")]
    [SerializeField] float dashTime;
    [Tooltip("Lifetime of the physical attack object")]
    [SerializeField] float attackTime;
    [Tooltip("Time between the start of an attack, and returning control to the player")]
    [SerializeField] float attackRecovery;
    [Tooltip("Force with which the player is bounced away from hit targets")]
    [SerializeField] float attackBouncebackForce;

    [Header("Technical data")]
    [SerializeField] float raycastMargin; //how far raycasts extend outside of the player's collider
    [SerializeField] float movementControl = 1f;
    [SerializeField] float controlRecoveryRate;
    [SerializeField] float totalControlThreshold;

    LayerMask ground;
    
    Rigidbody2D rb2d;
    
    Collider2D col2d;

    [SerializeField] TargetedCollider slashObject;

    [Header("Realtime variables")]
    [SerializeField] Vector2 forward = Vector2.right; //horizontal direction the player is facing
    [SerializeField] bool isWalking = false;
    [SerializeField] bool isSliding = false;

    [SerializeField] int maxDashes;
    [SerializeField] int remainingDashes = 0;

    [SerializeField] int maxJumps;
    [SerializeField] int remainingJumps = 0;

    bool isJumping = false;

    float gravityScale;

    public UnityEvent<bool> ChangeDirection; //fires when 'forward' variable changes
    public UnityEvent StartWalk;
    public UnityEvent StartIdle;
    public UnityEvent StartJump;
    public UnityEvent StartWalljump;

    private void Awake() {
        ground = LayerMask.GetMask("Ground");
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        gravityScale = rb2d.gravityScale;
        ResetJumps();
        StartIdle.Invoke();
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
    
    bool jumpQueued = false;
    bool attackQueued = false;
    bool dashQueued = false;

    //update method that runs when the player is able to move freely and not busy with a move
    private void FreeUpdate() {
        if (Input.GetButtonDown("Jump")) {
            jumpQueued = true;
        }
        if (Input.GetButtonDown("Fire1")) {
            attackQueued = true;
        }
        if (Input.GetButtonDown("Fire2")) {
            dashQueued = true;
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
        if (!isSliding)
            FreeMovement();
        if (dashQueued) {
            Dash();
        }
        else if (attackQueued) {
            Attack();
        }
        jumpQueued = false;
        attackQueued = false;
        dashQueued = false;
    }

    //read player inputs to determine horizontal speed
    private void FreeMovement() {
        float inputX = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(inputX) > 0.1f) {
            SetForwardDirection(new Vector2(inputX, 0f));
            if (!isWalking) {
                isWalking = true;
                if (OnGround())
                    StartWalk.Invoke();
            }
        } else {
            inputX = 0f;
            if (isWalking) {
                isWalking = false;
                if (OnGround())
                    StartIdle.Invoke();
            }
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
        if (!Input.GetButton("Jump") && rb2d.velocity.y > 0f && isJumping) {
            isJumping = false;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        }
        else if (rb2d.velocity.y <= 0f) {
            isJumping = false;
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
            isWalking = false;
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
        if (OnGround()) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            isJumping = true;
            movementControl = 1f;
            StartJump.Invoke();
        }
        else if (remainingJumps > 0) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            remainingJumps--;
            isJumping = true;
            movementControl = 1f;
            StartJump.Invoke();
        }
    }

    //wall jump can only be performed while sliding
    private void WallJump() {
        StartWalljump.Invoke();
        jumpQueued = false;
        isSliding = false;
        rb2d.velocity = new Vector2(-forward.x * horizontalSpeed, jumpForce);
        isJumping = true;
        movementControl = 0f;
    }

    private void Dash() {
        dashQueued = false;
        if (remainingDashes > 0) {
            remainingDashes--;
        }
        else {
            return;
        }

        playerState = state.busy;

        Vector2 initialVelocity = rb2d.velocity;
        Vector2 rawInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 dashDirection = forward;
        if (rawInputs.magnitude > 0.1f) {
            dashDirection = direction8(rawInputs);
        }

        rb2d.velocity = dashDirection.normalized * dashForce;
        rb2d.gravityScale = 0f;
        StartCoroutine("DashRecovery");
    }

    private void Attack() {
        attackQueued = false;

        playerState = state.busy;
        StartCoroutine("SwordAttack");
    }
    Vector2 lastAttackDirection = Vector2.zero;
    IEnumerator SwordAttack() {
        Vector2 rawInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 slashDirection = forward;
        if (rawInputs.magnitude > 0.1f) {
            slashDirection = direction8(rawInputs);
        }
        lastAttackDirection = slashDirection;

        TargetedCollider newSlash = Instantiate(slashObject);
        newSlash.transform.parent = transform;

        newSlash.SetTargets("Enemy", "Bounce");
        newSlash.TargetHit.AddListener(() => {
            AttackLanded();
            newSlash.TargetHit.RemoveAllListeners(); 
        });

        float slashLength = 0.5f;
        Collider2D slashCol = newSlash.GetComponent<Collider2D>();
        if (slashCol)
            slashLength = slashCol.bounds.extents.x;

        newSlash.transform.position = (Vector2)transform.position + slashDirection * slashLength;
        newSlash.transform.right = slashDirection;
        

        Destroy(newSlash.gameObject, attackTime);
        yield return new WaitForSeconds(attackRecovery);
        playerState = state.free;
        yield break;
    }
    private void AttackLanded() {
        Debug.Log("ATTACK LANDED");
        Vector2 bounceDirection = -lastAttackDirection;
        if (Mathf.Abs(bounceDirection.y) > 0.1f)
            rb2d.velocity = new Vector2(rb2d.velocity.x, attackBouncebackForce);
        else {
            movementControl = 0f;
            rb2d.velocity = bounceDirection * attackBouncebackForce;
        }
        ResetJumps();
    }

    IEnumerator DashRecovery() {
        yield return new WaitForSeconds(dashTime);
        playerState = state.free;
        rb2d.gravityScale = gravityScale;
        movementControl = 0f;
        yield break;
    }

    private void ResetJumps() {
        remainingDashes = maxDashes;
        remainingJumps = maxJumps;
        rb2d.gravityScale = gravityScale;
    }
    private void SetForwardDirection(Vector2 newDir) {
        newDir = newDir.normalized;
        if (forward != newDir) {
            ChangeDirection.Invoke(newDir == Vector2.right);
            forward = newDir;
        }
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

    //method that crushes all possible input directions into 1 of 8 possibilities
    private Vector2 direction8(Vector2 inputDirection) {
        Vector2 norm = inputDirection.normalized;
        
        //get the angle of the inputted vector2 in degrees
        float dir = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;

        //divide by 45, round, then multiply by 45 to make it snap to the nearest 45 degree angle
        float crushedDir = Mathf.RoundToInt(dir / 45f) * 45f;

        //convert back from degrees to radians
        float finalDir = crushedDir * Mathf.Deg2Rad;

        //calculate the new vector
        Vector2 newDirection = new Vector2(Mathf.Cos(finalDir), Mathf.Sin(finalDir));

        return newDirection;
    }


    public void PrintInput(string input) {
        Debug.Log(input);
    }
}
