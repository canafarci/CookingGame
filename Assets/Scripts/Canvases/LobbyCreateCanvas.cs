using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateCanvas : CanvasUI
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _createPublicButton;
    [SerializeField] private Button _createPrivateButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private CreateLobbyController _lobbyController;

    void Start()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        _createPublicButton.onClick.AddListener(() => { _lobbyController.OnCreateLobbyClicked(_inputField.text, false); });
        _createPrivateButton.onClick.AddListener(() => _lobbyController.OnCreateLobbyClicked(_inputField.text, true));
        _createPrivateButton.onClick.AddListener(() => Hide());
    }
}
