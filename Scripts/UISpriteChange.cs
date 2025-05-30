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

    [SerializeField] Image[] dashImages;
    PlayerMovement plrmov;

    private void Start() {
        plrmov = FindObjectOfType<PlayerMovement>();
        plrmov.StartDash.AddListener(UpdateDashes);
        plrmov.UIUpdate.AddListener(UpdateDashes);
    }
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

    public void UpdateDashes() {
        int dashCount = plrmov.GetDashes();
        for (int i = 0; i < dashImages.Length; i++) {
            if (i + 1 <= dashCount)
                dashImages[i].enabled = true;
            else
                dashImages[i].enabled = false;
        }
    }
    
}
