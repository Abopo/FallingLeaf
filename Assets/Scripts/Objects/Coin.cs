using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float rotationSpeed;

    bool _grabbed;

    LevelManager _levelManager;

	// Use this for initialization
	void Start () {
        _levelManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "PlayerLeaf" && !_grabbed) {
            // Update coin count in level manager
            _levelManager.EarnCoin();

            DestroyObject(this.gameObject);
        } else if(collision.tag == "Obstacle Death Plane") {
            DestroyObject(this.gameObject);
        }
    }
}
