using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    [SerializeField]
    private GameObject Falcion;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private float distance;
    [SerializeField] private Slider DistanceBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = CalculateDistance();

    }
    private float CalculateDistance()
    {
        return Vector3.Distance(Falcion.transform.position, Player.transform.position);
    }
}
