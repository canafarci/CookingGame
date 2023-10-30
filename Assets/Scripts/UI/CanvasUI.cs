using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanvasUI : MonoBehaviour
{
    protected void Show()
    {
        gameObject.SetActive(true);
    }

    protected void Hide()
    {
        gameObject.SetActive(false);
    }
}
