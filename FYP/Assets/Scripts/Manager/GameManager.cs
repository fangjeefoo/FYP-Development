using System.Collections;
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
    public GameObject[] cookedFoodPlate;
    public GameObject[] foodPlate;
    public GameObject[] customerList;
    public GameObject conversation1;
    public GameObject conversation2;

    public Image reticleFilled;

    public Sprite[] orderImage;

    [Tooltip("Max customers in restaurant")]  public int maxCustomer;
    public int currentLevel;
    public int goalScore;

    [Tooltip("time to spawn customer")] public float spawnCustomer;

    public Text goalText;
    public Text timerText;

    [HideInInspector] public bool pauseCounter;

    //private variable
    private GameObject _selectedObject;
    private GameObject _selectedKitchenware;
    private GameObject _grabObject;
    private GameObject _customer;

    private int _currentCustomer;
    private int _currentScore;
    private int _levelDuration;
    private int[] _performedTimes;

    private float _counter;
    private float _currentTimer;
    private float _buttonCounter;

    private string _goalText;
    private string _timerText;
    private string _dbName;
    private string _retrieveDbName;

    private bool[] _selectedExercise;
    private bool[] _currentSelectedExercise;
    private bool _buttonEntered;

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
        pauseCounter = false;
        _buttonEntered = false;
        _counter = spawnCustomer / 2;
        _currentCustomer = 1;
        _currentScore = 0;
        _buttonCounter = 0;
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
        Shuffle();
    }

    void Update()
    {
        if (_buttonEntered)
        {
            _buttonCounter += Time.deltaTime;
            reticleFilled.fillAmount += 0.005f;
        }

        if(_buttonCounter >= 1.5f)
            ConversationOnClick();

        if(!pauseCounter && _currentTimer > 0)
            _currentTimer -= Time.deltaTime;

        _counter += Time.deltaTime;
        timerText.text = _timerText + Mathf.Round(_currentTimer);

        if (_counter > spawnCustomer)
        {
            if (_currentCustomer <= maxCustomer)
            {
                _customer = Instantiate(customerList[0]);
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

        if (currentLevel == 1)
        {
            key = _database.GetReference(_dbName).Push().Key;
            PlayerPrefs.SetString("Database Key", key);
            _performanceData = new PerformanceData(_levelDuration, _selectedExercise);
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

                //_selectedExercise = new bool[4];
                //_currentSelectedExercise = new bool[4];
                _selectedExercise = customize.exercise;
                _currentSelectedExercise = customize.exercise;
                _levelDuration = customize.exerciseDurationPerLevel;
                _currentTimer = _levelDuration * 60f;

                for (int i = currentLevel; i < _currentSelectedExercise.Length; i++)
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
        if (increase)
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
        if (exercise < _performedTimes.Length)
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
            for (int i = 0; i < foodPlate.Length; i++)
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

    public void SelectObject(GameObject obj)
    {
        _selectedObject = obj;
    }

    public void DeselectObject()
    {
        _selectedObject = null;
    }

    public void SelectKitchenware(GameObject obj)
    {
        _selectedKitchenware = obj;
    }

    public void DeselectKitchenware()
    {
        _selectedKitchenware = null;
    }

    public void GrabObject()
    {
        if(_selectedObject != null)
        {           
            _grabObject = _selectedObject;
            _selectedObject = null;
            UpdatePerformedTimes(0);
            Debug.Log("Grab here: " + _grabObject);
            SoundManager.soundManager.MyPlay(5);
        }
    }

    public void ReleaseObject()
    {
        if (_selectedKitchenware != null && _grabObject != null)
        {
            Debug.Log("Release here: " + _grabObject);
            var pos = _selectedKitchenware.transform.position;
            pos.y += 0.1f;
            _grabObject.transform.position = pos;
            _grabObject = null;
        }        
    }

    /// <summary>
    /// Reference from: https://answers.unity.com/questions/1189736/im-trying-to-shuffle-an-arrays-order.html
    /// </summary>
    public void Shuffle()
    {
        for (int i = 0; i < foodList.Length - 1; i++)
        {
            int rnd = Random.Range(i, foodList.Length);
            var temp = foodList[rnd];
            foodList[rnd] = foodList[i];
            foodList[i] = temp;
        }
    }

    public void CallConversationCoroutine()
    {
        if (_currentTimer > 0)
            StartCoroutine(ShowConversation());
        else
            SaveGame();
    }

    IEnumerator ShowConversation()
    {
        pauseCounter = true;
        conversation1.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        conversation1.SetActive(false);
        _customer.GetComponent<Customer>().MyReset();
        yield return new WaitForSeconds(1.0f);        
        conversation2.SetActive(true);
    }

    public void ConversationOnClick()
    {
        pauseCounter = false;
        conversation2.SetActive(false);
        PointerExit();
    }

    public void PointerEnter()
    {
        _buttonEntered = true;
    }

    public void PointerExit()
    {
        _buttonEntered = false;
        reticleFilled.fillAmount = 0;
        _buttonCounter = 0;
    }

    public void SaveGame()
    {
        SoundManager.soundManager.MyPlay(6);
        PostData();
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.SetInt("duration", _levelDuration);
        PlayerPrefs.SetInt("score", _currentScore);
        PlayerPrefs.SetInt("goal", goalScore);

        if (_currentScore >= goalScore)
            SceneManager.LoadScene("Win");
        else
            SceneManager.LoadScene("Lose");
    }
}
