using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private LobbyLister _lobbyLister;
    [SerializeField] private Transform _lobbyTemplate;

    private void Start()
    {
        _lobbyLister.OnLobbyListChanged += LobbyLister_LobbyListChangedHandler;
    }

    private void LobbyLister_LobbyListChangedHandler(object sender, OnLobbyListChangedEventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child == _lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in e.Lobbies)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, transform);
            lobbyTransform.GetComponent<LobbyListItem>().SetLobby(lobby);
            lobbyTransform.gameObject.SetActive(true);
        }
    }
}
