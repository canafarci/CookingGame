using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkInitializer : NetworkBehaviour
{
    private const string CHARACTER_SELECT_SCENE = "Character Select Scene";
    private const int MAX_PLAYER_COUNT = 4;

    public EventHandler OnConnnectionAttempted;
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallbackHandler;

        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        OnConnnectionAttempted?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_ConnectionApprovalCallbackHandler(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
                                                                  NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int currentPlayerCount = NetworkManager.Singleton.ConnectedClientsIds.Count;

        if (!sceneName.Equals(CHARACTER_SELECT_SCENE))
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
        }
        else if (currentPlayerCount >= MAX_PLAYER_COUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has reached maximum player count";
        }
        else
        {
            connectionApprovalResponse.Approved = true;
        }
    }
}