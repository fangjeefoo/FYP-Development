using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodType;

public class DeepPan : MonoBehaviour
{
    public static DeepPan deepPan;

    public GameObject food;
    public ParticleSystem particleSystem;
    public ParticleSystem particleSystem2;

    public void Start()
    {
        deepPan = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*        if (MyHand.handManager.UpdateWristExercise || collision.gameObject.GetComponent<Food>().cookType != CookType.soup)
                {
                    Destroy(collision.gameObject);
                    if (SoundManager.soundManager)
                        SoundManager.soundManager.MyPlay(7);
                    return;
                }*/
        food = collision.gameObject;
        MyHand.handManager.UpdateWristExercise = true;
        particleSystem.Play();
        if (SoundManager.soundManager)
            SoundManager.soundManager.boiling.Play();
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
        // gameObject.GetComponent<Renderer>().material.color = new Color(23f / 255, 200f / 255, 23f / 255);
    }

    public void ResetColor()
    {
        gameObject.GetComponent<QuickOutline>().enabled = false;
        //gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f);
    }

    public void EnableParticleSystem()
    {
        particleSystem2.Play();
    }
}
