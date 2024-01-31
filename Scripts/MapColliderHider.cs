using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapColliderHider : MonoBehaviour
{
    [SerializeField] bool hideInEditor = true;
    private void Awake() {
        if (Application.isEditor && !hideInEditor)
            return;
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprites) {
            s.enabled = false;
        }
    }
}
