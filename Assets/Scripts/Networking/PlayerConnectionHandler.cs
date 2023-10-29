using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerConnectionHandler : NetworkBehaviour
{
    [SerializeField] private List<Vector3> _spawnPositions;
    private PlayerKitchenObjectParent _playerKitchenObjectParent;
    private void Awake()
    {
        _playerKitchenObjectParent = GetComponent<PlayerKitchenObjectParent>();
    }
    public override void OnNetworkSpawn()
    {
        MovePlayerToSpawnPosition();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_ClientDisconnectedCallbackHandler;
        }
    }

    private void MovePlayerToSpawnPosition()
    {
        //owner client id can be cast into an int to determine player index
        int spawnIndex = (int)OwnerClientId;
        transform.position = _spawnPositions[spawnIndex];
    }

    private void NetworkManager_ClientDisconnectedCallbackHandler(ulong clientID)
    {
        if (clientID == OwnerClientId && _playerKitchenObjectParent.HasKitchenObject())
        {
            KitchenObject kitchenObject = _playerKitchenObjectParent.GetKitchenObject();
            CookingGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
        }
    }
}
