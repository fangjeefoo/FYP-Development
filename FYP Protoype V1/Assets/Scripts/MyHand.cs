using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class MyHand : MonoBehaviour
{
    private Hand _leftHand;
    private Hand _rightHand;
    private Controller _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = new Controller();
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = _controller.Frame(0);

        Debug.Log("Frame hand count: " + frame.Hands.Count);

        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;
            if(frame.Hands.Count > 0)
            {
                _leftHand = frame.Hands[0];
                Debug.Log("Hand Position: " + _leftHand.PalmPosition);
                Debug.Log("Hand rotation: " + _leftHand.Rotation);
                Debug.Log("Arm rotation: " + _leftHand.Arm.Rotation);
                Debug.Log("Wrist position: " + _leftHand.Arm.WristPosition);

                if (_leftHand.IsLeft)
                    Debug.Log("Left hand");
                else
                    Debug.Log("right hand");

            }
                
        }

        
        //Debug.Log("Right hand: " + _rightHand.PalmPosition);
    }
}
