using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafCollider : MonoBehaviour {
    public ParticleSystem deathParticles;
    public AudioClip _coin;
    public AudioClip _death;

    int _shield;
    float _invulnTime = 2.0f;
    float _invulnTimer = 3.0f;

    LevelManager _levelManager;
    LeafController _leafController;
    AudioSource _audioSource;

    // Use this for initialization
    void Start () {
        InitPowerUps();

        _levelManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelManager>();
        _leafController = transform.parent.GetComponent<LeafController>();
        _audioSource = GetComponent<AudioSource>();
    }

    void InitPowerUps() {
        if(PlayerPrefs.GetInt("ShieldUnlocked") == 1) {
            _shield = 1;
        }
        if(PlayerPrefs.GetInt("SafeguardUnlocked") == 1) {
            _shield = 2;
        }
    }

    // Update is called once per frame
    void Update () {
        _invulnTimer += Time.deltaTime;
	}

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Obstacle" && !_leafController.HasDied && _invulnTimer >= _invulnTime) {
            if (_shield > 0) {
                _shield--;
                _invulnTimer = 0f;
                _leafController.Hit();
            } else {
                _levelManager.EndGame();
                deathParticles.Play();
                _leafController.Die();
            }

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
