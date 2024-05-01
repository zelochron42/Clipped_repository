using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string id = "";
    public void Set() {
        PositionReset pos = FindObjectOfType<PositionReset>();
        pos.UpdateCheckpoint(this);
    }
}
