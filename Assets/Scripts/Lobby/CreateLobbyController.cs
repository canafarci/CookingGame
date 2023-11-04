using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using UnityEngine;

public class CreateLobbyController : MonoBehaviour
{
    [SerializeField] private CreateLobbyModel _lobby;

    public void OnCreateLobbyClicked(string name, bool isPrivate)
    {
        if (!String.IsNullOrEmpty(name))
        {
            CreateLobbyOptions options = new() { IsPrivate = isPrivate };
            _lobby.CreateLobby(name, options);
        }
    }

    public void OnQuickJoinClicked()
    {
        _lobby.QuickJoin();
    }

    public void OnMainMenuButtonClicked()
    {
        Loader.LoadScene(Scene.MainMenu);
    }

    public void OnJoinWithCodeClicked(string code)
    {
        if (!String.IsNullOrEmpty(code))
        {
            _lobby.JoinLobbyWithCode(code);
        }
    }
}
