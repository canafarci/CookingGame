using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectedUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_ClientDisconnectedCallbackHandler;

        BindMainMenuButton();

        Hide();
    }

    private void BindMainMenuButton()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Scene.MainMenu);
        });
    }

    private void NetworkManager_ClientDisconnectedCallbackHandler(ulong clientID)
    {
        //server is shutting down
        if (clientID == NetworkManager.ServerClientId)
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
