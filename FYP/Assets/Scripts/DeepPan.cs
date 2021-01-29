﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

public class DeepPan : MonoBehaviour
{
    public static DeepPan deepPan;

    public GameObject food;

    public void Start()
    {
        deepPan = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateWristExercise || collision.gameObject.GetComponent<Food>().cookType != CookType.soup)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateWristExercise = true;
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
