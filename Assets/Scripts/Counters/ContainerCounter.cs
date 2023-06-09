using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;

    //events
    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(IKitchenObjectParent player)
    {
        if (player.HasKitchenObject())
        {
            // player has a KO
        }
        else
        {
            CookingGameMultiplayer.Instance.SpawnKitchenObject(_kitchenObjectSO, player);
            InteractLogicServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        //fire event
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
