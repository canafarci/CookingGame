using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectButton : NetworkBehaviour
{
    [SerializeField] private PlayerColorLookup _playerColorLookup;
    [SerializeField] private PlayerDataHolder _playerDataHolder;
    [SerializeField] private CharacterSelectManager _characterSelectManager;
    private Image _image;
    private Outline _outline;
    private Button _button;
    private int _colorIndex;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _outline = GetComponent<Outline>();

        _colorIndex = transform.GetSiblingIndex();
    }

    private void Start()
    {
        Color color = _playerColorLookup.GetColorByIndex(_colorIndex);
        _image.color = color;

        _playerDataHolder.OnPlayerDataListChanged += PlayerDataHolder_PlayerDataListChangedHandler;
        _button.onClick.AddListener(() => OnColorChangeButtonClicked());
    }

    private void PlayerDataHolder_PlayerDataListChangedHandler(object sender, PlayerDataListChangedArgs e)
    {
        ulong clientID = NetworkManager.Singleton.LocalClientId;
        bool isSelected = _playerDataHolder.IsColorSelectedByClient(clientID, _colorIndex);

        _outline.enabled = isSelected;
    }

    private void OnColorChangeButtonClicked()
    {
        _characterSelectManager.PlayerClickedChangeColorServerRpc(_colorIndex);
    }
}
