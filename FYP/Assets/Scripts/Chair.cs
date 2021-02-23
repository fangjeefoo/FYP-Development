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
    private Color color;

    private void Start()
    {
        color = platePrefab.GetComponent<Renderer>().sharedMaterial.color;
    }

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

    public void ChangeColor()
    {
        gameObject.GetComponent<QuickOutline>().enabled = true;
        //_currentPlate.GetComponent<Renderer>().material.color = new Color(23f / 255, 109f / 255, 23f / 255);
    }

    public void ResetColor()
    {
        gameObject.GetComponent<QuickOutline>().enabled = false;
        //_currentPlate.GetComponent<Renderer>().material.color = color;
    }

    public GameObject GetCurrentPlate()
    {
        return _currentPlate;
    }
}
