using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    //public variable
    public GameObject customer;
    [Tooltip("Max customers in restaurant")]
    public int maxCustomer;
    [Tooltip("time to spawn customer")]
    public float spawnCustomer;
    public Text goalText;
    public Text timerText;
    public int currentLevel;
    public int goalScore; 

    //private variable
    private float _counter;
    private int _currentCustomer;
    private int _currentScore;
    private float _currentTimer;
    private string _goalText;
    private string _timerText;
    private string _dbName;
    private int _levelDuration;
    private bool[] _selectedExercise;
    private int[] _performedTimes;
    private LevelData _currentLevel;
    private PerformanceData _performanceData;
    private FirebaseDatabase _database;

    void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    /// <summary>
    /// Initialize all private variables
    /// Initialize the HUD
    /// </summary>
    void Start()
    {
        //Initialize private variables
        _counter = spawnCustomer;
        _currentCustomer = 1;
        _currentScore = 0;
        _goalText = "Goal: ";
        _timerText = "Timer: ";
        _dbName = "Performance Data";
        _database = FirebaseDatabase.DefaultInstance;
        _performedTimes = new int[4] { 0, 0, 0, 0 };

        //Initialize the HUD
        timerText.text = _timerText + _currentTimer;
        goalText.text = _goalText + _currentScore + "/" + goalScore;

        RetrieveData();
    }

    void Update()
    {
        _counter += Time.deltaTime;

        if(_counter > spawnCustomer) 
        {
            if(_currentCustomer <= maxCustomer)
            {
                Instantiate(customer);
                _currentCustomer++;
            }            
            _counter = 0f;
        }
    }

    public void UpdateCustomer()
    {
        _currentCustomer--;
    }

    /// <summary>
    /// Post data to firebase
    /// </summary>
    void PostData()
    {
        string key = null;

        if(currentLevel == 1)
        {
            key = _database.GetReference(_dbName).Push().Key;
            PlayerPrefs.SetString("Database Key", key);
            _performanceData = new PerformanceData(_levelDuration,_selectedExercise);
            _database.GetReference(_dbName).Child(key).Push().SetRawJsonValueAsync(JsonUtility.ToJson(_performanceData));
        }
        else
        {
            PlayerPrefs.GetString("Database Key");
            _currentLevel = new LevelData(currentLevel, _performedTimes, _currentScore);
            _database.GetReference(_dbName).Child(key).Push().SetRawJsonValueAsync(JsonUtility.ToJson(_currentLevel));
        }
    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async void RetrieveData()
    {
        DataSnapshot dataSnapshot = null;
        Customization customize;

        await _database.GetReference(_dbName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed to retrieve data");
            }
            else if (task.IsCompleted)
            {
                dataSnapshot = task.Result;
                customize = JsonUtility.FromJson<Customization>(dataSnapshot.GetRawJsonValue());

                _selectedExercise = new bool[4];
                _selectedExercise = customize.exercise;
                _levelDuration = customize.exerciseDurationPerLevel;
            }
        });
    }
    public void OnDestroy()
    {
        _database = null;
    }
}
