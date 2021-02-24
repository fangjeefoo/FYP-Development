using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using FoodType;

public class MyHand : MonoBehaviour
{
    public static MyHand handManager;

    //private variable
    private Hand _lastHand;
    private Hand _hand;
    private Controller _controller;
    private bool _forearmExercise;
    private bool _elbowExercise;
    private bool _wristExercise;
    //private bool _holdingKnife;
    private bool[] _selectedExercise;
    private bool _called;
    private int _forearmCounter;
    private int _elbowCounter;
    private int _wristCounter;

    // Start is called before the first frame update
    void Start()
    {
        handManager = this;
        _controller = new Controller();
        _forearmExercise = false;
        _elbowExercise = false;
        _wristExercise = false;
        //_holdingKnife = true;
        _called = false;
        _forearmCounter = 0;
        _elbowCounter = 0;
        _wristCounter = 0;
        _selectedExercise = new bool[4] { true, false, false, false };
    }

    // Update is called once per frame
    void Update()
    {
/*        //get recent frame
        Frame frame = _controller.Frame(0);
        //get previous frame
        Frame lastFrame = _controller.Frame(1);

        if (frame.Hands[0] != null)
        {
            Debug.Log(frame.Hands[0].PalmNormal);
        }*/

        if (GameManager.gm.SelectedExercise != null && !_called)
        {
            _selectedExercise = GameManager.gm.SelectedExercise;
            _called = true;
        }

        if (_forearmExercise && _selectedExercise[1])
            GameManager.gm.PlayVideo(false);

        if (_elbowExercise && _selectedExercise[2]) //if (_elbowExercise && _holdingKnife && _selectedExercise[2])
            GameManager.gm.PlayVideo(false);

        if (_wristExercise && _selectedExercise[3])
            GameManager.gm.PlayVideo(false);

        /*        if (frame.Hands.Count > 0 && lastFrame.Hands.Count > 0)
                {
                    _lastHand = lastFrame.Hands[0];
                    _hand = frame.Hands[0];

                    if (_forearmExercise && _selectedExercise[1])
                        ForearmExercise();

                    if (_elbowExercise && _selectedExercise[2]) //if (_elbowExercise && _holdingKnife && _selectedExercise[2])
                        ElbowExercise();

                    if (_wristExercise && _selectedExercise[3])
                        WristExercise();
                }
                else
                {
                    _lastHand = null;
                    _hand = null;
                }*/
        //Debug.Log(_hand.PalmNormal);
        /*        if (_hand.PalmNormal.y < 0)
                    Debug.Log("Move towards body");
                else
                    Debug.Log("Move towards ground");*/
    }

/*    /// <summary>
    /// for pan
    /// </summary>
    public void ForearmExercise()
    {
        GameManager.gm.PlayVideo(false);
        //https://stackoverflow.com/questions/42051951/how-to-detect-if-hand-in-leap-motion-is-facing-up-c-unity
        //(_lastHand.PalmNormal.z > 0 && _hand.PalmNormal.z < 0 && _lastHand.PalmNormal.y < 0 && _hand.PalmNormal.y < 0)
        if (_lastHand.PalmNormal.y < 0 && _hand.PalmNormal.y > 0)
        {
            if (!_sfxPlaying && SoundManager.soundManager)
                SoundManager.soundManager.frying.Play();

            _sfxPlaying = true;
            GameManager.gm.UpdatePerformedTimes(1);
            _forearmCounter++;
            Debug.Log("forearm times");
        }

        if (_forearmCounter >= 3)
        {
            if (SoundManager.soundManager)
                SoundManager.soundManager.frying.Stop();
            _sfxPlaying = false;
            _forearmCounter = 0;
            Pan.pan.food.GetComponent<Food>().GenerateCookedFood();
            Debug.Log("clear");
            GameManager.gm.PlayVideo(true);
            Pan.pan.food.GetComponent<Food>().food = null;
            _forearmExercise = false;
        }
        //if (_hand.PalmNormal.y < 0)
        //    Debug.Log("Palm facing down");
        //else
        //    Debug.Log("Palm facing up");        
    }

    /// <summary>
    /// for cutting board
    /// </summary>
    public void ElbowExercise()
    {
        GameManager.gm.PlayVideo(false);

        //if(_lastHand.PalmNormal.x < 0 && _hand.PalmNormal.x > 0)
        if (_lastHand.PalmNormal.x < 0 && _hand.PalmNormal.x > 0)
        {
            if (SoundManager.soundManager)
                SoundManager.soundManager.MyPlay(8);

            GameManager.gm.UpdatePerformedTimes(3);
            _elbowCounter++;
            Debug.Log("elbow times");
        }

        if (_elbowCounter >= 3)
        {
            _elbowCounter = 0;
            CuttingBoard.cuttingBoard.food.GetComponent<Food>().GenerateCookedFood();
            CuttingBoard.cuttingBoard.food.GetComponent<Food>().food = null;
            GameManager.gm.PlayVideo(true);
            _elbowExercise = false;
        }
        //if (_hand.PalmNormal.y < 0)
        //    Debug.Log("Move towards body");
        //else
        //    Debug.Log("Move towards ground");

        //GameManager.gm.UpdatePerformedTimes(2);
    }

    /// <summary>
    /// for deep pan
    /// </summary>
    public void WristExercise()
    {
        GameManager.gm.PlayVideo(false);
        //if (_lastHand.PalmNormal.z < 0 && _hand.PalmNormal.z > 0 && _lastHand.PalmNormal.y < 0 && _hand.PalmNormal.y < 0)
        if (_lastHand.PalmNormal.z < 0 && _hand.PalmNormal.z > 0)
        {
            if (!_sfxPlaying && SoundManager.soundManager)
                SoundManager.soundManager.boiling.Play();

            _sfxPlaying = true;
            //Debug.Log("cook");
            //Debug.Log("last hand: " + _lastHand.PalmNormal);
            //Debug.Log("hand: " + _hand.PalmNormal);
            Debug.Log("wrist times");
            GameManager.gm.UpdatePerformedTimes(2);
            _wristCounter++;
        }

        if (_wristCounter >= 3)
        {
            if (SoundManager.soundManager)
                SoundManager.soundManager.boiling.Stop();
            _sfxPlaying = false;
            Debug.Log("Clear");
            _wristCounter = 0;
            DeepPan.deepPan.food.GetComponent<Food>().GenerateCookedFood();
            DeepPan.deepPan.food.GetComponent<Food>().food = null;
            GameManager.gm.PlayVideo(true);
            _wristExercise = false;
        }
        //if (_hand.PalmNormal.z < 0)
        //    Debug.Log("Palm facing out");
        //else
        //    Debug.Log("Palm facing body");

        //GameManager.gm.UpdatePerformedTimes(3);
    }*/

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

