using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
    public float moveSpeed;
    public bool turnAround;
    public bool fullScreen;

    float velocity;
    float leftX;
    float rightX;

    Transform modelTransform;

	// Use this for initialization
	void Start () {
        // Randomly decide stats
        moveSpeed = Random.Range(20f, 30f);

        if (fullScreen) {
            leftX = -60f;
            rightX = 60f;
        } else {
            float offset = Random.Range(20f, 40f);
            leftX = transform.position.x - offset;
            rightX = transform.position.x + offset;
        }

        velocity = moveSpeed;

        modelTransform = transform.GetChild(0).transform;

        if(Random.Range(0,2) == 0 || turnAround) {
            TurnAround();
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(velocity * Time.deltaTime, 0f, 0f);
        // If we have reached the target point
        if((velocity > 0 && transform.position.x > rightX) ||
            (velocity < 0 && transform.position.x < leftX)) {
            TurnAround();
        }
    }

    void TurnAround() {
        velocity = -velocity;
        modelTransform.Rotate(0f, 180f, 0f);
    }

    private void OnDestroy() {
    }
}
