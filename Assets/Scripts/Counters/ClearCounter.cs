using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    //private
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;
    public override void Interact(IKitchenObjectParent player)
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
                //player is carrying a KO other than plate
                else
                {
                    //KO on the table is a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            CookingGameMultiplayer.Instance.DestroyKitchenObject(player.GetKitchenObject());
                        }
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
}
