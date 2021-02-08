using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image reticleFilled;

    //private variable
    private enum Choice { none, start, setting, quit };
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

        RetrieveData();
    }

    public void Update()
    {
        if (_click)
        {
            _counter += Time.deltaTime;
            reticleFilled.fillAmount += 0.005f;
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
        SceneManager.LoadScene("Testing (VR Mode)");
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
        OnEnter();
        _choice = Choice.start;
    }

    public void SettingOnEnter()
    {
        OnEnter();
        _choice = Choice.setting;
    }

    public void QuitGameOnEnter()
    {
        OnEnter();
        _choice = Choice.quit;
    }

    public void OnEnter()
    {
        _click = true;
        reticleFilled.enabled = true;
    }

    public void OnExit()
    {
        _choice = Choice.none;
        _click = false;
        _counter = 0f;
        if(reticleFilled != null)
        {
            reticleFilled.enabled = false;
            reticleFilled.fillAmount = 0;
        }
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

        if (SoundManager.soundManager)
            SoundManager.soundManager.SetVolume(vol);
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
