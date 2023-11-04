using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;

public class CreateLobbyController : MonoBehaviour
{
    [SerializeField] private CreateLobbyModel _lobby;

    private void Start()
    {
        InitializeUnityAuthentication();
        _lobby.OnLobbyCreated += CreateLobbyModel_LobbyCreatedHandler;
    }

    private void CreateLobbyModel_LobbyCreatedHandler(object sender, EventArgs e)
    {
        Loader.NetworkLoadScene(Scene.CharacterSelect);
    }

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
