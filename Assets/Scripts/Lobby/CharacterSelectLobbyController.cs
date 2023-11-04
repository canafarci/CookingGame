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

    void Start()
    {
        SetCurrentLobby();
    }

    public void OnMainMenuButtonClicked(bool hostCanceled)
    {
        _model.MainMenuClicked(hostCanceled);
    }

    private async void SetCurrentLobby()
    {
        try
        {
            List<string> lobbyIds = await LobbyService.Instance.GetJoinedLobbiesAsync();
            string lobbyId = lobbyIds[0];

            Lobby currentLobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);

            InvokeInfoLobbyReceivedEvent(currentLobby);

            _model.SetCurrentLobby(currentLobby);
        }
        catch (LobbyServiceException e)
        {
            throw e;
        }
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
