using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlateIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    private KitchenObjectScriptableObject _currentKitchenObjectSO = null;
    public void SetKitchenObjectSO(KitchenObjectScriptableObject kitchenObjectSO)
    {
        _image.sprite = kitchenObjectSO.Sprite;
        _currentKitchenObjectSO = kitchenObjectSO;
    }
    public KitchenObjectScriptableObject GetKitchenObjectSO() => _currentKitchenObjectSO;
}
