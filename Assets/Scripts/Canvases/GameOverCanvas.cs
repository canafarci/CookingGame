using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : CanvasUI
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _recipesDeliveredCountText;
    private int _recipesDelivered = 0;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        DeliveryManager.Instance.OnPlateDelivered += PlateDeliveredHandler;
        BindMainMenuButton();
        Hide();
    }

    private void PlateDeliveredHandler(object sender, OnPlateDeliveredEventArgs e)
    {
        if (e.Successful)
            _recipesDelivered++;

        _recipesDeliveredCountText.text = _recipesDelivered.ToString();
    }

    private void GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        if (e.State == GameState.GameOver)
            Show();
        else
            Hide();
    }

    private void BindMainMenuButton()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Scene.MainMenu);
        });
    }
}
