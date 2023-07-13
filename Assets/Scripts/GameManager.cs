using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager Instance { get; private set; }
    private GameState _state;
    private GameState _stateBeforePaused;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private bool _localPlayerIsReady = false;
    //Constants
    private const float GAMEPLAYING_TIMER_MAX = 300f;
    //events
    public event EventHandler<OnGameStateChangedEventArgs> OnGameStateChanged;
    public event EventHandler<OnLocalPlayerReadyChangedEventArgs> OnLocalPlayerReadyChanged;
    private void Awake()
    {
        _state = GameState.WaitingToStart;

        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;

        _gamePlayingTimer = GAMEPLAYING_TIMER_MAX;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += PauseActionHandler;
        GameInput.Instance.OnInteractAction += InteractActionHandler;
    }

    private void InteractActionHandler(object sender, EventArgs e)
    {
        if (_state == GameState.WaitingToStart)
        {
            _localPlayerIsReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, new OnLocalPlayerReadyChangedEventArgs { PlayerIsReady = _localPlayerIsReady });
            //TODO remove: _state = GameState.CountdownToStart;
            //TODO remove: OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs { State = _state });
        }
    }

    private void PauseActionHandler(object sender, EventArgs e) => TogglePauseGame();
    public void TogglePauseGame()
    {
        if (_state != GameState.GamePaused)
        {
            _stateBeforePaused = _state;
            _state = GameState.GamePaused;
            Time.timeScale = 0f;
        }
        else
        {
            _state = _stateBeforePaused;
            Time.timeScale = 1f;
        }

        OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs { State = _state });
    }
    private void Update()
    {
        switch (_state)
        {
            case GameState.WaitingToStart:
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
            case GameState.GamePaused:
                break;
            default:
                break;
        }
    }
    //Getters-Setters
    public bool IsGamePlaying() => _state == GameState.GamePlaying;
    public float GetCountdownToStartTimer() => _countdownToStartTimer;
    public float GetGameCountdownTimerNormalized() => _gamePlayingTimer / GAMEPLAYING_TIMER_MAX;
    //TODO remove: public bool IsLocalPlayerReady() => _localPlayerIsReady;
}
public class OnGameStateChangedEventArgs : EventArgs
{
    public GameState State;
}
public class OnLocalPlayerReadyChangedEventArgs : EventArgs
{
    public bool PlayerIsReady;
}
public enum GameState
{
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver,
    GamePaused
}