using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudButton : MonoBehaviour {

    Vector3 _baseScale;

	// Use this for initialization
	void Start () {
        _baseScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown() {
        transform.localScale = new Vector3(_baseScale.x * 0.8f, _baseScale.y * 0.8f, _baseScale.z * 0.8f);
    }

    private void OnMouseUp() {
        transform.localScale = _baseScale;
    }
}
