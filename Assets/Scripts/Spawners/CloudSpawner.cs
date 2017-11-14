using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns pretty objects in the background
public class CloudSpawner : Spawner {
    public GameObject[] backgroundObjects;
    public bool leftSide;

    // Use this for initialization
    protected override void Start () {
        base.Update();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    protected override void Spawn() {
        int r = Random.Range(0, backgroundObjects.Length);
        GameObject newObj = GameObject.Instantiate(backgroundObjects[r]);
        float zOffset = Random.Range(0f, 1000f);
        float posZ = transform.position.z + zOffset;
        float posX;
        if (leftSide) {
            posX = transform.position.x - (700 * (zOffset/1000)) - Random.Range(-200f, 200f);
        } else {
            posX = transform.position.x + (700 * (zOffset/1000)) - Random.Range(-200f, 200f);
        }
        newObj.transform.position = new Vector3(posX, transform.position.y, posZ);
    }

    protected override float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = Random.Range(100f, 300f);
        return nextSpawn;
    }
}
