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

        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;
            if(frame.Hands.Count > 0)
            {
                _leftHand = frame.Hands[0];

                //ForearmExercise();


            }
                
        }
    }

    public void ForearmExercise()
    {
        //https://stackoverflow.com/questions/42051951/how-to-detect-if-hand-in-leap-motion-is-facing-up-c-unity
        if (_leftHand.PalmNormal.y < 0)
            Debug.Log("Palm facing down");
        else
            Debug.Log("Palm facing up");
    }

    public void ElbowAExercise()
    {
        if (_leftHand.PalmNormal.y < 0)
            Debug.Log("Move towards body");
        else
            Debug.Log("Move towards ground");
    }

    public void WristExercise()
    {
        if (_leftHand.PalmNormal.z < 0)
            Debug.Log("Palm facing out");
        else
            Debug.Log("Palm facing body");
    }
}
