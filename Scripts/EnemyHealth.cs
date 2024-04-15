using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public bool respawns = true;
    [SerializeField] bool hitstopWhenDamaged = true;
    [SerializeField] float despawnTimeAfterDeath = 0f;
    bool dead = false;
    [SerializeField] float maxHealth;
    [SerializeField] float health;
    public UnityEvent<Vector2> DamageReceived;
    public UnityEvent<float> HealthUpdate; //0 = dead, 1 = full health
    public UnityEvent Death;


    public bool damagePlayerOnContact = true;
    public void AttackDamage(Vector2 attackDirection) {
        health -= 1f;
        DamageReceived.Invoke(attackDirection);
        HealthUpdate.Invoke(health / maxHealth);
    }
    private void Awake() {
        health = maxHealth;
    }
    void Start()
    {
        if (respawns) {
            respawns = false;
            EnemyRespawner er = FindObjectOfType<EnemyRespawner>();
            if (er)
                er.AddToRespawn(this);
        }
        if (!hitstopWhenDamaged)
            return;
        PauseMenu pm = FindObjectOfType<PauseMenu>();
        DamageReceived.AddListener((Vector2 _) => { pm.HitStop(); });

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<TargetedCollider>()) {
            Vector2 direction = collision.transform.right;
            AttackDamage(direction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f && !dead) {
            dead = true;
            Death.Invoke();
            Destroy(gameObject, despawnTimeAfterDeath);
        }
    }
}
