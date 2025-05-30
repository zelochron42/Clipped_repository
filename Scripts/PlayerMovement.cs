using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Code to control the player's movement
/// Written by Joshua Cashmore,
/// Last updated 3/13/2024
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //"Busy" means the player is in the middle of dashing or attacking and cannot perform normal inputs.
    //"Free" means they are not busy, and are free to move normally.
    public enum state { free, busy };
    public state playerState = state.free;

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
    [Tooltip("Time it takes for attack object to be cleaned up if its script fails")]
    [SerializeField] float attackTime;
    [Tooltip("Time between the start of an attack, and returning control to the player")]
    [SerializeField] float attackRecovery;
    [Tooltip("Time it takes to be able to attack again after regaining control")]
    [SerializeField] float attackCooldown;
    [Tooltip("Force with which the player is bounced away from hit targets")]
    [SerializeField] float attackBouncebackForce;
    [SerializeField] float bounceUpLimit;

    [SerializeField] bool autoAimAttacks = false;

    [Header("Technical data")]
    public bool ignoreInputs = false;
    [SerializeField] float knockbackForce;
    [SerializeField] float raycastMargin; //how far raycasts extend outside of the player's collider
    [SerializeField] float movementControl = 1f;
    [SerializeField] float controlRecoveryRate;
    [SerializeField] float totalControlThreshold;
    [SerializeField] PhysicsMaterial2D friction;
    [SerializeField] PhysicsMaterial2D frictionless;

    [SerializeField] Transform rigContainer;

    LayerMask ground;
    LayerMask wall;
    
    Rigidbody2D rb2d;
    
    Collider2D col2d;

    [SerializeField] TargetedCollider slashObject;

    [Header("Realtime variables")]
    [SerializeField] Vector2 forward = Vector2.right; //horizontal direction the player is facing
    [SerializeField] bool isWalking = false;

    public bool isSliding = false;

    [SerializeField] int maxDashes;
    [SerializeField] int groundDashes;
    [SerializeField] int remainingDashes = 0;

    [SerializeField] int maxJumps;
    [SerializeField] int remainingJumps = 0;

    bool isJumping = false;
    public bool isDashing = false;

    float gravityScale;
    [SerializeField] float gravityPercentage = 100f;
    [SerializeField] float gravityRecoveryPerFrame = 1.67f;
    [SerializeField] float hoverParticleThreshold = 60f;
    [SerializeField] ParticleSystem hoverParticles;

    public UnityEvent<bool> ChangeDirection; //fires when 'forward' variable changes
    public UnityEvent StartWalk;
    public UnityEvent StartIdle;
    public UnityEvent StartJump;
    public UnityEvent StartWalljump;
    public UnityEvent StartDash;
    public UnityEvent EndDash;
    public UnityEvent AttackStart;
    public UnityEvent UIUpdate;
    public UnityEvent AttackBounce;

    public void SetInputs(bool canInput) {
        if (canInput)
            playerState = state.free;
        else {
            playerState = state.busy;
            StopAllCoroutines();
            canAttack = true;
            rigContainer.up = Vector2.up;
            rb2d.gravityScale = gravityScale;
            rb2d.velocity = Vector2.zero;
            rb2d.sharedMaterial = friction;
            hoverParticles.Stop();
            movementControl = 0f;
            if (isDashing)
                EndDash.Invoke();
            StartIdle.Invoke();
        }
    }
    public void End() {
        SetInputs(false);
        enabled = false;
    }


    private void Awake() {
        ground = LayerMask.GetMask("Ground", "Ice");
        wall = LayerMask.GetMask("Ground");
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
        isDashing = false;
        if (OnGround()) {
            isSliding = false;
            movementControl = 1.1f;
            //rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(0f, rb2d.velocity.y));
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
        else if (attackQueued && !isSliding) {
            Attack();
        }
        jumpQueued = false;
        attackQueued = false;
        dashQueued = false;
    }

    //read player inputs to determine horizontal speed
    private void FreeMovement() {
        rb2d.sharedMaterial = frictionless;
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
            rb2d.sharedMaterial = friction;
            if (isWalking) {
                isWalking = false;
                if (OnGround()) {
                    StartIdle.Invoke();
                }
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

        if (isSliding && Mathf.Abs(Input.GetAxisRaw("Horizontal") - forward.x) > 1.1f) { //if horizontal input is going away from the wall, drop off the wall
            isSliding = false;
        }

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
            ResetJumps();
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
            gravityPercentage = 100f;
            StartJump.Invoke();
        }
        else if (remainingJumps > 0) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            remainingJumps--;
            isJumping = true;
            movementControl = 1f;
            gravityPercentage = 100f;
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
        gravityPercentage = 100f;
    }

    private void Dash() {
        dashQueued = false;
        if (remainingDashes > 0) {
            remainingDashes--;
            UIUpdate.Invoke();
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
        rigContainer.up = dashDirection;
        rb2d.velocity = dashDirection.normalized * dashForce;
        rb2d.gravityScale = 0f;
        isJumping = false;
        isSliding = false;
        gravityPercentage = 100f;
        isDashing = true;
        StartDash.Invoke();
        hoverParticles.Stop();
        StartCoroutine("DashRecovery");
    }

    bool canAttack = true;
    private void Attack() {
        attackQueued = false;
        if (canAttack) {
            canAttack = false;
            playerState = state.busy;
            AttackStart.Invoke();
            StartCoroutine("SwordAttack");
        }
    }
    Vector2 lastAttackDirection = Vector2.zero;
    bool lastAttackGrounded = false;
    IEnumerator SwordAttack() {
        Vector2 rawInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 slashDirection = forward;
        if (rawInputs.magnitude > 0.1f) {
            slashDirection = direction4(rawInputs);
        }
        if (OnGround() || isSliding)
            lastAttackGrounded = true;
        else {
            lastAttackGrounded = false;
            if (autoAimAttacks)
                slashDirection = direction8TargetLocator(slashDirection);
        }
        lastAttackDirection = slashDirection;

        TargetedCollider newSlash = Instantiate(slashObject);
        //newSlash.transform.parent = transform;
        ObjectFollower objfol = newSlash.gameObject.GetComponent<ObjectFollower>();
        if (objfol)
            objfol.SetFollow(transform);

        newSlash.SetTargets("Enemy", "Bounce", "Damage");
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
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        yield break;
    }
    private void AttackLanded() {
        Debug.Log("ATTACK LANDED");
        Vector2 bounceDirection = -lastAttackDirection;
        if (!lastAttackGrounded) {
            //rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(bounceDirection.y * attackBouncebackForce, bounceUpLimit));
            movementControl = 0.5f;
            if (bounceDirection.y <= 0f) {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(rb2d.velocity.y, bounceUpLimit));
                gravityPercentage = 0f;
            }
            else {
                rb2d.velocity = new Vector2(rb2d.velocity.x, bounceDirection.y * attackBouncebackForce);
            }
            RecoverDash();
            ResetJumps();
            AttackBounce.Invoke();
        }
        else {
            rb2d.velocity = new Vector2(bounceDirection.x * attackBouncebackForce, rb2d.velocity.y);
        }
    }

    IEnumerator DashRecovery() {
        yield return new WaitForSeconds(dashTime);
        EndDash.Invoke();
        yield return new WaitForSeconds(dashTime * 0.2f);
        rigContainer.up = Vector2.up;
        playerState = state.free;
        rb2d.gravityScale = gravityScale;
        movementControl = 0f;
        isDashing = false;
        yield break;
    }

    private void RecoverDash() {
        remainingDashes = Mathf.Min(remainingDashes + 1, maxDashes);
        UIUpdate.Invoke();
    }
    private void ResetJumps() {
        remainingDashes = Mathf.Max(remainingDashes, groundDashes);
        remainingJumps = maxJumps;
        rb2d.gravityScale = gravityScale;
        isJumping = false;
        UIUpdate.Invoke();
    }
    private void SetForwardDirection(Vector2 newDir) {
        newDir = newDir.normalized;
        if (forward != newDir) {
            ChangeDirection.Invoke(newDir == Vector2.right);
            forward = newDir;
        }
    }
    
    private void ControlUpdate() {
        gravityPercentage = Mathf.Min(gravityPercentage + gravityRecoveryPerFrame, 100f);
        rb2d.gravityScale = gravityScale * (gravityPercentage / 100f);
        if (gravityPercentage <= hoverParticleThreshold && !hoverParticles.isPlaying)
            hoverParticles.Play();
        else if (gravityPercentage > hoverParticleThreshold && hoverParticles.isPlaying)
            hoverParticles.Stop();

        movementControl += controlRecoveryRate;
        if (movementControl >= totalControlThreshold)
            movementControl = 1.1f;
    }
    public bool OnGround() {
        RaycastHit2D groundCheck = Physics2D.BoxCast(transform.position, (Vector2)col2d.bounds.size - new Vector2(raycastMargin, raycastMargin), 0f, Vector2.down, raycastMargin * 1.5f, ground);
        //RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, col2d.bounds.extents.y + raycastMargin, ground);
        return groundCheck;
    }
    private bool FacingWall() {
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, forward, col2d.bounds.extents.x + raycastMargin, wall);
        return wallCheck;
    }

    //scan 8 directions for a target to hit with the sword
    private Vector2 direction8TargetLocator(Vector2 defaultDirection) {
        float maxDistance = 4f;
        Vector2 currentDirection = defaultDirection;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxDistance, LayerMask.GetMask("Default"));
        foreach (Collider2D h in hits) {
            CompareClosestPoint(ref maxDistance, ref currentDirection, h);
        }
        Vector2 finalDirection = direction8(currentDirection);
        return finalDirection;
    }

    private void CompareClosestPoint(ref float maxDistance, ref Vector2 currentDirection, Collider2D h) {
        if (h.gameObject.CompareTag("Enemy") || h.gameObject.CompareTag("Bounce") || h.gameObject.CompareTag("Damage")) {
            Vector2 nearestPoint = h.ClosestPoint(transform.position);
            float thisDist = Vector2.Distance(transform.position, nearestPoint);
            if (thisDist < maxDistance) {
                maxDistance = thisDist;
                currentDirection = (nearestPoint - (Vector2)transform.position).normalized;
            }
        }
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
    private Vector2 direction4(Vector2 inputDirection) {
        Vector2 norm = inputDirection.normalized;
        Vector2 newDirection;

        if (norm.y < -0.1f)
            newDirection = Vector2.down;
        else if (norm.y > 0.1f)
            newDirection = Vector2.up;
        else
            newDirection = forward;
        return newDirection;
    }

    public void Knockback(Vector2 originPoint) {
        float xSpeed = 0f;
        if (originPoint.x > transform.position.x) {
            xSpeed = -1f;
        } else {
            xSpeed = 1f;
        }
        rb2d.velocity = new Vector2(xSpeed * knockbackForce, knockbackForce);
        isJumping = false;
        isSliding = false;
        movementControl = 0f;
    }

    public int GetDashes() {
        return remainingDashes;
    }
}
