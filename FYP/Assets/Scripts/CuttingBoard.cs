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
            if (SoundManager.soundManager)
                SoundManager.soundManager.MyPlay(7);
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
    public void ChangeColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(23f / 255, 200f / 255, 23f / 255);
    }

    public void ResetColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
    }
}
