using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] Transform followObject;
    Vector2 offset;

    public void SetFollow(Transform t) {
        followObject = t;
    }
    void Start()
    {
        offset = transform.position - followObject.position;
    }
    void LateUpdate()
    {
        transform.position = (Vector2)followObject.position + offset;
    }
}
