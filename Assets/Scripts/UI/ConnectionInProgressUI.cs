using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionInProgressUI : NetworkBehaviour
{
    [SerializeField] private NetworkInitializer _networkInitializer;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _responseText;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() =>
        {
            _closeButton.gameObject.SetActive(false);
            Hide();
        });
    }
    private void Start()
    {
        _networkInitializer.OnConnnectionAttempted += NetworkInitializer_ConnnectionAttemptedHandler;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_ClientDisconnectCallbackHandler;

        Hide();
    }

    private void NetworkManager_ClientDisconnectCallbackHandler(ulong obj)
    {
        Show();

        _closeButton.gameObject.SetActive(true);
        string responseText = NetworkManager.Singleton.DisconnectReason;

        if (responseText == "")
        {
            responseText = "Failed to connect";
        }

        _responseText.text = responseText;
    }

    private void NetworkInitializer_ConnnectionAttemptedHandler(object sender, EventArgs e)
    {
        Show();

        _responseText.text = "Connecting...";
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _networkInitializer.OnConnnectionAttempted -= NetworkInitializer_ConnnectionAttemptedHandler;
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_ClientDisconnectCallbackHandler;
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
