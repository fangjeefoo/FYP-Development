using System.Collections;
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

    public void OnCollisionExit(Collision collision)
    {
        food = null;
        MyHand.handManager.UpdateWristExercise = false;
        MyHand.handManager.ResetCounter(3);
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
