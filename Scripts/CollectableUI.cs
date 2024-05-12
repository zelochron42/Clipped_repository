using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CollectableUI : MonoBehaviour
{
    CollectableTracker tracker;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponentInChildren<TextMeshProUGUI>();
        TrackerCheck();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void TrackerCheck() {
        tracker = CollectableTracker.singleton;
        if (!tracker) {
            Debug.LogError("No collectable tracker in scene, please add one.");
            return;
        }
        tracker.featherCollected.AddListener(UpdateText);
        UpdateText();
    }
    public void UpdateText() {
        if (tracker && text) {
            text.text = tracker.CollectionStatus();
        }
    }
}
