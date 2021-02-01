using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //public variable
    public static SoundManager soundManager;

    public AudioSource bgm;
    public AudioSource seaWaves;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip foodDone;
    public AudioClip orderSucessful;
    public AudioClip orderFailed;
    public AudioClip pickUp;
    public AudioClip pickUpWrong;
    public AudioClip timeEnd;

    //private variable
    private float _volume;
    private string _activeScene;
    
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (soundManager == null)
            soundManager = this;
        else
            Destroy(this.gameObject);

        _volume = 1.0f;     
    }

    public void Start()
    {
        _activeScene = SceneManager.GetActiveScene().name;
        PlaySound();
        
    }

    public void Update()
    {
        if(_activeScene != SceneManager.GetActiveScene().name)
        {
            PlaySound();
            _activeScene = SceneManager.GetActiveScene().name;
        }
    }

    public void PlaySound()
    {
        if(SceneManager.GetActiveScene().name == "Testing 1" || SceneManager.GetActiveScene().name == "Win" || SceneManager.GetActiveScene().name == "Lose" || SceneManager.GetActiveScene().name == "Setting")
        {
            bgm.Play();
            seaWaves.Stop();
        }
        else
        {
            bgm.Play();
            seaWaves.Play();
        }
    }

    public void SetVolume(float vol)
    {
        _volume = vol;
        bgm.volume = _volume;
        seaWaves.volume = _volume;
    }

    public float GetVolume()
    {
        return _volume;
    }

    /// <summary>
    /// win = 0, lose = 1, foodDone = 2, orderSucessful = 3, orderFailed = 4, pickUp = 5, timeEnd = 6, pickuUpWrong = 7
    /// </summary>
    /// <param name="choice"></param>
    public void MyPlay(int choice)
    {
        switch (choice)
        {
            case 0:
                bgm.PlayOneShot(win, _volume);
                break;
            case 1:
                bgm.PlayOneShot(lose, _volume);
                break;
            case 2:
                bgm.PlayOneShot(foodDone, _volume);
                break;
            case 3:
                bgm.PlayOneShot(orderSucessful, _volume);
                break;
            case 4:
                bgm.PlayOneShot(orderFailed, _volume);
                break;
            case 5:
                bgm.PlayOneShot(pickUp, _volume);
                break;
            case 6:
                bgm.PlayOneShot(timeEnd, _volume);
                break;
            case 7:
                bgm.PlayOneShot(pickUpWrong, _volume);
                break;
        }
    }
}
