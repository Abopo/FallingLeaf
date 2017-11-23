using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner {
    Obstacle[] _curObstacles;

    int _mainDifficulty;
    int _subDifficulty;
    float _tracker;

    float _minSpawnDist; // used to decide the next spawn distance

    Obstacle _borderBranch; // spawned on the borders of the game to prevent players from staying there

    // Use this for initialization
    protected override void Start () {
        base.Start();

        _mainDifficulty = 1;

        _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone1");
        _borderBranch = _curObstacles[0];

        UpdateDifficulty();
        _spawnDistance = DecideNextSpawnDistance();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        _tracker += _yDif;
        if(_tracker > 200) {
            UpdateDifficulty();
            _tracker = 0;
        }
    }

    protected override void Spawn() {
        int spawnIndex = GetSpawnIndex();
        Obstacle newObstacle = GameObject.Instantiate(_curObstacles[spawnIndex]);
        float posX = 0;
        if (newObstacle.spawnOffsetX > 0) {
            posX = Random.Range(-newObstacle.spawnOffsetX, newObstacle.spawnOffsetX);
        } else {
            posX = Random.Range(newObstacle.spawnOffsetLeft, newObstacle.spawnOffsetRight);
        }
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
        _subDifficulty++;

        if (_subDifficulty >= 10) {
            _mainDifficulty++;
            if (_mainDifficulty > 1) {
                _mainDifficulty = 4;

                // At this point, new zone will be selected randomly
                LoadNextZone();
            } else {
                // Load in the  new obstacles
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone" + _mainDifficulty);
            }

            _subDifficulty = 0;
        } else {
            foreach (Obstacle o in _curObstacles) {
                o.UpdateSpawnChance();
            }
        }
    }

    void LoadNextZone() {
        int r = Random.Range(0, 4);
        r = 3;

        switch (r) {
            case 0: // Flower zone
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/FlowerZone");
                break;
            case 1: // Bird zone
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BirdZone");
                break;
            case 2: // Basic Zone 4
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone4");
                break;
            case 3:
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/VillageZone");
                break;
        }
    }

    protected override float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = _minSpawnDist + (125 - (3 * _subDifficulty) + Random.Range(-25, 25));
        return nextSpawn;
    }

    public void SpawnBorderBranch(float xPos) {
        Obstacle newObstacle = GameObject.Instantiate(_borderBranch);
        float posX = xPos < 0 ? -58 : 58;
        float posZ = transform.position.z + newObstacle.spawnOffsetZ;
        newObstacle.transform.position = new Vector3(posX, transform.position.y, posZ);
        //_minSpawnDist = newObstacle.minSpawnDistance;
    }
}
