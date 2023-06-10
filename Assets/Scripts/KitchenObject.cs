using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;
    private IKitchenObjectParent _kitchenObjectParent;
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        _kitchenObjectParent?.ClearKitchenObject();

        _kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
            Debug.LogError("Parent has a kitchen object");

        _kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    //Getters and Setters
    public KitchenObjectScriptableObject GetKitchenObjectSO() => _kitchenObjectSO;
    public IKitchenObjectParent GetKitchenObjectParent() => _kitchenObjectParent;
}
