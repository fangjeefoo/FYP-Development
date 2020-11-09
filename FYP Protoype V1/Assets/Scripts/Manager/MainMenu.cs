using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private enum Choice { start, setting, quit };
    private float counter;
    private bool click;
    private Choice choice;

    public void Start()
    {
        counter = 0f;
        click = false;
    }

    public void Update()
    {
        if (click)
        {
            counter += Time.deltaTime;                
        }

        if(counter >= 2.0f)
        {
            switch (choice)
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
        click = true;
        choice = Choice.start;
    }

    public void SettingOnEnter()
    {
        click = true;
        choice = Choice.setting;
    }

    public void QuitGameOnEnter()
    {
        click = true;
        choice = Choice.quit;
    }

    public void OnExit()
    {
        click = false;
        counter = 0f;
    }
}
