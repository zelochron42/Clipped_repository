using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForninBossBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Vector2[] movePoints;
    [SerializeField] Rigidbody2D fireballPrefab;
    [SerializeField] int fireballCount = 3;
    [SerializeField] float fireballWindup = 0.5f;
    [SerializeField] float fireballDelay = 0.25f;
    [SerializeField] float fireballCooldown = 0.5f;
    [SerializeField] float fireballLifetime = 10f;
    [SerializeField] float fireballSpeed = 8f;
    [SerializeField] float fireballOffsetDegrees = 10f;
    [SerializeField] float goalResetThreshold = 0.5f;
    [SerializeField] float maxSpeed = 3f;
    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float turningSpeed = 45f;
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float proximityVelocityDampening = 3f;

    [SerializeField] bool activated = false;
    Rigidbody2D rb2d;
    Collider2D col2d;
    Transform player;
    bool neutral = true;

    Vector2 goalPos;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        player = FindObjectOfType<PlayerMovement>().transform;
        RandomGoal();
    }

    private void FixedUpdate() {
        if (activated && neutral)
            NeutralUpdate();
    }

    void NeutralUpdate() {
        float dist = Vector2.Distance(transform.position, goalPos);
        if (dist <= goalResetThreshold) {
            RandomGoal();
            rb2d.velocity = Vector2.zero;
            neutral = false;
            StartCoroutine(ShootRoutine());
        }
        else {
            //VelocityAim();
            AngleAim();
        }
    }

    void AngleAim() {
        if (rb2d.velocity.magnitude <= acceleration * Time.deltaTime) {
            VelocityAim();
            return;
        }
        //angles measured in radians

        //define current and desired directions
        Vector2 goalDir = (goalPos - (Vector2)transform.position).normalized;
        float goalAngle = Mathf.Atan2(goalDir.y, goalDir.x);
        Vector2 currentDir = rb2d.velocity.normalized;
        float currentAngle = Mathf.Atan2(currentDir.y, currentDir.x);
        float angleDiff = goalAngle - currentAngle;

        //
        if (angleDiff >= Mathf.Deg2Rad * 180f) {
            currentAngle += Mathf.Deg2Rad * 360f;
            angleDiff = goalAngle - currentAngle;
        }

        Vector2 finalDir = goalDir;
        if (Mathf.Abs(angleDiff) >= Mathf.Deg2Rad * turningSpeed * Time.deltaTime) {
            float angleMultiplier = Mathf.Abs(angleDiff) / angleDiff;
            float newAngle = currentAngle + Mathf.Deg2Rad * turningSpeed * Time.deltaTime * angleMultiplier;
            Vector2 newDir = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
            finalDir = newDir;
        }

        float currentSpeed = rb2d.velocity.magnitude;
        float goalSpeed = ClampedMaxSpeed();
        float speedDiff = goalSpeed - currentSpeed;
        float speedMultiplier = 0f;
        if (Mathf.Abs(speedDiff) > 0f)
            speedMultiplier = speedDiff / Mathf.Abs(speedDiff);
        float newSpeed = rb2d.velocity.magnitude + acceleration * Time.deltaTime * speedMultiplier;
        rb2d.velocity = finalDir * newSpeed;
    }
    void VelocityAim() {
        Vector2 dir = goalPos - (Vector2)transform.position;
        rb2d.velocity += dir.normalized * acceleration * Time.fixedDeltaTime;
        if (rb2d.velocity.magnitude >= maxSpeed) {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }
    }

    void RandomGoal() {
        List<Vector2> culledPoints = new List<Vector2>();
        Vector2 closestPoint = Vector2.zero;
        float distance = Mathf.Infinity;
        foreach (Vector2 v2 in movePoints) {
            float currentDistance = Vector2.Distance(v2, transform.position);
            if (currentDistance <= distance) {
                closestPoint = v2;
                distance = currentDistance;
            }
        }
        foreach (Vector2 v2 in movePoints) {
            if (v2 != closestPoint) {
                culledPoints.Add(v2);
            }
        }
        if (culledPoints.Count == 0) {
            culledPoints.Add(movePoints[Random.Range(0, movePoints.Length)]);
        }
        goalPos = culledPoints[Random.Range(0, culledPoints.Count)];
    }

    public void DamageKnockback(Vector2 direction) {
        Vector2 force = direction * knockbackForce;
        rb2d.velocity = force;
        RandomGoal();
    }

    float ClampedMaxSpeed() {
        float goalDistance = Vector2.Distance(goalPos, transform.position);
        float clampedMax = Mathf.Clamp(goalDistance * proximityVelocityDampening, minSpeed, maxSpeed);
        return clampedMax;
    }
    private void OnDrawGizmos() {
        foreach (Vector2 point in movePoints) {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }

    IEnumerator ShootRoutine() {
        yield return new WaitForSeconds(fireballWindup);
        for (int i = 0; i < fireballCount; i++) {
            yield return new WaitForSeconds(fireballDelay);
            ShootFireball();
        }
        yield return new WaitForSeconds(fireballCooldown);
        neutral = true;
        yield break;
    }

    void ShootFireball() {
        Rigidbody2D newFireball = Instantiate(fireballPrefab);
        newFireball.transform.position = transform.position;
        Vector2 aimDir = (player.position - newFireball.transform.position).normalized;
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x);
        aimAngle += Mathf.Deg2Rad * Random.Range(-fireballOffsetDegrees, fireballOffsetDegrees);
        newFireball.velocity = new Vector2(Mathf.Cos(aimAngle), Mathf.Sin(aimAngle)) * fireballSpeed;
        Destroy(newFireball.gameObject, fireballLifetime);
    }

    public void PrebattleReposition() {
        StartCoroutine("RepositionTween");
    }
    [SerializeField] Vector2 repositionGoal;
    [SerializeField] int repositionSteps;
    IEnumerator RepositionTween() {
        Vector2 startPos = transform.position;
        for (int i = 0; i < repositionSteps; i++) {
            transform.position = Vector2.Lerp(startPos, repositionGoal, (float)i / repositionSteps);
            yield return null;
        }
        yield break;
    }

    public void StartFight() {
        activated = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
