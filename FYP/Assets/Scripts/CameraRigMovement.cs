using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reference from: https://www.youtube.com/watch?v=lYIRm4QEqro
public class CameraRigMovement : MonoBehaviour
{
    private float speed = 2.0f;
    private float speedH;
    private float speedV;

    private float yaw = 0f;
    private float pitch = 0f;

    public void Start()
    {
        speedH = speed;
        speedV = speed;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Lock my cursor");
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            yaw = 0f;
            pitch = 0f;
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
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
}
