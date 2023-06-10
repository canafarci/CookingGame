using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public void ClearKitchenObject();
    public KitchenObject GetKitchenObject();
    public void SetKitchenObject(KitchenObject kitchenObject);
    public bool HasKitchenObject();
    public Transform GetKitchenObjectFollowTransform();
}
