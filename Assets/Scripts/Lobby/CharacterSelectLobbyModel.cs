using System;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelectLobbyModel : MonoBehaviour
{
    private Unity.Services.Lobbies.Models.Lobby _currentLobby;

    public void SetCurrentLobby(Unity.Services.Lobbies.Models.Lobby currentLobby)
    {
        _currentLobby = currentLobby;
    }

    public async void MainMenuClicked(bool hostClicked)
    {
        try
        {
            string lobbyId = _currentLobby.Id;

            if (hostClicked)
            {
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
            }
            else
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            }
        }
        catch (LobbyServiceException e)
        {
            throw e;
        }

        NetworkManager.Singleton.Shutdown();
        Loader.LoadScene(Scene.MainMenu);
    }
}

