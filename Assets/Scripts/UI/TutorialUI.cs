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
        GameInput.Instance.OnKeysBindingsRebound += KeyBindingsReboundHandler;
        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        GameManager.Instance.OnLocalPlayerReadyChanged += LocalPlayerReadyChangedHandler;
    }

    private void LocalPlayerReadyChangedHandler(object sender, OnLocalPlayerReadyChangedEventArgs e)
    {
        if (e.PlayerIsReady)
        {
            Hide();
        }
    }

    private void GameStateChangedHandler(object sender, OnGameStateChangedEventArgs e)
    {
        if (e.State != GameState.WaitingToStart)
        {
            Hide();
        }
    }

    private void KeyBindingsReboundHandler(object sender, EventArgs e)
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