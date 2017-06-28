using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.RainMaker;

public class EnvironmentManager : MonoBehaviour {

    private PlayerController player;

    private BaseRainScript rain;
    private DayNightCycleManager cycle;
    private ThunderManager thunder;

    private Light sunLight;
    private Light moonLight;

    private float nextRain;

    // Use this for initialization
    void Start () {
        rain = GameObject.Find("RainPrefab").GetComponent<RainScript>().GetComponent<BaseRainScript>();
        thunder = GetComponent<ThunderManager>();

        cycle = GetComponent<DayNightCycleManager>();
        cycle.sunLight = GameObject.Find("SunLight").GetComponent<Light>();
        cycle.moonLight = GameObject.Find("MoonLight").GetComponent<Light>();

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        rain.RainIntensity = 0;
        cycle.speed = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Inputs();

        if (nextRain < Time.time)
            NewRain();
    }

    void NewRain()
    {
        nextRain = Random.Range(5f,20f)+ Time.time;
        rain.RainIntensity = Random.Range(0.01f, 0.1f) * Random.Range(0.1f, 1f);
    }

    void Inputs()
    {
        if (Input.GetKey(KeyCode.PageUp))
            cycle.speed++;
        else if (Input.GetKey(KeyCode.PageDown))
            cycle.speed--;

        if (Input.GetKey(KeyCode.KeypadPlus))
            rain.RainIntensity += 0.01f;
        else if (Input.GetKey(KeyCode.KeypadMinus))
            rain.RainIntensity -= 0.01f;
    }
}
