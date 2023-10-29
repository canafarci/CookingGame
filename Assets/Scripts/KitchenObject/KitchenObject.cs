using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;
    private IKitchenObjectParent _kitchenObjectParent;
    private FollowTransform _followTransform;
    private void Awake()
    {
        _followTransform = GetComponent<FollowTransform>();
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }
    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        _kitchenObjectParent?.ClearKitchenObject();
        _kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
            Debug.LogError("Parent has a kitchen object");

        _kitchenObjectParent.SetKitchenObject(this);

        _followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    }
    public void DestroySelf()
    {
        ClearKitchenObjectParentClientRpc();
        Destroy(gameObject);
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
    [ClientRpc]
    private void ClearKitchenObjectParentClientRpc()
    {
        GetKitchenObjectParent()?.ClearKitchenObject();
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        CookingGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }

    //Getters and Setters
    public KitchenObjectScriptableObject GetKitchenObjectSO() => _kitchenObjectSO;
    public IKitchenObjectParent GetKitchenObjectParent() => _kitchenObjectParent;
}
