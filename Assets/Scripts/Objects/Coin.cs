using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float rotationSpeed;

    Vector2 velocity;
    float moveSpeed = 100f;

    LevelManager _levelManager;

	// Use this for initialization
	void Start () {
        velocity = Vector2.zero;
        _levelManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);

        transform.Translate(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "PlayerLeaf") {
            // Update coin count in level manager
            _levelManager.EarnCoin();

            DestroyObject(this.gameObject);
        } else if(collision.tag == "Obstacle Death Plane") {
            DestroyObject(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Magnet") {
            // Move towards the center of the magnet
            Vector2 dir = (Vector2)other.transform.position - (Vector2)transform.position;
            dir.Normalize();
            velocity = new Vector2(moveSpeed * dir.x, moveSpeed * dir.y);
        }
    }
}
