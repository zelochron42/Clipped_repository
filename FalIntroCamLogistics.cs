using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalIntroCamLogistics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Rise());
    }
    IEnumerator Rise()
    {
        for (int i = 0; i < 20; i++)
        {

            transform.Translate(0f, 3f, 0f);

            yield return new WaitForSeconds(20f);
        }
        yield break;
    }
}
