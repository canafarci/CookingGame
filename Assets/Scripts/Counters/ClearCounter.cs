using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    //private
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;
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
}
