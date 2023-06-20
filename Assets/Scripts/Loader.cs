using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static float LoadingProgress;
    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene
    }
    private static int _targetSceneIndex;
    private const string LOADING_SCENE_NAME = "LoadingScene";

    public static void Load(Scene scene)
    {
        switch (scene)
        {
            case (Scene.MainMenu):
                _targetSceneIndex = 0;
                break;
            case (Scene.GameScene):
                _targetSceneIndex = 1;
                break;
            case (Scene.LoadingScene):
                _targetSceneIndex = 2;
                break;
            default:
                _targetSceneIndex = 0;
                break;
        }
        SceneManager.LoadScene(LOADING_SCENE_NAME);
    }

    public static IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_targetSceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            LoadingProgress = asyncLoad.progress / 1f;
            yield return null;
        }
    }
}
