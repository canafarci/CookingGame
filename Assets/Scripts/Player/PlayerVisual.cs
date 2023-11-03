using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisual : NetworkBehaviour
{
    [SerializeField] private Renderer _hatRenderer;
    [SerializeField] private Renderer _characterRenderer;
    private PlayerDataHolder _playerDataHolder;

    private void Awake()
    {
        _playerDataHolder = FindObjectOfType<PlayerDataHolder>();
    }

    private void Start()
    {
        if (!IsOwner) return;
        SetPlayerColorServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerColorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong senderClientID = serverRpcParams.Receive.SenderClientId;
        SetColorClientRpc(senderClientID);
    }

    [ClientRpc]
    private void SetColorClientRpc(ulong clientID)
    {
        Color color = _playerDataHolder.GetPlayerColor(clientID);

        Material hatMaterial = _hatRenderer.material;
        Material jacketMaterial = _characterRenderer.materials[4];

        hatMaterial.SetColor("_BaseColor", color);
        jacketMaterial.SetColor("_BaseColor", color);
    }
}
