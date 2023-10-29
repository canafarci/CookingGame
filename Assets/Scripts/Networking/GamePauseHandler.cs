using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GamePauseHandler : NetworkBehaviour
{
    private bool _playerPausedGame = false;
    private readonly Dictionary<ulong, bool> _playerGamePauseLookup = new();

    public EventHandler<OnPauseToggledEventArgs> OnPauseToggled;

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_PauseActionHandler;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_ClientDisconnectedCallbackHandler;
        }
    }

    private void GameInput_PauseActionHandler(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        if (!_playerPausedGame)
        {
            _playerPausedGame = true;
            PauseGameServerRpc();
        }
        else
        {
            _playerPausedGame = false;
            UnpauseGameServerRpc();
        }
    }

    private void NetworkManager_ClientDisconnectedCallbackHandler(ulong obj)
    {
        StartCoroutine(DelayedCheckUnpause());
    }

    //delayed because ConnectedClientsIds is updated one frame after player disconnects
    private IEnumerator DelayedCheckUnpause()
    {
        yield return new WaitForEndOfFrame();
        CheckIfGameCanResume();
    }



    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        UpdatePauseLookup(serverRpcParams, false);

        CheckIfGameCanResume();
    }

    private void CheckIfGameCanResume()
    {
        bool allPlayersUnpaused = CheckIfAllPlayersUnpaused();

        if (allPlayersUnpaused)
        {
            Time.timeScale = 1f;
        }

        OnGamePauseStateChangedClientRpc(!allPlayersUnpaused);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Time.timeScale = 0f;

        UpdatePauseLookup(serverRpcParams, true);

        OnGamePauseStateChangedClientRpc(true);
    }

    [ClientRpc]
    private void OnGamePauseStateChangedClientRpc(bool gameIsPaused)
    {
        OnPauseToggled?.Invoke(this, new OnPauseToggledEventArgs
        {
            LocalPlayerPausedGame = _playerPausedGame,
            IsGamePaused = gameIsPaused
        });
    }

    private bool CheckIfAllPlayersUnpaused()
    {
        var connectedClientIds = NetworkManager.Singleton.ConnectedClientsIds;
        bool allPlayersUnpaused = true;

        foreach (ulong clientID in connectedClientIds)
        {
            if (_playerGamePauseLookup.ContainsKey(clientID) && _playerGamePauseLookup[clientID])
            {
                print(_playerGamePauseLookup[clientID]);
                allPlayersUnpaused = false;
                break;
            }
        }

        return allPlayersUnpaused;
    }

    private void UpdatePauseLookup(ServerRpcParams serverRpcParams, bool paused)
    {
        ulong playerID = serverRpcParams.Receive.SenderClientId;
        _playerGamePauseLookup[playerID] = paused;
    }
}

public class OnPauseToggledEventArgs : EventArgs
{
    public bool LocalPlayerPausedGame = false;
    public bool IsGamePaused = false;
}
