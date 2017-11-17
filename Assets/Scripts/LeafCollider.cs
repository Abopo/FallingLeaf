using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafCollider : MonoBehaviour {
    public ParticleSystem deathParticles;
    public AudioClip _coin;
    public AudioClip _death;

    LevelManager _levelManager;
    LeafController _leafController;
    AudioSource _audioSource;

    // Use this for initialization
    void Start () {
        _levelManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelManager>();
        _leafController = transform.parent.GetComponent<LeafController>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Obstacle" && !_leafController.HasDied) {
            _levelManager.EndGame();
            deathParticles.Play();
            _leafController.Die();
            // Play death sound
            _audioSource.volume = 1f;
            _audioSource.clip = _death;
            _audioSource.Play();
        }
        if (collision.tag == "Coin") {
            // Play coin sound
            _audioSource.volume = 0.5f;
            _audioSource.clip = _coin;
            _audioSource.Play();
        }
    }
}
