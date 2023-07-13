using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [System.Serializable]
    public struct KeyBindingDisplay
    {
        public TextMeshProUGUI ButtonText;
        public GameInput.Binding Binding;
        public Button BindButton;
    }
    [SerializeField] private KeyBindingDisplay[] _keyBindings;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Button _closeButton;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pressKeyPrompt;
    //constants
    private const string MIXER_MUSIC_PARAMETER = "MusicVolume";
    private const string MUSIC_VOLUME_SAVE_KEY = "MusVol";
    private const string MASTER_VOLUME_SAVE_KEY = "MixVol";
    private void Start()
    {
        Load();

        GameManager.Instance.OnGameStateChanged += GameStateChangedHandler;
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            _pauseMenu.SetActive(true);
        });

        Hide();
    }
    private void GameStateChangedHandler(object sender, OnGameStateChangedEventArgs eventArgs)
    {
        if (eventArgs.State != GameState.GamePaused)
            Hide();
    }
    private void SetMasterVolume(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
        //save preferences
        PlayerPrefs.SetFloat(MASTER_VOLUME_SAVE_KEY, value);
    }
    private void SetMusicVolume(float value)
    {
        float outputMin = -60f;  // Minimum value in the output range
        float outputMax = 0f;    // Maximum value in the output range
        float outputValue = outputMin + (value * (outputMax - outputMin));

        _mixer.SetFloat(MIXER_MUSIC_PARAMETER, outputValue);

        //save preferences
        PlayerPrefs.SetFloat(MUSIC_VOLUME_SAVE_KEY, value);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _musicVolumeSlider.Select();
    }
    private void Hide()
    {
        _pressKeyPrompt.SetActive(false);
        gameObject.SetActive(false);
    }
    private void InitializeKeyTexts()
    {
        foreach (KeyBindingDisplay kbd in _keyBindings)
        {
            kbd.ButtonText.text = GameInput.Instance.GetBindingText(kbd.Binding);
        }
    }
    private void InitializeButtons()
    {
        foreach (KeyBindingDisplay kbd in _keyBindings)
        {
            kbd.BindButton.onClick.AddListener(() =>
            {
                _pressKeyPrompt.SetActive(true);
                GameInput.Instance.RebindBinding(kbd.Binding, () =>
                {
                    _pressKeyPrompt.SetActive(false);
                    InitializeKeyTexts();
                });
            });
        }
    }
    private void Load()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME_SAVE_KEY))
        {
            float value = PlayerPrefs.GetFloat(MUSIC_VOLUME_SAVE_KEY);
            _musicVolumeSlider.value = value;
            SetMusicVolume(value);
        }
        if (PlayerPrefs.HasKey(MASTER_VOLUME_SAVE_KEY))
        {
            float value = PlayerPrefs.GetFloat(MASTER_VOLUME_SAVE_KEY);
            _masterVolumeSlider.value = value;
            SetMasterVolume(value);
        }

        InitializeKeyTexts();
        InitializeButtons();
    }
}