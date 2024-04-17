using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdperson : MonoBehaviour
{
    Vector3 direction;
    public CharacterController CristataController;
    public float speed = 6f;
    public float SmoothTurntime = 0.1f;
    float SmoothTurnVel;
    public Transform Cam;
    private float Gravity = -9.81f;
    //[SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float Jumpforce;
    private Vector3 _velocity;
    void Update()
    {
        ApplyGravity();
        Speedup();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
      direction = new Vector3(horizontal, 0f, vertical). normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref SmoothTurnVel, SmoothTurntime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            CristataController.Move(moveDir * speed * Time.deltaTime);
        }
    }
    private void ApplyGravity()
    {
        if (CristataController.isGrounded)
        {
            _velocity.y = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = Jumpforce;
            }
        }
        else
        {
            _velocity.y -= Gravity * -2f * Time.deltaTime;
        }
    }
    void Speedup()
    {
        if (Input.GetKeyDown("left shift"))
        {
            speed += 10;
        }
        if (Input.GetKeyUp("left shift"))
        {
            speed -= 10;
        }
    }

}
