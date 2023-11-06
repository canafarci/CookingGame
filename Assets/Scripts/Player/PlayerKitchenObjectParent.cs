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
    public event EventHandler<PlayerKitchenObjectChangedArgs> OnPlayerKitchenObjectChanged;

    //Kitchen object parent interface contract
    public void ClearKitchenObject()
    {
        _kitchenObject = null;

        OnPlayerKitchenObjectChanged?.Invoke(this, new PlayerKitchenObjectChangedArgs
        {
            IsHoldingItem = false
        });

    }
    public KitchenObject GetKitchenObject() => _kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyPlayerPickedUpObject?.Invoke(this, EventArgs.Empty);

            OnPlayerKitchenObjectChanged?.Invoke(this, new PlayerKitchenObjectChangedArgs
            {
                IsHoldingItem = true
            });

        }
    }
    public bool HasKitchenObject() => _kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}

public class PlayerKitchenObjectChangedArgs : EventArgs
{
    public bool IsHoldingItem;
}