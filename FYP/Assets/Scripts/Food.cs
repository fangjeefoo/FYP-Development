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
        for(int i = 0; i < GameManager.gm.cookedFoodPlate.Length; i++)
        {
            if(GameManager.gm.cookedFoodPlate[i].GetComponent<FoodPlate>().holdingFood == null)
            {
                var pos = GameManager.gm.cookedFoodPlate[i].transform.position;
                pos.y += 0.1f;

                GameManager.gm.cookedFoodPlate[i].GetComponent<FoodPlate>().holdingFood = Instantiate(food, pos, food.transform.rotation, _foodParent.transform);
                SoundManager.soundManager.MyPlay(2);
                Destroy(this.gameObject);
                break;
            }
        }
    }

    public void PointerEnter()
    {
        GameManager.gm.SelectObject(this.gameObject);
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectObject();
    }
}
