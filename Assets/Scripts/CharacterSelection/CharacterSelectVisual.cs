using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectVisual : MonoBehaviour
{
    [SerializeField] private PlayerColorLookup _playerColorLookup;
    [SerializeField] private PlayerDataHolder _playerDataHolder;
    [SerializeField] private int _playerIndex;
    [SerializeField] private Renderer _hatRenderer;
    [SerializeField] private Renderer _characterRenderer;

    private void Awake()
    {
        _playerDataHolder.OnPlayerDataListChanged += PlayerDataHolder_PlayerDataListChangedHandler;
    }

    private void PlayerDataHolder_PlayerDataListChangedHandler(object sender, PlayerDataListChangedArgs e)
    {
        if (_playerIndex >= e.PlayerCount) return;

        int colorIndex = _playerDataHolder.GetPlayerColorIndex(_playerIndex);
        Color color = _playerColorLookup.GetColorByIndex(colorIndex);

        Material hatMaterial = _hatRenderer.material;
        Material jacketMaterial = _characterRenderer.materials[4];

        hatMaterial.SetColor("_BaseColor", color);
        jacketMaterial.SetColor("_BaseColor", color);
    }
}
