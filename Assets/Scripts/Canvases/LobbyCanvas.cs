using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _createLobbyButton;
    [SerializeField] private Button _quickJoinButton;
    [SerializeField] private Button _joinWithCodeButton;
    [SerializeField] private TMP_InputField _joinCodeInputField;
    [SerializeField] private GameObject _createLobbyCanvas;
    [SerializeField] private CreateLobbyController _lobbyController;

    private void Awake()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        _createLobbyButton.onClick.AddListener(() => _createLobbyCanvas.SetActive(true));
        _quickJoinButton.onClick.AddListener(() => _lobbyController.OnQuickJoinClicked());
        _mainMenuButton.onClick.AddListener(() => _lobbyController.OnMainMenuButtonClicked());
        _joinWithCodeButton.onClick.AddListener(() => _lobbyController.OnJoinWithCodeClicked(_joinCodeInputField.text));
    }
}
