using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum TIME { DAY = 0, DUSK, NIGHT };

public class DayNightCycle : MonoBehaviour {
    public Transform skyboxCamera;

    TIME curTime;
    int loadCounter = 0;

    Light sunlight;

    // Day settings
    Color dayLightColor = new Color32(249, 187, 17, 255);
    float dayLightIntensity = 0.75f;
    Vector3 dayRotation = new Vector3(1.6f, 5f, 14f);
    Color dayFogColor = new Color32(247, 237, 248, 255);
    Material daySkybox;
    Vector3 daySkyboxRotation = new Vector3(-25f, -5f, 2f);

    // Dusk settings
    Color duskLightColor = new Color32(249, 187, 17, 255);
    float duskLightIntensity = 1.0f;
    Vector3 duskRotation = new Vector3(1.6f, 5f, 14f);
    Color duskFogColor = new Color32(255, 234, 144, 255);
    Material duskSkybox;
    Vector3 duskSkyboxRotation = new Vector3(-25f, -5f, 2f);

    // Night settings
    Color nightLightColor = new Color32(104, 160, 225, 255);
    float nightLightIntensity = 0.1f;
    Vector3 nightRotation = new Vector3(65f, 30f, 30f);
    Color nightsFogColor = new Color32(50, 45, 170, 255);
    Material nightSkybox;
    Vector3 nightSkyboxRotation = new Vector3(-35f, -5f, 2f);

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        sunlight = GameObject.FindGameObjectWithTag("Sunlight").GetComponent<Light>();

        daySkybox = Resources.Load<Material>("Day-Night Skyboxes/Materials/SkyBrightMorning");
        duskSkybox = Resources.Load<Material>("Day-Night Skyboxes/Materials/SkyMorning");
        nightSkybox = Resources.Load<Material>("Day-Night Skyboxes/Materials/SkyNight");

        curTime = (TIME)Random.Range(0, 3);
        ApplyTimeSettings();
    }

    void ApplyTimeSettings() {
        switch(curTime) {
            case TIME.DAY:
                sunlight.color = dayLightColor;
                sunlight.intensity = dayLightIntensity;
                sunlight.transform.rotation = Quaternion.Euler(dayRotation);
                RenderSettings.fogColor = dayFogColor;
                RenderSettings.skybox = daySkybox;
                skyboxCamera.rotation = Quaternion.Euler(daySkyboxRotation);
                break;
            case TIME.DUSK:
                sunlight.color = duskLightColor;
                sunlight.intensity = duskLightIntensity;
                sunlight.transform.rotation = Quaternion.Euler(duskRotation);
                RenderSettings.fogColor = duskFogColor;
                RenderSettings.skybox = duskSkybox;
                skyboxCamera.rotation = Quaternion.Euler(duskSkyboxRotation);
                break;
            case TIME.NIGHT:
                sunlight.color = nightLightColor;
                sunlight.intensity = nightLightIntensity;
                sunlight.transform.rotation = Quaternion.Euler(nightRotation);
                RenderSettings.fogColor = nightsFogColor;
                RenderSettings.skybox = nightSkybox;
                skyboxCamera.rotation = Quaternion.Euler(nightSkyboxRotation);
                break;
        }
    }

    void Update () {
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.buildIndex < 2) {
            loadCounter++;
        }

        if(loadCounter > 0) {
            curTime++;
            if((int)curTime > 2) {
                curTime = TIME.DAY;
            }
        }

        // Find the light
        sunlight = GameObject.FindGameObjectWithTag("Sunlight").GetComponent<Light>();

        ApplyTimeSettings();
    }
}
