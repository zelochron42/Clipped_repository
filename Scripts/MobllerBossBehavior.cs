using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enemy AI script controlling the Mobller boss enemy (final boss)
/// Written by Joshua Cashmore, last updated 11/14/2023
/// </summary>
public class MobllerBossBehavior : MonoBehaviour
{
    enum attack {none, pursuit, leftSlam, putridBreath, rightHook};
    attack currentState = attack.none;
    Rigidbody2D rb2d;
    Animator anim;
    [SerializeField] Rigidbody2D player;

    [SerializeField] float maxHorizontalSpeed = 1f;
    [SerializeField] float horizontalAcceleration = 0.1f;
    [SerializeField] float attackDelay = 2f;

    [SerializeField] Collider2D LeftFistCollider;
    [SerializeField] Collider2D RightFistCollider;
    [SerializeField] float fistSlamRange = 1f;

    float timeIdle = 0f;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState) {
            case attack.none:
                if (player != null)
                    FreeUpdate();
                break;
            case attack.pursuit:
                PunchSequenceUpdate();
                break;
        }
    }
    void FreeUpdate() {
        timeIdle += Time.fixedDeltaTime;
        if (timeIdle > attackDelay) {
            currentState = attack.pursuit;
        }
        /*
        int attackIndex = Random.Range(1, 3);
        currentState = (attack)attackIndex;
        */
    }
    void PunchSequenceUpdate() {
        float xDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(xDiff) <= fistSlamRange) {
            int attackNum = Random.Range(0, 2);
            if (attackNum == 0)
                FistSlam();
            else
                FistSweep();
        }
        else {
            PursuePlayer(xDiff);
        }
    }
    void PursuePlayer(float xDiff) {
        float rbSpeed = rb2d.velocity.x;
        if (xDiff < 0) {
            rbSpeed -= horizontalAcceleration;
            rbSpeed = Mathf.Max(rbSpeed, -maxHorizontalSpeed);
        }
        else if (xDiff > 0) {
            rbSpeed += horizontalAcceleration;
            rbSpeed = Mathf.Min(rbSpeed, maxHorizontalSpeed);
        }
        else {
            Debug.LogError("Impossible player distance");
            return;
        }
        rb2d.velocity = new Vector2(rbSpeed, 0f);
    }
    void FistSlam() {
        currentState = attack.leftSlam;
        rb2d.velocity *= 0f;
        anim.SetTrigger("LeftSmash");
    }

    void FistSweep() {
        currentState = attack.rightHook;
        rb2d.velocity *= 0f;
        anim.SetTrigger("RightHook");
    }
    public void ReturnToIdle() {
        timeIdle = 0f;
        currentState = attack.none;
    }

    //separate methods for enabling and disabling each collider because unity animation events are too limited
    public void EnableLeftFistCollider() {
        LeftFistCollider.enabled = true;
    }
    public void DisableLeftFistCollider() {
        LeftFistCollider.enabled = false;
    }
    public void EnableRightFistCollider() {
        RightFistCollider.enabled = true;
    }
    public void DisableRightFistCollider() {
        RightFistCollider.enabled = false;
    }

}
