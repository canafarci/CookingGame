using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] CuttingRecipeScriptableObject[] _cuttingRecipeSOs;
    private static Dictionary<KitchenObjectScriptableObject, CuttingRecipeScriptableObject> _kitchenObjectRecipeDict;
    private int _cuttingProgress;
    public event EventHandler<OnCuttingProgressEventArgs> OnCuttingProgress;

    //events
    private void Awake()
    {
        //if dict is not initialized, set key value pairs
        InitializeKitchenObjectRecipeDict();
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //table is empty
        {
            if (player.HasKitchenObject())
            {
                //player has a KO and table is empty
                player.GetKitchenObject().SetKitchenObjectParent(this);
                _cuttingProgress = 0;
                FireOnCuttingProgressEvent(0f);
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
                        GetKitchenObject().DestroySelf();
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
            CuttingRecipeScriptableObject cuttingRecipe = _kitchenObjectRecipeDict[GetKitchenObject().GetKitchenObjectSO()];
            int progressMax = cuttingRecipe.CuttingProgressMax;
            //increase counter
            _cuttingProgress++;
            FireOnCuttingProgressEvent((float)_cuttingProgress / progressMax);
            if (_cuttingProgress >= progressMax)
            {
                //spawn new object
                GetKitchenObject().DestroySelf();
                KitchenObjectScriptableObject outputKitchenObjectSO = cuttingRecipe.Output;
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private void FireOnCuttingProgressEvent(float normalizedProgress)
    {
        OnCuttingProgress?.Invoke(this, new OnCuttingProgressEventArgs
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
public class OnCuttingProgressEventArgs : EventArgs
{
    public float ProgressNormalized;
}