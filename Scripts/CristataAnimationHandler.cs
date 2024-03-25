using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristataAnimationHandler : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb2d;
    PlayerMovement plrmov;
    // Start is called before the first frame update
    private void Awake() {
        anim = GetComponent<Animator>();
        rb2d = gameObject.GetComponentInParent<Rigidbody2D>();
        plrmov = gameObject.GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plrmov.playerState == PlayerMovement.state.free && plrmov.enabled == true)
            anim.SetFloat("XVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        else
            anim.SetFloat("XVelocity", 0f);
        anim.SetFloat("YVelocity", rb2d.velocity.y);
        anim.SetBool("OnGround", plrmov.OnGround());
        anim.SetBool("IsSliding", plrmov.isSliding);
        anim.SetBool("IsFree", plrmov.playerState == PlayerMovement.state.free);
    }

    public void JumpTrigger() {
        anim.SetTrigger("Jump");
    }

    public void AttackTrigger() {
        anim.SetTrigger("Attack");
    }
}
