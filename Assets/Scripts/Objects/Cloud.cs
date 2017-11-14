using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    float velX;

	// Use this for initialization
	void Start () {
        int rot = Random.Range(0, 2);
        if(rot == 0) {
            //transform.Rotate(0f, 90f, 0f, Space.World);
        } else {
            //transform.Rotate(0f, -90f, 0f, Space.World);
        }

        velX = Random.Range(-5f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(velX * Time.deltaTime, 0f, 0f, Space.World);
	}
}
