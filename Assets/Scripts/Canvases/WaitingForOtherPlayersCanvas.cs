using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersCanvas : CanvasUI
{

    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReady += GameManager_LocalPlayerReadyHandler;
        GameManager.Instance.OnGameStateChanged += GameManager_GameStateChangedHandler;
        Hide();
    }

    private void GameManager_GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        if (e.State == GameState.CountdownToStart)
        {
            Hide();
        }
    }

    private void GameManager_LocalPlayerReadyHandler(object sender, EventArgs e)
    {
        Show();
    }
}
