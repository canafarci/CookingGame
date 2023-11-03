using System;
using System.Text.RegularExpressions;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    [SerializeField] private NetworkInitializer _networkInitializer;
    private const int MAX_PLAYER_COUNT = 4;
    private Unity.Services.Lobbies.Models.Lobby _joinedLobby;

    private void Start()
    {
        InitializeUnityAuthentication();
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            CreateLobbyOptions options = new() { IsPrivate = isPrivate };
            _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYER_COUNT, options);

            _networkInitializer.StartHost();
            Loader.NetworkLoadScene(Scene.CharacterSelect);
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
            _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            _networkInitializer.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new();
            string uniqueID = CreateUniqueID();

            options.SetProfile(uniqueID);

            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private static string CreateUniqueID()
    {
        Guid uniqueID = Guid.NewGuid();
        string idText = uniqueID.ToString();
        //authentication service requires names to be 30 charaters long and GUIDs are 36 characters,
        //so remove the last 6 characters
        string slicedID = idText[..^6];
        return slicedID;
    }
}
