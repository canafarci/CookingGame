using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] Image _timerImage;
    private void Update()
    {
        _timerImage.fillAmount = GameManager.Instance.GetGameCountdownTimerNormalized();
    }
}
