using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalcionPlayerDashing : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    [SerializeField] private bool dashing;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCD;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
   
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(dashKey))
        {
            Dash();
        }
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCD;
        Vector3 forceToApply = orientation.forward * dashForce;  //+orientation.up * dashUpwardForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
        dashing = true; 
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        dashing = false;
    }
}

