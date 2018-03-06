using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SkyboxController : MonoBehaviour {
    [System.Serializable]
    public class LightAndSky
    {
        public string Name;
        public Light Light;
        public Material Skybox;
        public Color FogColor;
        public Color AmbientLight;
    }
    public LightAndSky[] LightAndSkyList;
    public int CurrentSkyBox
    {
        get
        {
            if (_currentSkyBox == -1)
            { 
                int time = DateTime.Now.Hour;
                if (time >= 17 && time < 23)
                    _currentSkyBox = 0;
                else if (time >= 23 || time < 5)
                    _currentSkyBox = 1;
                else if (time >= 5 && time < 11)
                    _currentSkyBox = 2;
                else
                    _currentSkyBox = 3;
            }
            return _currentSkyBox;
        }
    }
    int _currentSkyBox = -1;
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        SwitchSkyBox();
    }
    
    void SwitchSkyBox()
    {
        // Switch skybox in RenderSettings
        RenderSettings.skybox = LightAndSkyList[CurrentSkyBox].Skybox;

        // Switch light
        for (int i = 0; i < LightAndSkyList.Length; i++)
        {
            LightAndSkyList[i].Light.gameObject.SetActive(false);
        }
        LightAndSkyList[CurrentSkyBox].Light.gameObject.SetActive(true);

        // Enable fog
        RenderSettings.fog = true;

        // Set the fog color
        if (CurrentSkyBox >= 0 && CurrentSkyBox < LightAndSkyList.Length)
        {
            RenderSettings.fogColor = LightAndSkyList[CurrentSkyBox].FogColor;
        }
        else
        {
            RenderSettings.fogColor = Color.white;
        }

        // Set the ambient lighting
        if (CurrentSkyBox >= 0 && CurrentSkyBox < LightAndSkyList.Length)
        {
            RenderSettings.ambientLight = LightAndSkyList[CurrentSkyBox].AmbientLight;
        }
        else
        {
            RenderSettings.ambientLight = Color.white;
        }
    }
}
