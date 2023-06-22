using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CookingGameMultiplayer : NetworkBehaviour
{
    public static CookingGameMultiplayer Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void SpawnKitchenObject(KitchenObjectScriptableObject kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate<Transform>(kitchenObjectSO.Prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }
}