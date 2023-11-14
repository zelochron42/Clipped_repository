using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalkerEnemyBehavior : MonoBehaviour
{
    Rigidbody2D rb2d;
    Collider2D col2d;

    [SerializeField] float walkSpeed;
    [SerializeField] float knockbackRecoveryTime = 0.25f;
    [SerializeField] float proximityThreshold = 0.5f;
    [SerializeField] int wanderIndex;
    public Vector2[] wanderPoints;
    Vector2[] worldWanderPoints;

    [SerializeField] Transform model;

    bool isWalking = true;

    public UnityEvent<bool> ChangeDirection;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        worldWanderPoints = new Vector2[wanderPoints.Length];
        for (int i = 0; i < wanderPoints.Length; i++) {
            worldWanderPoints[i] = wanderPoints[i] + (Vector2)transform.position;
        }
    }

    void Update()
    {
        if (isWalking) {
            WalkUpdate();
        }
    }
    void WalkUpdate() {
        Vector2 goalPos = worldWanderPoints[wanderIndex];

        float toGoal = Vector2.Distance(transform.position, goalPos);

        if (toGoal <= proximityThreshold) {
            wanderIndex = (wanderIndex + 1) % worldWanderPoints.Length;

            bool newDir = (worldWanderPoints[wanderIndex].x - transform.position.x) > 0f;

            ChangeDirection.Invoke(newDir);

            if (newDir) {
                model.localScale = new Vector3(1f, 1f, -1f);
            }
            else {
                model.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else {
            float xDiff = goalPos.x - transform.position.x;
            float xDir = 0f;
            if (xDiff != 0f)
                xDir = xDiff / Mathf.Abs(xDiff);

            rb2d.velocity = new Vector2(walkSpeed * xDir, rb2d.velocity.y);
        }
    }

    public void Knockback(Vector2 dir) {
        isWalking = false;
        rb2d.velocity = dir * 2f;
        Debug.Log("received knockback in direction " + dir);
        StopAllCoroutines();
        StartCoroutine("KnockbackRecovery");
    }
    IEnumerator KnockbackRecovery() {
        yield return new WaitForSeconds(knockbackRecoveryTime);
        isWalking = true;
        yield break;
    }

    private void OnDrawGizmos() {
        if (!Application.isPlaying) {
            foreach (Vector2 p in wanderPoints) {
                Vector2 mod = transform.position;
                Gizmos.DrawSphere(p + mod, 0.2f);
            }
        }
        else {
            foreach (Vector2 p in worldWanderPoints) {
                Gizmos.DrawSphere(p, 0.15f);
            }
        }
    }
}
