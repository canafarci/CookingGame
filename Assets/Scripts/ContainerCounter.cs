using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;

    //events
    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(Player player)
    {
        Transform kitchenObjectTransform = Instantiate<Transform>(_kitchenObjectSO.Prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        //fire event
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
