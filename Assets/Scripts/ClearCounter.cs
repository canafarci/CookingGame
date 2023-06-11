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
        if (!HasKitchenObject())
        {

        }
        else
        {
        }
    }
}
