using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float delay = 5f;
    [SerializeField] bool destroyOnPlayerContact = false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delay);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (destroyOnPlayerContact && collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject, 0.05f);
        }
    }
}
