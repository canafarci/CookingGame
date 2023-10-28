using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerReadyChecker : NetworkBehaviour
{
    private readonly Dictionary<ulong, bool> _playerReadyLookup = new Dictionary<ulong, bool>();
    public EventHandler OnAllPlayersReady;

    public void LocalPlayerIsReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong senderClientID = serverRpcParams.Receive.SenderClientId;
        _playerReadyLookup[senderClientID] = true;

        if (CheckIfAllClientsAreReady())
        {
            OnAllPlayersReady?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CheckIfAllClientsAreReady()
    {
        var connectedClientIds = NetworkManager.Singleton.ConnectedClientsIds;

        bool allPlayersAreReady = true;
        foreach (ulong clientID in connectedClientIds)
        {
            if (!_playerReadyLookup.ContainsKey(clientID) || !_playerReadyLookup[clientID])
            {
                allPlayersAreReady = false;
                break;
            }
        }

        return allPlayersAreReady;
    }
}

