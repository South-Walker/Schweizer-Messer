// Fantasy Skybox
// Version: 1.3.8
// Compatilble: Unity 5.6.1 or higher, see more info in Readme.txt file.
//
// Developer:			Gold Experience Team (https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:4162)
// Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/18353
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#endregion // Namespaces

public class GE_FantasySkyboxFREE_Demo : MonoBehaviour
{
    
	#region Variables
	
	[System.Serializable]			// Embed this class with sub properties in the inspector. http://docs.unity3d.com/ScriptReference/Serializable.html
	public class LightAndSky
	{
		// Name
		public string m_Name;
		
		// Light
		public Light m_Light;
		
		// Skybox
		public Material m_Skybox;
		
		// Fog
		public Color m_FogColor;
		
		// Ambient
		public Color m_AmbientLight;
	}
	
	// List of LightAndSky class
	public LightAndSky[] m_LightAndSkyList;

	// Index to current Skybox
	int m_CurrentSkyBox = 0;
	
#endregion // Variables
#region MonoBehaviour

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start ()
	{
		/*
		// Manually set fog color in m_LightAndSkyList

		// Fog Night
		m_LightAndSkyList[0].m_FogColor =	new Color(0.1176470588235294f,0.196078431372549f,0.392156862745098f,1f);
		m_LightAndSkyList[1].m_FogColor =	new Color(0.1176470588235294f,0.196078431372549f,0.392156862745098f,1f);
		m_LightAndSkyList[2].m_FogColor =	new Color(0.0784313725490196f,0.1568627450980392f,0.392156862745098f,1f);
		m_LightAndSkyList[3].m_FogColor =	new Color(0.0784313725490196f,0.1568627450980392f,0.392156862745098f,1f);

		// Fog Sunny
		m_LightAndSkyList[4].m_FogColor =	new Color(0.431372549f,0.705882353f,1f,1f);
		m_LightAndSkyList[5].m_FogColor =	new Color(0.431372549f,0.705882353f,1f,1f);
		m_LightAndSkyList[6].m_FogColor =	new Color(0.352941176f,0.823529412f,1f,1f);
		m_LightAndSkyList[7].m_FogColor =	new Color(0.352941176f,0.823529412f,1f,1f);
		m_LightAndSkyList[8].m_FogColor =	new Color(0.745098039f,0.901960784f,0.901960784f,1f);
		m_LightAndSkyList[9].m_FogColor =	new Color(0.745098039f,0.901960784f,0.901960784f,1f);
		m_LightAndSkyList[10].m_FogColor =	new Color(0.666666667f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[11].m_FogColor =	new Color(0.666666667f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[12].m_FogColor =	new Color(0.490196078f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[13].m_FogColor =	new Color(0.490196078f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[14].m_FogColor =	new Color(0.784313725f,0.823529412f,1f,1f);
		m_LightAndSkyList[15].m_FogColor =	new Color(0.784313725f,0.823529412f,1f,1f);
		m_LightAndSkyList[16].m_FogColor =	new Color(0.901960784f,1f,1f,1f);
		m_LightAndSkyList[17].m_FogColor =	new Color(0.901960784f,1f,1f,1f);
		m_LightAndSkyList[18].m_FogColor =	new Color(0.705882353f,0.862745098f,1f,1f);
		m_LightAndSkyList[19].m_FogColor =	new Color(0.705882353f,0.862745098f,1f,1f);
		m_LightAndSkyList[20].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[21].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[22].m_FogColor =	new Color(0.823529412f,0.901960784f,0.784313725f,1f);
		m_LightAndSkyList[23].m_FogColor =	new Color(0.823529412f,0.901960784f,0.784313725f,1f);
		m_LightAndSkyList[24].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[25].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[26].m_FogColor =	new Color(0.6431372549019608f,0.8509803921568627f,1f,1f);
		m_LightAndSkyList[27].m_FogColor =	new Color(0.6431372549019608f,0.8509803921568627f,1f,1f);

		// Ambient Night
		m_LightAndSkyList[0].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);
		m_LightAndSkyList[1].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);
		m_LightAndSkyList[2].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);
		m_LightAndSkyList[3].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);

		// Ambient Sunny
		m_LightAndSkyList[4].m_AmbientLight =	new Color(0.62745098f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[5].m_AmbientLight =	new Color(0.62745098f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[6].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[7].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[8].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[9].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[10].m_AmbientLight =	new Color(0.62745098f,0.62745098f,0.666666667f,1f);
		m_LightAndSkyList[11].m_AmbientLight =	new Color(0.62745098f,0.62745098f,0.666666667f,1f);
		m_LightAndSkyList[12].m_AmbientLight =	new Color(0.549019608f,0.588235294f,0.705882353f,1f);
		m_LightAndSkyList[13].m_AmbientLight =	new Color(0.549019608f,0.588235294f,0.705882353f,1f);
		m_LightAndSkyList[14].m_AmbientLight =	new Color(0.666666667f,0.705882353f,0.745098039f,1f);
		m_LightAndSkyList[15].m_AmbientLight =	new Color(0.666666667f,0.705882353f,0.745098039f,1f);
		m_LightAndSkyList[16].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.784313725f,1f);
		m_LightAndSkyList[17].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.784313725f,1f);
		m_LightAndSkyList[18].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[19].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[20].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[21].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[22].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[23].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[24].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[25].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[26].m_AmbientLight =	new Color(0.5098039215686275f,0.5098039215686275f,0.5098039215686275f,1f);
		m_LightAndSkyList[27].m_AmbientLight =	new Color(0.5098039215686275f,0.5098039215686275f,0.5098039215686275f,1f);
		*/

		// Display first skybox in m_LightAndSkyList
		SwitchSkyBox(0);
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update ()
	{
		// User press Left key
		if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
		{
			// Show previous skybox
			OnPreviousSkybox();
		}
		// User press Right key
		if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
		{
			// Show next skybox
			OnNextSkybox();
		}
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	void OnTriggerExit(Collider other)
	{
		Debug.Log("OnTriggerExit="+other.name);
		
		// Reset player position when user move it away from terrain
		this.transform.localPosition = new Vector3(0,1,0);
	}

	#endregion // MonoBehaviour

	// ########################################
	// Switch skybox functions
	// ########################################

	#region Switch skybox functions

	// Switch to previous skybox
	public void OnPreviousSkybox()
	{
		SwitchSkyBox(-1);
	}
	
	// Switch to next skybox
	public void OnNextSkybox()
	{
		SwitchSkyBox(+1);
	}

	#endregion // MonoBehaviour

	// ########################################
	// Show skybox functions
	// ########################################

	#region Show skybox functions

	// Switch to a skybox by direction
	// DiffNum less than 0 means previous skybox
	// DiffNum larger than 0 means next skybox
	void SwitchSkyBox(int DiffNum)
	{
		// Update add m_CurrentSkyBox with DiffNum
		m_CurrentSkyBox += DiffNum;
		
		// Clamp m_CurrentSkyBox between 0 and m_LightAndSkyList.Length
		if(m_CurrentSkyBox<0)
		{
			m_CurrentSkyBox = m_LightAndSkyList.Length-1;
		}
		if(m_CurrentSkyBox>=m_LightAndSkyList.Length)
		{
			m_CurrentSkyBox = 0;
		}
		
		// Switch skybox in RenderSettings
		RenderSettings.skybox = m_LightAndSkyList[m_CurrentSkyBox].m_Skybox;
		
		// Switch light
		for(int i=0;i<m_LightAndSkyList.Length;i++)
		{
			m_LightAndSkyList[i].m_Light.gameObject.SetActive(false);
		}
		m_LightAndSkyList[m_CurrentSkyBox].m_Light.gameObject.SetActive(true);
		
		// Enable fog
		RenderSettings.fog = true;
		
		// Set the fog color
		if(m_CurrentSkyBox>=0 && m_CurrentSkyBox<m_LightAndSkyList.Length)
		{
			RenderSettings.fogColor = m_LightAndSkyList[m_CurrentSkyBox].m_FogColor;
		}
		else
		{
			RenderSettings.fogColor = Color.white;
		}
		
		// Set the ambient lighting
		if(m_CurrentSkyBox>=0 && m_CurrentSkyBox<m_LightAndSkyList.Length)
		{
			RenderSettings.ambientLight = m_LightAndSkyList[m_CurrentSkyBox].m_AmbientLight;
		}
		else
		{
			RenderSettings.ambientLight = Color.white;
		}
	}
	
	#endregion 
	
}
