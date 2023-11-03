using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private PlayerDataHolder _playerDataHolder;

    private void Start()
    {
        _playerDataHolder.OnPlayerDataListChanged += CharacterSelectManager_PlayerDataListChangedHandler;

        if (_playerIndex != 0)
        {
            Hide();
        }
    }

    private void CharacterSelectManager_PlayerDataListChangedHandler(object sender, PlayerDataListChangedArgs e)
    {
        if (e.PlayerCount > _playerIndex)
        {
            Show();
        }
        else
        {
            Hide();
        }

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public int GetPlayerIndex() => _playerIndex;
}
