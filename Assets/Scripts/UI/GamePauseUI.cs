using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private OptionsMenuUI _optionsMenu;
    private GamePauseHandler _gamePauseHandler;

    private void Awake()
    {
        _gamePauseHandler = FindObjectOfType<GamePauseHandler>();

        _mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        _resumeButton.onClick.AddListener(() =>
        {
            _gamePauseHandler.TogglePauseGame();
        });
        _optionsButton.onClick.AddListener(() =>
        {
            Hide();
            _optionsMenu.Show();
        });
    }
    private void Start()
    {
        _gamePauseHandler.OnPauseToggled += GamePauseHandler_PauseToggledHandler;
        Hide();
    }

    private void GamePauseHandler_PauseToggledHandler(object sender, OnPauseToggledEventArgs e)
    {
        if (e.LocalPlayerPausedGame)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        _resumeButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
