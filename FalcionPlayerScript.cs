using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FalcionPlayerScript : MonoBehaviour
{
    public int PLayerHealth = 30;
    [SerializeField] private Slider healthBar;


    //variables for invincibility blink effect
    List<Renderer> renders = new List<Renderer>();
    [SerializeField] [Tooltip("Duration of each blink while invulnerable")] float blinkTime = 0.2f;
    [SerializeField] [Tooltip("Duration of invulnerability after taking damage")] float invulnTime = 1f;
    bool invulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = PLayerHealth;

        //invulnerability code
        Renderer[] allRenders = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allRenders) {
            if (r.enabled) {
                renders.Add(r); //add active renderers to the list of renderers that will be toggled while invulnerable
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PLayerHealth <= 0)
        {
            Time.timeScale = 0;
           
        } 
        healthBar.value = PLayerHealth;
        BlinkEffect();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage") && !invulnerable)
        {
            invulnerable = true;
            StartCoroutine("InvulnBlink");
            PLayerHealth -= 1;
        }

     
    }

    
    void BlinkEffect() {
        if (invulnerable)
            SetRenderers((int)Mathf.Floor(Time.time / blinkTime) % 2 == 0);
        else
            SetRenderers(true);
    }

    void SetRenderers(bool enabled) {
        foreach (Renderer r in renders)
            r.enabled = enabled;
    }

    IEnumerator InvulnBlink() {
        yield return new WaitForSeconds(invulnTime);
        invulnerable = false;
        yield break;
    }
    

}
