using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalkerEnemyBehavior : MonoBehaviour
{
    Rigidbody2D rb2d;
    Collider2D col2d;

    [SerializeField] float walkSpeed;

    [SerializeField] float proximityThreshold = 0.5f;
    [SerializeField] int wanderIndex;
    [SerializeField] Vector2[] wanderPoints;

    [SerializeField] Transform model;

    bool isWalking = true;

    public UnityEvent<bool> ChangeDirection;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        for (int i = 0; i < wanderPoints.Length; i++) {
            wanderPoints[i] = wanderPoints[i] + (Vector2)transform.position;
        }
    }

    void Update()
    {
        if (isWalking) {

            WalkUpdate();
        }
    }
    void WalkUpdate() {
        Vector2 goalPos = wanderPoints[wanderIndex];

        float toGoal = Vector2.Distance(transform.position, goalPos);

        if (toGoal <= proximityThreshold) {
            wanderIndex = (wanderIndex + 1) % wanderPoints.Length;

            bool newDir = (wanderPoints[wanderIndex].x - transform.position.x) > 0f;

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

    private void OnDrawGizmos() {
        foreach (Vector2 p in wanderPoints) {
            Vector2 mod = Vector2.zero;
            if (Application.isEditor)
                mod = transform.position;
            Gizmos.DrawSphere(p + mod, 0.2f);
        }
    }
}
