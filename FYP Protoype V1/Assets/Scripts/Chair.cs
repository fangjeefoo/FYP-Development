using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    //public variable
    public bool occupied; //chair is seated
    public GameObject platePrefab;
    public GameObject currentCustomer;

    //private variable
    private GameObject _currentPlate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GeneratePlate()
    {
        _currentPlate = Instantiate(platePrefab, new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z - 1), Quaternion.Euler(0f, 0f, 0f));
        _currentPlate.GetComponent<Plate>().SetCustomer(currentCustomer);
    }

    public void MyReset()
    {
        occupied = false;
        currentCustomer = null;
        Destroy(_currentPlate.gameObject);
        _currentPlate = null;
    }
}
