using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class CreateLobbyModel : MonoBehaviour
{
    [SerializeField] private NetworkInitializer _networkInitializer;
    private const int MAX_PLAYER_COUNT = 4;
    private const string RELAY_JOIN_CODE_KEY = "RelayJoinCode";

    public event EventHandler OnLobbyCreated;

    public async void CreateLobby(string lobbyName, CreateLobbyOptions options)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYER_COUNT, options);
            LobbyHolder.Instance.SetLobby(lobby);

            Allocation allocation = await AllocateRelay();
            string relayJoinCode = await GetRelayJoinCode(allocation);

            RelayServerData serverData = new RelayServerData(allocation, "dtls");
            SetServerRelayData(serverData);

            UpdateLobbyOptions updateOptions = CreateUpdateLobbyOptions(relayJoinCode);

            await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, updateOptions);

            _networkInitializer.StartHost();

            OnLobbyCreated?.Invoke(this, EventArgs.Empty);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    private UpdateLobbyOptions CreateUpdateLobbyOptions(string relayJoinCode)
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>
        {
            {RELAY_JOIN_CODE_KEY,  new DataObject (DataObject.VisibilityOptions.Member, relayJoinCode)}
        };

        UpdateLobbyOptions updateOptions = new UpdateLobbyOptions
        {
            Data = lobbyData
        };

        return updateOptions;
    }

    public async void QuickJoin()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            await SetRelayDataAsClient(lobby);

            OnJoinedAsClient(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    private async Task SetRelayDataAsClient(Lobby lobby)
    {
        string relayJoinCode = lobby.Data[RELAY_JOIN_CODE_KEY].Value;
        JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

        RelayServerData serverData = new RelayServerData(joinAllocation, "dtls");
        SetServerRelayData(serverData);
    }

    private void SetServerRelayData(RelayServerData serverData)
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetRelayServerData(serverData);
    }

    public async void JoinLobbyWithCode(string code)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            await SetRelayDataAsClient(lobby);
            OnJoinedAsClient(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void JoinLobbyWithId(string code)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(code);
            await SetRelayDataAsClient(lobby);
            OnJoinedAsClient(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    private void OnJoinedAsClient(Lobby lobby)
    {
        LobbyHolder.Instance.SetLobby(lobby);
        _networkInitializer.StartClient();
    }

    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
            return default;
        }
    }

    private async Task<Allocation> AllocateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYER_COUNT - 1);
            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
            return default;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogWarning(e);
            return default;
        }
    }
}
