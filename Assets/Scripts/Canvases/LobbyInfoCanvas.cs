using System;
using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyInfoCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    [SerializeField] private CharacterSelectLobbyController _controller;

    private void Start()
    {
        _controller.OnLobbyInfoReceived += CharacterSelectLobbyModel_LobbyInfoReceivedHandler;
    }

    private void CharacterSelectLobbyModel_LobbyInfoReceivedHandler(object sender, OnLobbyInfoReceivedArgs e)
    {
        _lobbyNameText.text = $"Lobby Name: {e.LobbyName}";
        _lobbyCodeText.text = $"Lobby Code: {e.LobbyCode}";
    }
}
