using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Setting : MonoBehaviour
{
    //public variable 
    public GameObject[] exercise;

    //private variable
    private FirebaseDatabase _database;
    private enum Choice { exercise1, exercise2, exercise3, exercise4, back, submit}
    private bool _click;
    private float _counter;
    private Choice _choice;

    void Start()
    {
        _click = false;
        _counter = 0f;
        _database = FirebaseDatabase.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_click)
        {
            _counter += Time.deltaTime;
        }

        if(_counter >= 2f)
        {
            switch (_choice)
            {
                case Choice.exercise1:
                    break;
                case Choice.exercise2:
                    break;
                case Choice.exercise3:
                    break;
                case Choice.exercise4:
                    break;
                case Choice.back:
                    break;
                case Choice.submit:
                    break;
            }
        }
    }

    public void Exercise1OnEnter()
    {
        _click = true;
        _choice = Choice.exercise1;
    }

    public void Exercise2OnEnter()
    {
        _click = true;
        _choice = Choice.exercise2;
    }

    public void Exercise3OnEnter()
    {
        _click = true;
        _choice = Choice.exercise3;
    }

    public void Exercise4OnEnter()
    {
        _click = true;
        _choice = Choice.exercise4;
    }

    public void BackOnEnter()
    {
        _click = true;
        _choice = Choice.back;
    }

    public void SubmitOnEnter()
    {
        _click = true;
        _choice = Choice.submit;
    }

    /// <summary>
    /// Push data to firebase
    /// </summary>
    public void PostData()
    {
        Customization customize = new Customization(1,"s", 2, 2.3f);

        _database.GetReference("Customization").SetRawJsonValueAsync(JsonUtility.ToJson(customize));


    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async Task<Customization> RetrieveData()
    {
        var dataSnapshot = await _database.GetReference("Customization").GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        return JsonUtility.FromJson<Customization>(dataSnapshot.GetRawJsonValue());
    }

    /// <summary>
    /// Check data is save properly in database
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Save()
    {
        var dataSnapshot = await _database.GetReference("Customization").GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
