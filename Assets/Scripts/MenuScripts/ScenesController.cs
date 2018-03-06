using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesController : MonoBehaviour
{
    static AsyncOperation ChangeProg;
    public static void LoadScene(string scenename)
    {
        ChangeProg = SceneManager.LoadSceneAsync("FantasyPlanetScene");
        ChangeProg.allowSceneActivation = false;
    }
    public static void ChangeScene()
    {
        Thread.Sleep(500);
        ChangeProg.allowSceneActivation = true;
    }
}
