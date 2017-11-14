using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafCollider : MonoBehaviour {
    public ParticleSystem deathParticles;

    LevelManager _levelManager;
    LeafController _leafController;

	// Use this for initialization
	void Start () {
        _levelManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelManager>();
        _leafController = transform.parent.GetComponent<LeafController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Obstacle") {
            _levelManager.EndGame();
            deathParticles.Play();
            _leafController.Die();
        }
    }
}
