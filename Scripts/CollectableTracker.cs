using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableTracker : MonoBehaviour
{
    public static CollectableTracker singleton;

    public UnityEvent featherCollected;

    public bool wingsRecovered = false;

    Dictionary<string, bool> feathersCollected = new Dictionary<string, bool>();

    private void Awake() {
        if (singleton && singleton != this)
            Destroy(gameObject);
        else {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string CollectionStatus() {
        int maxNum = feathersCollected.Count;
        int collectedNum = 0;
        foreach (KeyValuePair<string, bool> kp in feathersCollected) {
            if (kp.Value == true) {
                collectedNum++;
            }
        }
        return "" + collectedNum + "/" + maxNum;
    }

    public void AddFeather(string newFeather) {
        if (newFeather.ToLower() != "key" && !feathersCollected.ContainsKey(newFeather))
            feathersCollected.Add(newFeather, false);
    }

    public void SetFeather(string featherName, bool collected) {
        feathersCollected[featherName] = collected;
        featherCollected.Invoke();
    }
}
