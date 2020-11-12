using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public variable
    [HideInInspector]
    public GameObject soundManager;

    //private variable
    private enum Choice { start, setting, quit };
    private float _counter;
    private bool _click;
    private Choice _choice;
    private FirebaseDatabase _database;
    private string _dbName;

    public void Start()
    {
        _counter = 0f;
        _click = false;
        _database = FirebaseDatabase.DefaultInstance;
        _dbName = "Customization";
        soundManager = GameObject.FindGameObjectWithTag("SoundManager");

        RetrieveData();
    }

    public void Update()
    {
        if (_click)
        {
            _counter += Time.deltaTime;                
        }

        if(_counter >= 1.5f)
        {
            switch (_choice)
            {
                case Choice.start:
                    StartGame();
                    break;
                case Choice.setting:
                    Setting();
                    break;
                case Choice.quit:
                    QuitGame();
                    break;
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Setting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGameOnEnter()
    {
        _click = true;
        _choice = Choice.start;
    }

    public void SettingOnEnter()
    {
        _click = true;
        _choice = Choice.setting;
    }

    public void QuitGameOnEnter()
    {
        _click = true;
        _choice = Choice.quit;
    }

    public void OnExit()
    {
        _click = false;
        _counter = 0f;
    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async void RetrieveData()
    {
        DataSnapshot dataSnapshot = null;
        Customization customize;
        float vol = 0f;
            
        await _database.GetReference(_dbName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed to retrieve data");
            }
            else if (task.IsCompleted)
            {
                dataSnapshot = task.Result;
                customize = JsonUtility.FromJson<Customization>(dataSnapshot.GetRawJsonValue());
                vol = customize.volume;
            }
        });

        if (soundManager != null)
            soundManager.GetComponent<AudioSource>().volume = vol;           
        else
            Debug.Log("Sound Manager not found");
    }
}
