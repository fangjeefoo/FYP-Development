using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class MyHand : MonoBehaviour
{
    public static MyHand handManager;

    //private variable
    private Hand _hand;
    private Controller _controller;
    private bool _forearmExercise;    
    private bool _elbowExercise;
    private bool _wristExercise;
    private bool _holdingKnife;

    // Start is called before the first frame update
    void Start()
    {
        handManager = this;
        _controller = new Controller();
        _forearmExercise = false;
        _elbowExercise = false;
        _wristExercise = false;
        _holdingKnife = false;
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = _controller.Frame(0);

        if(frame.Hands.Count > 0)
        {
            _hand = frame.Hands[0];

            if (_forearmExercise)
                ForearmExercise();

            if (_elbowExercise && _holdingKnife)
                ElbowAExercise();

            if (_wristExercise)
                WristExercise();
        }
    }

    /// <summary>
    /// for pan
    /// </summary>
    public void ForearmExercise()
    {
        //https://stackoverflow.com/questions/42051951/how-to-detect-if-hand-in-leap-motion-is-facing-up-c-unity
        //adding one more condition, maybe check x or z, because got 2 y (in elbow exercise)
        if (_hand.PalmNormal.y < 0)
            Debug.Log("Palm facing down");
        else
            Debug.Log("Palm facing up");
    }

    public void ElbowAExercise()
    {
        if (_hand.PalmNormal.y < 0)
            Debug.Log("Move towards body");
        else
            Debug.Log("Move towards ground");
    }

    /// <summary>
    /// for deep pan
    /// </summary>
    public void WristExercise()
    {
        if (_hand.PalmNormal.z < 0)
            Debug.Log("Palm facing out");
        else
            Debug.Log("Palm facing body");
    }

    public bool UpdateWristExercise
    {
        get { return _wristExercise; }
        set { _wristExercise = value; }
    }

    public bool UpdateElbowExercise
    {
        get { return _elbowExercise; }
        set { _elbowExercise = value; }
    }

    public bool UpdateForearmExercise
    {
        get { return _forearmExercise; }
        set { _forearmExercise = value; }
    }

    public bool HoldingKnife
    {
        get { return _holdingKnife; }
        set { _holdingKnife = value; }
    }
}
