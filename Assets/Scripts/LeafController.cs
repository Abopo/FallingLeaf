using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshDistort;

public class LeafController : MonoBehaviour {
    public Transform mainCamera;
    public MeshRenderer model;
    public GameEffects gameEffects;

    public Vector2 _velocity;
    // X
    float _moveAccelerationX = 80f;
    float _moveDecelerationX = 10f;
    float _maxMoveSpeedX = 100f;
    float _curMaxMoveSpeedX;
    // Y
    float _moveAccelerationY = 50f;
    float _moveDecelerationY = 40f;
    float _maxMoveSpeedY = 100f;
    float _curMaxMoveSpeedY;

    float _windForce = 150f;
    float _windTime = 0.5f;
    float _windTimer = 0.6f;
    public float windResource = 1.6f;
    public float windResourceMax = 1.6f;

    // Direction vectors
    Vector3 up = new Vector3(0f, 1f, 0f);
    Vector3 right = new Vector3(1f, 0f, 0f);
    Vector3 upright = new Vector3(1f, 1f, 0f);
    Vector3 upleft = new Vector3(-1f, 1f, 0f);
    Vector3 downleft = new Vector3(-1f, -1f, 0f);
    Vector3 downright = new Vector3(1f, -1f, 0f);

    bool _hasDied;

    AnimatedDistort _animatedDistort;

    public bool HasDied {
        get { return _hasDied; }
    }

    // Currnet starting position : (0, 261, 43)

    // Use this for initialization
    void Start() {
        _velocity.x = 0;
        _velocity.y = 0;

        _hasDied = false;
        _animatedDistort = GetComponentInChildren<AnimatedDistort>();
    }

    // Update is called once per frame
    void Update() {
        if(_hasDied) {
            return;
        }

        UpdateWind();

        CheckInput();

        DetermineVelocity();

        transform.Translate(_velocity.x * Time.deltaTime, _velocity.y * Time.deltaTime, 0f, Space.World);

        StayInBounds();

        // Move camera along
        mainCamera.position = new Vector3(mainCamera.position.x, transform.position.y-15f, mainCamera.position.z);
    }

    void UpdateWind() {
        _windTimer += Time.deltaTime;
        windResource += Time.deltaTime / 25;
        if (windResource > windResourceMax) {
            windResource = windResourceMax;
        }
        //Debug.Log(windResource.ToString());
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
        if(Input.GetKey(KeyCode.Space) && windResource > _windTime) {
            // Wind burst
            _windTimer = 0f;
            gameEffects.WindBurst();
        }
    }

    void DetermineVelocity() {
        float xVel = 0, yVel = 0;
        // X velocity
        float dif = Vector3.Dot(up, transform.up);

        // If we are leaning right
        if (transform.rotation.eulerAngles.z < 360 && transform.rotation.eulerAngles.z > 270 ||
            transform.rotation.eulerAngles.z < 180 && transform.rotation.eulerAngles.z > 90) {
            // Go right
            dif = Vector3.Dot(upright, transform.up);
            _curMaxMoveSpeedX = _maxMoveSpeedX * (-1 + Mathf.Abs(dif));
            if (_velocity.x > _curMaxMoveSpeedX) {
                xVel = _velocity.x - (_moveDecelerationX * Time.deltaTime);
            } else {
                xVel = _velocity.x + (_moveAccelerationX * Time.deltaTime);
            }
            // If we are leaning left
        } else if (transform.rotation.eulerAngles.z > 0 && transform.rotation.eulerAngles.z < 90 ||
                   transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 270) {
            // Go left
            dif = Vector3.Dot(upleft, transform.up);
            _curMaxMoveSpeedX = -_maxMoveSpeedX * (-1 + Mathf.Abs(dif));
            if (_velocity.x < _curMaxMoveSpeedX) {
                xVel = _velocity.x + (_moveDecelerationX * Time.deltaTime);
            } else {
                xVel = _velocity.x - (_moveAccelerationX * Time.deltaTime);
            }
        }

        // Y velocity
        dif = Vector3.Dot(right, transform.up);
        _curMaxMoveSpeedY = -_maxMoveSpeedY * Mathf.Abs(dif);
        // If we used a wind burst, override downward velocity
        if (_windTimer <= _windTime) {
            yVel = _velocity.y + (_windForce * Time.deltaTime);
            windResource -= Time.deltaTime;
        } else {
            yVel = _velocity.y - (_moveAccelerationY * Time.deltaTime);

            // If we need to decelerate
            if (yVel < _curMaxMoveSpeedY) {
                yVel = _velocity.y + (_moveDecelerationY * Time.deltaTime);
                xVel += Mathf.Sign(xVel) * ((_moveDecelerationY * Time.deltaTime) / 2);

                AnimateDistortion(dif);
            }
            if (yVel > -15f) {
                //yVel = -15f;
                yVel = _velocity.y - (_moveAccelerationY * Time.deltaTime);
            }
        }

        _velocity = new Vector2(xVel, yVel);
    }

    void AnimateDistortion(float dif) {
        if (Mathf.Abs(dif) < 0.75f) {
            // Show a slowing down style of animation
            _animatedDistort.animate = AnimatedDistort.Animate.force;
            // Based on how fast we are falling, increase the min/max values and animation speed.
            _animatedDistort.minValue = 0.8f + (0.02f * -_velocity.y);
            _animatedDistort.maxValue = _animatedDistort.minValue + (0.2f + 0.02f * -_velocity.y);
            _animatedDistort.constantSpeed = 8 + (0.01f * -_velocity.y);
        } 
    }

    void StayInBounds() {
        if (transform.position.x > 60) {
            transform.position = new Vector3(60, transform.position.y, transform.position.z);
            _velocity.x = 0;
        } else if (transform.position.x < -60) {
            transform.position = new Vector3(-60, transform.position.y, transform.position.z);
            _velocity.x = 0;
        }
        if (transform.position.y > 650) {
            transform.position = new Vector3(transform.position.x, 650, transform.position.z);
        }
    }

    public void Die() {
        model.enabled = false;
        _hasDied = true;
    }
}
