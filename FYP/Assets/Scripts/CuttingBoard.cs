using System.Collections;
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

    public void PointerEnter()
    {
        GameManager.gm.SelectKitchenware(this.gameObject);
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectKitchenware();
    }
}
