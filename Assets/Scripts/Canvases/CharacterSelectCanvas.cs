using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectCanvas : NetworkBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private CharacterSelectManager _characterSelectManager;
    [SerializeField] private CharacterSelectLobbyController _controller;

    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(() => _controller.OnMainMenuButtonClicked(IsServer));

        _readyButton.onClick.AddListener(() =>
        {
            _characterSelectManager.PlayerClickedReady();
        });
    }
}

