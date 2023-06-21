using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failSprite;

    private void Start()
    {
        DeliveryManager.Instance.OnPlateDelivered += PlateDeliveredHandler;
        gameObject.SetActive(false);
    }

    private void PlateDeliveredHandler(object sender, OnPlateDeliveredEventArgs e)
    {
        if (e.Successful)
        {
            _backgroundImage.color = _successColor;
            _iconImage.sprite = _successSprite;
            _messageText.text = "DELIVERY\nSUCCESS";
        }
        else
        {
            _backgroundImage.color = _failColor;
            _iconImage.sprite = _failSprite;
            _messageText.text = "DELIVERY\nFAILED";
        }

        gameObject.SetActive(true);
    }
}
