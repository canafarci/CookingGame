using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;
    [SerializeField] private NetworkInitializer _networkInitializer;

    private void Awake()
    {
        BindButtons();
    }

    private void Start()
    {

    }

    private void BindButtons()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            _networkInitializer.StartHost();
            Hide();
        });
        _startClientButton.onClick.AddListener(() =>
        {
            _networkInitializer.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
