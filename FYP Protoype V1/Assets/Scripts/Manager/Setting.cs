using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    //public variable 
    public GameObject[] exercise;
    public GameObject volumeSlider;
    public GameObject exerciseDurationInput;
    public GameObject performedTimesInput;

    //private variable
    private FirebaseDatabase _database;
    private enum Choice { exercise1, exercise2, exercise3, exercise4, back, save}
    private bool _click;
    private float _counter;
    private Choice _choice;
    private string _dbName;

    void Start()
    {
        _click = false;
        _counter = 0f;
        _database = FirebaseDatabase.DefaultInstance;
        _dbName = "Customization";

        //ExtractData();
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
                    if (exercise[0].GetComponent<Image>().color == Color.green)
                    {
                        exercise[0].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[0].GetComponent<Image>().color = Color.green;
                    }                        
                    OnExit();
                    break;
                case Choice.exercise2:
                    if (exercise[1].GetComponent<Image>().color == Color.green)
                    {
                        exercise[1].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[1].GetComponent<Image>().color = Color.green;
                    }
                    OnExit();
                    break;
                case Choice.exercise3:
                    if (exercise[2].GetComponent<Image>().color == Color.green)
                    {
                        exercise[2].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[2].GetComponent<Image>().color = Color.green;
                    }
                    OnExit();
                    break;
                case Choice.exercise4:
                    if (exercise[3].GetComponent<Image>().color == Color.green)
                    {
                        exercise[3].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        exercise[3].GetComponent<Image>().color = Color.green;
                    }
                    OnExit();
                    break;
                case Choice.back:
                    SceneManager.LoadScene("Menu");
                    break;
                case Choice.save:
                    PostData();
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

    public void SaveOnEnter()
    {
        _click = true;
        _choice = Choice.save;
    }

    public void OnExit()
    {
        _click = false;
        _counter = 0;
    }

    /// <summary>
    /// Create a object to store all informations
    /// </summary>
    /// <returns></returns>
    public Customization PackData()
    {
        Customization customize = new Customization();

        return customize;
    }

    public void ExtractData()
    {
        Customization customize = RetrieveData().Result;
    }

    /// <summary>
    /// Push data to firebase
    /// </summary>
    public void PostData()
    {
        _database.GetReference(_dbName).SetRawJsonValueAsync(JsonUtility.ToJson(PackData()));
    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async Task<Customization> RetrieveData()
    {
        var dataSnapshot = await _database.GetReference(_dbName).GetValueAsync();
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
        var dataSnapshot = await _database.GetReference(_dbName).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
