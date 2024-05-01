using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enemy AI script controlling the Mobller boss enemy (final boss)
/// Written by Joshua Cashmore
/// </summary>
public class MobllerBossBehavior : MonoBehaviour
{
    enum attack {none, pursuit, leftSlam, putridBreath, rightHook, overhead};
    [SerializeField] attack currentState = attack.none;
    Rigidbody2D rb2d;
    Animator anim;
    [SerializeField] Rigidbody2D player;

    [SerializeField] float maxHorizontalSpeed = 1f;
    [SerializeField] float horizontalAcceleration = 0.1f;
    [SerializeField] float attackDelay = 2f;
    [SerializeField] float rageDelay = 0.25f;
    [SerializeField] float rageThreshold = 35f;

    [SerializeField] float projectileForce;
    [SerializeField] float projectileSpread;

    [SerializeField] MeshRenderer headModel;
    [SerializeField] Material damageMaterial;
    [SerializeField] Material headBaseMaterial;

    [SerializeField] Collider2D LeftFistCollider;
    [SerializeField] Collider2D RightFistCollider;
    [SerializeField] Rigidbody2D projectilePrefab;
    [SerializeField] Transform projectileLaunch;
    [SerializeField] float fistSlamRange = 1f;
    [SerializeField] float antiAirThreshold;

    [SerializeField] float healthPercentage = 100f;

    [SerializeField] float xLowerLimit;
    [SerializeField] float xUpperLimit;

    [SerializeField] bool activated = false;

    float timeIdle = 0f;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        headBaseMaterial = headModel.material;
    }
    private void LateUpdate() {
        if (!activated)
            return;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xLowerLimit, xUpperLimit), transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!activated)
            return;
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
        if (timeIdle > attackDelay || (healthPercentage < rageThreshold && timeIdle < rageDelay)) {
            currentState = attack.pursuit;
        }
        /*
        int attackIndex = Random.Range(1, 3);
        currentState = (attack)attackIndex;
        */
    }
    void PunchSequenceUpdate() {
        float xDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(xDiff) <= fistSlamRange
            || Mathf.Abs(xLowerLimit - transform.position.x) < fistSlamRange && xDiff < 0f
            || Mathf.Abs(xUpperLimit - transform.position.x) < fistSlamRange && xDiff > 0f) {

            if (player.position.y < antiAirThreshold)
                RandomAttack();
            else
                RandomOverhead();
        }
        else {
            PursuePlayer(xDiff);
        }
    }
    void RandomAttack() {
        int attackNum = Random.Range(0, 3);
            if (attackNum == 0)
                FistSlam();
            else if (attackNum == 1)
                FistSweep();
            else
                ProjectileAttack();
    }
    void RandomOverhead() {
        int attackNum = Random.Range(0, 2);
        if (attackNum == 0)
            OverheadClap();
        else
            ProjectileAttack();
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
    void ProjectileAttack() {
        currentState = attack.putridBreath;
        rb2d.velocity *= 0f;
        anim.SetTrigger("Vomit");
    }

    void OverheadClap() {
        currentState = attack.overhead;
        rb2d.velocity *= 0f;
        anim.SetTrigger("Overhead");
    }
    public void LaunchProjectile() {
        Rigidbody2D newProjectile = Instantiate(projectilePrefab);
        Destroy(newProjectile.gameObject, 10f);
        newProjectile.transform.position = projectileLaunch.position;
        float launchAngle = 90f + Random.Range(-projectileSpread/2f, projectileSpread/2f);
        float launchRadian = launchAngle * Mathf.Deg2Rad;
        Vector2 launchVector = new Vector2(Mathf.Cos(launchRadian), Mathf.Sin(launchRadian));
        newProjectile.velocity = launchVector * projectileForce;
    }
    public void ReturnToIdle() {
        timeIdle = 0f;
        currentState = attack.none;
    }

    public void DamageBlink() {
        headModel.material = damageMaterial;
        StartCoroutine("DamageUnblink");
    }
    IEnumerator DamageUnblink() {
        yield return new WaitForSeconds(0.05f);
        headModel.material = headBaseMaterial;
        yield break;
    }

    public void UpdateHealth(float hp) {
        healthPercentage = hp * 100f;
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

    public void Activate() {
        activated = true;
    }
}
