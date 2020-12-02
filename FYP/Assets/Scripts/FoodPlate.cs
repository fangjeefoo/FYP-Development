using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlate : MonoBehaviour
{
    public GameObject food;
    public Transform foodParent;

    private GameObject _holdingFood;

    void Start()
    {
        _holdingFood = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(_holdingFood == null && food != null)
        {
            var temp = this.transform.position;
            temp.y += 0.1f;
            _holdingFood = Instantiate(food, temp, food.transform.rotation, foodParent);
        }          
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_holdingFood != collision.gameObject)
        {
            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        _holdingFood = null;
    }
}
