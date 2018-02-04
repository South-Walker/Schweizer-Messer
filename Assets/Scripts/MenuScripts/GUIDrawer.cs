using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class GUIDrawer : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        resetData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {
        GUI.matrix = Const.guiMatrix;
        var layers = GetComponents<IGUILayer>();
        foreach (var item in layers)
        {
            if (item.getFlag())
            {
                item.Draw();
            } 
        }
    }
    public void resetData()
    {
        var layers = GetComponents<IGUILayer>();
        foreach (var item in layers)
        {
            item.resetData();
        }
    }
}
