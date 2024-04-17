using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationandmove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;

    //[SerializeField] private Vector3 _position;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        transform.Rotate(_rotation * _speed * Time.deltaTime);
        // transform.Position(_position * Time.deltaTime);
        

    }

 
}
