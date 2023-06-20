using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredCountText;
    private int _recipesDelivered = 0;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        DeliveryManager.Instance.OnPlateDelivered += PlateDeliveredHandler;
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
        if (e.State == GameManager.GameState.GameOver)
            Show();
        else
            Hide();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
