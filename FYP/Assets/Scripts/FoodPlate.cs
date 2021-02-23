using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlate : MonoBehaviour
{
    public GameObject food;
    public Transform foodParent;
    [HideInInspector]
    public GameObject holdingFood;

    private Color color;

    void Start()
    {
        holdingFood = null;
        color = gameObject.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {        
        if(holdingFood == null && food != null)
        {           
            var temp = transform.position;
            temp.y += 0.1f;
            holdingFood = Instantiate(food, temp, food.transform.rotation, foodParent);
        }   
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (holdingFood != collision.gameObject)
        {
            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        holdingFood = null;
    }

    public void ChangeColor()
    {
        gameObject.GetComponent<QuickOutline>().enabled = true;
        //gameObject.GetComponent<Renderer>().material.color = new Color(23f / 255, 109f / 255, 23f / 255);
    }

    public void ResetColor()
    {
        gameObject.GetComponent<QuickOutline>().enabled = false;
        //gameObject.GetComponent<Renderer>().material.color = color;
    }
}
