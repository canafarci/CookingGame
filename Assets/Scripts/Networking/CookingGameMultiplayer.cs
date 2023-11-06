using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CookingGameMultiplayer : NetworkBehaviour
{
    [SerializeField] private KitchenObjectListScriptableObject _kitchenObjectSOListSO;
    public static CookingGameMultiplayer Instance { get; private set; }
    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    public void SpawnKitchenObject(KitchenObjectScriptableObject kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(_kitchenObjectSOListSO.KitchenObjectSOList.IndexOf(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }

    //Initialize Singleton
    private void Awake() => Instance = this;

    //Spawn Kitchen Objects on the server
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int index, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        if (!kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject)) return;

        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (!kitchenObjectParent.HasKitchenObject())
        {
            Transform kitchenObjectTransform = SpawnKitchenObject(index);
            KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        if (kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject))
        {
            KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

            kitchenObject.DestroySelf();
        }
    }

    private Transform SpawnKitchenObject(int index)
    {
        Transform kitchenObjectTransform = Instantiate<Transform>(GetKitchenObjectSOFromIndex(index).Prefab);
        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);
        return kitchenObjectTransform;
    }


    //Getters - Setters
    public int GetIndexFromKitchenObjectSO(KitchenObjectScriptableObject kitchenObjectSO)
    {
        return _kitchenObjectSOListSO.KitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    public KitchenObjectScriptableObject GetKitchenObjectSOFromIndex(int index)
    {
        return _kitchenObjectSOListSO.KitchenObjectSOList[index];
    }
}