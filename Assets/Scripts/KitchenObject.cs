using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
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
    public void DestroySelf()
    {
        _kitchenObjectParent?.ClearKitchenObject();
        Destroy(gameObject);
    }
    public static void SpawnKitchenObject(KitchenObjectScriptableObject kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        CookingGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    //Getters and Setters
    public KitchenObjectScriptableObject GetKitchenObjectSO() => _kitchenObjectSO;
    public IKitchenObjectParent GetKitchenObjectParent() => _kitchenObjectParent;
}
