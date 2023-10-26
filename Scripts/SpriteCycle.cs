using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteCycle : MonoBehaviour
{
    private enum endState {stop, loop, terminate}
    [SerializeField] float spritesPerSecond;
    [SerializeField] Sprite[] spriteList;
    [SerializeField] endState whenFinished;

    SpriteRenderer render;
    float startTime;
    private void Awake() {
        render = GetComponent<SpriteRenderer>();
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - startTime;
        int spriteIndex = (int)Mathf.Floor(timeElapsed * spritesPerSecond);

        if (spriteIndex > spriteList.Length - 1) {
            switch (whenFinished) {
                case endState.loop:
                    spriteIndex %= spriteList.Length;
                    break;
                case endState.stop:
                    enabled = false;
                    return;
                case endState.terminate:
                    Destroy(gameObject);
                    return;
            }
        }
        render.sprite = spriteList[spriteIndex];
    }
}
