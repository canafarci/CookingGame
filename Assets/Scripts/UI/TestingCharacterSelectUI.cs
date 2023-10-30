using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _readyButton;
    [SerializeField] private CharacterSelectSceneLoader _characterSelectSceneLoader;

    private void Awake()
    {
        _readyButton.onClick.AddListener(() =>
        {
            _characterSelectSceneLoader.OnPlayerReadyButtonClicked();
        });
    }
}
