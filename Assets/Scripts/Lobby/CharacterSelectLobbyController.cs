using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    public async void OnMainMenuButtonClicked(bool hostCanceled)
    {
        await _model.LeaveLobby(hostCanceled);

        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Scene.MainMenu);
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
