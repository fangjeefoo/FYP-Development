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
        Debug.Log("Is connected: " + _controller.IsConnected);
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = _controller.Frame(0);

        if (frame.Hands.Count > 0)
        {
            Debug.Log("More than 0 hand");
            List<Hand> hands = frame.Hands;
            if(frame.Hands.Count > 0)
            {
                _leftHand = frame.Hands[0];
                Debug.Log("Left hand: " + _leftHand.PalmPosition);
            }
                
        }

        
        //Debug.Log("Right hand: " + _rightHand.PalmPosition);
    }
}
