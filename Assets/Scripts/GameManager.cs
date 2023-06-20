using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private GameState _state;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer = 10f;
    //events
    public event EventHandler<OnGameStateChangedEventArgs> OnGameStateChanged;
    private void Awake()
    {
        _state = GameState.WaitingToStart;

        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Update()
    {
        switch (_state)
        {
            case GameState.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer <= 0f)
                {
                    _state = GameState.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs { State = _state });
                }
                break;

            case GameState.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer <= 0f)
                {
                    _state = GameState.GamePlaying;
                    OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs { State = _state });
                }
                break;

            case GameState.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer <= 0f)
                {
                    _state = GameState.GameOver;
                    OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs { State = _state });
                }
                break;

            default:
                break;
        }
    }
    //Getters-Setters
    public bool IsGamePlaying()
    {
        return _state == GameState.GamePlaying;
    }
    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }
}
public class OnGameStateChangedEventArgs : EventArgs
{
    public GameManager.GameState State;
}
