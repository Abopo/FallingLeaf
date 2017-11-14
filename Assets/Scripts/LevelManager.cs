﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public GameObject gameOverScreen;
    public Text distanceText;
    public Text coinsText;

    float _distanceTracker = 0f;
    float _lastYPos;
    float _yDif;

    int _totalCoins;

    // Use this for initialization
    void Start () {
        _lastYPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        _yDif = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_lastYPos));
        _distanceTracker += _yDif;
        _lastYPos = transform.position.y;

        distanceText.text = ((int)_distanceTracker/100).ToString() + "m";
    }

    public void EarnCoin() {
        _totalCoins += 10;
        coinsText.text = "$ " + _totalCoins.ToString();
    }

    public void EndGame() {
        gameOverScreen.SetActive(true);
    }

    public void ResetLevel() {
        SceneManager.LoadScene("Gameplay");
    }

    public int GetDistanceTraveled() {
        return (int)_distanceTracker;
    }
}
