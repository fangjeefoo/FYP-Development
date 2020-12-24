using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lose : MonoBehaviour
{
    //public variable
    public GameObject restartButton;
    public Text scoreText;
    public Text durationText;

    //private variable
    private string _scoreText;
    private string _durationText;
    private enum Choice { menu, restart, none};
    private Choice _choice;
    private float _counter;
    private bool _click;

    public void Start()
    {
        _scoreText = "Score: ";
        _durationText = "Duration: ";
        _counter = 0;
        _click = false;

        if (!PlayerPrefs.HasKey("level"))
            restartButton.GetComponent<Button>().interactable = false;

        durationText.text = _durationText + PlayerPrefs.GetInt("duration");
        scoreText.text = _scoreText + PlayerPrefs.GetInt("score") + "/" + PlayerPrefs.GetInt("goal");

        SoundManager.soundManager.MyPlay(1);
    }

    public void Update()
    {
        if(_click)
            _counter += Time.deltaTime;

        if(_counter >= 1.5f)
        {
            _counter = 0;

            switch (_choice)
            {
                case Choice.menu:
                    MainMenu();
                    break;
                case Choice.restart:
                    Restart();
                    break;
            }
        }
    }

    public void MainMenuOnEnter()
    {
        _choice = Choice.menu;
        _click = true;
    }

    public void RestartOnEnter()
    {
        _choice = Choice.restart;
        _click = true;
    }

    public void OnExit()
    {
        _choice = Choice.none;
        _click = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        int level = PlayerPrefs.GetInt("level");
        SceneManager.LoadScene("Level " + level);
    }
}
