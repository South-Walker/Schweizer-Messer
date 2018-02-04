using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts;

public class BackgroundLayer : MonoBehaviour,IGUILayer
{

    public Texture2D[] dayBackground = new Texture2D[3];
    public Texture2D[] nightBackground = new Texture2D[3];
    public static bool isMoving
    {
        get
        {
            return _ismoving;
        }
        private set
        {
            _ismoving = value;
        }
    }
    public static bool isTouching
    {
        get
        {
            return _istouching;
        }
        private set
        {
            _istouching = value;
        }
    }
    public static int currentIndex
    {
        get
        {
            return _currentindex;
        }
        set
        {
            if (value >= 3)
            {
                _currentindex = 3 - 1;
            }
            else if (value < 0)
            {
                _currentindex = 0;
            }
            else
            {
                _currentindex = value;
                isMoving = true;
            }
        }
    }
    private float Smooth = 20;
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
    private static int _currentindex;
    private static bool _istouching = false;
    private static bool _ismoving = false;
    private const float beginX = -1 * Const.designWidth;
    private const float maxOffsetX = Const.designWidth;
    private const float minOffsetX = Const.designWidth * -1;
    private float offsetX
    {
        get
        {
            return _offsetx;
        }
        set
        {
            value = (value > maxOffsetX) ? maxOffsetX : value;
            _offsetx = (value < minOffsetX) ? minOffsetX : value;
        }
    }
    private float _offsetx;
    private Vector2 touchBeginPosition;
    private Vector2 touchPrePosition;

    // Use this for initialization
    void Start()
    {
        resetData();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float aimX = getNaturalX(currentIndex);
            float distance = aimX - offsetX;
            if (Math.Abs(distance) <= 30)
            {
                isMoving = false;
                offsetX = aimX;
            }
            else
            {
                offsetX += distance / Smooth;
            }
            return;
        }
        if (!isMoving && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchBeginPosition = touch.position;
                touchPrePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                isTouching = true;
                offsetX = offsetX + touch.position.x - touchPrePosition.x;
                touchPrePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Math.Abs(touchBeginPosition.x - touchPrePosition.x) >= Const.designWidth / 2)
                {
                    if (touchBeginPosition.x > touchPrePosition.x)
                    {
                        currentIndex += 1;
                    }
                    else
                    {
                        currentIndex -= 1;
                    }
                }
                else
                {
                    currentIndex = currentIndex;
                }
                isTouching = false;
            }
            return;
        }
    }
    public void Draw()
    {
        float nowx = offsetX;
        foreach (var part in nowBackground)
        {
            GUI.DrawTexture(new Rect(beginX + nowx, 0, Const.designWidth, Const.designHeight), part);
            nowx += Const.designWidth;
        }
    }
    private float getNaturalX(int currentIndex)
    {
        return (currentIndex - 1) * -1 * Const.designWidth;
    }
    public bool isDay()
    {
        DateTime now = DateTime.Now;
        return (now.Hour >= 6 && now.Hour < 6) ? true : false;
    }
    public void resetData()
    {
        touchBeginPosition = Vector2.zero;
        touchPrePosition = Vector2.zero;
        offsetX = 0;
        currentIndex = 1;
        nowBackground = null;
        isMoving = false;
        isTouching = false;
    }
    public bool getFlag()
    {
        return true;
    }
}