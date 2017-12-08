using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner {
    Obstacle[] _curObstacles;
    Obstacle[][] _allObstacles;

    int _mainDifficulty;
    int _subDifficulty;
    float _tracker;
    int _curZone;

    float _minSpawnDist; // used to decide the next spawn distance
    float _spawnDistOffset;

    Obstacle _borderBranch; // spawned on the borders of the game to prevent players from staying there
    Obstacle _darkBorderBranch; // just to keep visuals consistent in certain zones

    // Use this for initialization
    protected override void Start () {
        base.Start();

        _mainDifficulty = 1;

        LoadObstacles();
        //_curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone1");
        _borderBranch = _curObstacles[0];
        _darkBorderBranch = Resources.Load<Obstacle>("Prefabs/SpikeyZone/Branch1-6");
        _curZone = -1;

        UpdateDifficulty();
        _spawnDistance = DecideNextSpawnDistance();
	}

    void LoadObstacles() {
        _allObstacles = new Obstacle[10][];

        _allObstacles[0] = Resources.LoadAll<Obstacle>("Prefabs/BasicZone1");
        _allObstacles[1] = Resources.LoadAll<Obstacle>("Prefabs/BasicZone2");
        _allObstacles[2] = Resources.LoadAll<Obstacle>("Prefabs/BasicZone3");
        _allObstacles[3] = Resources.LoadAll<Obstacle>("Prefabs/BasicZone4");
        _allObstacles[4] = Resources.LoadAll<Obstacle>("Prefabs/BasicZone5");
        _allObstacles[5] = Resources.LoadAll<Obstacle>("Prefabs/FlowerZone");
        _allObstacles[6] = Resources.LoadAll<Obstacle>("Prefabs/BirdZone");
        _allObstacles[7] = Resources.LoadAll<Obstacle>("Prefabs/VillageZone");
        _allObstacles[8] = Resources.LoadAll<Obstacle>("Prefabs/SpikeyZone");
        _allObstacles[9] = Resources.LoadAll<Obstacle>("Prefabs/WindyZone");

        _curObstacles = _allObstacles[0];
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
        _spawnDistOffset = newObstacle.spawnDistanceOffset;
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
            if (_mainDifficulty > 3) {
                _mainDifficulty = 4;

                // At this point, new zone will be selected randomly
                LoadNextZone();
            } else {
                // Load in the  new obstacles
                //_curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone" + _mainDifficulty);
                _curObstacles = _allObstacles[_mainDifficulty-1];
            }

            _subDifficulty = 0;
        } else {
            foreach (Obstacle o in _curObstacles) {
                o.UpdateSpawnChance();
            }
        }
    }

    void LoadNextZone() {
        int r = Random.Range(0, 7);

        // Make sure the same zone doesn't happen twice in a row
        while (r == _curZone) {
            r = Random.Range(0, 7);
        }

        _curZone = r;
        _curObstacles = _allObstacles[3 + r];
        /*
        switch (r) {
            case 0: // Flower zone
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/FlowerZone");
                break;
            case 1: // Bird zone
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BirdZone");
                break;
            case 2: // Basic Zone 4
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/VillageZone");
                break;
            case 3:
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone4");
                break;
            case 4:
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/BasicZone5");
                break;
            case 5:
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/SpikeyZone");
                break;
            case 6:
                _curObstacles = Resources.LoadAll<Obstacle>("Prefabs/WindyZone");
                break;
        }
        */
    }

    protected override float DecideNextSpawnDistance() {
        float nextSpawn;
        nextSpawn = _spawnDistOffset + (125 - (3*_subDifficulty) - (3*_mainDifficulty) + Random.Range(-20, 20));
        if(nextSpawn < _minSpawnDist) {
            nextSpawn = _minSpawnDist + Random.Range(0, 20);
        }
        return nextSpawn;
    }

    public void SpawnBorderBranch(float xPos) {
        Obstacle newObstacle;
        if(_curZone != 5) {
            newObstacle = GameObject.Instantiate(_borderBranch);
        } else {
            newObstacle = GameObject.Instantiate(_darkBorderBranch);
        }
        float posX = xPos < 0 ? -58 : 58;
        float posZ = transform.position.z + newObstacle.spawnOffsetZ;
        newObstacle.transform.position = new Vector3(posX, transform.position.y, posZ);
        //_minSpawnDist = newObstacle.minSpawnDistance;
    }
}
