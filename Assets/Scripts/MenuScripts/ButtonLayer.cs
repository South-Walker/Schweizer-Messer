using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using UnityEngine;

public class ButtonLayer : MonoBehaviour,IGUILayer {
    public GUIStyle[] buttonStyle = new GUIStyle[3];
    private static bool scaleFlag = true;
    private float scaleFactor
    {
        get
        {
            return _scalefactor;
        }
        set
        {
            if (value > 1)
                _scalefactor = 1;
            else if (value < 0)
                _scalefactor = 0;
            else
                _scalefactor = value;
        }
    }
    private float _scalefactor = 0.0f;
    // Use this for initialization
    void Start () {
        resetData();
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void Draw()
    {
        var size = new Vector2(213 * scaleFactor, 219 * scaleFactor);
        scaleFactor += 0.05f;
        var position = new Vector2(Const.designWidth / 2, Const.designHeight / 2) - size / 2;
        if (GUI.Button(new Rect(position, size), "", buttonStyle[BackgroundLayer.currentIndex]))
        {
            if (BackgroundLayer.currentIndex == 1)
            {
                ScenesController.LoadScene("FantasyPlanetScene");
            }
            else if (BackgroundLayer.currentIndex == 0)
            {
                ScenesController.LoadScene("FantasyPlanetScene");
            }
            else
            {
                ScenesController.LoadScene("DiceScene");
            }
            BackgroundLayer.endBackground();
        }
    }
    public void resetData()
    {
        scaleFlag = true;
        scaleFactor = 0f;
    }
    public bool getFlag()
    {
        if (BackgroundLayer.isMoving || BackgroundLayer.isTouching || BackgroundLayer.isEnding)  
        {
            scaleFlag = false;
            scaleFactor = 0;
        }
        else
        {
            scaleFlag = true;
            scaleFactor += 0.05f;
        }
        return scaleFlag;
    }
}
