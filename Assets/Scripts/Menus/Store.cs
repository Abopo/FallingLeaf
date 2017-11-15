using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Store : MonoBehaviour {
    public Text coinText;

	// Use this for initialization
	void Start () {
        coinText.text = "$ " + PlayerPrefs.GetInt("Coins").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButton() {
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
