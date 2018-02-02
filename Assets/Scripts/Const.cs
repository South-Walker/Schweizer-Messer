using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const : MonoBehaviour {
    public const float  designWidth = 480f;
    public const float designHeight = 800f;

    public static Matrix4x4 getMatrix()
    {
        Matrix4x4 guiMatrix = Matrix4x4.identity;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
       // Quaternion rotateAngle = (screenWidth > screenHight) ? Quaternion.identity : Quaternion.Euler(90, 0, 0);
        guiMatrix.SetTRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(screenWidth / designWidth, screenHeight / designHeight, 1));
        return guiMatrix;
    }
    public static Matrix4x4 getInvertMatrix()
    {
        Matrix4x4 guiInverseMatrix = getMatrix();
        guiInverseMatrix = Matrix4x4.Inverse(guiInverseMatrix);
        return guiInverseMatrix;
    }
}
