using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static float LoadingProgress { get; private set; }
    private static string _targetSceneName;
    private static readonly Dictionary<Scene, string> _sceneNames = new()
    {
        { Scene.MainMenu, "Main Menu Scene" },
        { Scene.GameScene, "Game Scene" },
        { Scene.Loading, "Loading Scene" },
        { Scene.Lobby, "Lobby Scene" },
        { Scene.CharacterSelect, "Character Select Scene" }
    };

    public static void LoadScene(Scene scene)
    {
        SetTargetScene(scene);

        string loadingScene = _sceneNames[Scene.Loading];
        SceneManager.LoadScene(loadingScene);
    }

    public static void NetworkLoadScene(Scene scene)
    {
        string sceneName = GetTargetScene(scene);

        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private static string GetTargetScene(Scene scene)
    {
        if (_sceneNames.ContainsKey(scene))
        {
            string targetSceneName = _sceneNames[scene];
            return targetSceneName;
        }
        else
        {
            throw new Exception("Scene is invalid and does not exist in the lookup!");
        }
    }

    private static void SetTargetScene(Scene scene)
    {
        if (_sceneNames.ContainsKey(scene))
        {
            _targetSceneName = _sceneNames[scene];
        }
        else
        {
            throw new Exception("Scene is invalid and does not exist in the lookup!");
        }
    }

    public static IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_targetSceneName);

        while (!asyncLoad.isDone)
        {
            LoadingProgress = asyncLoad.progress;
            yield return null;
        }
    }
}

public enum Scene
{
    MainMenu,
    GameScene,
    Loading,
    Lobby,
    CharacterSelect
}
