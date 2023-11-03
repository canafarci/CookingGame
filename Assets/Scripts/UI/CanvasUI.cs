using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanvasUI : MonoBehaviour
{
    protected virtual void Show()
    {
        gameObject.SetActive(true);
    }

    protected virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
