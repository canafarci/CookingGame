using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkInitializer : NetworkBehaviour
{
    private GameState _currentGameState;
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_GameStateChangedHandler;
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallbackHandler;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();

    }

    private void GameManager_GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        _currentGameState = e.State;
    }

    private void NetworkManager_ConnectionApprovalCallbackHandler(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
                                                                  NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (_currentGameState == GameState.WaitingToStart)
        {
            connectionApprovalResponse.Approved = true;
            connectionApprovalResponse.CreatePlayerObject = true;
        }
        else
        {
            connectionApprovalResponse.Approved = false;
        }
    }
}
