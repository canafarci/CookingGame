using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneUI : MonoBehaviour
{
    [SerializeField] Image _loadingBar;

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(Loader.LoadSceneAsync());
    }

    private void Update()
    {
        _loadingBar.fillAmount = Loader.LoadingProgress;
    }
}
