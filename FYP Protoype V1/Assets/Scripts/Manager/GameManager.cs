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
    public Text scoreText;

    //private variable
    private float _counter;
    private int _currentCustomer;
    private int _currentScore;
    private float _currentTimer;
    private string _scoreText;
    private string _goalText;
    private string _timerText;
    private string _currentGoal;
    private string _dbName;
    private LevelData _level1;
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
        _scoreText = "Score: ";
        _goalText = "Goal: ";
        _timerText = "Timer: ";
        _dbName = "Performance Data";
        _database = FirebaseDatabase.DefaultInstance;

        //Initialize the HUD
        scoreText.text = _scoreText + _currentScore;
        timerText.text = _timerText + _currentTimer;
        goalText.text = _goalText + _currentGoal;
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
        //fake information
        //int[] performedTimes = new int[4] { 0, 5, 4, 3 };
        //_level1 = new LevelData(1, performedTimes, 210);
        //bool[] selectedExercise = new bool[4] { true, true, false, false };
        //LevelData[] level = new LevelData[1] { _level1 };
        //_performanceData = new PerformanceData(30, selectedExercise);

        var key = _database.GetReference(_dbName).Push().Key;
        _database.GetReference(_dbName).Child(key).Push().SetRawJsonValueAsync(JsonUtility.ToJson(_performanceData));
        _database.GetReference(_dbName).Child(key).Push().SetRawJsonValueAsync(JsonUtility.ToJson(_level1));
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
