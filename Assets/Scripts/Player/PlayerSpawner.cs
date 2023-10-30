using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private Transform _playerPrefab;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_LoadEventCompletedhandler;
        }
    }

    private void NetworkManager_LoadEventCompletedhandler(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        var clientIDs = NetworkManager.Singleton.ConnectedClientsIds;

        foreach (ulong clientID in clientIDs)
        {
            Transform player = Instantiate(_playerPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
        }
    }
}
