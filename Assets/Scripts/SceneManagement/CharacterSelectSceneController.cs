using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelectSceneController : MonoBehaviour
{
    [SerializeField] private PlayerReadyChecker _playerReadyChecker;
    [SerializeField] private CharacterSelectLobbyModel _model;

    private void Start()
    {
        _playerReadyChecker.OnAllPlayersReady += PlayerReadyChecker_AllPlayersReadyHandler;
    }

    private async void PlayerReadyChecker_AllPlayersReadyHandler(object sender, EventArgs e)
    {
        await _model.LeaveLobby(CheckIfPlayerIsHost());

        Loader.NetworkLoadScene(Scene.GameScene);
    }

    private bool CheckIfPlayerIsHost()
    {
        try
        {
            Lobby lobby = LobbyHolder.Instance.GetLobby();
            return lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId;
        }
        catch (LobbyServiceException e)
        {
            throw e;
        }
    }
}
