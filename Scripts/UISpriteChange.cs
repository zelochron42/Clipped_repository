using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteChange : MonoBehaviour
{
    [SerializeField] float blinkDelay = 0.75f;
    [SerializeField] Image[] imageObjects;
    [SerializeField] Sprite image1;
    [SerializeField] Sprite image2;
    public void UIBlink() {
        StopAllCoroutines();
        StartCoroutine("Blink");
    }

    IEnumerator Blink() {
        foreach (Image i in imageObjects) {
            i.sprite = image2;
        }
        yield return new WaitForSeconds(blinkDelay);
        foreach (Image i in imageObjects) {
            i.sprite = image1;
        }
    }
    
}
