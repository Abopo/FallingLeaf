using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : Spawner {
    public GameObject coinObj;

    int _maxSpawnDistance = 250;

    // Use this for initialization
    protected override void Start () {
        base.Update();
	}
	
	// Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    protected override void Spawn() {
        GameObject newBranch = GameObject.Instantiate(coinObj);
        float posX = Random.Range(-50f, 50f);
        float posZ = 40f;
        newBranch.transform.position = new Vector3(posX, transform.position.y, posZ);
    }

    protected override float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = Random.Range(_maxSpawnDistance - 50, _maxSpawnDistance);
        _maxSpawnDistance--;

        return nextSpawn;
    }
}
