using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] CuttingRecipeScriptableObject[] _cuttingRecipeSOs;
    private static Dictionary<KitchenObjectScriptableObject, KitchenObjectScriptableObject> _kitchenObjectRecipeDict;

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
            }
            else
            {
                //player hasn't got anything
            }
        }
        else //there is a KO on table
        {
            if (player.HasKitchenObject())
            {
                //player has a KO
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
        if (HasKitchenObject())
        {
            KitchenObjectScriptableObject outputKitchenObjectSO = _kitchenObjectRecipeDict[GetKitchenObject().GetKitchenObjectSO()];
            //only cut if item can be cut
            if (outputKitchenObjectSO != null)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }
    private void InitializeKitchenObjectRecipeDict()
    {
        if (_kitchenObjectRecipeDict == null)
        {
            _kitchenObjectRecipeDict = new Dictionary<KitchenObjectScriptableObject, KitchenObjectScriptableObject>();

            foreach (CuttingRecipeScriptableObject crso in _cuttingRecipeSOs)
                _kitchenObjectRecipeDict[crso.Input] = crso.Output;
        }
    }
}
