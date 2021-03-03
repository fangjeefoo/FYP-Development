using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image reticleFilled;
    public GameObject levelSelectionCanvas;
    public GameObject mainCanvas;

    //private variable
    private enum Choice { none, start, setting, quit , tutorial, level1, level2, level3, level4, back};
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
            reticleFilled.fillAmount += 1f/1.5f * Time.deltaTime;
        }

        if(_counter >= 1.5f)
        {
            switch (_choice)
            {
                case Choice.start:
                    LevelSelection();
                    break;
                case Choice.back:
                    CloseLevelSelection();
                    break;
                case Choice.setting:
                    Setting();
                    break;
                case Choice.quit:
                    QuitGame();
                    break;
                case Choice.tutorial:
                    Tutorial();
                    break;
                case Choice.level1:
                    LoadLevel1();
                    break;
                case Choice.level2:
                    LoadLevel2();
                    break;
                case Choice.level3:
                    LoadLevel3();
                    break;
                case Choice.level4:
                    LoadLevel4();
                    break;
            }
        }
    }

    private void LevelSelection()
    {
        mainCanvas.SetActive(false);
        levelSelectionCanvas.SetActive(true);
        OnExit();
    }

    private void CloseLevelSelection()
    {
        mainCanvas.SetActive(true);
        levelSelectionCanvas.SetActive(false);
        OnExit();
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    private void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    private void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    private void LoadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    private void Setting()
    {
        SceneManager.LoadScene("Setting");
    }

    private void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void QuitGame()
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

    public void TutorialOnEnter()
    {
        OnEnter();
        _choice = Choice.tutorial;
    }

    public void QuitGameOnEnter()
    {
        OnEnter();
        _choice = Choice.quit;
    }

    public void BackOnEnter()
    {
        OnEnter();
        _choice = Choice.back;
    }

    public void Level1OnEnter()
    {
        OnEnter();
        _choice = Choice.level1;
    }

    public void Level2OnEnter()
    {
        OnEnter();
        _choice = Choice.level2;
    }

    public void Level3OnEnter()
    {
        OnEnter();
        _choice = Choice.level3;
    }

    public void Level4OnEnter()
    {
        OnEnter();
        _choice = Choice.level4;
    }

    public void OnEnter()
    {
        _click = true;
    }

    public void OnExit()
    {
        _choice = Choice.none;
        _click = false;
        _counter = 0f;
        if(reticleFilled != null)
        {
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
