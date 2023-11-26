using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPointer : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 eulerOffset;

    private void LateUpdate() {
        transform.LookAt(target);
        transform.eulerAngles = transform.rotation.eulerAngles + eulerOffset;
    }
}
