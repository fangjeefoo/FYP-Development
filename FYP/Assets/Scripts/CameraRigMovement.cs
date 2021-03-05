using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reference from: https://www.youtube.com/watch?v=lYIRm4QEqro
public class CameraRigMovement : MonoBehaviour
{
    public static CameraRigMovement cameraRig;

    private float speed = 5.0f;
    private float speedH;
    private float speedV;

    private float yaw = 0f;
    private float pitch = 0f;

    public void Start()
    {
        speedH = speed;
        speedV = speed;
        cameraRig = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetCamera();
            return;
        }

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        if(transform.rotation.x >= 0.5 && GameManager.gm)
        {
            if(!GameManager.gm.pauseCounter)
                GameManager.gm.PauseGame();
        }
    }

    public void ResetCamera()
    {
        yaw = 0f;
        pitch = 0f;
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
