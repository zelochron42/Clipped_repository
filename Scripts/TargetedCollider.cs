using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Code to be placed on melee weapon slash effects
/// Written by Joshua Cashmore
/// last updated 5/1/2024
/// </summary>
public class TargetedCollider : MonoBehaviour
{
    //SetTargets defines whether this attack hits enemies, players or obstacles, and should be run whenever an attack is instantiated by player or enemy

    [SerializeField] string[] targets;
    List<GameObject> hitObjects = new List<GameObject>();
    public void SetTargets(params string[] tags) {
        targets = tags;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        
        foreach (GameObject g in hitObjects) { //multihit prevention
            if (g == collision.gameObject)
                return;
        }

        foreach (string t in targets) {
            if (collision.gameObject.CompareTag(t)) {
                Debug.Log("targeted collider hit object");
                TargetHit.Invoke();
                hitObjects.Add(collision.gameObject);
                return;
            }
        }
    }

    public UnityEvent TargetHit;
}
