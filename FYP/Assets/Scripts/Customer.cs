using System.Collections;
using UnityEngine;
using FoodType;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    //public variable
    public GameObject door;
    public GameObject[] moodIndicator;
    public GameObject orderCanvas;
    public GameObject moodCanvas;
    public GameObject order;
    [Tooltip("time to decrease one star (Mood)")]
    public float moodCounter; 
    public float finishMealCounter;
    [Tooltip("Score per heart")]
    public int score;


    //private variable
    private bool _isSitting; //check if customer is sitting
    private float _moodCounter; //time to decrease one star (mood)
    private int _mood; //current star of the customer (max = 5)
    private bool _isServing; //check customer is serving by player
    private bool _coroutineRunning; //check coroutine "FinishMeal" is running
    private bool _reset;
    private bool _isLeaving;
    private bool _startEating;
    private bool _halfEaten;
    private float _speed;
    private float _eatingCounter;
    private GameObject _chair;
    private Animator animator;
    private MyFoodType _foodOrder;

    /// <summary>
    /// Initialize all private variable
    /// </summary>
    void Start()
    {
        _isSitting = false;
        _reset = false;
        _speed = 5f;
        _chair = GameObject.FindGameObjectWithTag("Chair");
        animator = gameObject.GetComponent<Animator>();

        MyReset2();
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    void Update()
    {
        //if is sitting false, means the customer just reach the restaurant

        if (!_isSitting && !GameManager.gm.pauseCounter)
        {
            if (Vector3.Distance(_chair.transform.position, transform.position) > 1f)
            {
                animator.SetBool("Moving", true);
                animator.SetFloat("VelocityX", 20f);
                animator.SetFloat("VelocityY", 150f);
                var pos = new Vector3(_chair.transform.position.x, transform.position.y, _chair.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Moving", false);
                animator.SetBool("ChairSit", true);
                _isSitting = true;               
                GameManager.gm.CallConversationCoroutine();                
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (!GameManager.gm.pauseCounter)
        {
            if (_isServing && !_isLeaving) 
            {
                if (!_startEating)
                {
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.eating.Play();
                    animator.SetBool("ChairEat", true);
                    orderCanvas.SetActive(false);
                    GameManager.gm.StopVideo();
                    GameManager.gm.CancelHint();

                    _startEating = true;
                }

                _eatingCounter += Time.deltaTime;

                if(_eatingCounter >= finishMealCounter/2 && !_halfEaten)
                {
                    _halfEaten = true;
                    _chair.GetComponent<Chair>().GetCurrentPlate().GetComponent<CustomerPlate>().GetCurrentFood().transform.GetChild(0).localScale /= 2;
                }
                   
                if(_eatingCounter >= finishMealCounter)
                {
                    animator.SetBool("ChairEat", false);
                    GameManager.gm.UpdateScore(_mood * score, true);
                    _mood = 0;
                    _isLeaving = true;
                    if (SoundManager.soundManager)
                    {
                        SoundManager.soundManager.eating.Stop();
                        SoundManager.soundManager.MyPlay(3);
                    }
                }
            }
            //if not serve by player, add time on mood counter
            //customer wait for too long, decrease one mood indicator
            else
            {
                _moodCounter += Time.deltaTime;
            }

            if (_moodCounter > moodCounter && !_isLeaving) 
            {
                _moodCounter = 0;
                _mood--;
                GameManager.gm.CallFeedbackCoroutine(true);

                if (_mood <= 0)
                {
                    if(SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(4);
                    GameManager.gm.UpdateScore(score * moodIndicator.Length / 2, false);
                    _isLeaving = true;
                }
            }
            //display mood indicator

            for (int i = 0; i < moodIndicator.Length; i++) 
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
                if (GameManager.gm.GetTimer() > 0)
                {
                    CameraRigMovement.cameraRig.ResetCamera();
                    orderCanvas.SetActive(false);
                    GameManager.gm.CancelHint();
                    _chair.GetComponent<Chair>().MyReset();
                    GameManager.gm.CallConversationCoroutine(true, !_isServing);
                }                   
                else
                    LeaveRestaurant();
            }
        }
    }

    public void LeaveRestaurant()
    {
        orderCanvas.SetActive(false);
        animator.SetBool("ChairSit", false);
        animator.SetBool("Moving", true);
        animator.SetFloat("VelocityX", 20f);
        animator.SetFloat("VelocityY", 150f);

        var animation = animator.GetCurrentAnimatorClipInfo(0);

        if (!_reset)
        {
            _reset = true;
            _chair.GetComponent<Chair>().MyReset();
            GameManager.gm.UpdateCustomer();
            GameManager.gm.ResetGame();
        }

        if(animation[0].clip.name != "Chair-Idle" && animation[0].clip.name != "Chair-Stand")
        {
            if (Vector3.Distance(door.transform.position, transform.position) > 1.0f)
            {
                var pos = new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
                GameManager.gm.SaveGame();
            }
        }
    }

    /*
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
                    GameManager.gm.CallConversationCoroutine();
                    animator.SetBool("Walk", false);
                    _chair[_chairCounter].GetComponent<Chair>().GeneratePlate();                    
                }                
            }
        }
        else if(!GameManager.gm.pauseCounter)//customer waiting to be served
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
                if(GameManager.gm.GetTimer() > 0)
                    GameManager.gm.CallConversationCoroutine();
                else
                    LeaveShop();
                //MyReset();
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
            GameManager.gm.SaveGame();
        }
    }*/

    public bool Serving
    {
        get { return _isServing; }
        set { _isServing = value; }
    }

    public bool Sitting
    {
        get { return _isSitting; }
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
        if (SoundManager.soundManager)
            SoundManager.soundManager.eating.Play();
        animator.SetBool("ChairEat", true);
        _coroutineRunning = true;
        orderCanvas.SetActive(false);
        GameManager.gm.StopVideo();
        GameManager.gm.CancelHint();

        yield return new WaitForSeconds(finishMealCounter / 2);
        _chair.GetComponent<Chair>().GetCurrentPlate().GetComponent<CustomerPlate>().GetCurrentFood().transform.GetChild(0).localScale /= 2; 

        
        yield return new WaitForSeconds(finishMealCounter);
        animator.SetBool("ChairEat", false);
        GameManager.gm.UpdateScore(_mood * score, true);
        _mood = 0;
        _isLeaving = true;
        if (SoundManager.soundManager)
        {
            SoundManager.soundManager.eating.Stop();
            SoundManager.soundManager.MyPlay(3);
        }            
    }

    public void MyReset()
    {
        MyReset2();

        int num = Random.Range(0, 4);

        _chair.GetComponent<Chair>().currentCustomer = gameObject;
        _chair.GetComponent<Chair>().GeneratePlate();
        _foodOrder = GameManager.gm.foodPlate[num].GetComponent<FoodPlate>().food.GetComponent<Food>().foodType;
        for (int i = 0; i < GameManager.gm.orderImage.Length; i++)
        {
            if (GameManager.gm.orderImage[i].name == _foodOrder.ToString())
            {
                order.GetComponent<Image>().sprite = GameManager.gm.orderImage[i];
                break;
            }
        }

        orderCanvas.SetActive(true);
    }

    public void MyReset2()
    {
        _isServing = false;
        _coroutineRunning = false;
        _isLeaving = false;
        _startEating = false;
        _halfEaten = false;
        _moodCounter = 0f;
        _eatingCounter = 0;
        _mood = 5;
    }

    public GameObject GetChair()
    {
        return _chair;
    }

    public MyFoodType GetFoodType()
    {
        return _foodOrder;
    }
}
