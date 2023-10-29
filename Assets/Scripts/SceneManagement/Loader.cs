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
    private static string _targetSceneName;
    private const string LOADING_SCENE_NAME = "Loading Scene";
    private const string MAIN_MENU_SCENE_NAME = "Main Menu Scene";
    private const string GAME_SCENE_NAME = "Game Scene";

    public static void Load(Scene scene)
    {
        switch (scene)
        {
            case (Scene.MainMenu):
                _targetSceneName = MAIN_MENU_SCENE_NAME;
                break;
            case (Scene.GameScene):
                _targetSceneName = GAME_SCENE_NAME;
                break;
            case (Scene.LoadingScene):
                _targetSceneName = LOADING_SCENE_NAME;
                break;
            default:
                _targetSceneName = MAIN_MENU_SCENE_NAME;
                break;
        }
        SceneManager.LoadScene(LOADING_SCENE_NAME);
    }

    public static IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_targetSceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            LoadingProgress = asyncLoad.progress / 1f;
            yield return null;
        }
    }
}
