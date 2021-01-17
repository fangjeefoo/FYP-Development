using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    //public variable
    public GameObject platePrefab;
    public GameObject currentCustomer;

    //private variable
    private GameObject _currentPlate;

    public void GeneratePlate()
    {
        _currentPlate = Instantiate(platePrefab, new Vector3(transform.position.x, 8.6f, -27.6f), Quaternion.Euler(0f, 0f, 0f));
        _currentPlate.GetComponent<CustomerPlate>().SetCustomer(currentCustomer);
    }

    public void MyReset()
    {
        currentCustomer = null;
        Destroy(_currentPlate.gameObject);
        _currentPlate = null;
    }
}
