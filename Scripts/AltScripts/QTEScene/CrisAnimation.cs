using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisAnimation : MonoBehaviour
{
    Animator animator;
    int isDodgingHash;
    public float DodgeTime;
    public float BlockTime;
    public float AttackTime;
    int isIdleHash;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public CameraSLowScript CamSpeedControl;
    public GameObject SlowMo;

    public float BeginTime;
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        isDodgingHash = Animator.StringToHash("isDodging");
        isIdleHash = Animator.StringToHash("Idle");
        GameObject Slow = Instantiate(SlowMo);
        CamSpeedControl = Slow.GetComponent<CameraSLowScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool DodgePress = Input.GetKey("q");
        bool isDodging = animator.GetBool(isDodgingHash);
        bool isIdle = animator.GetBool(isIdleHash);
        if (DodgePress)
        {
            Dodge();
        }
    }
    void Dodge()

    {
        //yield return new WaitForSeconds(DodgeTime);
        animator.SetBool(isDodgingHash, true);
        animator.SetBool(isIdleHash, false);
        CamSpeedControl.StopSlow();

    }
    IEnumerator Block()

    {
        yield return new WaitForSeconds(BlockTime);


    }
    IEnumerator Attack()

    {
        yield return new WaitForSeconds(AttackTime);


    }
    private void CameraOne()
    {
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        Camera3.SetActive(false);
    }
    private void Cameratwo()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        Camera3.SetActive(false);
    }
    private void Camerathree()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(true);
    }
}
