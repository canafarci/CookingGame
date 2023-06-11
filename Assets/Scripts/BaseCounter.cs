using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _counterTopPoint;
    private KitchenObject _kitchenObject = null;

    //IKitchenParent contract
    public void ClearKitchenObject() => _kitchenObject = null;
    public KitchenObject GetKitchenObject() => _kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;
    public bool HasKitchenObject() => _kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => _counterTopPoint;
    public virtual void Interact(Player player) { }
}
