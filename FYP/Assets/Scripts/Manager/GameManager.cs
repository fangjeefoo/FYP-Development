using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Video;
using Firebase.Extensions;
using FoodType;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public enum ButtonChoice { none, returnGame, mainMenu, conversationOK};

    //public variable
    public GameObject[] foodList;
    public GameObject[] cookedFoodPlate;
    public GameObject[] foodPlate;
    public GameObject[] customerList;
    public GameObject conversation1;
    public GameObject conversation2;
    public GameObject conversation3;
    public GameObject moodFeedback;
    public GameObject scoreFeedback;
    public GameObject pauseCanvas;
    public GameObject HUDCanvas;

    public Image reticleFilled;

    public Sprite[] orderImage;

    public VideoClip elbowVideo;
    public VideoClip wristVideo;
    public VideoClip forearmVideo;
    public VideoClip fistVideo;

    public VideoPlayer leftVideoPlayer;
    public VideoPlayer rightVideoPlayer;

    [Tooltip("Max customers in restaurant")]  public int maxCustomer;
    public int currentLevel;
    public int goalScore;

    [Tooltip("time to spawn customer")] public float spawnCustomer;

    public TextMesh goalText;
    public TextMesh timerText;
    public TextMesh scoreText;

    [HideInInspector] public bool pauseCounter;

    //private variable
    private GameObject _selectedObject;
    private GameObject _selectedKitchenware;
    private GameObject _grabObject;
    private GameObject _customer;
    private GameObject _chair;

    private int _currentCustomer;
    private int _currentScore;
    private int _levelDuration;
    private int _numberPlateReset;
    private int[] _performedTimes;

    private float _counter;
    private float _currentTimer;
    private float _buttonCounter;

    private string _scoreText;
    private string _dbName;
    private string _retrieveDbName;    

    private bool[] _selectedExercise;
    private bool[] _currentSelectedExercise;
    private bool _buttonEntered;
    private bool _timeEndSFX;

    private LevelData _currentLevelData;
    private PerformanceData _performanceData;
    private FirebaseDatabase _database;

    private ButtonChoice _selectedButton;

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
        _timeEndSFX = false;
        _currentTimer = 60f;
        _counter = spawnCustomer / 2;
        _currentCustomer = 1;
        _currentScore = 0;
        _buttonCounter = 0;
        _scoreText = "$";
        _dbName = "Performance Data";
        _retrieveDbName = "Customization";
        _database = FirebaseDatabase.DefaultInstance;
        _performedTimes = new int[4] { 0, 0, 0, 0 };
        _chair = GameObject.FindGameObjectWithTag("Chair");

        //Initialize the HUD        
        goalText.text = _scoreText + goalScore;
        scoreText.text = _scoreText + _currentScore;
       
        RetrieveData();
        Shuffle();
    }

    void Update()
    {
        if (_buttonEntered)
        {
            _buttonCounter += Time.deltaTime;
            reticleFilled.fillAmount += 1f / 1.5f * Time.deltaTime;
        }

        if(_buttonCounter >= 1.5f)
        {
            switch (_selectedButton)
            {
                case ButtonChoice.returnGame:
                    UnpauseGame();
                    break;
                case ButtonChoice.mainMenu:
                    LoadMainMenu();
                    break;
                case ButtonChoice.conversationOK:
                    ConversationOnClick();
                    break;
            }
        }
            
        if(!pauseCounter && _currentTimer > 0)
        {
            _counter += Time.deltaTime;
            _currentTimer -= Time.deltaTime;
            timerText.text = Mathf.Round(_currentTimer).ToString();
        }           

        if (_currentTimer <= 0 && SoundManager.soundManager && !_timeEndSFX)
        {
            SoundManager.soundManager.MyPlay(6);
            _timeEndSFX = true;
        }          

        if (_counter > spawnCustomer && _currentTimer > 0 && _currentCustomer <= maxCustomer)
        {
            //if (_currentCustomer <= maxCustomer)
            //{
                _customer = Instantiate(customerList[0]);
                _currentCustomer++;
            //}
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
    /// 
    
    public async void RetrieveData()
    {
        Customization customize;
        DataSnapshot dataSnapshot;

        await _database.GetReference(_retrieveDbName).GetValueAsync().ContinueWithOnMainThread(task =>
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
                _currentSelectedExercise = new bool[4];
                _selectedExercise = customize.exercise;
                _levelDuration = customize.exerciseDurationPerLevel;
                _currentTimer = _levelDuration * 60f;

                timerText.text = _currentTimer.ToString();

                for (int i = 0; i < 4; i++)
                {
                    if (_selectedExercise[i])
                        _currentSelectedExercise[i] = true;
                    else
                        _currentSelectedExercise[i] = false;
                }

                for (int i = currentLevel; i < _currentSelectedExercise.Length; i++)
                    _currentSelectedExercise[i] = false;
            }
        });

        GenerateFood();
    }
    

    /*
    public async void RetrieveData()
    {
        DataSnapshot dataSnapshot = null;

        await _database.GetReference(_retrieveDbName).GetValueAsync().ContinueWith(task =>
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

        Debug.Log("all: ");
        foreach (var mybool in _customize.exercise)
            Debug.Log(mybool);

        _selectedExercise = new bool[4];
        _currentSelectedExercise = new bool[4];
        _selectedExercise = _customize.exercise;
        _levelDuration = _customize.exerciseDurationPerLevel;
        _currentTimer = _levelDuration * 60f;

        for (int i = 0; i < 4; i++)
        {
            if (_selectedExercise[i])
                _currentSelectedExercise[i] = true;
            else
                _currentSelectedExercise[i] = false;
        }

        for (int i = currentLevel; i < _currentSelectedExercise.Length; i++)
            _currentSelectedExercise[i] = false;

        GenerateFood();
    }
    */

    public void OnDestroy()
    {
        _database = null;
    }

    public void UpdateScore(int score, bool increase)
    {
        if (increase)
        {
            _currentScore += score;
            scoreFeedback.GetComponent<TextMesh>().text = "+$" + score;
        }

        else
        {
            _currentScore -= score;
            scoreFeedback.GetComponent<TextMesh>().text = "-$" + score;
        } 
        
        scoreText.text = _scoreText + _currentScore;
        CallFeedbackCoroutine();
    }

    /// <summary>
    /// 0 = fist exercise, 1 = forearm exercise, 2 = wrist exercise, 3 = elbow exercise
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
                //0 = fist exercise, 1 = forearm exercise (frying), , 2 = wrist exercise (soup), 3 = elbow exercise (shredding)
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
                        temp = foodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.soup);
                        possibleFoodList.AddRange(temp);
                        break;
                    case 3:
                        temp = foodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.shredding);
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
                            temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.soup);
                            break;
                        case 3:
                            temp = possibleFoodList.Where(x => x.GetComponent<Food>().cookType == FoodType.CookType.shredding);
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
        if(_selectedObject != null && !pauseCounter)
        {    
            if(_customer.GetComponent<Customer>().Order == _selectedObject.gameObject.GetComponent<Food>().foodType)
            {
                DisplayHint(_selectedObject);
                _grabObject = _selectedObject;

                if(_grabObject.GetComponent<Renderer>())
                    _grabObject.GetComponent<Renderer>().enabled = false;
                else
                {
                    foreach(var obj in _grabObject.transform.GetComponentsInChildren<Renderer>())
                    {
                        obj.enabled = false;
                    }
                }
                _selectedObject = null;
                UpdatePerformedTimes(0);
                Debug.Log("Grab here: " + _grabObject);
                if (SoundManager.soundManager)
                    SoundManager.soundManager.MyPlay(5);
            }
            else
            {
                if (SoundManager.soundManager)
                    SoundManager.soundManager.MyPlay(7);
            }
        }
    }

    public void ReleaseObject()
    {
        if (_selectedKitchenware != null && _grabObject != null && !pauseCounter)
        {
            if(_selectedKitchenware == Pan.pan.gameObject)
            {
                if (MyHand.handManager.UpdateForearmExercise || _grabObject.GetComponent<Food>().cookType != CookType.frying)
                {
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(7);
                    return;
                }
            }
            else if(_selectedKitchenware == DeepPan.deepPan.gameObject)
            {
                if (MyHand.handManager.UpdateWristExercise || _grabObject.GetComponent<Food>().cookType != CookType.soup)
                {
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(7);
                    return;
                }
            }
            else if(_selectedKitchenware == CuttingBoard.cuttingBoard.gameObject)
            {
                if (MyHand.handManager.UpdateElbowExercise ||_grabObject.GetComponent<Food>().cookType != CookType.shredding)
                {
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(7);
                    return;
                }
            }
            else if(_selectedKitchenware == _chair.GetComponent<Chair>().GetCurrentPlate())
            {
                if(!(_customer.GetComponent<Customer>().Order == _grabObject.GetComponent<Food>().foodType) && !(_grabObject.GetComponent<Food>().cookType == CookType.cooked))
                {
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(7);
                    return;
                }
            }

            Debug.Log("Release here: " + _grabObject);
            if (_grabObject.GetComponent<Renderer>())
                _grabObject.GetComponent<Renderer>().enabled = true;
            else
            {
                foreach (var obj in _grabObject.transform.GetComponentsInChildren<Renderer>())
                {
                    obj.enabled = true;
                }
            }
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

    public void CallConversationCoroutine(bool commentFood = false, bool failedOrder = false)
    {
        StopVideo();
        ResetKitchenware();
        if (!commentFood)
            StartCoroutine(ShowConversation());
        else
            StartCoroutine(ShowFoodComment(failedOrder));
    }

    private void ResetKitchenware()
    {
        if (cookedFoodPlate[0].GetComponent<FoodPlate>().holdingFood)
            Destroy(cookedFoodPlate[0].GetComponent<FoodPlate>().holdingFood.gameObject);
        if (Pan.pan.GetComponent<Pan>().food)
            Destroy(Pan.pan.GetComponent<Pan>().food.gameObject);
        if (DeepPan.deepPan.GetComponent<DeepPan>().food)
            Destroy(DeepPan.deepPan.GetComponent<DeepPan>().food.gameObject);
        if (CuttingBoard.cuttingBoard.GetComponent<CuttingBoard>().food)
            Destroy(CuttingBoard.cuttingBoard.GetComponent<CuttingBoard>().food.gameObject);
    }

    public void CallFeedbackCoroutine(bool mood = false)
    {
        StartCoroutine(ShowFeedback(mood));
    }

    IEnumerator ShowFeedback(bool mood = false)
    {
        if (mood)
            moodFeedback.SetActive(true);
        else
            scoreFeedback.SetActive(true);
            
        yield return new WaitForSeconds(2f);
        moodFeedback.SetActive(false);
        scoreFeedback.SetActive(false);
    }

    IEnumerator ShowFoodComment(bool failedOrder)
    {
        pauseCounter = true;
        Debug.Log("Failed order: " + failedOrder);
        Debug.Log("Condition Failed order: " + !failedOrder);
        if (!failedOrder)
        {
            Debug.Log("Show comment");
            string[] foodComment = new string[] { "Food is yummy", "Food tastes great!", "Food is really good!" };
            int rand = Random.Range(0, foodComment.Length);


            conversation3.SetActive(true);
            conversation3.transform.GetChild(0).GetComponent<Text>().text = foodComment[rand];
        }
        else
            Debug.Log("Correct, no show comment");
        yield return new WaitForSeconds(2f);
        conversation3.SetActive(false);
        StartCoroutine(ShowConversation());
    }

    IEnumerator ShowConversation()
    {
        pauseCounter = true;
        conversation1.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        conversation1.SetActive(false);
        _customer.GetComponent<Customer>().MyReset();
        yield return new WaitForSeconds(1.5f);        
        conversation2.SetActive(true);
    }

    public void ConversationOnClick()
    {
        for(int i = 0; i < foodPlate.Length; i++)
        {
            if(foodPlate[i].GetComponent<FoodPlate>().holdingFood.GetComponent<Food>().foodType == _customer.GetComponent<Customer>().GetFoodType())
            {
                foodPlate[i].GetComponent<FoodPlate>().ChangeColor();
                _numberPlateReset = i;
                break;
            }
        }

        PlayVideo(true);
        pauseCounter = false;
        conversation2.SetActive(false);
        PointerExit();
    }

    public void PointerEnter(string choice)
    {
        _buttonEntered = true;
        switch (choice)
        {
            case "menu":
                _selectedButton = ButtonChoice.mainMenu;
                break;
            case "return":
                _selectedButton = ButtonChoice.returnGame;
                break;
            case "conversation":
                _selectedButton = ButtonChoice.conversationOK;
                break;
        }
    }

    public void PointerExit()
    {
        _buttonEntered = false;
        reticleFilled.fillAmount = 0;
        _buttonCounter = 0;
        _selectedButton = ButtonChoice.none;
    }

    public void SaveGame()
    {
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

    public float GetTimer()
    {
        return _currentTimer;
    }

    public void PlayVideo(bool playFistVideo = false)
    {
        if(MyHand.handManager.UpdateWristExercise && leftVideoPlayer.clip != wristVideo)
        {
            leftVideoPlayer.clip = wristVideo;
            rightVideoPlayer.clip = wristVideo;
        }
        else if (MyHand.handManager.UpdateElbowExercise && leftVideoPlayer.clip != elbowVideo)
        {
            leftVideoPlayer.clip = elbowVideo;
            rightVideoPlayer.clip = elbowVideo;
        }
        else if (MyHand.handManager.UpdateForearmExercise && leftVideoPlayer.clip != forearmVideo)
        {
            leftVideoPlayer.clip = forearmVideo;
            rightVideoPlayer.clip = forearmVideo;
        }
        else if(playFistVideo)
        {
            leftVideoPlayer.clip = fistVideo;
            rightVideoPlayer.clip = fistVideo;
        }
    }

    public void StopVideo()
    {
        leftVideoPlayer.clip = null;
        rightVideoPlayer.clip = null;
    }

    public void DisplayHint(GameObject obj, bool generateFood = false)
    {
        CancelHint();


        if (generateFood)
        {
            cookedFoodPlate[0].GetComponent<FoodPlate>().ChangeColor();
            return;
        }
        

        if (obj)
        {
            switch (obj.GetComponent<Food>().cookType)
            {
                case CookType.cooked:
                    _chair.GetComponent<Chair>().ChangeColor();
                    break;
                case CookType.frying:
                    Pan.pan.ChangeColor();
                    break;
                case CookType.shredding:
                    CuttingBoard.cuttingBoard.ChangeColor();
                    break;
                case CookType.soup:
                    DeepPan.deepPan.ChangeColor();
                    break;
            }
        }
    }

    public void CancelHint()
    {
        foodPlate[_numberPlateReset].GetComponent<FoodPlate>().ResetColor();
        _chair.GetComponent<Chair>().ResetColor();
        cookedFoodPlate[0].GetComponent<FoodPlate>().ResetColor();
        DeepPan.deepPan.ResetColor();
        Pan.pan.ResetColor();
        CuttingBoard.cuttingBoard.ResetColor();
    }

    public void PauseGame()
    {
        pauseCounter = true;
        pauseCanvas.SetActive(true);
        HUDCanvas.SetActive(false);

        if (_customer)
        {
            _customer.GetComponent<Animator>().enabled = false;
            _customer.transform.GetChild(2).gameObject.SetActive(false);
            _customer.transform.GetChild(3).gameObject.SetActive(false);
        }
            

        if (leftVideoPlayer && rightVideoPlayer)
        {
            leftVideoPlayer.Pause();
            rightVideoPlayer.Pause();
        }

        if (SoundManager.soundManager)
        {
            SoundManager.soundManager.eating.Stop();
            SoundManager.soundManager.frying.Stop();
            SoundManager.soundManager.boiling.Stop();
        }
    }

    public void UnpauseGame()
    {
        PointerExit();
        pauseCounter = false;
        pauseCanvas.SetActive(false);
        HUDCanvas.SetActive(true);
        if (_customer)
        {
            _customer.GetComponent<Animator>().enabled = true;
            _customer.transform.GetChild(2).gameObject.SetActive(true);
            if (_customer.GetComponent<Customer>().Sitting && !_customer.GetComponent<Customer>().Serving)
                _customer.transform.GetChild(3).gameObject.SetActive(true);
        }


        if (leftVideoPlayer && rightVideoPlayer)
        {
            leftVideoPlayer.Play();
            rightVideoPlayer.Play();
        }

        if(SoundManager.soundManager)
        {
            if(_customer.GetComponent<Customer>().Serving)
                SoundManager.soundManager.eating.Play();
            if(Pan.pan.food)
                SoundManager.soundManager.frying.Play();
            if(DeepPan.deepPan.food)
                SoundManager.soundManager.boiling.Play();
        }
    }

    public void LoadMainMenu()
    {
        PointerExit();
        SceneManager.LoadScene("Menu");
    }
}
