using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            if(_currentCustomer < maxCustomer)
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
}
