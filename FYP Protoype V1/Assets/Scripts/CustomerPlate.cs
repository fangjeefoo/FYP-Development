using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPlate : MonoBehaviour
{
    private GameObject _currentCustomer;

    public void SetCustomer(GameObject customer)
    {
        _currentCustomer = customer;
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if(_currentCustomer.GetComponent<Customer>().Order == collision.gameObject.GetComponent<Food>().FoodType && collision.gameObject.GetComponent<Food>().Cook)
        {
            Debug.Log("Start eating");
            _currentCustomer.GetComponent<Customer>().Serving = true;
        }        
    }
}
