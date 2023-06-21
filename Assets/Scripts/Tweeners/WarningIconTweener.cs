using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WarningIconTweener : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().DOFade(0f, .3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
