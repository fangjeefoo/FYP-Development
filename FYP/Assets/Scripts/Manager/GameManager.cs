﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    //public variable
    public GameObject[] foodList;
    public GameObject[] foodPlate;
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
    private string _retrieveDbName;
    private int _levelDuration;
    private bool[] _selectedExercise;
    private bool[] _currentSelectedExercise;
    private int[] _performedTimes;
    private LevelData _currentLevelData;
    private PerformanceData _performanceData;
    private FirebaseDatabase _database;

    void Awake()
    {
        gm = this;
    }

    /// <summary>
    /// Initialize all private variables
    /// Initialize the HUD
    /// </summary>
    void Start()
    {
        //Initialize private variables
        _counter = spawnCustomer/2;
        _currentCustomer = 1;
        _currentScore = 0;
        _goalText = "Goal: ";
        _timerText = "Timer: ";
        _dbName = "Performance Data";
        _retrieveDbName = "Customization";
        _database = FirebaseDatabase.DefaultInstance;
        _performedTimes = new int[4] { 0, 0, 0, 0 };

        //Initialize the HUD        
        goalText.text = _goalText + _currentScore + "/" + goalScore;
        timerText.text = _timerText + _currentTimer;

        RetrieveData();
    }

    void Update()
    {
        _currentTimer -= Time.deltaTime;
        _counter += Time.deltaTime;
        timerText.text = _timerText + Mathf.Round(_currentTimer);

        //if(_currentTimer <= 0)
        //{
        //    PostData();
        //    PlayerPrefs.SetInt("level", currentLevel);
        //    PlayerPrefs.SetInt("duration", _levelDuration);
        //    PlayerPrefs.SetInt("score", _currentScore);
        //    PlayerPrefs.SetInt("goal", goalScore);

        //    if (_currentScore >= goalScore)
        //        SceneManager.LoadScene("Win");             
        //    else
        //        SceneManager.LoadScene("Lose");
        //}

        if (_counter > spawnCustomer) 
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
            key = PlayerPrefs.GetString("Database Key");           
        }

        _currentLevelData = new LevelData(currentLevel, _performedTimes, _currentScore);
        _database.GetReference(_dbName).Child(key).Push().SetRawJsonValueAsync(JsonUtility.ToJson(_currentLevelData));

    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async void RetrieveData()
    {
        Customization customize;

        await _database.GetReference(_retrieveDbName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed to retrieve data");
            }
            else if (task.IsCompleted)
            {
                var dataSnapshot = task.Result;
                customize = JsonUtility.FromJson<Customization>(dataSnapshot.GetRawJsonValue());

                _selectedExercise = new bool[4];
                _currentSelectedExercise = new bool[4];
                _selectedExercise = customize.exercise;
                _currentSelectedExercise = customize.exercise;
                _levelDuration = customize.exerciseDurationPerLevel;
                _currentTimer = _levelDuration * 60f;

                for(int i = currentLevel; i < _currentSelectedExercise.Length; i++)
                {
                    _currentSelectedExercise[i] = false;
                }
            }
        });

        GenerateFood();
    }

    public void OnDestroy()
    {
        _database = null;
    }

    public void UpdateScore(int score, bool increase)
    {
        if(increase)
            _currentScore += score;
        else
            _currentScore -= score;
        goalText.text = _goalText + _currentScore + "/" + goalScore;
    }

    /// <summary>
    /// 0 = fist exercise, 1 = forearm exercise, 2 = elbow exercise, 3 = wrist exercise
    /// </summary>
    /// <param name="exercise"></param>
    public void UpdatePerformedTimes(int exercise)
    {
        if(exercise < _performedTimes.Length)
        {
            _performedTimes[exercise]++;
        }
    }

    public bool[] SelectedExercise
    {
        get { return _currentSelectedExercise; }
    }

    public void GenerateFood()
    {
        List<GameObject> possibleFoodList = new List<GameObject>();
        IEnumerable<GameObject> temp = null;
        IEnumerable<GameObject> temp2 = null;

        for (int i = 0; i < _currentSelectedExercise.Length; i++)
        {
            if (!_currentSelectedExercise[i])
                continue;
            else
            {
                //0 = fist exercise, 1 = forearm exercise (frying), 2 = elbow exercise (shredding), 3 = wrist exercise (soup)
                switch (i)
                {
                    case 0:
                        temp = foodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.cooked);
                        possibleFoodList.AddRange(temp);                        
                        break;
                    case 1:
                        temp = foodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.frying);
                        possibleFoodList.AddRange(temp);
                        break;
                    case 2:
                        temp = foodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.shredding);
                        possibleFoodList.AddRange(temp);
                        break;
                    case 3:
                        temp = foodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.soup);
                        possibleFoodList.AddRange(temp);
                        break;
                }
            }
        }

        if (possibleFoodList.Count > 4)
        {
            for(int i = 0; i < foodPlate.Length; i++)
            {
                if (_currentSelectedExercise[i])
                {
                    switch (i)
                    {
                        case 0:
                            temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.cooked);                                              
                            break;
                        case 1:
                            temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.frying);
                            break;
                        case 2:
                            temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.shredding);
                            break;
                        case 3:
                            temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.soup);
                            break;
                    }

                    if (temp.Count() > 0)
                    {
                        foodPlate[i].GetComponent<FoodPlate>().food = temp.ElementAt(0);
                        temp2 = possibleFoodList.Where(x => x.GetComponent<Food>().foodType == temp.ElementAt(0).GetComponent<Food>().foodType);
                        possibleFoodList = possibleFoodList.Except(temp2).ToList();
                    }
                    else
                    {
                        foodPlate[i].GetComponent<FoodPlate>().food = possibleFoodList[0];
                        temp2 = possibleFoodList.Where(x => x.GetComponent<Food>().foodType == possibleFoodList[0].GetComponent<Food>().foodType);
                        possibleFoodList = possibleFoodList.Except(temp2).ToList();
                    }
                }
                else
                {
                    foodPlate[i].GetComponent<FoodPlate>().food = possibleFoodList[0];
                    temp2 = possibleFoodList.Where(x => x.GetComponent<Food>().foodType == possibleFoodList[0].GetComponent<Food>().foodType);
                    possibleFoodList = possibleFoodList.Except(temp2).ToList();
                }
            }
            //temp2 = _currentSelectedExercise.Where(x => x == true);
            //if(temp2.Count() == 4)
            //{
            //    for (int i = 0; i < foodPlate.Length; i++)
            //    {
            //        switch (i)
            //        {
            //            case 0:
            //                temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.cooked);
            //                foodPlate[i].GetComponent<FoodPlate>().food = temp.ElementAt(0);
            //                break;
            //            case 1:
            //                temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.frying);
            //                foodPlate[i].GetComponent<FoodPlate>().food = temp.ElementAt(0);
            //                break;
            //            case 2:
            //                temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.shredding);
            //                foodPlate[i].GetComponent<FoodPlate>().food = temp.ElementAt(0);
            //                break;
            //            case 3:
            //                temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.soup);
            //                foodPlate[i].GetComponent<FoodPlate>().food = temp.ElementAt(0);
            //                break;
            //        }
            //    }                    
            //}
            //else
            //{

            //}
        }
        else
        {
            for (int i = 0; i < possibleFoodList.Count; i++)
                foodPlate[i].GetComponent<FoodPlate>().food = possibleFoodList[i];
        }
    }
}