    public void ForearmExercise()
    {
        if (!GameManager.gm.pauseCounter)
        {
            Debug.Log("Forearm getting input");
            if (_forearmExercise && _selectedExercise[1])
            {
                Debug.Log("Forearm increase");
                GameManager.gm.UpdatePerformedTimes(1);
                _forearmCounter++;
                Debug.Log("forearm times");
            }

            if (_forearmCounter >= 3)
            {
                _forearmCounter = 0;
                if (SoundManager.soundManager)
                    SoundManager.soundManager.frying.Stop();
                Pan.pan.particleSystem.Stop();
                Pan.pan.food.GetComponent<Food>().GenerateCookedFood();
                Debug.Log("clear");
                GameManager.gm.PlayVideo(true);
                Pan.pan.food.GetComponent<Food>().food = null;
                _forearmExercise = false;
            }
        }
    }

     public void WristExercise()
     {
        if(!GameManager.gm.pauseCounter)
        {
            Debug.Log("Wrist getting input");
            if (_wristExercise && _selectedExercise[3])
            {
                Debug.Log("Forearm increase");
                //Debug.Log("cook");
                //Debug.Log("last hand: " + _lastHand.PalmNormal);
                //Debug.Log("hand: " + _hand.PalmNormal);
                Debug.Log("wrist times");
                GameManager.gm.UpdatePerformedTimes(2);
                _wristCounter++;
            }

            if (_wristCounter >= 3)
            {
                Debug.Log("Clear");
                DeepPan.deepPan.particleSystem.Stop();
                if (SoundManager.soundManager)
                    SoundManager.soundManager.boiling.Stop();

                _wristCounter = 0;
                DeepPan.deepPan.food.GetComponent<Food>().GenerateCookedFood();
                DeepPan.deepPan.food.GetComponent<Food>().food = null;
                GameManager.gm.PlayVideo(true);
                _wristExercise = false;
            }
        }
     }
    public void ElbowExercise()
    {
        if (!GameManager.gm.pauseCounter)
        {
            Debug.Log("Elbow getting input");
            if (_elbowExercise && _selectedExercise[2]) //if (_elbowExercise && _holdingKnife && _selectedExercise[2])
            {
                Debug.Log("Forearm increase");
                if (SoundManager.soundManager)
                    SoundManager.soundManager.MyPlay(8);
                CuttingBoard.cuttingBoard.EnableParticleSystem();
                GameManager.gm.UpdatePerformedTimes(3);
                _elbowCounter++;
                Debug.Log("elbow times");
            }

            if (_elbowCounter >= 3)
            {
                _elbowCounter = 0;
                CuttingBoard.cuttingBoard.food.GetComponent<Food>().GenerateCookedFood();
                CuttingBoard.cuttingBoard.food.GetComponent<Food>().food = null;
                GameManager.gm.PlayVideo(true);
                _elbowExercise = false;
            }
        }
    }

    //public bool HoldingKnife
    //{
    //    get { return _holdingKnife; }
    //    set { _holdingKnife = value; }
    //}
}


