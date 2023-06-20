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
        GameOver,
        GamePaused
    }
    private GameState _state;
    private GameState _stateBeforePaused;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private const float GAMEPLAYING_TIMER_MAX = 30f;
    //events
    public event EventHandler<OnGameStateChangedEventArgs> OnGameStateChanged;
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
            case GameState.GamePaused:
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
    public float GetGameCountdownTimerNormalized()
    {
        return _gamePlayingTimer / GAMEPLAYING_TIMER_MAX;
    }
}
public class OnGameStateChangedEventArgs : EventArgs
{
    public GameManager.GameState State;
}
