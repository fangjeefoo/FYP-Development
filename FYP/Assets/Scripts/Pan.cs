﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

public class Pan : MonoBehaviour
{
    public static Pan pan;

    public GameObject food;

    public void Start()
    {
        pan = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //check whether player putting wrong food type or the pan is already start cooking
        if (MyHand.handManager.UpdateForearmExercise || collision.gameObject.GetComponent<Food>().cookType != CookType.frying)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateForearmExercise = true;
        food = collision.gameObject;
    }

    public void PointerEnter()
    {
        GameManager.gm.SelectKitchenware(this.gameObject);
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectKitchenware();
    }
}
