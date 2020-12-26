using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{ 
    //public variable 
    public GameObject[] exercise;
    public GameObject volumeSlider;
    public GameObject exerciseDurationSlider;
    public GameObject performedTimesSlider;
    public Text exerciseDurationText;
    public Text performedTimesText;
    public Text volumeText;
    public Camera cam;
    public Image reticleFilled;

    //private variable
    private FirebaseDatabase _database;
    private enum Choice { exercise2, exercise3, exercise4, back, save, exerciseDuration, performedTimes, volume, none}
    private bool _click;
    private float _counter;
    private Choice _choice;
    private string _dbName;
    private string _performedTimesText;
    private string _exerciseDurationText;
    private string _volumeText;
    private Customization _customize;

    void Start()
    {
        _click = false;
        _counter = 0f;
        _database = FirebaseDatabase.DefaultInstance;
        _dbName = "Customization";        
        _performedTimesText = "Exercise Performed Times: ";
        _exerciseDurationText = "Exercise Duration Per Level: ";
        _volumeText = "Volume: ";
        
        RetrieveData();        
    }

    void Update()
    {
        if (_click)
        {
            _counter += Time.deltaTime;
            reticleFilled.fillAmount += 0.005f;
        }

        if(_counter >= 1.5f)
        {           
            switch (_choice)
            {
                case Choice.exercise2:
                    if (exercise[1].GetComponent<Image>().color == Color.green)
                        exercise[1].GetComponent<Image>().color = Color.white;
                    else
                        exercise[1].GetComponent<Image>().color = Color.green;
                    OnExit();
                    break;
                case Choice.exercise3:
                    if (exercise[2].GetComponent<Image>().color == Color.green)
                        exercise[2].GetComponent<Image>().color = Color.white;
                    else
                        exercise[2].GetComponent<Image>().color = Color.green;
                    OnExit();
                    break;
                case Choice.exercise4:
                    if (exercise[3].GetComponent<Image>().color == Color.green)
                        exercise[3].GetComponent<Image>().color = Color.white;
                    else
                        exercise[3].GetComponent<Image>().color = Color.green;
                    OnExit();
                    break;
                case Choice.back:
                    SceneManager.LoadScene("Menu");
                    break;
                case Choice.save:
                    PostData();
                    break;
            }
        }

        Vector3 cameraPos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        cameraPos = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.down) * Quaternion.AngleAxis(cam.transform.eulerAngles.x, Vector3.left) * cameraPos;
        Transform child;
        //Slider length = 80 - 220
        //reticle = 53 - 153 
        //1 reticle position = 1.4 slider position
        float knobPosition;

        switch (_choice)
        {
            case Choice.exerciseDuration:
                child = exerciseDurationSlider.transform.GetChild(2).transform.GetChild(0);
                knobPosition = child.position.x / 1.4f;

                if (Mathf.Abs(knobPosition - cameraPos.x) <= 2)
                {
                    if ((153f - cameraPos.x) <= 3)
                        exerciseDurationSlider.GetComponent<Slider>().value = exerciseDurationSlider.GetComponent<Slider>().maxValue;
                    else if ((cameraPos.x - 53f) <= 3)
                        exerciseDurationSlider.GetComponent<Slider>().value = exerciseDurationSlider.GetComponent<Slider>().minValue;

                    exerciseDurationText.text = _exerciseDurationText + exerciseDurationSlider.GetComponent<Slider>().value;
                    OnExit();
                }
                else if (cameraPos.x > knobPosition)
                    exerciseDurationSlider.GetComponent<Slider>().value += 1f;                    
                else if (cameraPos.x < knobPosition)
                    exerciseDurationSlider.GetComponent<Slider>().value -= 1f;            

                exerciseDurationText.text = _exerciseDurationText + exerciseDurationSlider.GetComponent<Slider>().value;
                break;
            case Choice.performedTimes:
                child = performedTimesSlider.transform.GetChild(2).transform.GetChild(0);
                knobPosition = child.position.x / 1.4f;

                if (Mathf.Abs(knobPosition - cameraPos.x) <= 2)
                {
                    if ((153f - cameraPos.x) <= 3)
                        performedTimesSlider.GetComponent<Slider>().value = performedTimesSlider.GetComponent<Slider>().maxValue;
                    else if ((cameraPos.x - 53f) <= 3)
                        performedTimesSlider.GetComponent<Slider>().value = performedTimesSlider.GetComponent<Slider>().minValue;

                    performedTimesText.text = _performedTimesText + performedTimesSlider.GetComponent<Slider>().value;
                    OnExit();
                }
                else if (cameraPos.x > knobPosition)
                    performedTimesSlider.GetComponent<Slider>().value += 1f;
                else if (cameraPos.x < knobPosition)
                    performedTimesSlider.GetComponent<Slider>().value -= 1f;

                performedTimesText.text = _performedTimesText + performedTimesSlider.GetComponent<Slider>().value;
                break;
            case Choice.volume:
                child = volumeSlider.transform.GetChild(2).transform.GetChild(0);
                knobPosition = child.position.x / 1.4f;

                if (Mathf.Abs(knobPosition - cameraPos.x) <= 0.5)
                {
                    if ((153f - cameraPos.x) <= 3)
                        volumeSlider.GetComponent<Slider>().value = volumeSlider.GetComponent<Slider>().maxValue;
                    else if ((cameraPos.x - 53f) <= 3)
                        volumeSlider.GetComponent<Slider>().value = volumeSlider.GetComponent<Slider>().minValue;

                    volumeText.text = _volumeText + volumeSlider.GetComponent<Slider>().value;
                    OnExit();
                }
                else if (cameraPos.x > knobPosition)
                    volumeSlider.GetComponent<Slider>().value += 0.01f;
                else if(cameraPos.x < knobPosition)
                    volumeSlider.GetComponent<Slider>().value -= 0.01f;

                volumeText.text = _volumeText + volumeSlider.GetComponent<Slider>().value;
                break;
        }
    }

    public void Exercise2OnEnter()
    {
        OnEnter();
        _choice = Choice.exercise2;
    }

    public void Exercise3OnEnter()
    {
        OnEnter();
        _choice = Choice.exercise3;
    }

    public void Exercise4OnEnter()
    {
        OnEnter();
        _choice = Choice.exercise4;
    }

    public void BackOnEnter()
    {
        OnEnter();
        _choice = Choice.back;
    }

    public void SaveOnEnter()
    {
        OnEnter();
        _choice = Choice.save;
    }

    public void ExerciseDurationSliderOnEnter()
    {
        _choice = Choice.exerciseDuration;
    }

    public void PerformedTimesSliderOnEnter()
    {
        _choice = Choice.performedTimes;
    }

    public void VolumeSliderOnEnter()
    {
        _choice = Choice.volume;
    }

    public void OnEnter()
    {
        _click = true;
        reticleFilled.enabled = true;
    }

    public void OnExit()
    {
        if (_choice == Choice.volume && SoundManager.soundManager != null)
            SoundManager.soundManager.GetComponent<AudioSource>().volume = volumeSlider.GetComponent<Slider>().value;
           
        _click = false;
        _counter = 0;
        _choice = Choice.none;

        if(reticleFilled != null)
        {
            reticleFilled.enabled = false;
            reticleFilled.fillAmount = 0;
        }
    }

    /// <summary>
    /// Create a object to store all informations
    /// </summary>
    /// <returns></returns>
    public Customization PackData()
    {
        int exerciseDuration = (int)exerciseDurationSlider.GetComponent<Slider>().value;
        int performedTimes = (int)performedTimesSlider.GetComponent<Slider>().value;
        float vol = volumeSlider.GetComponent<Slider>().value;
        bool[] exerciseArray = new bool[4];
        
        for(int i = 0; i < exercise.Length; i++)
        {
            if (exercise[i].GetComponent<Image>().color == Color.green)
                exerciseArray[i] = true;
            else
                exerciseArray[i] = false;
        }

        Customization customize = new Customization(exerciseDuration, exerciseArray, performedTimes, vol);

        return customize;
    }

    /// <summary>
    /// extract data from json
    /// </summary>
    public void ExtractData()
    {
        for (int i = 0; i < _customize.exercise.Length; i++)
        {
            if (_customize.exercise[i])
                exercise[i].GetComponent<Image>().color = Color.green;
        }
        exerciseDurationSlider.GetComponent<Slider>().value = _customize.exerciseDurationPerLevel;
        performedTimesSlider.GetComponent<Slider>().value = _customize.exerciseTime;
        volumeSlider.GetComponent<Slider>().value = _customize.volume;

        exerciseDurationText.text = _exerciseDurationText + exerciseDurationSlider.GetComponent<Slider>().value;
        performedTimesText.text = _performedTimesText + performedTimesSlider.GetComponent<Slider>().value;
        volumeText.text = _volumeText + volumeSlider.GetComponent<Slider>().value;
    }

    /// <summary>
    /// Push data to firebase, return back to main menu
    /// </summary>
    public void PostData()
    {
        _database.GetReference(_dbName).SetRawJsonValueAsync(JsonUtility.ToJson(PackData()));
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async void RetrieveData()
    {
        DataSnapshot dataSnapshot = null;

        await _database.GetReference(_dbName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed to retrieve data");
            }
            else if (task.IsCompleted)
            {
                dataSnapshot = task.Result;
                _customize = JsonUtility.FromJson<Customization>(dataSnapshot.GetRawJsonValue());
            }
        });

        ExtractData();
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
