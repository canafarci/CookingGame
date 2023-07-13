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
    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        _resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        _optionsButton.onClick.AddListener(() =>
        {
            Hide();
            _optionsMenu.Show();
        });
    }
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        Hide();
    }

    private void GameStateChangedHandler(object sender, OnGameStateChangedEventArgs eventArgs)
    {
        if (eventArgs.State == GameState.GamePaused)
            Show();
        else
            Hide();
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
