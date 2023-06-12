using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private readonly static int _cutHash = Animator.StringToHash("Cut");
    private Animator _animator;
    private CuttingCounter _cuttingCounter;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _cuttingCounter = GetComponent<CuttingCounter>();
    }

    private void Start()
    {
        _cuttingCounter.OnCuttingProgress += CuttingProgressHandler;
    }

    private void CuttingProgressHandler(object sender, OnCuttingProgressEventArgs eventArgs)
    {
        if (eventArgs.ProgressNormalized != 0f)
        {
            _animator.SetTrigger(_cutHash);
        }
    }
}
