using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private RectTransform _recipePrefab;

    private void Start()
    {
        DeliveryManager.Instance.OnWaitingRecipeSOListChanged += WaitingRecipeSOListChangedHandler;
    }

    private void WaitingRecipeSOListChangedHandler(object sender, OnWaitingRecipeSOListChangedEventArgs eventArgs)
    {
        if (eventArgs.Added)
        {
            RectTransform recipePrefab = Instantiate<RectTransform>(_recipePrefab, _container);
            recipePrefab.GetComponent<RecipeInfoBox>().SetRecipeInfoBox(eventArgs.ChangedRecipe);
        }
        //if removed
        else
        {
            foreach (Transform child in _container)
            {
                if (child.GetComponent<RecipeInfoBox>().GetRecipeSO() == eventArgs.ChangedRecipe)
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }
    }
}
