using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //public variable
    public GameObject[] chair;
    public GameObject door;
    public GameObject[] moodIndicator;
    public GameObject orderCanvas;
    [Tooltip("time to decrease one star (Mood)")]
    public float moodCounter; 
    public float finishMealCounter;


    //private variable
    private int _chairCounter; //current available chair
    private bool _isSitting; //check if customer is sitting
    private float _moodCounter; //time to decrease one star (mood)
    private int _mood; //current star of the customer (max = 5)
    private bool _isServing; //check customer is serving by player
    private bool _coroutineRunning; //check coroutine "FinishMeal" is running
    private float _speed;
    private bool _chairFound;

    /// <summary>
    /// Initialize all private variable
    /// </summary>
    void Start()
    {
        _isSitting = false;
        _isServing = false;
        _coroutineRunning = false;
        _chairFound = false;
        _moodCounter = 0f;
        _mood = 5;
        _speed = 1f;
    }

    void Update()
    {
        //if is sitting false, means the customer just reach the restaurant
        if (!_isSitting)
        {
            if (!_chairFound)
            {
                //check the chair is available
                for (int i = 0; i < chair.Length; i++)
                {
                    if (!chair[i].GetComponent<Chair>().occupied)
                    {
                        chair[i].GetComponent<Chair>().occupied = true;
                        _chairCounter = i;
                        break;
                    }
                }
                _chairFound = true;
            }
            else
            {
                float direction = chair[_chairCounter].transform.position.x - transform.position.x;
                if(direction > 0.1f)
                {
                    Debug.Log("Moving");
                    transform.position = new Vector3(transform.position.x + _speed * direction * Time.deltaTime, transform.position.y, transform.position.z);
                }
                else
                {
                    Debug.Log("Sitting");
                    transform.GetChild(0).rotation = Quaternion.Euler(0f, 180f, 0f);
                    _isSitting = true;
                    orderCanvas.SetActive(true);
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
                else //if coroutine running, return
                {
                    return;
                }
            }
            else //if not serve by player, add time on mood counter
            {
                _moodCounter += Time.deltaTime;
            }
            
            if(_moodCounter > moodCounter) //customer wait for too long, decrease one mood indicator
            {
                _moodCounter = 0;
                _mood--;
                for(int i = 0; i < moodIndicator.Length; i++)
                {
                    if(i < _mood)
                    {
                        moodIndicator[i].SetActive(true);
                    }                        
                    else
                    {
                        moodIndicator[i].SetActive(false);
                    }
                }
                if(_mood <= 0)
                {
                    //leave the restaurant
                    //destroy current game object
                    //update chair 
                    //update game manager
                }
            }
        }
    }

    /// <summary>
    /// activate this function when player place the food in front of player
    /// take time to finish meal and leave
    /// </summary>
    IEnumerator FinishMeal()
    {
        _coroutineRunning = true;
        yield return new WaitForSeconds(finishMealCounter);
        _coroutineRunning = false;
        //leave the shop
        //destroy current game object
        //update chair 
        //update game manager
    }
}
