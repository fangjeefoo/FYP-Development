using Leap;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public UnityEngine.UI.Image reticleFilled;

    public VideoPlayer leftVideoPlayer;
    public VideoPlayer rightVideoPlayer;

    public VideoClip[] exerciseVideo;

    public GameObject mainCanvas;
    public GameObject endExerciseCanvas;
    public GameObject finalCanvas;
    public GameObject[] exerciseCanvas;

    private int _currentExercise;
    private int _exerciseCounter;
    private bool _buttonEntered;
    private float _buttonCounter;
    private string _choice;
    private bool _pause;

    private bool _fistTrigger;
    private bool _forearmTrigger;
    private bool _wristTrigger;
    private bool _elbowTrigger;

    private Controller controller;


    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        mainCanvas.SetActive(true);

        _pause = true;
        _buttonEntered = false;
        _currentExercise = -1;
        _buttonCounter = 0f;
        _fistTrigger = false;
        _wristTrigger = false;
        _elbowTrigger = false;
        _forearmTrigger = false;
        _exerciseCounter = 0;
        controller = new Controller();
    }

    // Update is called once per frame
    void Update()
    {
        if (_buttonEntered)
        {
            _buttonCounter += Time.deltaTime;
            reticleFilled.fillAmount += 1f / 1.5f * Time.deltaTime;
        }

        if (_buttonCounter >= 1.5f)
        {
            switch (_choice)
            {
                case "NextExercise":
                    NextExercise();
                    break;
                case "Menu":
                    LoadMainMenu();
                    break;
            }
        }
    }

    public void PointerEnter(string choice)
    {
        _buttonEntered = true;
        _choice = choice;
    }

    public void PointerExit()
    {
        _buttonEntered = false;
        reticleFilled.fillAmount = 0;
        _buttonCounter = 0;
        _choice = null;
    }

    public void EndExercise(int exerciseNum)
    {
/*        Debug.Log("Trigger function");
        Debug.Log("current exercise num: " + _currentExercise + " exerciseNum pass in: " + exerciseNum);
        Debug.Log("Pause: " + _pause);*/
        if (_currentExercise == exerciseNum && !_pause)
        {
            _exerciseCounter++;

            if(_exerciseCounter >= 2)
            {
                MyReset();
                _exerciseCounter = 0;
                if (_currentExercise + 1 < exerciseCanvas.Length)
                {
                    _pause = true;
                    endExerciseCanvas.SetActive(true);
                }
                else
                {
                    _pause = true;
                    finalCanvas.SetActive(true);
                }
            }                
        }
    }

    public void NextExercise()
    {
        //if (!_pause)
        //{
            _pause = false;
            MyReset();
            _currentExercise++;

            leftVideoPlayer.clip = exerciseVideo[_currentExercise];
            rightVideoPlayer.clip = exerciseVideo[_currentExercise];
            exerciseCanvas[_currentExercise].SetActive(true);
        //}
    }

    public void MyReset()
    {
        PointerExit();
        leftVideoPlayer.clip = null;
        rightVideoPlayer.clip = null;

        mainCanvas.SetActive(false);
        endExerciseCanvas.SetActive(false);
        finalCanvas.SetActive(false);

        foreach (var obj in exerciseCanvas)
            obj.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Fist()
    {
/*        if (_fistTrigger && !_pause)
        {
            _fistTrigger = false;
           
            Debug.Log("Fist trigger 2");
            return;
        }
        else */if (!_pause)
        {
            EndExercise(0);
            Debug.Log("Fist trigger 1");
            _fistTrigger = true;
        }
    }

    public void Elbow()
    {
/*        if (_elbowTrigger && !_pause)
        {
            _elbowTrigger = false;
            
            Debug.Log("elbow trigger 2");
            return;
        }
        else */if (!_pause)
        {
            Debug.Log("elbow trigger 1");
            EndExercise(3);
            _elbowTrigger = true;
        }
    }

    public void Forearm()
    {
/*        Debug.Log("Forearm");
        if (_forearmTrigger && !_pause) //_forearmTrigger && !_pause
        {
            _forearmTrigger = false;
            
            Debug.Log("forearm trigger 2");
            return;
        }
        else */if(!_pause)
        {
            Debug.Log("forearm trigger 1");
            EndExercise(1);
            _forearmTrigger = true;
        }
    }

    public void Wrist()
    {
/*        if (_wristTrigger && !_pause)
        {
            _wristTrigger = false;            
            Debug.Log("wrist trigger 2");
            return;
        }
        else */if (!_pause)
        {
            Debug.Log("wrist trigger 1");
            EndExercise(2);
            _wristTrigger = true;
        }
    }


}
