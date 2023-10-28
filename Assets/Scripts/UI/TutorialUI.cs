using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [System.Serializable]
    public struct KeyDisplay
    {
        public TextMeshProUGUI ButtonText;
        public GameInput.Binding Binding;
    }
    [SerializeField] private KeyDisplay[] _keyBindings;

    private void Start()
    {
        InitializeKeyTexts();
        GameInput.Instance.OnKeysBindingsRebound += GameInput_KeyBindingsReboundHandler;
        GameManager.Instance.OnGameStateChanged += GameManager_GameStateChangedHandler;
        GameManager.Instance.OnLocalPlayerReady += GameManager_LocalPlayerReadyHandler;
    }

    private void GameManager_LocalPlayerReadyHandler(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        if (e.State != GameState.WaitingToStart)
        {
            Hide();
        }
    }

    private void GameInput_KeyBindingsReboundHandler(object sender, EventArgs e)
    {
        InitializeKeyTexts();
    }

    private void InitializeKeyTexts()
    {
        foreach (KeyDisplay kd in _keyBindings)
        {
            kd.ButtonText.text = GameInput.Instance.GetBindingText(kd.Binding);
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