using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _counterTopPoint;
    private KitchenObject _kitchenObject = null;
    //events
    public static event EventHandler OnAnyObjectPlaced;

    //IKitchenParent contract
    public void ClearKitchenObject() => _kitchenObject = null;
    public KitchenObject GetKitchenObject() => _kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
    }
    public bool HasKitchenObject() => _kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => _counterTopPoint;
    public virtual void Interact(IKitchenObjectParent player) { Debug.LogError("Interact called from base class"); }
    public virtual void InteractAlternate(IKitchenObjectParent player) { Debug.LogError("Interact alternate called from base class"); }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
