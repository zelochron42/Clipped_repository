using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewind : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsRewinding = false;
    List<Vector3> positions;
    Rigidbody rb;
    void Start()
    {
        positions = new List<Vector3>();
        rb = gameObject.GetComponentInParent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartRewind();
        }

    }
    void FixedUpdate()
    {
        if (IsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    void Rewind()
    {
        transform.position = positions[0];
        positions.RemoveAt(0);
    }
    void Record()
    {
        positions.Insert(0, transform.position);

    }
    public void StartRewind()
    {
        IsRewinding = true;
    }
}
