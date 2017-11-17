using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEffects : MonoBehaviour {
    public Transform leafPlayer;
    public ParticleSystem windBurstEffect;
    public RectTransform windMeter;
    float windMeterHeight;

    // Use this for initialization
    void Start () {
        windMeterHeight = windMeter.sizeDelta.y;
		
	}
	
	// Update is called once per frame
	void Update () {
        windBurstEffect.transform.position = new Vector3(leafPlayer.position.x,
                                                        windBurstEffect.transform.position.y,
                                                        windBurstEffect.transform.position.z);

        windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 
                                         windMeterHeight * (leafPlayer.GetComponent<LeafController>().windResource / leafPlayer.GetComponent<LeafController>().windResourceMax));
	}

    public void WindBurst() {
        windBurstEffect.Play();
    }
}
