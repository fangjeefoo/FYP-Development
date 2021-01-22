using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlate : MonoBehaviour
{
    public GameObject food;
    public Transform foodParent;
    [HideInInspector]
    public GameObject holdingFood;

    private bool called = false;

    void Start()
    {
        holdingFood = null;
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
}
