using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTreeTrunk : MonoBehaviour {
    public GameObject treeTrunkObj;
    GameObject[] treeTrunks = new GameObject[2];

    float _spawnDistanceTracker = 0f;
    float _spawnDistance = 378.9f;
    float _lastYPos;
    float _yDif;

    // Use this for initialization
    void Start () {
        // Spawn initial trunks
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 250f);
        treeTrunks[0] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        pos.y = treeTrunks[0].transform.position.y - 378.9f;
        treeTrunks[1] = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);

        _lastYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update () {
        _yDif = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_lastYPos));
        _spawnDistanceTracker += _yDif;
        _lastYPos = transform.position.y;
        if (_spawnDistanceTracker >= _spawnDistance) {
            SpawnTrunk();
            _spawnDistanceTracker = 0f;
        }
    }

    void SpawnTrunk() {
        Vector3 pos = new Vector3(transform.position.x, treeTrunks[1].transform.position.y - 378.9f, 250f);
        GameObject newTrunk = GameObject.Instantiate(treeTrunkObj, pos, Quaternion.identity);
        DestroyObject(treeTrunks[0]);

        treeTrunks[0] = treeTrunks[1];
        treeTrunks[1] = newTrunk;
    }
}
