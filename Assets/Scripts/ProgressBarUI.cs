using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image _progressBarImage;
    CuttingCounter _cuttingCounter;
    private void Awake()
    {
        _cuttingCounter = GetComponentInParent<CuttingCounter>();
    }
    private void Start()
    {
        _cuttingCounter.OnCuttingProgress += CuttingProgressHandler;
        _progressBarImage.fillAmount = 0f;
        Hide();
    }
    private void CuttingProgressHandler(object sender, OnCuttingProgressEventArgs eventArgs)
    {
        _progressBarImage.fillAmount = eventArgs.ProgressNormalized;

        if (eventArgs.ProgressNormalized == 0f || eventArgs.ProgressNormalized == 1f)
            Hide();
        else
            Show();
    }
    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
