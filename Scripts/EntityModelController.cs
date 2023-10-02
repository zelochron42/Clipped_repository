using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModelController : MonoBehaviour
{
    [SerializeField] bool enabled = true;
    Rigidbody2D rb2d;
    Vector3 defaultScale;
    [SerializeField] Transform model;
    private void Awake() {
        defaultScale = model.localScale;
    }
    public void FlipModel(bool right) {
        if (enabled) {
            if (right) {
                model.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
            }
            else {
                model.localScale = new Vector3(defaultScale.x, defaultScale.y, -defaultScale.z);
            }
        }
    }
}
