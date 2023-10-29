using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private readonly NetworkVariable<GameState> _currentGameState = new(GameState.WaitingToStart);
    private readonly NetworkVariable<float> _countdownToStartTimer = new(3f);
    private readonly NetworkVariable<float> _timePlayedTimer = new(0f);
    private GameState _stateBeforePaused;
    private bool _localPlayerIsReady = false;
    private readonly Dictionary<ulong, bool> _playerReadyLookup = new();

    private PlayerReadyChecker _playerReadyChecker;
    private GamePauseHandler _gamePauseHandler;

    private const float GAMEPLAYING_TIMER_MAX = 10f;

    public event EventHandler<OnGameStateChangedEventArgs> OnGameStateChanged;
    public event EventHandler OnLocalPlayerReady;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        InitializeSingleton();
    }
    private void Start()
    {
        _playerReadyChecker = GetComponent<PlayerReadyChecker>();
        _gamePauseHandler = GetComponent<GamePauseHandler>();

        GameInput.Instance.OnInteractAction += GameInput_InteractActionHandler;
        _playerReadyChecker.OnAllPlayersReady += PlayerReadyChecker_AllPlayersReadyHandler;
        _gamePauseHandler.OnPauseToggled += GamePauseHandler_PauseToggledHandler;

    }

    public override void OnNetworkSpawn()
    {
        _currentGameState.OnValueChanged += GameState_ValueChangedHandler;
    }

    private void Update()
    {
        if (!IsServer) { return; }

        switch (_currentGameState.Value)
        {
            case GameState.WaitingToStart:
                break;
            case GameState.CountdownToStart:
                _countdownToStartTimer.Value -= Time.deltaTime;
                if (_countdownToStartTimer.Value <= 0f)
                {
                    _currentGameState.Value = GameState.GamePlaying;
                }
                break;

            case GameState.GamePlaying:
                _timePlayedTimer.Value += Time.deltaTime;
                if (_timePlayedTimer.Value >= GAMEPLAYING_TIMER_MAX)
                {
                    _currentGameState.Value = GameState.GameOver;
                }
                break;
            case GameState.GamePaused:
                break;
            default:
                break;
        }
    }

    private void GamePauseHandler_PauseToggledHandler(object sender, OnPauseToggledEventArgs e)
    {
        if (!IsServer) return;
        if (e.IsGamePaused && _currentGameState.Value == GameState.GamePaused) return;

        TogglePauseState(e);
    }

    private void TogglePauseState(OnPauseToggledEventArgs e)
    {
        if (e.IsGamePaused)
        {
            _stateBeforePaused = _currentGameState.Value;
            _currentGameState.Value = GameState.GamePaused;
        }
        else if (!e.IsGamePaused)
        {
            _currentGameState.Value = _stateBeforePaused;
        }
    }

    private void GameInput_InteractActionHandler(object sender, EventArgs e)
    {
        if (_currentGameState.Value == GameState.WaitingToStart)
        {
            OnLocalPlayerReady?.Invoke(this, EventArgs.Empty);

            _playerReadyChecker.LocalPlayerIsReady();
        }
    }

    private void GameState_ValueChangedHandler(GameState previousValue, GameState newValue)
    {
        OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs { State = newValue });
    }

    private void PlayerReadyChecker_AllPlayersReadyHandler(object sender, EventArgs e)
    {
        if (IsServer)
        {
            _currentGameState.Value = GameState.CountdownToStart;
        }
    }



    private void InitializeSingleton()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    //Getters-Setters
    public bool IsGamePlaying() => _currentGameState.Value == GameState.GamePlaying;
    public float GetCountdownToStartTimer() => _countdownToStartTimer.Value;
    public float GetGameCountdownTimerNormalized() => _timePlayedTimer.Value / GAMEPLAYING_TIMER_MAX;
}
public class OnGameStateChangedEventArgs : EventArgs
{
    public GameState State;
}

public enum GameState
{
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver,
    GamePaused
}