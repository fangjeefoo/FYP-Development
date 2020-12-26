using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    //public variable
    public GameObject nextLevelButton;
    public Text scoreText;
    public Text durationText;
    public Image reticleFilled;

    //private variable
    private int _maxLevel;
    private string _scoreText;
    private string _durationText;
    private enum Choice { menu, nextLevel, none };
    private Choice _choice;
    private float _counter;
    private bool _click;

    public void Start()
    {
        _maxLevel = 4;
        _scoreText = "Score: ";
        _durationText = "Duration: ";
        _counter = 0;
        _click = false;

        if (!PlayerPrefs.HasKey("level"))
            nextLevelButton.GetComponent<Button>().interactable = false;

        durationText.text = _durationText + PlayerPrefs.GetInt("duration");
        scoreText.text = _scoreText + PlayerPrefs.GetInt("score") + "/" + PlayerPrefs.GetInt("goal");

        SoundManager.soundManager.MyPlay(0);
    }

    public void Update()
    {
        if (_click)
        {
            _counter += Time.deltaTime;
            reticleFilled.fillAmount += 0.005f;
        }
            

        if (_counter >= 1.5f)
        {
            _counter = 0;

            switch (_choice)
            {
                case Choice.menu:
                    MainMenu();
                    break;
                case Choice.nextLevel:
                    NextLevel();
                    break;
            }
        }
    }

    public void NextLevelOnEnter()
    {
        _choice = Choice.nextLevel;
        OnEnter();
    }

    public void MainMenuOnEnter()
    {
        _choice = Choice.menu;
        OnEnter();
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

        if(reticleFilled != null)
        {
            reticleFilled.enabled = false;
            reticleFilled.fillAmount = 0;
        }
    }

    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt("level");

        if (level + 1 > _maxLevel)
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene("Level " + level + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
