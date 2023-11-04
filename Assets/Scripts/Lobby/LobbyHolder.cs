using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyHolder : MonoBehaviour
{
    private Lobby _lobby;
    public static LobbyHolder Instance { get; private set; }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }


    public Lobby GetLobby()
    {
        return _lobby;
    }

    public void SetLobby(Lobby lobby)
    {
        _lobby = lobby;
    }

    public void Cleanup()
    {
        Instance = null;
    }
}
