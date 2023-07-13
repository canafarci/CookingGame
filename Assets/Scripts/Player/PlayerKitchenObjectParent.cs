using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKitchenObjectParent : NetworkBehaviour, IKitchenObjectParent
{
    //Private
    [SerializeField] Transform _kitchenObjectHoldPoint;
    private KitchenObject _kitchenObject;
    //Events
    public static event EventHandler OnAnyPlayerPickedUpObject;

    //Kitchen object parent interface contract
    public void ClearKitchenObject() => _kitchenObject = null;
    public KitchenObject GetKitchenObject() => _kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnAnyPlayerPickedUpObject?.Invoke(this, EventArgs.Empty);
    }
    public bool HasKitchenObject() => _kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
