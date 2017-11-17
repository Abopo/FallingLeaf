using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    protected float _spawnDistanceTracker = 0f;
    protected float _spawnDistance = 100.0f;
    protected float _totalDistance = 0f;
    protected float _lowestYPos;
    protected float _yDif;

    // Use this for initialization
    protected virtual void Start () {
        _lowestYPos = transform.position.y;
    }

    // Update is called once per frame
    protected virtual void Update () {
        // Only update spawn values if we have moved downward
        if (transform.position.y < _lowestYPos) {
            _yDif = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_lowestYPos));
            _spawnDistanceTracker += _yDif;
            _totalDistance += _yDif;
            _lowestYPos = transform.position.y;

            if (_spawnDistanceTracker >= _spawnDistance) {
                Spawn();
                _spawnDistanceTracker = 0;
                _spawnDistance = DecideNextSpawnDistance();
            }
        }
    }

    protected virtual void Spawn() {

    }

    protected virtual float DecideNextSpawnDistance() {
        return 100f;
    }
}
