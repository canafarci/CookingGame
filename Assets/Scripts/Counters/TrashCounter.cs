using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            //destroy KO if player is holding one
            player.GetKitchenObject().DestroySelf();
            //fire event
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
