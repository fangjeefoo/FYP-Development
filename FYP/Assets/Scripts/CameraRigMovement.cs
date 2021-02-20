using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reference from: https://www.youtube.com/watch?v=lYIRm4QEqro
public class CameraRigMovement : MonoBehaviour
{
    public bool called = false;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0f;
    private float pitch = 0f;

    private void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        if(transform.rotation.x >= 0.5 && !GameManager.gm.pauseCounter)
        {
            called = true;
            GameManager.gm.PauseGame();
        }
    }
}
