using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalTrigger : MonoBehaviour
{
    [SerializeField] string[] stringList;
    [SerializeField] protected bool singleUse = true;
    public UnityEvent<string[]> triggerEvent;
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            triggerEvent.Invoke(stringList);
            if (singleUse)
                Destroy(gameObject);
        }
    }


}
