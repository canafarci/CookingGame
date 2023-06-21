using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DeliveryUITweener : MonoBehaviour
{
    Vector3 _baseScale = Vector3.one;

    private void OnEnable()
    {
        transform.localScale = _baseScale / 10f;
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(_baseScale, .3f));
        seq.Append(transform.DOPunchRotation(new Vector3(15f, 15f, 15f), .3f));
        seq.AppendInterval(.5f);
        seq.Append(transform.DOScale(_baseScale / 10f, .3f));

        seq.onComplete = () => { gameObject.SetActive(false); };
    }
}
