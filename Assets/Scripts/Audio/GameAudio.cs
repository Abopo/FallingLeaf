using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour {
    public AudioClip[] BGMs;
    AudioSource _audioSource;

	// Use this for initialization
	void Start () {
        _audioSource = GetComponent<AudioSource>();
        int r = Random.Range(0, BGMs.Length);
        _audioSource.clip = BGMs[r];
        _audioSource.Play();
	}


    // Update is called once per frame
    void Update () {
		
	}
}
