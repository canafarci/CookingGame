using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image _progressBarImage;
    IHasProgress _progressCounter;
    private void Awake()
    {
        _progressCounter = GetComponentInParent<IHasProgress>();
    }
    private void Start()
    {
        _progressCounter.OnCuttingProgress += CuttingProgressHandler;
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
