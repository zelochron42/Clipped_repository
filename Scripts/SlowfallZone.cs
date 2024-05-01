using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowfallZone : MonoBehaviour
{
    BoxCollider2D col2d;
    [SerializeField] float velocityLowerLimit = -6f;
    Collider2D[] CheckOverlaps() {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(col2d.bounds.center, col2d.bounds.size, 0f);
        return overlaps;
    }
    // Start is called before the first frame update
    void Start()
    {
        col2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Collider2D[] colliders = CheckOverlaps();
        foreach (Collider2D c in colliders) {
            PlayerMovement player = c.gameObject.GetComponent<PlayerMovement>();
            if (player && !player.isDashing) {
                Rigidbody2D rb2d = c.GetComponent<Rigidbody2D>();
                if (!rb2d)
                    return;
                if (rb2d.velocity.y < velocityLowerLimit) {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, velocityLowerLimit);
                }
            }
        }
    }
}
