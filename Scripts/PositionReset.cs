using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug script for resetting player position as necessary
/// Written by Joshua Cashmore
/// last updated 3/6/2024
/// </summary>

public class PositionReset : MonoBehaviour
{
    [SerializeField] float resetThreshold = -500f;
    PlayerStats stats;
    Vector2 startPos;
    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        startPos = transform.position;
    }
    void Update()
    {
        if (enabled && Input.GetKeyDown(KeyCode.R)) {
            ResetPos();
        } else if (transform.position.y <= resetThreshold) {
            stats.Damage(transform.position);
            ResetPos();
        }
    }

    public void ResetPos() {
        transform.position = startPos;
        rb2d.velocity = Vector2.zero;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, (GetComponent<Collider2D>().bounds.size));
    }
}
