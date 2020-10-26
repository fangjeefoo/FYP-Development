using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private GameObject _currentCustomer;

    public void SetCustomer(GameObject customer)
    {
        _currentCustomer = customer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Start eating");
        _currentCustomer.GetComponent<Customer>().SetServing(true);
    }
}
