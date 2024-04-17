using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationandmove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _Translate;


    [SerializeField] private float _TranslateSpeed;

    //[SerializeField] private Vector3 _position;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     

    }
    void FixedUpdate()
    {
        transform.Rotate(_rotation * _speed * Time.deltaTime);
        // transform.Position(_position * Time.deltaTime);
        transform.Translate(_Translate * _TranslateSpeed * Time.deltaTime);

    }

    /*IEnumerator SpikeTransformsLefttoRight()
       {
           transform.Translate(_Translate * _TranslateSpeed * Time.deltaTime);
           yield  return new WaitForSeconds(3f);

       }*/
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OppositeDirection"))
        {
            _Translate = -_Translate;
        }
        if (other.gameObject.CompareTag("Direction"))
        {
            _Translate = _Translate;
        }
    }
}

