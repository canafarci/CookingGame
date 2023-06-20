using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter _stove;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _stove.OnStoveStateChanged += StoveStateChangedHandler;
    }

    private void StoveStateChangedHandler(object sender, OnStoveStateChangedEventArgs e)
    {
        bool activateSound = e.State == StoveCounter.State.Frying;

        if (activateSound)
            _audioSource.Play();
        else
            _audioSource.Pause();
    }
}
