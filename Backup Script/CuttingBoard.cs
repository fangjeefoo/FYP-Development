using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

public class CuttingBoard : MonoBehaviour
{
    public static CuttingBoard cuttingBoard;

    public GameObject food;
    public ParticleSystem particleSystem;

    public void Start()
    {
        cuttingBoard = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*        if (MyHand.handManager.UpdateElbowExercise || collision.gameObject.GetComponent<Food>().cookType != CookType.shredding)
                {
                    Destroy(collision.gameObject);
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(7);
                    return;
                }*/

        food = collision.gameObject;
        MyHand.handManager.UpdateElbowExercise = true;
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
        gameObject.GetComponent<QuickOutline>().enabled = true;
        //gameObject.GetComponent<Renderer>().material.color = new Color(23f / 255, 200f / 255, 23f / 255);
    }

    public void ResetColor()
    {
        gameObject.GetComponent<QuickOutline>().enabled = false;
        //gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
    }

    public void EnableParticleSystem()
    {
        particleSystem.Play();
    }
}
