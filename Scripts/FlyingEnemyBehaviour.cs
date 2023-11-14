using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyingEnemyBehaviour : MonoBehaviour
{
    Rigidbody2D rb2d;
    Collider2D col2d;

    [SerializeField] float flySpeed;
    [SerializeField] float knockbackRecoveryTime = 0.25f;
    [SerializeField] float proximityThreshold = 0.25f;

    [SerializeField] float detectionRadius = 1f;
    [SerializeField] float chaseRadius = 1f;
    float activeRadius;
    Vector2 startPos;

    [SerializeField] Transform model;

    bool isWalking = true;

    public UnityEvent<bool> ChangeDirection;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        startPos = transform.position;
        activeRadius = detectionRadius;
    }

    void Update() {
        if (isWalking) {
            WalkUpdate();
        }
    }
    void WalkUpdate() {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, activeRadius);
        foreach (Collider2D r in results) {
            if (r.CompareTag("Player")) {
                ChaseUpdate(r.transform);
                return;
            }
        }
        activeRadius = detectionRadius;
        MoveTowards(startPos);
    }
    void ChaseUpdate(Transform player) {
        activeRadius = chaseRadius;
        MoveTowards(player.position);
    }
    void MoveTowards(Vector2 goalPoint) {
        if (Vector2.Distance(transform.position, goalPoint) > proximityThreshold) {
            Vector2 chaseVector = goalPoint - (Vector2)transform.position;
            rb2d.velocity = chaseVector.normalized * flySpeed;
            bool newDir = (goalPoint.x - transform.position.x) > 0f;
            ChangeDirection.Invoke(newDir);
            if (newDir) {
                model.localScale = new Vector3(1f, 1f, -1f);
            }
            else {
                model.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else {
            rb2d.velocity = Vector2.zero;
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
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(detectionRadius, activeRadius));
    }
}
