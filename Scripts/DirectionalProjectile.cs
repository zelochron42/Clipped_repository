using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalProjectile : MonoBehaviour
{
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.up = -rb2d.velocity;
    }
}
