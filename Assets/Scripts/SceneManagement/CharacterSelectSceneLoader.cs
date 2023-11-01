using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSceneLoader : MonoBehaviour
{
    [SerializeField] private PlayerReadyChecker _playerReadyChecker;

    private void Start()
    {
        _playerReadyChecker.OnAllPlayersReady += PlayerReadyChecker_AllPlayersReadyHandler;
    }

    private void PlayerReadyChecker_AllPlayersReadyHandler(object sender, EventArgs e)
    {
        Loader.NetworkLoadScene(Scene.GameScene);
    }
}
