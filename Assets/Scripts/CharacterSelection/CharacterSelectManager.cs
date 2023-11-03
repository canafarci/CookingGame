using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectManager : NetworkBehaviour
{
    [SerializeField] private PlayerReadyChecker _playerReadyChecker;
    [SerializeField] private PlayerDataHolder _playerDataHolder;
    public event EventHandler<PlayerReadyClickeddArgs> OnPlayerReadyClicked;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_ClientConnectedCallbackHandler;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        AddClientIdToList(clientId);
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
        _playerDataHolder.RemoveClientData(clientID);
    }

    public void PlayerClickedReady()
    {
        _playerReadyChecker.LocalPlayerIsReady();
        PlayerClickedReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerClickedReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong senderClientID = serverRpcParams.Receive.SenderClientId;

        int index = _playerDataHolder.GetPlayerIndex(senderClientID);

        PlayerClickedReadyClientRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerClickedChangeColorServerRpc(int colorIndex, ServerRpcParams serverRpcParams = default)
    {
        ulong senderClientID = serverRpcParams.Receive.SenderClientId;

        _playerDataHolder.ChangePlayerColor(colorIndex, senderClientID);
    }

    [ClientRpc]
    private void PlayerClickedReadyClientRpc(int index)
    {
        OnPlayerReadyClicked?.Invoke(this, new PlayerReadyClickeddArgs { SenderIndex = index });
    }

    private void NetworkManager_ClientConnectedCallbackHandler(ulong clientId)
    {
        AddClientIdToList(clientId);
    }

    private void AddClientIdToList(ulong clientId)
    {
        _playerDataHolder.AddPlayerData(clientId);
    }
}



public class PlayerReadyClickeddArgs : EventArgs
{
    public int SenderIndex;
}