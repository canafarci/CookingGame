using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerPausedUI : MonoBehaviour
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
        print($"game is paused:   {e.IsGamePaused}");
        print($"LocalPlayerPausedGame:   {e.LocalPlayerPausedGame}");

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

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
