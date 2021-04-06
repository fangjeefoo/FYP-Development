using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using FoodType;

public class MyHand : MonoBehaviour
{
    public static MyHand handManager;

    //private variable
    private bool _forearmExercise;
    private bool _elbowExercise;
    private bool _wristExercise;
    private int _forearmCounter;
    private int _elbowCounter;
    private int _wristCounter;

    // Start is called before the first frame update
    void Start()
    {
        handManager = this;
        _forearmExercise = false;
        _elbowExercise = false;
        _wristExercise = false;
        _forearmCounter = 0;
        _elbowCounter = 0;
        _wristCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_forearmExercise)
            GameManager.gm.PlayVideo(false);

        if (_wristExercise)
            GameManager.gm.PlayVideo(false);

        if (_elbowExercise) 
            GameManager.gm.PlayVideo(false);
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

    public void ForearmExercise()
    {
        if (!GameManager.gm.pauseCounter)
        {
            if (_forearmExercise)                             
            {
                GameManager.gm.UpdatePerformedTimes(1);
                _forearmCounter++;
                Pan.pan.EnableParticleSystem();
            }

            if (_forearmCounter >= 3)
            {
                _forearmCounter = 0;
                if (SoundManager.soundManager)
                    SoundManager.soundManager.frying.Stop();
                Pan.pan.particleSystem.Stop();
                Pan.pan.food.GetComponent<Food>().GenerateCookedFood();
                GameManager.gm.PlayVideo(true);
                Pan.pan.food.GetComponent<Food>().food = null;
                _forearmExercise = false;
                GameManager.gm.isCooking = false;
            }
        }
    }

    public void WristExercise()
     {
        if(!GameManager.gm.pauseCounter)
        {
            if (_wristExercise)
            {
                DeepPan.deepPan.EnableParticleSystem();
                GameManager.gm.UpdatePerformedTimes(2);
                _wristCounter++;
            }

            if (_wristCounter >= 3)
            {
                DeepPan.deepPan.particleSystem.Stop();
                if (SoundManager.soundManager)
                    SoundManager.soundManager.boiling.Stop();

                _wristCounter = 0;
                DeepPan.deepPan.food.GetComponent<Food>().GenerateCookedFood();
                DeepPan.deepPan.food.GetComponent<Food>().food = null;
                GameManager.gm.PlayVideo(true);
                _wristExercise = false;
                GameManager.gm.isCooking = false;
            }
        }
    }

    public void ElbowExercise()
    {
        if (!GameManager.gm.pauseCounter)
        {
            if (_elbowExercise)
            {
                if (SoundManager.soundManager)
                    SoundManager.soundManager.MyPlay(8);
                CuttingBoard.cuttingBoard.EnableParticleSystem();
                GameManager.gm.UpdatePerformedTimes(3);
                _elbowCounter++;
            }

            if (_elbowCounter >= 3)
            {
                _elbowCounter = 0;
                CuttingBoard.cuttingBoard.food.GetComponent<Food>().GenerateCookedFood();
                CuttingBoard.cuttingBoard.food.GetComponent<Food>().food = null;
                GameManager.gm.PlayVideo(true);
                _elbowExercise = false;
                GameManager.gm.isCooking = false;
            }
        }
    }
}


