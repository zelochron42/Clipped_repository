using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VanishingCloud : MonoBehaviour
{
    Collider2D col;
    [SerializeField] bool reappear = true;
    [SerializeField] SkinnedMeshRenderer meshrend;
    Animator anim;
    bool vanishing = false;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        if (anim) {
            AnimatorStateInfo currentAnim = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(currentAnim.fullPathHash, 0, Random.Range(0f, 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<TargetedCollider>()) {
            Vanish();
        }
    }

    private void Vanish() {
        if (!vanishing) {
            vanishing = true;
            StopAllCoroutines();
            StartCoroutine("VanishRoutine");
        }
    }

    IEnumerator VanishRoutine() {
        col.enabled = false;
        Color c = meshrend.material.color;
        for (int i = 10; i >= 0f; i--) {
            yield return new WaitForSeconds(0.05f);
            meshrend.material.color = new Color(c.r, c.g, c.b, i/10f);
        }
        if (!reappear) {
            Destroy(gameObject);
            yield break;
        }
        yield return new WaitForSeconds(1f);
        col.enabled = true;
        vanishing = false;
        for (int i = 0; i <= 10f; i++) {
            yield return new WaitForSeconds(0.05f);
            meshrend.material.color = new Color(c.r, c.g, c.b, i/10f);
        }
        yield break;
    }
}
