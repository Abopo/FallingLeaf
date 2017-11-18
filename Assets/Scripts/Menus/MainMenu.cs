using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("Coins", 1000);
        PlayerPrefs.SetInt("ShieldUnlocked", 0);
        PlayerPrefs.SetInt("UpdraftUnlocked", 0);
        PlayerPrefs.SetInt("MagnetUnlocked", 0);
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
