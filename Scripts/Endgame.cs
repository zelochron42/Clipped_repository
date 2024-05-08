using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    [SerializeField] Renderer[] bigWings;
    [SerializeField] Renderer[] smallWings;
    [SerializeField] int fadeStepCount = 10;
    [SerializeField] float stepTime = 0.04f;
    PlayerStats stats;

    private void Start() {
        stats = FindObjectOfType<PlayerStats>();
    }
    public void WingFade() {
        CollectableTracker.singleton.wingsRecovered = true;
        StartCoroutine("FadeEffect");
    }

    public void EndgameTransition() {
        if (!stats) {
            Debug.LogError("NO STATS");
            return;
        }
        stats.AdvanceScene();
    }

    IEnumerator FadeEffect() {
        for (int i = 0; i <= fadeStepCount; i++) {
            foreach (Renderer r in bigWings) {
                Color c = r.material.color;
                r.material.color = new Color(c.r, c.g, c.b, 1f - ((float)i / fadeStepCount));
            }            
            yield return new WaitForSeconds(stepTime);
        }
        for (int i = 0; i <= fadeStepCount; i++) {
            foreach (Renderer r in smallWings) {
                if (!r.enabled)
                    r.enabled = true;
                Color c = r.material.color;
                r.material.color = new Color(c.r, c.g, c.b, (float)i / fadeStepCount);
            }
            yield return new WaitForSeconds(stepTime);
        }
        yield break;
    }
}
