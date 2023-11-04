using System;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelectLobbyModel : MonoBehaviour
{
    public async void MainMenuClicked(bool hostClicked)
    {
        try
        {
            Lobby lobby = LobbyHolder.Instance.GetLobby();
            string lobbyId = lobby.Id;

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

