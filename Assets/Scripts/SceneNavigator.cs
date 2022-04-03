using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNavigator
{
    public static void GoToMenuTitleScreen()
    {
        SceneManager.LoadScene(Scenes.menu.ToString(), LoadSceneMode.Single);
    }


    public static void GoToMainGameScene()
    {
        SceneManager.LoadScene(Scenes.main.ToString(), LoadSceneMode.Single);
    }
}
