using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    public Text distanceText;

    LevelManager _levelManager;

	// Use this for initialization
	void Start () {
        _levelManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelManager>();

        //distanceText.text = _levelManager.GetDistanceTraveled().ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
