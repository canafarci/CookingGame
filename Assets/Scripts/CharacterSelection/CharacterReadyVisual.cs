using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterReadyVisual : MonoBehaviour
{
    [SerializeField] private GameObject _readyText;
    [SerializeField] private CharacterSelectManager _characterSelectManager;
    private CharacterSelectPlayer _player;

    private void Awake()
    {
        _player = GetComponent<CharacterSelectPlayer>();
    }

    private void Start()
    {
        _characterSelectManager.OnPlayerReadyClicked += CharacterSelectUI_PlayerReadyClickedHandler;
    }

    private void CharacterSelectUI_PlayerReadyClickedHandler(object sender, PlayerReadyClickeddArgs e)
    {
        int playerIndex = _player.GetPlayerIndex();

        if (playerIndex == e.SenderIndex)
        {
            _readyText.SetActive(true);
        }
    }

}
