using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanup : MonoBehaviour
{
    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
        }

        var playerDataHolder = FindObjectOfType<PlayerDataHolder>();
        if (playerDataHolder != null)
        {
            Destroy(playerDataHolder.gameObject);
        }

        if (LobbyHolder.Instance != null)
        {
            Destroy(LobbyHolder.Instance.gameObject);
            LobbyHolder.Instance.Cleanup();
        }
    }
}
