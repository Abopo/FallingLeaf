using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEffects : MonoBehaviour {
    public LeafController leafPlayer;
    public ParticleSystem windBurstEffect;
    public RectTransform windMeter;
    public WindResource[] windResources;

    float windMeterHeight;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("UpdraftUnlocked") == 1) {
            windResources[1].gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SquallUnlocked") == 1) {
            windResources[2].gameObject.SetActive(true);
        }

        //if (leafPlayer.windResourceMax < 1.0f) {
        //    windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 45f);
        //} else if(leafPlayer.windResourceMax < 1.5f) {
        //    windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 95f);
        //}

        //windMeterHeight = windMeter.sizeDelta.y;
	}
	
	// Update is called once per frame
	void Update () {
        //windMeter.sizeDelta = new Vector2(windMeter.sizeDelta.x, 
        //                                 windMeterHeight * (leafPlayer.windResource / leafPlayer.windResourceMax));
	}
    
    public bool CanWindBurst() {
        foreach(WindResource wr in windResources) {
            if(wr.gameObject.activeSelf && wr.isFull) {
                return true;
            }
        }

        return false;
    }

    public void WindBurst() {
        windBurstEffect.Play();

        for (int i = 2; i >= 0; --i) {
            if (windResources[i].gameObject.activeSelf && windResources[i].isFull) {
                windResources[i].Drain();
                break;
            }
        }
    }
}
