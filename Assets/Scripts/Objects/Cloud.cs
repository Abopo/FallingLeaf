using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
    public float velX;

    float _destroyTime = 240;
    float _destroyTimer;

	// Use this for initialization
	void Start () {
        int rot = Random.Range(0, 2);
        if(rot == 0) {
            //transform.Rotate(0f, 90f, 0f, Space.World);
        } else {
            //transform.Rotate(0f, -90f, 0f, Space.World);
        }

        if (velX == 0) {
            velX = Random.Range(-10f, 10f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(velX * Time.deltaTime, 0f, 0f, Space.World);

        _destroyTimer += Time.deltaTime;
        if(_destroyTimer >= _destroyTime) {
            DestroyObject(this.gameObject);
        }
	}
}
