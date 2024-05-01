using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSection : MonoBehaviour
{
    public float speed = 30;
    public bool RiseOnStart = true;
   

    // Start is called before the first frame update
    void Start()
    {
       
        Destroy(gameObject, 5);
        if(RiseOnStart)
        StartCoroutine(Rise());
    }

    // Update is called once per frame
    void Update()
    {

        
        transform.position += new Vector3(0, 0, -2) * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Rise()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.Translate(0f, 3f, 0f);
            yield return new WaitForSeconds(0.05f);

        }
        yield break;
    }

 
}
