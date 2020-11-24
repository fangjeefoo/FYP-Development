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
    private bool[] _selectedExercise;
    private bool _called;

    // Start is called before the first frame update
    void Start()
    {
        handManager = this;
        _controller = new Controller();
        _forearmExercise = false;
        _elbowExercise = false;
        _wristExercise = false;
        _holdingKnife = false;
        _called = false;
        _selectedExercise = new bool[4] { true, false, false, false };
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = _controller.Frame(0);        

        if(GameManager.gm.SelectedExercise != null && !_called)
        {
            _selectedExercise = GameManager.gm.SelectedExercise;
            _called = true;
        }          
           
        if(frame.Hands.Count > 0)
        {
            _hand = frame.Hands[0];

            if (_forearmExercise && _selectedExercise[1])
                ForearmExercise();

            if (_elbowExercise && _holdingKnife && _selectedExercise[2])
                ElbowAExercise();

            if (_wristExercise && _selectedExercise[3])
                WristExercise();
        }
    }

    /// <summary>
    /// for pan
    /// </summary>
    public void ForearmExercise()
    {
        //https://stackoverflow.com/questions/42051951/how-to-detect-if-hand-in-leap-motion-is-facing-up-c-unity
        if (_hand.PalmNormal.y < 0)
            Debug.Log("Palm facing down");
        else
            Debug.Log("Palm facing up");

        GameManager.gm.UpdatePerformedTimes(1);
    }

    public void ElbowAExercise()
    {
        if (_hand.PalmNormal.y < 0)
            Debug.Log("Move towards body");
        else
            Debug.Log("Move towards ground");

        GameManager.gm.UpdatePerformedTimes(2);
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

        GameManager.gm.UpdatePerformedTimes(3);
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
