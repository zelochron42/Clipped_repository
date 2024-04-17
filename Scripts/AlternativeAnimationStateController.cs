using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeAnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isBeginningHash;
    int isIdleHash;
   


    // Start is called before the first frame update
 
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
       
        isIdleHash = Animator.StringToHash("isIdle");
        animator.SetBool(isIdleHash, true);
   
    }

    // Update is called once per frame
    void Update()
    {
        bool isIdle = animator.GetBool(isIdleHash);
        bool isBeginning = animator.GetBool(isBeginningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        

        if (forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
            animator.SetBool(isIdleHash, false);
        }
     
        if(!forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isIdleHash, true);
        }
    }
    

}


