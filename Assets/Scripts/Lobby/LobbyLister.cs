using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyLister : MonoBehaviour
{
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    private const float LOBBY_REFRESH_RATE = 3f;

    private void Start()
    {
        InvokeRepeating(nameof(ListLobbies), 0f, LOBBY_REFRESH_RATE);
    }
    private async void ListLobbies()
    {
        if (LobbyHolder.Instance.GetLobby() != null || !AuthenticationService.Instance.IsSignedIn) return;

        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            }
            };

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
            {
                Lobbies = queryResponse.Results
            });
        }
        catch (LobbyServiceException e)
        {

            throw e;
        }
    }
}

public class OnLobbyListChangedEventArgs : EventArgs
{
    public List<Lobby> Lobbies;
}