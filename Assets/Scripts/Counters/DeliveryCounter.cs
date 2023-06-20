using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }
    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public override void Interact(Player player)
    {
        //if player has a KO and that KO is a plate
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            player.GetKitchenObject().DestroySelf();
        }
    }
}
