using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using FoodType;
using System;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    //public variable
    public GameObject door;
    public GameObject[] moodIndicator;
    public GameObject orderCanvas;
    public GameObject moodCanvas;
    [Tooltip("time to decrease one star (Mood)")]
    public float moodCounter; 
    public float finishMealCounter;
    [Tooltip("Score per heart")]
    public int score;


    //private variable
    private int _chairCounter; //current available chair
    private bool _isSitting; //check if customer is sitting
    private float _moodCounter; //time to decrease one star (mood)
    private int _mood; //current star of the customer (max = 5)
    private bool _isServing; //check customer is serving by player
    private bool _coroutineRunning; //check coroutine "FinishMeal" is running
    private float _speed;
    private bool _reset;
    private bool _chairFound;
    private bool _isLeaving;
    private GameObject[] _chair;
    private Animator animator;
    private MyFoodType _foodOrder;
    private GameObject UICamera;

    /// <summary>
    /// Initialize all private variable
    /// </summary>
    void Start()
    {
        //UICamera = GameObject.FindGameObjectWithTag("UICamera");
        //orderCanvas.GetComponent<Canvas>().worldCamera = UICamera.GetComponent<Camera>();
        //moodCanvas.GetComponent<Canvas>().worldCamera = UICamera.GetComponent<Camera>();

        _isSitting = false;
        _isServing = false;
        _coroutineRunning = false;
        _chairFound = false;
        _reset = false;
        _isLeaving = false;
        _moodCounter = 0f;
        _mood = 5;
        _speed = 1f;
        _chair = GameObject.FindGameObjectsWithTag("Chair");
        animator = gameObject.GetComponent<Animator>();

        animator.SetBool("Walk", true);

        int num = UnityEngine.Random.Range(0, 4);
        _foodOrder = GameManager.gm.foodPlate[num].GetComponent<FoodPlate>().food.GetComponent<Food>().foodType;
        for(int i = 0; i < GameManager.gm.orderImage.Length; i++)
        {
            if (GameManager.gm.orderImage[i].name == _foodOrder.ToString())
            {
                orderCanvas.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.gm.orderImage[i];
                break;
            }                
        }
    }

    void Update()
    {
        //if is sitting false, means the customer just reach the restaurant
        if (!_isSitting)
        {
            if (!_chairFound)
            {
                //check the chair is available
                for (int i = 0; i < _chair.Length; i++)
                {
                    if (!_chair[i].GetComponent<Chair>().occupied)
                    {
                        _chair[i].GetComponent<Chair>().occupied = true;
                        _chair[i].GetComponent<Chair>().currentCustomer = this.gameObject;
                        _chairCounter = i;
                        break;
                    }
                }
                _chairFound = true;
            }
            else
            {
                Vector3 direction = _chair[_chairCounter].transform.position - transform.position;
                if(direction.normalized.x > 0.2f)
                {
                    transform.position = new Vector3(transform.position.x + _speed * direction.normalized.x * Time.deltaTime, transform.position.y, transform.position.z);                    
                }
                else
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0f, 90f, -90f);
                    _isSitting = true;
                    orderCanvas.SetActive(true);
                    animator.SetBool("Walk", false);
                    _chair[_chairCounter].GetComponent<Chair>().GeneratePlate();
                }                
            }
        }
        else //customer waiting to be served
        {           
            if (_isServing) //if serve by player, starts coroutine
            {
                if (!_coroutineRunning) //if coroutine not run, run it
                {
                    StartCoroutine(FinishMeal());
                }
            }
            else //if not serve by player, add time on mood counter
            {
                _moodCounter += Time.deltaTime;
            }
            
            if(_moodCounter > moodCounter && !_isLeaving) //customer wait for too long, decrease one mood indicator
            {
                _moodCounter = 0;
                _mood--;         
                
                if(_mood <= 0)
                {
                    SoundManager.soundManager.MyPlay(4);
                    GameManager.gm.UpdateScore(score * moodIndicator.Length / 2, false);
                    _isLeaving = true;
                }
            }

            for (int i = 0; i < moodIndicator.Length; i++) //display mood indicator
            {
                if (i < _mood)
                {
                    moodIndicator[i].SetActive(true);
                }
                else
                {
                    moodIndicator[i].SetActive(false);
                }
            }

            if (_isLeaving)
            {
                LeaveShop();
            }
        }
    }

    public void LeaveShop()
    {
        orderCanvas.SetActive(false);
        animator.SetBool("Walk", true);
        Vector3 direction = door.transform.position - transform.position;

        if (!_reset)
        {
            _reset = true;
            _chair[_chairCounter].GetComponent<Chair>().MyReset();
            GameManager.gm.UpdateCustomer();            
        }

        if (direction.normalized.x > 0.2f)
        {
            if (transform.GetChild(0).rotation.y != 90f)
                transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, -90f);
            transform.position = new Vector3(transform.position.x + _speed * direction.normalized.x * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {           
            Destroy(this.gameObject);
        }
    }

    public bool Serving
    {
        get { return _isServing; }
        set { _isServing = value; }
    }

    public MyFoodType Order
    {
        get { return _foodOrder; }
        set { _foodOrder = value; }
    }
    /// <summary>
    /// activate this function when player place the food in front of player
    /// take time to finish meal and leave
    /// </summary>
    IEnumerator FinishMeal()
    {
        Debug.Log("Eating");
        _coroutineRunning = true;
        orderCanvas.SetActive(false);
        yield return new WaitForSeconds(finishMealCounter);
        GameManager.gm.UpdateScore(_mood * score, true);
        _mood = 0;
        _isLeaving = true;
        SoundManager.soundManager.MyPlay(3);
    }
}
