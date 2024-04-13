using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim) {
            AnimatorStateInfo currentAnim = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(currentAnim.fullPathHash, 0, Random.Range(0f, 1f));
            anim.speed = Random.Range(0.9f, 1.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
