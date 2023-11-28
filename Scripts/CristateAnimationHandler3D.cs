using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristateAnimationHandler3D : MonoBehaviour
{

    Animator anim;
    Rigidbody rb3d;
 
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb3d = gameObject.GetComponentInParent<Rigidbody>();
 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void JumpTrigger()
    {
        anim.SetTrigger("Jump");
    }
}


