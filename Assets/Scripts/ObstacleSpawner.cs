using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject[] branchObj = new GameObject[4];

    int _difficulty;
    float _spawnDistanceTracker = 0f;
    float _spawnDistance = 100.0f;
    float _totalDistance = 0f;
    float _lastYPos;
    float _yDif;

	// Use this for initialization
	void Start () {
        _lastYPos = transform.position.y;
        _difficulty = 0;
        _spawnDistance = DecideNextSpawnDistance();
	}
	
	// Update is called once per frame
	void Update () {
        _yDif = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_lastYPos));
        _spawnDistanceTracker += _yDif;
        _totalDistance += _yDif;
        _lastYPos = transform.position.y;
        if(_spawnDistanceTracker >= _spawnDistance) {
            SpawnBranch();
            UpdateDifficulty();
            _spawnDistanceTracker = 0;
            _spawnDistance = DecideNextSpawnDistance();
        }
	}

    void SpawnBranch() {
        int r = Random.Range(0, 2+_difficulty);
        GameObject newBranch = GameObject.Instantiate(branchObj[r]);
        float posX = Random.Range(-35f, 35f);
        float posZ = transform.position.z + newBranch.GetComponent<Obstacle>().spawnOffset;
        newBranch.transform.position = new Vector3(posX, transform.position.y, posZ);
    }

    void UpdateDifficulty() {
        if (_totalDistance > 500) {
            _difficulty = 2;
        } else if(_totalDistance > 1000) {
            _difficulty = 2;
        }
    }

    float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = 100 - (25 * _difficulty);
        return nextSpawn;
    }
}
