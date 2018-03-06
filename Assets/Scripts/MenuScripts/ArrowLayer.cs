using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ArrowLayer : MonoBehaviour,IGUILayer{
    public GUIStyle[] arrowStyle = new GUIStyle[2];
    // Use this for initialization
    void Start () {
        resetData();
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void Draw()
    {
        if (BackgroundLayer.currentIndex == 2 || BackgroundLayer.currentIndex == 1)
        {
            if (GUI.Button(new Rect(0, 360, 90, 80), "", arrowStyle[0]))
            {
                BackgroundLayer.currentIndex -= 1;
            }
            
        }
        if (BackgroundLayer.currentIndex == 1 || BackgroundLayer.currentIndex == 0)
        {
            if (GUI.Button(new Rect(390, 360, 90, 80), "", arrowStyle[1]))
            {
                BackgroundLayer.currentIndex += 1;
            }
        }
    }
    public void resetData()
    {

    }
    public bool getFlag()
    {
        return !(BackgroundLayer.isMoving || BackgroundLayer.isTouching || BackgroundLayer.isEnding);
    }
}
