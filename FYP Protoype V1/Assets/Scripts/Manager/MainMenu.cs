using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private enum Choice { start, setting, quit };
    private float _counter;
    private bool _click;
    private Choice _choice;

    public void Start()
    {
        _counter = 0f;
        _click = false;
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
}
