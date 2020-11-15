using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //public variable
    public GameObject door;
    public GameObject[] moodIndicator;
    public GameObject orderCanvas;
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

    /// <summary>
    /// Initialize all private variable
    /// </summary>
    void Start()
    {
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
        animator = transform.GetChild(0).GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetBool("Grounded", true);
            animator.SetFloat("MoveSpeed", _speed);
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
                    transform.GetChild(0).rotation = Quaternion.Euler(0f, 180f, 0f);
                    _isSitting = true;
                    orderCanvas.SetActive(true);
                    animator.SetFloat("MoveSpeed", 0f);
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
        animator.SetFloat("MoveSpeed", _speed);
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
                transform.GetChild(0).rotation = Quaternion.Euler(0f, 90f, 0f);
            transform.position = new Vector3(transform.position.x + _speed * direction.normalized.x * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {           
            Destroy(this.gameObject);
        }
    }

    public void SetServing(bool set)
    {
        _isServing = set;
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
    }
}
