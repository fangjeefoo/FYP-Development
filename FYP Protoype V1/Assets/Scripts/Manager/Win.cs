using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private int _maxLevel;

    public void Start()
    {
        _maxLevel = 4;
    }

    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt("level");

        if (level + 1 > _maxLevel)
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene("Level" + level + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
