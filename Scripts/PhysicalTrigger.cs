using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalTrigger : MonoBehaviour
{

    [SerializeField] string[] stringList;
    bool singleUse = true;
    public UnityEvent<string[]> triggerEvent;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            triggerEvent.Invoke(stringList);
            if (singleUse)
                Destroy(gameObject);
        }
    }
}
