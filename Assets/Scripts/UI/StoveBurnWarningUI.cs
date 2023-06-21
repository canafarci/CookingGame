using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private const float BURN_WARNING_SHOW_TRESHOLD = .5f;
    private void Start()
    {
        _stoveCounter.OnProgressChanged += ProgressChangedHandler;
        Hide();
    }

    private void ProgressChangedHandler(object sender, OnProgressChangedEventArgs e)
    {
        bool show = _stoveCounter.OutputCanBeBurned() && e.ProgressNormalized > BURN_WARNING_SHOW_TRESHOLD;

        if (show)
            Show();
        else
            Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
