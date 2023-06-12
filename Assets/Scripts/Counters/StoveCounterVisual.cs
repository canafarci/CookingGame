using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject _stoveOnGameobject;
    [SerializeField] private GameObject _stoveOnParticles;
    private StoveCounter _stoveCounter;
    private void Awake()
    {
        _stoveCounter = GetComponent<StoveCounter>();
    }
    private void Start()
    {
        _stoveCounter.OnStoveStateChanged += StoveStateChangedHandler;
    }

    private void StoveStateChangedHandler(object sender, OnStoveStateChangedEventArgs e)
    {
        bool showVisual = e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.Burned;
        _stoveOnGameobject.SetActive(showVisual);
        _stoveOnParticles.SetActive(showVisual);
    }
}
