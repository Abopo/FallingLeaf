using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour {
    public AudioClip[] BGMs;
    AudioSource _audioSource;

    DayNightCycle _dayNight;
    float _fadeInTime = 5f;
    float _fadeInTimer = 0f;

    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();

        _dayNight = GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightCycle>();

        _audioSource.clip = BGMs[(int)_dayNight.CurTime];
        _audioSource.volume = 0f;
        _audioSource.Play();
	}


    // Update is called once per frame
    void Update () {
        _fadeInTimer += Time.deltaTime;
        if (_fadeInTimer < _fadeInTime) {
            _audioSource.volume = 0.25f * (_fadeInTimer / _fadeInTime);
        }
    }
}
