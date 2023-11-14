using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public bool respawns = true;
    [SerializeField] float despawnTimeAfterDeath = 0f;
    bool dead = false;
    [SerializeField] float maxHealth;
    public UnityEvent<Vector2> DamageReceived;
    public UnityEvent Death;
    float health;
    public void AttackDamage(Vector2 attackDirection) {
        health -= 1f;
        DamageReceived.Invoke(attackDirection);
    }
    private void Awake() {
        health = maxHealth;
    }
    void Start()
    {
        if (respawns) {
            respawns = false;
            EnemyRespawner er = FindObjectOfType<EnemyRespawner>();
            er.AddToRespawn(this);
        }

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
