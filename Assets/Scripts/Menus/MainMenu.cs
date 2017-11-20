﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Text distanceRecord;

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetInt("FirstTime") == 0) {
            PlayerPrefs.SetInt("BestDistance", 0);
            PlayerPrefs.SetInt("Coins", 1000);
            PlayerPrefs.SetInt("ShieldUnlocked", 0);
            PlayerPrefs.SetInt("UpdraftUnlocked", 0);
            PlayerPrefs.SetInt("MagnetUnlocked", 0);

            PlayerPrefs.SetInt("FirstTime", 1);
        }

        distanceRecord.text = "Best: " + PlayerPrefs.GetInt("BestDistance").ToString() + "m";

        SetScreenResolution();
    }

    void SetScreenResolution() {
        Resolution[] resolutions = Screen.resolutions;

        int width, height;
        height = resolutions[resolutions.Length-1].height;
        width = (int)(height * 0.625f);

        Screen.SetResolution(width, height, true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadGameplay() {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadStore() {
        SceneManager.LoadScene("Store");
    }
}
