using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelectLobbyModel : MonoBehaviour
{
    public async Task LeaveLobby(bool hostClicked)
    {
        try
        {
            Lobby lobby = LobbyHolder.Instance.GetLobby();
            string lobbyId = lobby.Id;

            if (hostClicked)
            {
                await DeleteLobby(lobbyId);
            }
            else
            {
                await LeaveLobbyAsClient(lobbyId);
            }
        }
        catch (LobbyServiceException e)
        {
            throw e;
        }
    }

    private static async Task LeaveLobbyAsClient(string lobbyId)
    {
        string playerId = AuthenticationService.Instance.PlayerId;
        await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
    }

    private async Task DeleteLobby(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
        }
        catch (LobbyServiceException e)
        {
            throw e;
        }
    }
}

