using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectManager : NetworkBehaviour
{
    [SerializeField] private PlayerReadyChecker _playerReadyChecker;

    private NetworkList<PlayerData> _playerDataList;
    public event EventHandler<PlayerDataListChangedArgs> OnPlayerDataListChanged;
    public event EventHandler<PlayerReadyClickeddArgs> OnPlayerReadyClicked;


    private void Awake()
    {
        _playerDataList = new();
        _playerDataList.OnListChanged += PlayerDataList_OnPlayerDataListChanged;
    }

    private void PlayerDataList_OnPlayerDataListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        int playerCount = _playerDataList.Count;
        ulong clientID = _playerDataList[playerCount - 1].ClientID;

        OnPlayerDataListChanged?.Invoke(this,
                                        new PlayerDataListChangedArgs
                                        {
                                            PlayerCount = playerCount,
                                        });
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_ClientConnectedCallbackHandler;

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        AddClientIdToList(clientId);


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


        int index = 0;

        for (int i = 0; i < _playerDataList.Count; i++)
        {
            PlayerData playerData = _playerDataList[i];
            if (playerData.ClientID == senderClientID)
            {
                index = i;
            }
        }

        PlayerClickedReadyClientRpc(index);
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
        PlayerData playerData = new(clientId);
        _playerDataList.Add(playerData);
    }
}

public class PlayerDataListChangedArgs : EventArgs
{
    public int PlayerCount;
}

public class PlayerReadyClickeddArgs : EventArgs
{
    public int SenderIndex;
}