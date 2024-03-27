using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeAnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isBeginningHash;
    int isIdleHash;
    public float BeginTime;


    // Start is called before the first frame update
 
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isBeginningHash = Animator.StringToHash("Begin");
        isIdleHash = Animator.StringToHash("Idle");
        animator.SetBool(isIdleHash, true);
        StartCoroutine(Begin());
    }

    // Update is called once per frame
    void Update()
    {
        bool isIdle = animator.GetBool(isIdleHash);
        bool isBeginning = animator.GetBool(isBeginningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool beginPress = Input.GetKey("e");

        if (forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (beginPress)
        {
            animator.SetBool(isBeginningHash, true);
            animator.SetBool(isIdleHash, false);
        }
        if(isIdle && !beginPress)
        {
            animator.SetBool(isIdleHash, true);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Test"))
        {
            //Destroy(other.gameObject);
            Debug.Log("Working");
        }
    }
    IEnumerator Begin()

    {
        yield return new WaitForSeconds(BeginTime);
        animator.SetBool(isBeginningHash, true);
        animator.SetBool(isIdleHash, false);

    }
}


