using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private KitchenObjectSOSceneObjectLink[] _kitchenObjectSOSceneObjectLinks;
    private PlateKitchenObject _plateKitchenObject;
    private void Awake()
    {
        _plateKitchenObject = GetComponent<PlateKitchenObject>();
    }
    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += IngredientAddedHandler;

        foreach (KitchenObjectSOSceneObjectLink link in _kitchenObjectSOSceneObjectLinks)
        {
            link.SceneObject.SetActive(false);
        }
    }

    private void IngredientAddedHandler(object sender, OnIngredientChangedEventArgs eventArgs)
    {
        foreach (KitchenObjectSOSceneObjectLink link in _kitchenObjectSOSceneObjectLinks)
        {
            if (link.KitchenObjectSO == eventArgs.ChangedKitchenObjectSO)
            {
                link.SceneObject.SetActive(true);
            }
        }
    }
}
[System.Serializable]
public struct KitchenObjectSOSceneObjectLink
{
    public KitchenObjectScriptableObject KitchenObjectSO;
    public GameObject SceneObject;
}
