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
    private int _panObject;
    private int _deepPanObject;
    private int _cuttingBoardObject;
    private bool _forearmExercise;    
    private bool _elbowExercise;
    private bool _wristExercise;
    private bool _holdingKnife;

    // Start is called before the first frame update
    void Start()
    {
        handManager = this;
        _controller = new Controller();
        _panObject = 0;
        _deepPanObject = 0;
        _cuttingBoardObject = 0;
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

    public void UpdatePanObject(bool increase)
    {
        if (increase)
            _panObject++;
        else
            _panObject--;

        if(_panObject > 0)
            _forearmExercise = true;
        else
            _forearmExercise = false;
    }

    public void UpdateDeepPanObject(bool increase)
    {
        if (increase)
            _deepPanObject++;
        else
            _deepPanObject--;

        if (_deepPanObject > 0)
            _wristExercise = true;
        else
            _wristExercise = false;
    }

    public void UpdateCuttingBoardObject(bool increase)
    {
        if (increase)
            _cuttingBoardObject++;
        else
            _cuttingBoardObject--;

        if (_cuttingBoardObject > 0)
            _elbowExercise = true;
        else
            _elbowExercise = false;
    }

    public bool HoldingKnife
    {
        get { return _holdingKnife; }
        set { _holdingKnife = value; }
    }
}
