using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _createLobbyButton;
    [SerializeField] private Button _quickJoinButton;
    [SerializeField] private Lobby _lobby;


    private void Awake()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        _mainMenuButton.onClick.AddListener(() => Loader.LoadScene(Scene.MainMenu));
        _createLobbyButton.onClick.AddListener(() => _lobby.CreateLobby("MyLobby", false));
        _quickJoinButton.onClick.AddListener(() => _lobby.QuickJoin());
    }
}
