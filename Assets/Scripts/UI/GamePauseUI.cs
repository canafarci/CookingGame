using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resumeButton;
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
    }
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        Hide();
    }

    private void GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        if (e.State == GameManager.GameState.GamePaused)
            Show();
        else
            Hide();
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
