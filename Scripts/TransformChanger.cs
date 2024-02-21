using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChanger : MonoBehaviour
{
    public void SetEuler(float x, float y, float z) {
        transform.rotation = Quaternion.Euler(x, y, z);
    }
    public void SetEuler(Vector3 xyz) {
        SetEuler(xyz.x, xyz.y, xyz.z);
    }
    public void SetEulerY(float y) {
        transform.rotation = Quaternion.Euler(0f, y, 0f);
    }
}
