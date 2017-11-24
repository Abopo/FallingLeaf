using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns pretty objects in the background
public class CloudSpawner : Spawner {
    public bool leftSide;

    GameObject whaleObj;

    static GameObject[] backgroundObjects;
    static GameObject theWhale;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        whaleObj = Resources.Load<GameObject>("Prefabs/Background/Whale");

        if (backgroundObjects == null) {
            int r = Random.Range(0, 2);
            if (r == 0) {
                backgroundObjects = Resources.LoadAll<GameObject>("Prefabs/Background/BubbleClouds");
            } else {
                backgroundObjects = Resources.LoadAll<GameObject>("Prefabs/Background/FlatClouds");
            }
        }
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    protected override void Spawn() {
        if(theWhale == null && (Random.Range(0, 100) == 0 || Input.GetKey(KeyCode.W))) {
            SpawnWhale();
        }

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

    void SpawnWhale() {
        Debug.Log("Whaaaale");
        theWhale = GameObject.Instantiate(whaleObj);
        float zOffset = Random.Range(800f, 1100f);
        float posZ = transform.position.z + zOffset;
        float posX;
        if (leftSide) {
            posX = transform.position.x - (1000 * (zOffset / 1000)) ;
        } else {
            posX = transform.position.x + (700 * (zOffset / 1000)) ;
        }
        theWhale.transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
