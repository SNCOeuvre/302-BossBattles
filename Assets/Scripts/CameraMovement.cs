using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement: MonoBehaviour
{
    //allows to select a target to focus on
    public Transform target;
    public float lookSensitivityX = 1;
    public float lookSensitivityY = 1;

    //up or down
    float pitch = 0;
    //side to side
    float yaw = 0;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

   
    void Update()
    {
        //gets movement based on the mouse
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //when mouse moves, it is multiplied by the sensitivity
        yaw += mx * lookSensitivityX;
        pitch += my * lookSensitivityY;

        pitch = Mathf.Clamp(pitch, -80, 80);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);


        if (target != null)
        {
            transform.position = AnimMath.Ease(transform.position, target.position, .01f);
        }
    }
}
