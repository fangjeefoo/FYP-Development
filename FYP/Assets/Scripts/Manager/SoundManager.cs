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
    public AudioSource eating;
    public AudioSource frying;
    public AudioSource boiling;
    public AudioClip cutting;
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
        bgm.volume = vol;
        seaWaves.volume = vol;
        frying.volume = 1.0f;
        boiling.volume = 1.0f;
        eating.volume = 1.0f;
    }

    public float GetVolume()
    {
        return _volume;
    }

    /// <summary>
    /// win = 0, lose = 1, foodDone = 2, orderSucessful = 3, orderFailed = 4, pickUp = 5, timeEnd = 6, pickuUpWrong = 7, cutting = 8
    /// </summary>
    /// <param name="choice"></param>
    public void MyPlay(int choice)
    {
        switch (choice)
        {
            case 0:
                //bgm.PlayOneShot(win, _volume);
                bgm.PlayOneShot(win, 1.0f);
                break;
            case 1:
                //bgm.PlayOneShot(lose, _volume);
                bgm.PlayOneShot(lose, 1.0f);
                break;
            case 2:
                //bgm.PlayOneShot(foodDone, _volume);
                bgm.PlayOneShot(foodDone, 1.0f);
                break;
            case 3:
                //bgm.PlayOneShot(orderSucessful, _volume);
                bgm.PlayOneShot(orderSucessful, 1.0f);
                break;
            case 4:
                //bgm.PlayOneShot(orderFailed, _volume);
                bgm.PlayOneShot(orderFailed, 1.0f);
                break;
            case 5:
                //bgm.PlayOneShot(pickUp, _volume);
                bgm.PlayOneShot(pickUp, 1.0f);
                break;
            case 6:
                //bgm.PlayOneShot(timeEnd, _volume);
                bgm.PlayOneShot(timeEnd, 1.0f);
                break;
            case 7:
                //bgm.PlayOneShot(pickUpWrong, _volume);
                bgm.PlayOneShot(pickUpWrong, 1.0f);
                break;
            case 8:
                //bgm.PlayOneShot(cutting, _volume);
                bgm.PlayOneShot(cutting, 1.0f);
                break;
        }
    }
}
