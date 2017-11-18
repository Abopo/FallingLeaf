using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEffects : MonoBehaviour {

    Cloud[] _clouds;

    float cloudSpawnTime = 5.0f;
    float cloudSpawnTimer = 0f;

	// Use this for initialization
	void Start () {
        LoadClouds();

        SpawnClouds();
	}

    void LoadClouds() {
        int r = 0;
        switch (r) {
            case 0:
                _clouds = Resources.LoadAll<Cloud>("Prefabs/MainMenu/FlatClouds");
                break;
            case 1:
                _clouds = Resources.LoadAll<Cloud>("Prefabs/MainMenu/BubbleClouds");
                break;
        }
    }

    void SpawnClouds() {
        // min: (-3000,-2000,-1622)
        // max: (4500,1000,6000)

        float x, y, z;
        int index;
        // Spawn 50 clouds within the boundaries
        for (int i = 0; i < 25; ++i) {
            x = Random.Range(-3500, 4500);
            y = Random.Range(-2000, 1000);
            z = Random.Range(-1622, 6000);
            index = Random.Range(0, _clouds.Length);

            Cloud newCloud = GameObject.Instantiate(_clouds[index], new Vector3(x, y, z), Quaternion.Euler(-90,0,-90));
            newCloud.velX = Random.Range(50, 200);
        }
    }

    // Update is called once per frame
    void Update () {
        cloudSpawnTimer += Time.deltaTime;
        if(cloudSpawnTimer >= cloudSpawnTime) {
            SpawnCloud();
            cloudSpawnTimer = 0f;
        }
	}

    void SpawnCloud() {
        float x, y, z;
        int index;

        x = -3500;
        y = Random.Range(-2000, 1000);
        z = Random.Range(-1622, 6000);
        index = Random.Range(0, _clouds.Length);

        Cloud newCloud = GameObject.Instantiate(_clouds[index], new Vector3(x, y, z), Quaternion.Euler(-90, 0, -90));
        newCloud.velX = Random.Range(50, 200);
    }
}
