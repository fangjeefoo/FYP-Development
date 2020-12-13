using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

namespace FoodType
{
    public enum MyFoodType { none, steak, mushroom, sausage, shrimp, sandwich, tomato, onion};
    public enum CookType { cooked, shredding, frying, soup}
}

public class Food : MonoBehaviour
{
    public GameObject food;
    public MyFoodType foodType;
    public CookType cookType;

    private GameObject _foodParent;


    public void Start()
    {
        _foodParent = GameObject.FindGameObjectWithTag("FoodManager");
    }

    public void GenerateCookedFood()
    {
        Instantiate(food, _foodParent.transform);
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand" && this.gameObject.layer == 9)
        {
            Debug.Log("grab and release");
            GameManager.gm.UpdatePerformedTimes(0);
        }
    }

    public void PointerEnter()
    {
        GameManager.gm.SelectedObject(this.gameObject);
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectedObject();
    }
}
