﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTreeTrunk : MonoBehaviour {
    public GameObject treeTrunkObj;
    GameObject[] treeTrunks = new GameObject[6];

    float _spawnDistanceTracker = -1890f; // start backwards so we pass the tree top before spawning more
    float _spawnDistance = 378f;
    float _lastYPos;
    float _yDif;

    // Use this for initialization
    void Start () {
        // Spawn initial trunks
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 378f, 249f);
        treeTrunks[0] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        pos.y = treeTrunks[0].transform.position.y - 378f;
        treeTrunks[1] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        pos.y = treeTrunks[1].transform.position.y - 378f;
        treeTrunks[2] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        pos.y = treeTrunks[2].transform.position.y - 378f;
        treeTrunks[3] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        pos.y = treeTrunks[3].transform.position.y - 378f;
        treeTrunks[4] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        pos.y = treeTrunks[4].transform.position.y - 378f;
        treeTrunks[5] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);

        _lastYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.y < _lastYPos) {
            _yDif = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_lastYPos));
            _spawnDistanceTracker += _yDif;
            _lastYPos = transform.position.y;
            if (_spawnDistanceTracker >= _spawnDistance) {
                SpawnTrunk();
                _spawnDistanceTracker = 0f;
            }
        }
    }

    void SpawnTrunk() {
        Vector3 pos = new Vector3(transform.position.x, treeTrunks[5].transform.position.y - 378f, 249f);
        GameObject newTrunk = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        DestroyObject(treeTrunks[0]);

        treeTrunks[0] = treeTrunks[1];
        treeTrunks[1] = treeTrunks[2];
        treeTrunks[2] = treeTrunks[3];
        treeTrunks[3] = treeTrunks[4];
        treeTrunks[4] = treeTrunks[5];
        treeTrunks[5] = newTrunk;
    }
}
