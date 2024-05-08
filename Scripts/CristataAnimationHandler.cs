using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristataAnimationHandler : MonoBehaviour
{
    bool finished = false;
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
        if (!finished) {
            if (plrmov.playerState == PlayerMovement.state.free && plrmov.enabled == true)
                anim.SetFloat("XVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
            else
                anim.SetFloat("XVelocity", 0f);
            anim.SetFloat("YVelocity", rb2d.velocity.y);
            anim.SetBool("OnGround", plrmov.OnGround());
            anim.SetBool("IsSliding", plrmov.isSliding);
            anim.SetBool("IsFree", plrmov.playerState == PlayerMovement.state.free);
        }
    }

    public void JumpTrigger() {
        anim.SetTrigger("Jump");
    }

    public void AttackTrigger() {
        anim.SetTrigger("Attack");
    }

    public void Finale() {
        finished = true;
        anim.SetFloat("YVelocity", 0f);
        anim.SetFloat("XVelocity", 0f);
        anim.SetBool("OnGround", true);
        anim.SetBool("IsSliding", false);
        anim.SetBool("IsFree", false);
        StartCoroutine("FinaleDelay");
    }
    IEnumerator FinaleDelay() {
        yield return new WaitForSeconds(0.1f);
        anim.Play("cristata_finale");
        yield break;
    }
}
