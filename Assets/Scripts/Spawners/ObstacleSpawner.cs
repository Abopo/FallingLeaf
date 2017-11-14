using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner {
    public GameObject[] branchObj = new GameObject[4];

    int _difficulty;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        _difficulty = 0;
        _spawnDistance = DecideNextSpawnDistance();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    protected override void Spawn() {
        SpawnBranch();
        UpdateDifficulty();
    }

    void SpawnBranch() {
        int r = Random.Range(0, 2+_difficulty);
        GameObject newBranch = GameObject.Instantiate(branchObj[r]);
        float posX = Random.Range(-65f, 65f);
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

    protected override float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = 150 - (25 * _difficulty);
        return nextSpawn;
    }
}
