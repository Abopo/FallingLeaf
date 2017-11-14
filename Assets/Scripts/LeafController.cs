using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafController : MonoBehaviour {
    public Transform mainCamera;
    public MeshRenderer model;

    public Vector2 _velocity;
    // X
    float _moveAccelerationX = 60f;
    float _moveDecelerationX = 40f;
    float _maxMoveSpeedX = 100f;
    float _curMaxMoveSpeedX;
    // Y
    float _moveAccelerationY = 50f;
    float _moveDecelerationY = 40f;
    float _maxMoveSpeedY = 100f;
    float _curMaxMoveSpeedY;

    // Direction vectors
    Vector3 up = new Vector3(0f, 1f, 0f);
    Vector3 right = new Vector3(1f, 0f, 0f);
    Vector3 upright = new Vector3(1f, 1f, 0f);
    Vector3 upleft = new Vector3(-1f, 1f, 0f);
    Vector3 downleft = new Vector3(-1f, -1f, 0f);
    Vector3 downright = new Vector3(1f, -1f, 0f);

    bool _hasDied;

    // Currnet starting position : (0, 261, 43)

    // Use this for initialization
    void Start() {
        _velocity.x = 0;
        _velocity.y = 0;

        _hasDied = false;
    }

    // Update is called once per frame
    void Update() {
        if(_hasDied) {
            return;
        }

        CheckInput();

        DetermineVelocity();

        transform.Translate(_velocity.x * Time.deltaTime, _velocity.y * Time.deltaTime, 0f, Space.World);
        if (transform.position.x > 38) {
            transform.position = new Vector3(38, transform.position.y, transform.position.z);
            _velocity.x = 0;
        } else if (transform.position.x < -38) {
            transform.position = new Vector3(-38, transform.position.y, transform.position.z);
            _velocity.x = 0;
        }

        // Move camera along
        mainCamera.position = new Vector3(mainCamera.position.x, transform.position.y-15f, mainCamera.position.z);
    }

    void CheckInput() {
        if (Input.GetKey(KeyCode.A)) {
            // rotate left
            transform.Rotate(0f, 0f, 100 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            // rotate right
            transform.Rotate(0f, 0f, -100 * Time.deltaTime);
        }
    }

    void DetermineVelocity() {
        float xVel = 0, yVel = 0;
        // X velocity
        float dif = Vector3.Dot(up, transform.up);

        // TODO - I haven't found a clean way to math this yet
        // If we are upright
        if (dif > 0) {
            // If we are leaning right
            if (transform.rotation.z < 0) {
                // Go right
                dif = Vector3.Dot(upright, transform.up);
                _curMaxMoveSpeedX = _maxMoveSpeedX * (-1 + Mathf.Abs(dif));
                xVel = _velocity.x + (_moveAccelerationX * Time.deltaTime);

                // If we are leaning left
            } else if (transform.rotation.z > 0) {
                // Go left
                dif = Vector3.Dot(upleft, transform.up);
                _curMaxMoveSpeedX = -_maxMoveSpeedX * (-1 + Mathf.Abs(dif));
                xVel = _velocity.x - (_moveAccelerationX * Time.deltaTime);
            }
            // If we are upside down
        } else if (dif < 0) {
            // If we are leaning right
            if (transform.rotation.z < 0) {
                // Go right
                dif = Vector3.Dot(downleft, transform.up);
                _curMaxMoveSpeedX = _maxMoveSpeedX * (-1 + Mathf.Abs(dif));
                xVel = _velocity.x + (_moveAccelerationX * Time.deltaTime);

                // If we are leaning left
            } else if (transform.rotation.z > 0) {
                // Go left
                dif = Vector3.Dot(downright, transform.up);
                _curMaxMoveSpeedX = -_maxMoveSpeedX * (-1 + Mathf.Abs(dif));
                xVel = _velocity.x - (_moveAccelerationX * Time.deltaTime);
            }
        }

        if (xVel > _curMaxMoveSpeedX) {
            xVel = _velocity.x - (_moveDecelerationX * Time.deltaTime);
        } else if (xVel < _curMaxMoveSpeedX) {
            xVel = _velocity.x + (_moveDecelerationX * Time.deltaTime);
        }

        // Y velocity
        dif = Vector3.Dot(right, transform.up);
        _curMaxMoveSpeedY = -_maxMoveSpeedY * Mathf.Abs(dif);
        yVel = _velocity.y - (_moveAccelerationY * Time.deltaTime);

        if (yVel < _curMaxMoveSpeedY) {
            yVel = _velocity.y + (_moveDecelerationY * Time.deltaTime);
        }
        if (yVel > -15f) {
            yVel = -15f;
        }

        _velocity = new Vector2(xVel, yVel);
    }

    public void Die() {
        model.enabled = false;
        _hasDied = true;
    }
}
