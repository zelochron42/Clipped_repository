using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigheadTracker : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float trackingMultiplier = 5f;
    Quaternion baseRot;
    private void Awake() {
        baseRot = transform.rotation;
    }
    void Update()
    {
        transform.rotation = baseRot;
        float xDiff = player.position.x - transform.position.x;
        transform.Rotate(new Vector3(0f, xDiff * trackingMultiplier, xDiff * trackingMultiplier / 2f));
    }
}
