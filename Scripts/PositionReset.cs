using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug script for resetting player position as necessary
/// Written by Joshua Cashmore
/// last updated 10/3/2023
/// </summary>

public class PositionReset : MonoBehaviour
{
    Vector2 startPos;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (enabled && Input.GetKeyDown(KeyCode.R)) {
            transform.position = startPos;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, (GetComponent<Collider2D>().bounds.size));
    }
}
