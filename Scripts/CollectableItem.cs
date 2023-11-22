using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    public UnityEvent collected;
    public string objectID = "default";
    CollectableTracker tracker;

    private void Awake() {
        tracker = CollectableTracker.singleton;
        if (tracker)
            tracker.AddFeather(objectID);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            tracker.SetFeather(objectID, true);
            collected.Invoke();
            Destroy(gameObject);
        }
    }
}
