using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

namespace FoodType
{
    public enum MyFoodType { none, steak, mushroom, sausage, shrimp};
}

public class Food : MonoBehaviour
{
    public MyFoodType _foodType;
    public bool _isCooked;

    public void Start()
    {
        _isCooked = false;
    }

    public MyFoodType FoodType
    {
        get { return _foodType; }
        set { _foodType = value; }
    }

    public bool Cook
    {
        set { _isCooked = value; }
        get { return _isCooked; }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand" && this.gameObject.layer == 9)
        {
            Debug.Log("grab and release");
            GameManager.gm.UpdatePerformedTimes(0);
        }
    }
}
