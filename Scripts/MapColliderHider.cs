using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapColliderHider : MonoBehaviour
{
    [SerializeField] bool autoHide = true;
    [SerializeField] bool hideInEditor = true;
    private void Awake() {
        if (!autoHide)
            return;
        if (Application.isEditor && !hideInEditor)
            return;
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprites) {
            s.enabled = false;
        }
    }
}
