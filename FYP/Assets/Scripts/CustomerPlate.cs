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
        _currentCustomer = customer;
    }

    public GameObject GetCurrentFood()
    {
        return _food;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Before Collision");
        if(_currentCustomer.GetComponent<Customer>().Order == collision.gameObject.GetComponent<Food>().foodType && collision.gameObject.GetComponent<Food>().cookType == FoodType.CookType.cooked)
        {
            _currentCustomer.GetComponent<Customer>().Serving = true;
            _food = collision.gameObject;
            //Destroy(collision.gameObject.GetComponent<EventTrigger>());
        }        
/*        else
        {
            if (SoundManager.soundManager)
                SoundManager.soundManager.MyPlay(7);
            Destroy(collision.gameObject);
        }*/
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
        Destroy(_food);
    }
}
