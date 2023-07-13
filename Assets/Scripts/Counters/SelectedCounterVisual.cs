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
        if (PlayerInteraction.LocalInstance != null)
        {
            PlayerInteraction.LocalInstance.OnSelectedCounterChanged += SelectedCounterChangedHandler;
        }
        else
        {
            PlayerInteraction.OnAnyPlayerInteracterSpawned += AnyPlayerInteracterSpawnedHandler;
        }
    }
    private void AnyPlayerInteracterSpawnedHandler(object sender, EventArgs e)
    {
        if (PlayerInteraction.LocalInstance != null)
        {
            //in order to avoid multiple identical listeners
            PlayerInteraction.LocalInstance.OnSelectedCounterChanged -= SelectedCounterChangedHandler;
            PlayerInteraction.LocalInstance.OnSelectedCounterChanged += SelectedCounterChangedHandler;
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
        if (PlayerInteraction.LocalInstance != null)
            PlayerInteraction.LocalInstance.OnSelectedCounterChanged -= SelectedCounterChangedHandler;

        PlayerInteraction.OnAnyPlayerInteracterSpawned -= AnyPlayerInteracterSpawnedHandler;
    }
}
