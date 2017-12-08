using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantFlower : MonoBehaviour {

    float fallSpeed = 9f;
    float rotateSpeed = 50f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f, Space.World);
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
	}
}
