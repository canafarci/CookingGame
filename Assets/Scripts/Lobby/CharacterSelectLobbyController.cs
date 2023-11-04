using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelectLobbyController : MonoBehaviour
{
    [SerializeField] private CharacterSelectLobbyModel _model;

    public event EventHandler<OnLobbyInfoReceivedArgs> OnLobbyInfoReceived;

    private void Start()
    {
        Lobby lobby = LobbyHolder.Instance.GetLobby();
        InvokeInfoLobbyReceivedEvent(lobby);
    }

    public void OnMainMenuButtonClicked(bool hostCanceled)
    {
        _model.MainMenuClicked(hostCanceled);
    }

    private void InvokeInfoLobbyReceivedEvent(Lobby currentLobby)
    {
        OnLobbyInfoReceived?.Invoke(this, new OnLobbyInfoReceivedArgs
        {
            LobbyName = currentLobby.Name,
            LobbyCode = currentLobby.LobbyCode
        });
    }
}

public class OnLobbyInfoReceivedArgs : EventArgs
{
    public string LobbyName;
    public string LobbyCode;
}
