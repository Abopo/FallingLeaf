using System.Collections;
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

    int _collectedCoins;

    // Use this for initialization
    void Start () {
        _lastYPos = transform.position.y;

        coinsText.text = /*"$ " + */PlayerPrefs.GetInt("Coins").ToString();
        _collectedCoins = PlayerPrefs.GetInt("Coins");
    }

    // Update is called once per frame
    void Update () {
        _yDif = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_lastYPos));
        _distanceTracker += _yDif;
        _lastYPos = transform.position.y;

        distanceText.text = ((int)_distanceTracker/100).ToString() + "m";
    }

    public void EarnCoin() {
        _collectedCoins += 10;
        coinsText.text = /*"$ " + */_collectedCoins.ToString();
    }

    public void EndGame() {
        gameOverScreen.SetActive(true);

        // If we beat our best distance, save it
        int convertedDistance = ((int)_distanceTracker / 100);
        if (PlayerPrefs.GetInt("BestDistance") < convertedDistance) {
            PlayerPrefs.SetInt("BestDistance", convertedDistance);
        }

        // Save coins
        PlayerPrefs.SetInt("Coins", _collectedCoins);
    }

    public void ResetLevel() {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadStore() {
        SceneManager.LoadScene("Store");
    }

    public void LoadHome() {
        SceneManager.LoadScene("MainMenu");
    }

    public int GetDistanceTraveled() {
        return (int)_distanceTracker;
    }
}
