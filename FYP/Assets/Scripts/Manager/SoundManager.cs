using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public variable
    public static SoundManager soundManager;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip foodDone;
    public AudioClip orderSucessful;
    public AudioClip orderFailed;
    public AudioClip pickUp;
    public AudioClip timeEnd;

    //private variable
    private float _volume;
    private AudioSource _audio;
    


    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (soundManager == null)
            soundManager = this;
        else
            Destroy(this.gameObject);

        _volume = 1.0f;     
        _audio = gameObject.GetComponent<AudioSource>();
        _audio.loop = true;
        _volume = _audio.volume;
    }

    public void Start()
    {
        _audio.Play();
    }

    public void SetVolume(float vol)
    {
        _volume = vol;
        _audio.volume = _volume;
    }

    public float GetVolume()
    {
        return _volume;
    }

    /// <summary>
    /// win = 0, lose = 1, foodDone = 2, orderSucessful = 3, orderFailed = 4, pickUp = 5, timeEnd = 6
    /// </summary>
    /// <param name="choice"></param>
    public void MyPlay(int choice)
    {
        switch (choice)
        {
            case 0:
                _audio.PlayOneShot(win, _volume);
                break;
            case 1:
                _audio.PlayOneShot(lose, _volume);
                break;
            case 2:
                _audio.PlayOneShot(foodDone, _volume);
                break;
            case 3:
                _audio.PlayOneShot(orderSucessful, _volume);
                break;
            case 4:
                _audio.PlayOneShot(orderFailed, _volume);
                break;
            case 5:
                _audio.PlayOneShot(pickUp, _volume);
                break;
            case 6:
                _audio.PlayOneShot(timeEnd, _volume);
                break;
        }
    }
}
