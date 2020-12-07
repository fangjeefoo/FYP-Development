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
        if(_currentCustomer.GetComponent<Customer>().Order == collision.gameObject.GetComponent<Food>().foodType && collision.gameObject.GetComponent<Food>().cookType == FoodType.CookType.cooked)
        {
            _currentCustomer.GetComponent<Customer>().Serving = true;
        }        
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
