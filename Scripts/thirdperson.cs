using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdperson : MonoBehaviour
{
    public CharacterController CristataController;
    public float speed = 6f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical). normalized;
        if (direction.magnitude >= 0.1f)
        {
            CristataController.Move(direction * speed * Time.deltaTime);
        }
    }
}
