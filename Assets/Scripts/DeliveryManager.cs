using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListScriptableObject _recipeListSO;
    private List<RecipeScriptableObject> _waitingRecipeSOList = new List<RecipeScriptableObject>();
    private float _spawnRecipeTimer;
    //constants
    private const float SPAWN_RECIPE_TIMER_MAX = 4f;
    private const int WAITING_RECIPES_MAX = 5;
    //Singleton reference
    public static DeliveryManager Instance { get; private set; }
    private void Awake()
    {
        //initialize singleton
        if (Instance)
            Destroy(gameObject);

        Instance = this;
    }
    private void Update()
    {
        _spawnRecipeTimer += Time.deltaTime;

        if (_spawnRecipeTimer >= SPAWN_RECIPE_TIMER_MAX)
        {
            _spawnRecipeTimer = 0f;
            SpawnRecipe();
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        List<KitchenObjectScriptableObject> plateKitchenObjectList = plateKitchenObject.GetCurrentKitchenObjectSOList();

        foreach (RecipeScriptableObject recipeSO in _waitingRecipeSOList)
        {
            //if the lists have the same length, continue checking
            if (recipeSO.KitchenObjectSOList.Count == plateKitchenObjectList.Count)
            {
                bool allItemsMatch = true;
                foreach (KitchenObjectScriptableObject koso in recipeSO.KitchenObjectSOList)
                {
                    if (!plateKitchenObjectList.Contains(koso))
                    {
                        //if list doesnt include any of the KO's in the recipe, break out of the loop and set the bool to false
                        allItemsMatch = false;
                        break;
                    }
                }

                //found the recipe!
                if (allItemsMatch)
                {
                    print("INGREDIENTS MATCH!");
                    _waitingRecipeSOList.Remove(recipeSO);
                    return;
                }
            }
        }
        //if execution reaches this part, plate doesnt match any of the recipes
        print("NOT FOUND THE RECIPE!");
    }
    private void SpawnRecipe()
    {
        if (_waitingRecipeSOList.Count < WAITING_RECIPES_MAX)
        {
            RecipeScriptableObject recipeSO = _recipeListSO.RecipeSOList[Random.Range(0, _recipeListSO.RecipeSOList.Count)];
            _waitingRecipeSOList.Add(recipeSO);
            print(recipeSO.RecipeName);
        }
    }
}
