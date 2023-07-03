using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] CuttingRecipeScriptableObject[] _cuttingRecipeSOs;
    private static Dictionary<KitchenObjectScriptableObject, CuttingRecipeScriptableObject> _kitchenObjectRecipeDict;
    private int _cuttingProgress;
    //events
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public static event EventHandler OnAnyCut;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //table is empty
        {
            if (player.HasKitchenObject())
            {
                //player has a KO and table is empty
                player.GetKitchenObject().SetKitchenObjectParent(this);
                //sync state
                OnInteractPlaceObjectOnCounterClientRpc();
            }
            else
            {
                //player hasn't got anything
            }
        }
        else //there is a KO on table
        {
            //player has a KO
            if (player.HasKitchenObject())
            {
                //KO is a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        CookingGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                //player hasn't got anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        //only cut if item can be cut
        if (HasKitchenObject() && _kitchenObjectRecipeDict.ContainsKey(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();
        }
    }
    private void Awake()
    {
        //if dict is not initialized, set key value pairs
        InitializeKitchenObjectRecipeDict();
    }
    [ServerRpc(RequireOwnership = false)]
    private void OnInteractPlaceObjectOnCounterServerRpc()
    {
        OnInteractPlaceObjectOnCounterClientRpc();
    }
    [ClientRpc]
    private void OnInteractPlaceObjectOnCounterClientRpc()
    {
        _cuttingProgress = 0;
        FireOnCuttingProgressEvent(0f);
    }
    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }
    [ClientRpc]
    private void CutObjectClientRpc()
    {
        CuttingRecipeScriptableObject cuttingRecipe = _kitchenObjectRecipeDict[GetKitchenObject().GetKitchenObjectSO()];
        int progressMax = cuttingRecipe.CuttingProgressMax;
        //increase counter
        _cuttingProgress++;
        //fire events
        FireOnCuttingProgressEvent((float)_cuttingProgress / progressMax);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        if (IsOwner && _cuttingProgress >= progressMax)
        {
            CookingGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
            //spawn new object
            KitchenObjectScriptableObject outputKitchenObjectSO = cuttingRecipe.Output;
            CookingGameMultiplayer.Instance.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }
    private void FireOnCuttingProgressEvent(float normalizedProgress)
    {
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
        {
            ProgressNormalized = normalizedProgress
        });
    }

    private void InitializeKitchenObjectRecipeDict()
    {
        if (_kitchenObjectRecipeDict == null)
        {
            _kitchenObjectRecipeDict = new Dictionary<KitchenObjectScriptableObject, CuttingRecipeScriptableObject>();

            foreach (CuttingRecipeScriptableObject crso in _cuttingRecipeSOs)
                _kitchenObjectRecipeDict[crso.Input] = crso;
        }
    }
}
public class OnProgressChangedEventArgs : EventArgs
{
    public float ProgressNormalized;
}