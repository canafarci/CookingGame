using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CreateLobbyModel : MonoBehaviour
{
    [SerializeField] private NetworkInitializer _networkInitializer;
    private const int MAX_PLAYER_COUNT = 4;
    public event EventHandler OnLobbyCreated;

    public async void CreateLobby(string lobbyName, CreateLobbyOptions options)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYER_COUNT, options);
            LobbyHolder.Instance.SetLobby(lobby);

            _networkInitializer.StartHost();
            OnLobbyCreated?.Invoke(this, EventArgs.Empty);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void QuickJoin()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            LobbyHolder.Instance.SetLobby(lobby);

            _networkInitializer.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void JoinLobbyWithCode(string code)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            LobbyHolder.Instance.SetLobby(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }
}
