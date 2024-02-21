using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdperson : MonoBehaviour
{
    public CharacterController CristataController;
    public float speed = 6f;
    public float SmoothTurntime = 0.1f;
    float SmoothTurnVel;
    public Transform Cam;

    void Update()
    {
        Speedup();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical). normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref SmoothTurnVel, SmoothTurntime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            CristataController.Move(moveDir * speed * Time.deltaTime);
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
