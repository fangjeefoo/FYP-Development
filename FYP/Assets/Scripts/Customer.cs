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
    [Tooltip("time to decrease one heart (Mood)")] public float moodCounter; 
    public float finishMealCounter;
    [Tooltip("Score per heart")] public int score;

    //private variable
    private bool _isSitting; 
    private float _moodCounter; 
    private int _mood; 
    private bool _isServing; 
    private bool _reset;
    private bool _isLeaving;
    private bool _startEating;
    private bool _halfEaten;
    private float _speed;
    private float _eatingCounter;
    private GameObject _chair;
    private Animator animator;
    private MyFoodType _foodOrder;


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

    public void MyReset()
    {
        MyReset2();

        Random.InitState(System.DateTime.Now.Millisecond);
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
        _isLeaving = false;
        _startEating = false;
        _halfEaten = false;
        _moodCounter = 0f;
        _eatingCounter = 0;
        _mood = 5;
    }

    public MyFoodType GetFoodType()
    {
        return _foodOrder;
    }
}
