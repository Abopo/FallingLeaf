using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEffects : MonoBehaviour {
    public LeafController leafPlayer;
    public ParticleSystem windBurstEffect;
    public RectTransform windMeter;
    float windMeterHeight;

    // Use this for initialization
    void Start () {
        if (leafPlayer.windResourceMax < 1.0f) {
            windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 45f);
            windMeter.parent.localPosition = new Vector3(windMeter.parent.localPosition.x, 430f, windMeter.parent.localPosition.z);
        } else if(leafPlayer.windResourceMax < 1.5f) {
            windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 95f);
            //windMeter.parent.localPosition = new Vector3(windMeter.parent.localPosition.x, 380f, windMeter.parent.localPosition.z);
        }
        //131

        windMeterHeight = windMeter.sizeDelta.y;
	}
	
	// Update is called once per frame
	void Update () {
        windBurstEffect.transform.position = new Vector3(leafPlayer.transform.position.x,
                                                        windBurstEffect.transform.position.y,
                                                        windBurstEffect.transform.position.z);

        windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 
                                         windMeterHeight * (leafPlayer.windResource / leafPlayer.windResourceMax));
	}

    public void WindBurst() {
        windBurstEffect.Play();
    }
}
