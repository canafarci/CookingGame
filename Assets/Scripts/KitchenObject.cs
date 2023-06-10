using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public KitchenObjectScriptableObject KitchenObjectSO { get { return _kitchenObjectSO; } }
    public ClearCounter ClearCounter { get { return _clearCounter; } }
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;
    private ClearCounter _clearCounter;
    public void SetClearCounter(ClearCounter clearCounter)
    {
        _clearCounter?.ClearKitchenObject();

        _clearCounter = clearCounter;

        if (clearCounter.HasKitchenObject)
            Debug.LogError("Counter already has a kitchen object");

        _clearCounter.KitchenObject = this;

        transform.parent = clearCounter.TargetTransform;
        transform.localPosition = Vector3.zero;
    }
}
