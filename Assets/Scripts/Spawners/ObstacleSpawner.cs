using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner {
    Obstacle[] _curObstacles;

    int _mainDifficulty;
    int _subDifficulty;
    float _tracker;

    float _minSpawnDist; // used to decide the next spawn distance

    // Use this for initialization
    protected override void Start () {
        base.Start();

        _mainDifficulty = 1;

        _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/Difficulty1Obstacles");

        UpdateDifficulty();
        _spawnDistance = DecideNextSpawnDistance();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        _tracker += _yDif;
        if(_tracker > 200) {
            _subDifficulty++;
            UpdateDifficulty();
            _tracker = 0;
        }
    }

    protected override void Spawn() {
        int spawnIndex = GetSpawnIndex();
        Obstacle newObstacle = GameObject.Instantiate(_curObstacles[spawnIndex]);
        float posX = Random.Range(-newObstacle.spawnOffsetX, newObstacle.spawnOffsetX);
        float posZ = transform.position.z + newObstacle.spawnOffsetZ;
        newObstacle.transform.position = new Vector3(posX, transform.position.y, posZ);
        _minSpawnDist = newObstacle.minSpawnDistance;
    }

    int GetSpawnIndex() {
        // Get the total spawn chance for all posible obstacles
        int totalSpawnChance = 0;
        foreach (Obstacle o in _curObstacles) {
            totalSpawnChance += o.spawnChance;
        }

        // Generate an index to spawn
        int decision = Random.Range(0, totalSpawnChance);

        // Find the corresponding obstacle to that index
        int spawnIndex = 0;
        totalSpawnChance = 0;
        foreach (Obstacle o in _curObstacles) {
            totalSpawnChance += o.spawnChance;
            if (decision <= totalSpawnChance) {
                break;
            }
            ++spawnIndex;
        }

        return spawnIndex;
    }

    void UpdateDifficulty() {
        if (_subDifficulty >= 10) {
            _mainDifficulty++;
            if(_mainDifficulty > 4) {
                _mainDifficulty = 4;
            }
            _subDifficulty = 0;
        }

        foreach (Obstacle o in _curObstacles) {
            o.UpdateSpawnChance();
        }
        
        _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/Difficulty" + _mainDifficulty + "Obstacles");
    }

    protected override float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = _minSpawnDist + (150 - (25 * _mainDifficulty) + Random.Range(-25, 25));
        return nextSpawn;
    }
}
