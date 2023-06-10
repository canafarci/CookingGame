using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private Renderer _counterRenderer;
    [SerializeField] private Material _activeMaterial, _inactiveMaterial;
    private ClearCounter _counter;
    private bool _selectedVisualActive = false;
    private void Awake()
    {
        _counter = GetComponent<ClearCounter>();
    }
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += SelectedCounterChangedHandler;
    }
    private void OnDisable()
    {
        Player.Instance.OnSelectedCounterChanged -= SelectedCounterChangedHandler;
    }

    private void SelectedCounterChangedHandler(object sender, Player.OnSelectedCounterChangedEventArgs eventArgs)
    {
        if (!_selectedVisualActive && eventArgs.SelectedCounter == _counter)
        {
            _counterRenderer.material = _activeMaterial;
            _selectedVisualActive = true;
        }
        else if (_selectedVisualActive && eventArgs.SelectedCounter != _counter)
        {
            _counterRenderer.material = _inactiveMaterial;
            _selectedVisualActive = false;
        }
    }
}
