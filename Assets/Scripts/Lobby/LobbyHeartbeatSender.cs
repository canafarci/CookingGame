using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyHeartbeatSender : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(SendHeartbeat());
    }

    private IEnumerator SendHeartbeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            Lobby lobby = LobbyHolder.Instance.GetLobby();

            if (CheckIfPlayerIsHost(lobby))
            {
                string lobbyId = lobby.Id;
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            }
        }
    }

    private bool CheckIfPlayerIsHost(Lobby lobby)
    {
        return lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId;
    }
}
