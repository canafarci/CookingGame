using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectScriptableObject> _validKitchenObjectSOList;
    private List<KitchenObjectScriptableObject> _kitchenObjectSOList = new List<KitchenObjectScriptableObject>();
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public bool TryAddIngredient(KitchenObjectScriptableObject kitchenObjectSO)
    {
        if (_kitchenObjectSOList.Contains(kitchenObjectSO) || !_validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //already in the plate or not a valid KO for plate
            return false;
        }
        else
        {
            _kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { AddedKitchenObjectSO = kitchenObjectSO });
            return true;
        }
    }
}

public class OnIngredientAddedEventArgs : EventArgs
{
    public KitchenObjectScriptableObject AddedKitchenObjectSO;
}
