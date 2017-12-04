using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyNut : MonoBehaviour {
    Transform _model;

    float _fallSpeed = -110f;
    float _rotSpeed = 200f;

    float _deathTime = 20.0f;
    float _deathTimer = 0f;

	// Use this for initialization
	void Start () {
        _model = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
        _deathTimer += Time.deltaTime;
        if(_deathTimer >= _deathTime) {
            DestroyObject(this.gameObject);
        }

        transform.Translate(0f, _fallSpeed * Time.deltaTime, 0f, Space.World);
        _model.Rotate(0f, 0f, _rotSpeed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Obstacle") {
            DestroyObject(this.gameObject);
        }
    }
}
