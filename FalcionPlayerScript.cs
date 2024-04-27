using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FalcionPlayerScript : MonoBehaviour
{
    public int PLayerHealth = 30;
    [SerializeField] private Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = PLayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(PLayerHealth <= 0)
        {
            Time.timeScale = 0;
           
        } 
        healthBar.value = PLayerHealth;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            PLayerHealth -= 1;
        }
    }
}
