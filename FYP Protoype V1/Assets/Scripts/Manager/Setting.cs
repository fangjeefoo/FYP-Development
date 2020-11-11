using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics.Tracing;
using Firebase.Auth;

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

    //private variable
    private FirebaseDatabase _database;
    private enum Choice { exercise1, exercise2, exercise3, exercise4, back, save, exerciseDuration, performedTimes, volume, none}
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
        _exerciseDurationText = "Exercise Duration: ";
        _volumeText = "Volume: ";

        RetrieveData();        
    }

    // Update is called once per frame
    void Update()
    {
        if (_click)
        {
            _counter += Time.deltaTime;
        }

        if(_counter >= 1.5f)
        {           
            switch (_choice)
            {
                case Choice.exercise1:
                    if (exercise[0].GetComponent<Image>().color == Color.green)
                    {
                        exercise[0].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[0].GetComponent<Image>().color = Color.green;
                    }                        
                    OnExit();
                    break;
                case Choice.exercise2:
                    if (exercise[1].GetComponent<Image>().color == Color.green)
                    {
                        exercise[1].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[1].GetComponent<Image>().color = Color.green;
                    }
                    OnExit();
                    break;
                case Choice.exercise3:
                    if (exercise[2].GetComponent<Image>().color == Color.green)
                    {
                        exercise[2].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[2].GetComponent<Image>().color = Color.green;
                    }
                    OnExit();
                    break;
                case Choice.exercise4:
                    if (exercise[3].GetComponent<Image>().color == Color.green)
                    {
                        exercise[3].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[3].GetComponent<Image>().color = Color.green;
                    }
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

        switch (_choice)
        {
            case Choice.exerciseDuration:
                exerciseDurationSlider.GetComponent<Slider>().value += 1f;
                exerciseDurationText.text = _exerciseDurationText + exerciseDurationSlider.GetComponent<Slider>().value;
                break;
            case Choice.performedTimes:
                performedTimesSlider.GetComponent<Slider>().value += 1f;
                performedTimesText.text = _performedTimesText + performedTimesSlider.GetComponent<Slider>().value;
                break;
            case Choice.volume:
                volumeSlider.GetComponent<Slider>().value += 0.01f;
                volumeText.text = _volumeText + volumeSlider.GetComponent<Slider>().value;
                break;
        }
        //if (_drag)
        //{
        //    Transform child = volumeSlider.transform.GetChild(2).transform.GetChild(0);
        //    Vector3 p = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        //    Vector3 q = cam.ScreenToWorldPoint(new Vector3(child.position.x, child.position.y, cam.nearClipPlane));
        //    Debug.Log(p);
        //    Debug.Log(q);
        //    if (p.x > q.x)
        //        volumeSlider.GetComponent<Slider>().value += 0.01f;
        //    else if (p.x < q.x)
        //        volumeSlider.GetComponent<Slider>().value -= 0.01f;
        //    else
        //        VolumeSliderOnExit();
        //    //Debug.Log("Drag here");
        //    //PointerEventData e = new PointerEventData(EventSystem.current);
        //    //var myevent = e;
        //    //Debug.Log(myevent.pressPosition);
        //    //Debug.Log(PointerEventData.InputButton.Left);
        //    //volumeSlider.GetComponent<Slider>().OnDrag(e);
        //}
    }

    public void Exercise1OnEnter()
    {
        _click = true;
        _choice = Choice.exercise1;
    }

    public void Exercise2OnEnter()
    {
        _click = true;
        _choice = Choice.exercise2;
    }

    public void Exercise3OnEnter()
    {
        _click = true;
        _choice = Choice.exercise3;
    }

    public void Exercise4OnEnter()
    {
        _click = true;
        _choice = Choice.exercise4;
    }

    public void BackOnEnter()
    {
        _click = true;
        _choice = Choice.back;
    }

    public void SaveOnEnter()
    {
        _click = true;
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
        //Debug.Log("Enter");
        //PointerEventData e = new PointerEventData(EventSystem.current);
        ////Vector3 p = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        //Vector3 p = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        ////Vector3 q = cam.ViewportToWorldPoint(new Vector3(volumeSlider.transform.position.x, volumeSlider.transform.position.y, cam.nearClipPlane));
        //Transform child = volumeSlider.transform.GetChild(2).transform.GetChild(0);
        //Vector3 q = cam.ScreenToWorldPoint(new Vector3(child.position.x, child.position.y,child.position.z));
        //float posXMax = Camera.main.pixelWidth;
        //Debug.Log(p);
        //Debug.Log(q);
        // Debug.Log(posXMax);
        //volumeSlider.GetComponent<Slider>().OnPointerDown(e);
       // volumeSlider.GetComponent<Slider>().value = p.x / q.x;
       // Debug.Log(p.x / q.x);
        //_drag = true;
    }
    public void OnExit()
    {
        _click = false;
        _counter = 0;
        _choice = Choice.none;
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

        exerciseDurationSlider.GetComponent<Slider>().value = _customize.exerciseDuration;
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

    /// <summary>
    /// Check data is save properly in database
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Save()
    {
        var dataSnapshot = await _database.GetReference(_dbName).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
