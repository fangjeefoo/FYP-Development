﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

public class CuttingBoard : MonoBehaviour
{
    public static CuttingBoard cuttingBoard;

    public GameObject food;

    public void Start()
    {
        cuttingBoard = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateElbowExercise || collision.gameObject.GetComponent<Food>().cookType != CookType.shredding)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateElbowExercise = true;
        food = collision.gameObject;
    }

    public void OnCollisionExit(Collision collision)
    {
        food = null;
        MyHand.handManager.UpdateElbowExercise = false;
        MyHand.handManager.ResetCounter(2);
    }

    public void PointerEnter()
    {
        GameManager.gm.SelectedKitchenware(this.gameObject);
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectedKitchenware();
    }
}
