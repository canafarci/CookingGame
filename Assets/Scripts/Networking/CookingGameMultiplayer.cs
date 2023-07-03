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
        Transform kitchenObjectTransform = Instantiate<Transform>(GetKitchenObjectFromIndex(index).Prefab);
        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }
    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.DestroySelf();
    }
    //Getters - Setters
    public int GetKitchenObjectSOFromIndex(KitchenObjectScriptableObject kitchenObjectSO)
    {
        return _kitchenObjectSOListSO.KitchenObjectSOList.IndexOf(kitchenObjectSO);
    }
    public KitchenObjectScriptableObject GetKitchenObjectFromIndex(int index)
    {
        return _kitchenObjectSOListSO.KitchenObjectSOList[index];
    }


}