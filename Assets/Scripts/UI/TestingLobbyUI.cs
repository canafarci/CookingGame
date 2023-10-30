using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button _createGameButton;
    [SerializeField] private Button _joinGameButton;
    [SerializeField] private NetworkInitializer _networkInitializer;

    private void Awake()
    {
        _createGameButton.onClick.AddListener(() =>
        {
            _networkInitializer.StartHost();
            Loader.NetworkLoadScene(Scene.CharacterSelect);
        });

        _joinGameButton.onClick.AddListener(() =>
        {
            _networkInitializer.StartClient();
        });
    }
}
