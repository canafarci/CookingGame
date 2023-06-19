using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private RectTransform _iconTemplate;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += IngredientAddedHandler;
    }

    private void IngredientAddedHandler(object sender, OnIngredientChangedEventArgs eventArgs)
    {
        if (eventArgs.Added)
        {
            RectTransform iconHolder = Instantiate<RectTransform>(_iconTemplate, transform);
            iconHolder.GetComponent<PlateIcon>().SetKitchenObjectSO(eventArgs.ChangedKitchenObjectSO);
        }
        //if removed
        else
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<PlateIcon>().GetKitchenObjectSO() == eventArgs.ChangedKitchenObjectSO)
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }
    }
}
