using System.Collections;
using System.Collections.Generic;
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
    public LightAndSky[] m_LightAndSkyList;
    int m_CurrentSkyBox = 0;
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        SwitchSkyBox(0);
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        // User press Left key
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            // Show previous skybox
            OnPreviousSkybox();
        }
        // User press Right key
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            // Show next skybox
            OnNextSkybox();
        }
    }
    
    public void OnPreviousSkybox()
    {
        SwitchSkyBox(-1);
    }
    
    public void OnNextSkybox()
    {
        SwitchSkyBox(+1);
    }
    
    void SwitchSkyBox(int DiffNum)
    {
        // Update add m_CurrentSkyBox with DiffNum
        m_CurrentSkyBox += DiffNum;

        // Clamp m_CurrentSkyBox between 0 and m_LightAndSkyList.Length
        if (m_CurrentSkyBox < 0)
        {
            m_CurrentSkyBox = m_LightAndSkyList.Length - 1;
        }
        if (m_CurrentSkyBox >= m_LightAndSkyList.Length)
        {
            m_CurrentSkyBox = 0;
        }

        // Switch skybox in RenderSettings
        RenderSettings.skybox = m_LightAndSkyList[m_CurrentSkyBox].Skybox;

        // Switch light
        for (int i = 0; i < m_LightAndSkyList.Length; i++)
        {
            m_LightAndSkyList[i].Light.gameObject.SetActive(false);
        }
        m_LightAndSkyList[m_CurrentSkyBox].Light.gameObject.SetActive(true);

        // Enable fog
        RenderSettings.fog = true;

        // Set the fog color
        if (m_CurrentSkyBox >= 0 && m_CurrentSkyBox < m_LightAndSkyList.Length)
        {
            RenderSettings.fogColor = m_LightAndSkyList[m_CurrentSkyBox].FogColor;
        }
        else
        {
            RenderSettings.fogColor = Color.white;
        }

        // Set the ambient lighting
        if (m_CurrentSkyBox >= 0 && m_CurrentSkyBox < m_LightAndSkyList.Length)
        {
            RenderSettings.ambientLight = m_LightAndSkyList[m_CurrentSkyBox].AmbientLight;
        }
        else
        {
            RenderSettings.ambientLight = Color.white;
        }
    }
}
