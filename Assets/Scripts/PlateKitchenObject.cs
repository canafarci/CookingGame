using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectScriptableObject> _validKitchenObjectSOList;
    private List<KitchenObjectScriptableObject> _kitchenObjectSOList = new List<KitchenObjectScriptableObject>();
    public event EventHandler<OnIngredientChangedEventArgs> OnIngredientAdded;
    public bool TryAddIngredient(KitchenObjectScriptableObject kitchenObjectSO)
    {
        if (_kitchenObjectSOList.Contains(kitchenObjectSO) || !_validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //already in the plate or not a valid KO for plate
            return false;
        }
        else
        {
            int kitchenObjectSOIndex = CookingGameMultiplayer.Instance.GetIndexFromKitchenObjectSO(kitchenObjectSO);
            AddIngredientServerRpc(kitchenObjectSOIndex);
            return true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectScriptableObject kitchenObjectSO = CookingGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientChangedEventArgs { ChangedKitchenObjectSO = kitchenObjectSO, Added = true });
    }
    //Getters-Setters
    public List<KitchenObjectScriptableObject> GetCurrentKitchenObjectSOList() => _kitchenObjectSOList;
}

public class OnIngredientChangedEventArgs : EventArgs
{
    public KitchenObjectScriptableObject ChangedKitchenObjectSO;
    public bool Added;
}
