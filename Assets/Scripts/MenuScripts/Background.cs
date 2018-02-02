using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    public Texture2D[] dayBackground = new Texture2D[3];
    public Texture2D[] nightBackground = new Texture2D[3];
    private Texture2D[] nowBackground
    {
        get
        {
            if (_nowbackground == null)
                _nowbackground = (isDay()) ? dayBackground : nightBackground;
            return _nowbackground;
        }
        set
        {
            _nowbackground = value;
        }
    }
    private Texture2D[] _nowbackground;
    private int currentIndex;
    private const float beginX = -1 * Const.designWidth;
    private float offsetX;
    private Matrix4x4 guiMatrix = Const.getMatrix();
    private Vector2 touchBeginPosition;
    private Vector2 touchPrePosition;
	// Use this for initialization
	void Start () {
        resetData();
	}
	
	// Update is called once per frame
	void Update () {
        guiMatrix = Const.getMatrix();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchBeginPosition = touch.position;
                touchPrePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                offsetX = offsetX + touchPrePosition.x - touchBeginPosition.x;
                touchPrePosition = touch.position;
            }
        }
	}
    private void OnGUI()
    {
        drawBackground();
    }
    private void drawBackground()
    {
        GUI.matrix = guiMatrix;
        float nowx = offsetX;
        foreach (var part in nowBackground)
        {
            GUI.DrawTexture(new Rect(beginX + nowx, 0, Const.designWidth, Const.designHeight), part);
            nowx += part.width;
        }
    }
    public bool isDay()
    {
        DateTime now = DateTime.Now;
        return (now.Hour >= 6 && now.Hour < 6) ? true : false;
    }
    public void resetData()
    {
        offsetX = 0;
        currentIndex = 1;
        nowBackground = null;
    }
}
