using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }
    public override void Interact(IKitchenObjectParent player)
    {
        //if player has a KO and that KO is a plate
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            CookingGameMultiplayer.Instance.DestroyKitchenObject(player.GetKitchenObject());
        }
    }
    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
