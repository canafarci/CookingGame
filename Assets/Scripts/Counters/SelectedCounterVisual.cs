using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private Renderer[] _counterRenderers;
    [SerializeField] private Material _activeMaterial, _inactiveMaterial;
    private BaseCounter _counter;
    private bool _selectedVisualActive = false;
    private void Awake()
    {
        _counter = GetComponent<BaseCounter>();
    }
    private void Start()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged += SelectedCounterChangedHandler;
        }
        else
        {
            Player.OnAnyPlayerSpawned += AnyPlayerSpawnedHandler;
        }
    }
    private void AnyPlayerSpawnedHandler(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            //in order to avoid multiple identical listeners
            Player.LocalInstance.OnSelectedCounterChanged -= SelectedCounterChangedHandler;
            Player.LocalInstance.OnSelectedCounterChanged += SelectedCounterChangedHandler;
        }
    }
    private void SelectedCounterChangedHandler(object sender, OnSelectedCounterChangedEventArgs eventArgs)
    {
        if (!_selectedVisualActive && eventArgs.SelectedCounter == _counter)
        {
            foreach (Renderer rnd in _counterRenderers)
                rnd.material = _activeMaterial;

            _selectedVisualActive = true;
        }
        else if (_selectedVisualActive && eventArgs.SelectedCounter != _counter)
        {
            foreach (Renderer rnd in _counterRenderers)
                rnd.material = _inactiveMaterial;

            _selectedVisualActive = false;
        }
    }
    //cleanup
    private void OnDisable()
    {
        if (Player.LocalInstance != null)
            Player.LocalInstance.OnSelectedCounterChanged -= SelectedCounterChangedHandler;

        Player.OnAnyPlayerSpawned -= AnyPlayerSpawnedHandler;
    }
}
