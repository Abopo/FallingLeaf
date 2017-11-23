using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public float spawnOffsetZ;
    public float spawnOffsetX;
    public float spawnOffsetLeft;
    public float spawnOffsetRight;
    public int spawnChance;
    public int spawnChanceChange;
    public int minSpawnDistance;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateSpawnChance() {
        spawnChance = spawnChance + spawnChanceChange;
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.tag == "Obstacle Death Plane") {
            DestroyObject(this.gameObject);
        }
    }
}
