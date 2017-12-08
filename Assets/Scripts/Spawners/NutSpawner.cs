using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutSpawner : MonoBehaviour {
    public GameObject spikeyNutObj;

    float _spawnTime = 2.25f;
    float _spawnTimer = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _spawnTimer += Time.deltaTime;
        if(_spawnTimer >= _spawnTime) {
            SpawnNut();
            _spawnTimer = 0f;
        }
	}

    void SpawnNut() {
        GameObject.Instantiate(spikeyNutObj, transform.position, Quaternion.identity);
    }
}
