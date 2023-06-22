using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    [SerializeField] private RecipeListScriptableObject _recipeListSO;
    private List<RecipeScriptableObject> _waitingRecipeSOList = new List<RecipeScriptableObject>();
    private float _spawnRecipeTimer;
    //constants
    private const float SPAWN_RECIPE_TIMER_MAX = 8f;
    private const int WAITING_RECIPES_MAX = 5;
    //Singleton reference
    public static DeliveryManager Instance { get; private set; }
    //Events
    public event EventHandler<OnWaitingRecipeSOListChangedEventArgs> OnWaitingRecipeSOListChanged;
    public event EventHandler<OnPlateDeliveredEventArgs> OnPlateDelivered;
    private void Awake()
    {
        //initialize singleton
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Update()
    {
        //if not the server, pop call stack frame
        if (!IsServer) return;

        _spawnRecipeTimer += Time.deltaTime;

        if (_spawnRecipeTimer >= SPAWN_RECIPE_TIMER_MAX)
        {
            _spawnRecipeTimer = 0f;
            //check if game is playing and max amount of wating recipes then call RPC
            if (GameManager.Instance.IsGamePlaying() && _waitingRecipeSOList.Count < WAITING_RECIPES_MAX)
            {
                SpawnNewWaitingRecipeClientRpc(UnityEngine.Random.Range(0, _recipeListSO.RecipeSOList.Count));
                print("CALLED");
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int randIndex)
    {
        RecipeScriptableObject waitingRecipeSO = _recipeListSO.RecipeSOList[randIndex];
        _waitingRecipeSOList.Add(waitingRecipeSO);
        OnWaitingRecipeSOListChanged?.Invoke(this, new OnWaitingRecipeSOListChangedEventArgs { Added = true, ChangedRecipe = waitingRecipeSO });
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        List<KitchenObjectScriptableObject> plateKitchenObjectSOList = plateKitchenObject.GetCurrentKitchenObjectSOList();

        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            RecipeScriptableObject recipeSO = _waitingRecipeSOList[i];
            //if the lists have the same length, continue checking
            if (recipeSO.KitchenObjectSOList.Count == plateKitchenObjectSOList.Count)
            {
                bool allItemsMatch = true;
                foreach (KitchenObjectScriptableObject koso in recipeSO.KitchenObjectSOList)
                {
                    if (!plateKitchenObjectSOList.Contains(koso))
                    {
                        //if list doesnt include any of the KO's in the recipe, break out of the loop and set the bool to false
                        allItemsMatch = false;
                        break;
                    }
                }
                //found the recipe!
                if (allItemsMatch)
                {
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }
        //if execution reaches this part, plate doesnt match any of the recipes
        DeliverIncorrectRecipeServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }
    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        OnPlateDelivered?.Invoke(this, new OnPlateDeliveredEventArgs { Successful = false });
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int index)
    {

        DeliverCorrectRecipeClientRpc(index);
    }
    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int index)
    {
        RecipeScriptableObject recipeSO = _waitingRecipeSOList[index];
        _waitingRecipeSOList.Remove(recipeSO);
        OnWaitingRecipeSOListChanged?.Invoke(this, new OnWaitingRecipeSOListChangedEventArgs { Added = false, ChangedRecipe = recipeSO });
        OnPlateDelivered?.Invoke(this, new OnPlateDeliveredEventArgs { Successful = true });
    }
}
public class OnWaitingRecipeSOListChangedEventArgs : EventArgs
{
    public RecipeScriptableObject ChangedRecipe;
    public bool Added;
}
public class OnPlateDeliveredEventArgs : EventArgs
{
    public bool Successful;
}
