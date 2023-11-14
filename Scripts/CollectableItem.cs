using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    public UnityEvent collected;
    public string objectID = "default";
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collected.Invoke();
            Destroy(gameObject);
        }
    }
}
