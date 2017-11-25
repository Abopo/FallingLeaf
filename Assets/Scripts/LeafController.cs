using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshDistort;

public class LeafController : MonoBehaviour {
    public Transform mainCamera;
    public ObstacleSpawner obstacleSpawner;
    public MeshRenderer model;
    public GameEffects gameEffects;
    public SphereCollider magnetCollider;

    public Vector2 _velocity;
    // X
    float _moveAccelerationX = 80f;
    float _baseMoveDecelerationX = 10f;
    float _moveDecelerationX = 10f;
    float _maxMoveSpeedX = 100f;
    float _curMaxMoveSpeedX;
    // Y
    float _moveAccelerationY = 40f;
    float _moveDecelerationY = 50f;
    float _maxMoveSpeedY = 100f;
    float _curMaxMoveSpeedY;

    float rotationSpeed = 100f;

    float _windForce = 175f;
    float _windTime = 0.5f;
    float _windTimer = 0.6f;
    public float windResource = 0.51f;
    public float windResourceMax = 0.51f;

    // Direction vectors
    Vector3 up = new Vector3(0f, 1f, 0f);
    Vector3 right = new Vector3(1f, 0f, 0f);
    Vector3 upright = new Vector3(1f, 1f, 0f);
    Vector3 upleft = new Vector3(-1f, 1f, 0f);

    bool _hasDied;
    public bool HasDied {
        get { return _hasDied; }
    }

    // These track how long the player is riding the border (will spawn a branch there if it's too long)
    float _borderTime = 4.0f;
    float _borderTimer = 0.0f;

    AnimatedDistort _animatedDistort;
    AudioSource _audioSource;

    // Currnet starting position : (0, 261, 43)

    private void Awake() {
        InitPowerUps();
    }

    // Use this for initialization
    void Start() {
        _velocity.x = 0;
        _velocity.y = 0;

        _hasDied = false;
        _animatedDistort = GetComponentInChildren<AnimatedDistort>();
        _audioSource = GetComponent<AudioSource>();
    }

    void InitPowerUps() {
        if(PlayerPrefs.GetInt("UpdraftUnlocked") == 1) {
            windResourceMax = 1.1f;
            windResource = 1.1f;
        }
        if(PlayerPrefs.GetInt("MagnetUnlocked") == 1) {
            magnetCollider.enabled = true;
        }
        if(PlayerPrefs.GetInt("RotationUnlocked") == 1) {
            rotationSpeed = 150f;
        }
        if(PlayerPrefs.GetInt("GaleforceUnlocked") == 1) {
            _windForce = 225f;
        }
        if(PlayerPrefs.GetInt("SquallUnlocked") == 1) {
            windResourceMax = 1.6f;
            windResource = 1.6f;
        }
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
        CheckPosition();

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
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            // rotate right
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.Space) && windResource > _windTime) {
            // Wind burst
            _windTimer = 0f;
            gameEffects.WindBurst();
            _audioSource.Play();
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

        // Adjust moveDecelerationX based on vertical orientation
        _moveDecelerationX = _baseMoveDecelerationX + (30 * Mathf.Abs(dif));

        // If we used a wind burst, override downward velocity
        if (_windTimer <= _windTime) {
            yVel = _velocity.y + (_windForce * Time.deltaTime);
            windResource -= Time.deltaTime;
            if(windResource < 0) {
                windResource = 0;
            }
        } else {
            yVel = _velocity.y - (_moveAccelerationY * Time.deltaTime);

            // If we need to decelerate
            if (yVel < _curMaxMoveSpeedY) {
                yVel = _velocity.y + (_moveDecelerationY * Time.deltaTime);
                xVel += Mathf.Sign(xVel) * ((_moveDecelerationY * Time.deltaTime) / 2);

                AnimateDistortion(dif);
            }
            if (yVel > -20f) {
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

    void CheckPosition() {
        if(transform.position.x > 55 || transform.position.x < -55) {
            _borderTimer += Time.deltaTime;
            if(_borderTimer >= _borderTime) {
                obstacleSpawner.SpawnBorderBranch(transform.position.x);
                _borderTimer = 0f;
            }
        }
    }

    public void Hit() {
        _velocity.x = _velocity.x / 2f;
        _velocity.y = _velocity.y / 2f;
    }

    public void Die() {
        model.enabled = false;
        _hasDied = true;
    }
}
