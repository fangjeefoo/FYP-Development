using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //private variable
    private float _volume;
    private AudioSource _audio;


    public void Awake()
    {
        _volume = 1.0f;
        DontDestroyOnLoad(this.gameObject);

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
}
