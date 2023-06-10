using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    //properties
    public Transform TargetTransform { get { return _counterTopPoint; } }
    public KitchenObject KitchenObject { get { return _kitchenObject; } set { _kitchenObject = value; } }
    public bool HasKitchenObject { get { return _kitchenObject != null; } }
    //private
    [SerializeField] KitchenObjectScriptableObject _kitchenObjectSO;
    [SerializeField] Transform _counterTopPoint;
    [SerializeField] ClearCounter _secondClearCounter;
    [SerializeField] bool _testing;
    private KitchenObject _kitchenObject = null;
    private void Update()
    {
        if (_testing && Input.GetKeyDown(KeyCode.T))
        {
            if (_kitchenObject != null)
            {
                _kitchenObject.SetClearCounter(_secondClearCounter);
            }
        }
    }

    public void ClearKitchenObject() => _kitchenObject = null;
    public void Interact()
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate<Transform>(_kitchenObjectSO.Prefab, _counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetClearCounter(this);
        }
        else
        {
            print(_kitchenObject.ClearCounter);
        }
    }
}
