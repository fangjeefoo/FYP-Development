using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerPlate : MonoBehaviour
{
    private GameObject _currentCustomer;
    private GameObject _food;

    public void SetCustomer(GameObject customer)
    {
        Debug.Log("Setting up the customer");
        _currentCustomer = customer;
        if(_currentCustomer)
            Debug.Log("not null");
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if(_currentCustomer.GetComponent<Customer>().Order == collision.gameObject.GetComponent<Food>().foodType && collision.gameObject.GetComponent<Food>().cookType == FoodType.CookType.cooked)
        {
            _currentCustomer.GetComponent<Customer>().Serving = true;
            _food = collision.gameObject;
            //Destroy(collision.gameObject.GetComponent<EventTrigger>());
        }        
        else
        {
            Destroy(collision.gameObject);
        }
    }

    public void PointerEnter()
    {
        GameManager.gm.SelectKitchenware(this.gameObject);
    }

    public void PointerExit()
    {
        GameManager.gm.DeselectKitchenware();
    }

    public void OnDestroy()
    {
        Debug.Log("Destroy: " + _food);
        Destroy(_food);
        Debug.Log("Finish Destroy: " + _food);
    }
}
