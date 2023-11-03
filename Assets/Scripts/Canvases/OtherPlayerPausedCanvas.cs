using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerPausedCanvas : CanvasUI
{
    private GamePauseHandler _gamePauseHandler;

    private void Awake()
    {
        _gamePauseHandler = FindObjectOfType<GamePauseHandler>();

    }
    void Start()
    {
        _gamePauseHandler.OnPauseToggled += GamePauseHandler_PauseToggledHandler;
        Hide();
    }

    private void GamePauseHandler_PauseToggledHandler(object sender, OnPauseToggledEventArgs e)
    {
        if (!e.LocalPlayerPausedGame && e.IsGamePaused)
        {
            Show();
        }
        else if (e.LocalPlayerPausedGame && e.IsGamePaused)
        {
            Hide();
        }
        else
        {
            Hide();
        }
    }
}
