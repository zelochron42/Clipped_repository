using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectableProjectile : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<TargetedCollider>()) {
            
        }
    }
}
