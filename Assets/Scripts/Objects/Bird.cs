using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
    public float moveSpeed;
    public bool horizontal;

    public Transform[] _points = new Transform[2];
    Transform _targetPoint;
    float velocity;

	// Use this for initialization
	void Start () {
        _points[0] = transform.GetChild(0);
        _points[1] = transform.GetChild(1);
        transform.DetachChildren();
        _targetPoint = _points[1];

        // Randomly decide stats
        moveSpeed = Random.Range(20f, 30f);
        horizontal = true;
        
        if(horizontal) {
            float offset = Random.Range(30f, 50f);
            if(Random.Range(0,2) == 0) {
                _points[0].position = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
                // Make sure the bird doesn't go offscreen
                if(_points[0].position.x < -60) {
                    float dif = Mathf.Abs(_points[0].position.x) - 60;
                    _points[0].Translate(dif, 0f, 0f);
                    transform.Translate(dif, 0f, 0f);
                } else if(_points[0].position.x > 60) {
                    float dif = _points[0].position.x - 60;
                    _points[0].Translate(-dif, 0f, 0f);
                    transform.Translate(-dif, 0f, 0f);
                }
                _points[1].position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            } else {
                _points[0].position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                _points[1].position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
            }

            if (_targetPoint.position.x < transform.position.x) {
                velocity = -moveSpeed;
            } else {
                velocity = moveSpeed;
            }
        } else {
            float offset = Random.Range(30f, 50f);
            if (Random.Range(0, 2) == 0) {
                _points[0].position = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
                _points[1].position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            } else {
                _points[0].position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                _points[1].position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
            }

            if (_targetPoint.position.x < transform.position.x) {
                velocity = -moveSpeed;
            } else {
                velocity = moveSpeed;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(_points[0].position, _points[1].position);

        if (horizontal) {
            transform.Translate(velocity * Time.deltaTime, 0f, 0f);
            // If we have reached the target point
            if((velocity > 0 && transform.position.x > _targetPoint.position.x) ||
              (velocity < 0 && transform.position.x < _targetPoint.position.x)) {
                // Turn around
                velocity = -velocity;
                _targetPoint = GetOtherPoint();
            }
        } else {
            transform.Translate(0f, velocity * Time.deltaTime, 0f);
            // If we have reached the target point
            if ((velocity > 0 && transform.position.y > _targetPoint.position.y) ||
                (velocity < 0 && transform.position.y < _targetPoint.position.y)) {
                // Turn around
                velocity = -velocity;
                _targetPoint = GetOtherPoint();
            }
        }
    }

    Transform GetOtherPoint() {
        if(_targetPoint == _points[0]) {
            return _points[1];
        } else {
            return _points[0];
        }
    }

    private void OnDestroy() {
        if (_points[0] != null) {
            DestroyObject(_points[0].gameObject);
        }
        if (_points[1] != null) {
            DestroyObject(_points[1].gameObject);
        }
    }
}
