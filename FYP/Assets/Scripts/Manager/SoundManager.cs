using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{   
    //private variable
    private float _volume;
    private AudioSource _audio;
    private static SoundManager _soundManager;


    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (_soundManager == null)
            _soundManager = this;
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
}
