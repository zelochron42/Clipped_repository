using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapColliderHider : MonoBehaviour
{
    private void Awake() {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprites) {
            s.enabled = false;
        }
    }
}
