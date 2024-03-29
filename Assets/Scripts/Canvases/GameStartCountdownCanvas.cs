using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownCanvas : CanvasUI
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        Hide();
    }
    private void GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        if (e.State == GameState.CountdownToStart)
            Show();
        else
            Hide();
    }

    private void Update()
    {
        _countdownText.text = Mathf.Ceil(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }
}
