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
        _progressCounter.OnProgressChanged += ProgressChangedHandler;
        _progressBarImage.fillAmount = 0f;

        Hide();
    }
    private void ProgressChangedHandler(object sender, OnProgressChangedEventArgs eventArgs)
    {
        if (eventArgs.ProgressNormalized == 0f || eventArgs.ProgressNormalized >= 1f)
            Hide();
        else
            Show();

        _progressBarImage.fillAmount = eventArgs.ProgressNormalized;
    }
    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
