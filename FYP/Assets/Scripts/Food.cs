﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

namespace FoodType
{
    public enum MyFoodType { none, steak, mushroom, sausage, shrimp, sandwich, tomato, onion, apple, pear, avocado};
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
                GameManager.gm.DisplayHint(null, true);
                if(SoundManager.soundManager)
                    SoundManager.soundManager.MyPlay(2);
                Destroy(gameObject);
                break;
            }
        }
    }

    public void PointerEnter()
    {
        if (!GameManager.gm.delayFoodPointerEnter)
        {
            GameObject customerFood = null;
            GameObject panFood = Pan.pan.GetComponent<Pan>().food;
            GameObject boardFood = CuttingBoard.cuttingBoard.GetComponent<CuttingBoard>().food;
            GameObject deepPanFood = DeepPan.deepPan.GetComponent<DeepPan>().food;

            if (GameManager.gm.GetChair().GetComponent<Chair>().GetCurrentPlate())
                customerFood = GameManager.gm.GetChair().GetComponent<Chair>().GetCurrentPlate().GetComponent<CustomerPlate>().GetCurrentFood();

            if (GameManager.gm.pauseCounter || gameObject == customerFood || gameObject == panFood || gameObject == boardFood || gameObject == deepPanFood)
            {
                return;
            }


            GameManager.gm.SelectObject(gameObject);
        }
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectObject();
    }
}
