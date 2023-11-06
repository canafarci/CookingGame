using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class CreateLobbyController : MonoBehaviour
{
    [SerializeField] private CreateLobbyModel _model;

    private void Start()
    {
        InitializeUnityAuthentication();
        _model.OnLobbyCreated += CreateLobbyModel_LobbyCreatedHandler;
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
            _model.CreateLobby(name, options);
        }
    }

    public void OnQuickJoinClicked()
    {
        _model.QuickJoin();
    }

    public void OnMainMenuButtonClicked()
    {
        Loader.LoadScene(Scene.MainMenu);
    }

    public void OnJoinWithCodeClicked(string code)
    {
        if (!String.IsNullOrEmpty(code))
        {
            _model.JoinLobbyWithCode(code);
        }
    }
    public void OnLobbyButtonClicked(string code)
    {
        if (!String.IsNullOrEmpty(code))
        {
            _model.JoinLobbyWithId(code);
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
