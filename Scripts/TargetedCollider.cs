using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Code to be placed on melee weapon slash effects
/// Written by Joshua Cashmore
/// last updated 10/3/2023
/// </summary>
public class TargetedCollider : MonoBehaviour
{
    //SetTargets defines whether this attack hits enemies, players or obstacles, and should be run whenever an attack is instantiated by player or enemy

    [SerializeField] string[] targets;
    public void SetTargets(params string[] tags) {
        targets = tags;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string t in targets) {
            if (collision.gameObject.CompareTag(t)) {
                TargetHit.Invoke();
                return;
            }
        }
    }

    public UnityEvent TargetHit;
}
