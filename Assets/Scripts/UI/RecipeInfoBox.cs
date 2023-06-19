using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeInfoBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private RectTransform _iconContainer;
    private RecipeScriptableObject _recipeSO;

    public void SetRecipeInfoBox(RecipeScriptableObject recipeSO)
    {
        _recipeNameText.text = recipeSO.RecipeName;
        _recipeSO = recipeSO;

        InstantiateIcons(recipeSO);
    }

    private void InstantiateIcons(RecipeScriptableObject recipeSO)
    {
        foreach (KitchenObjectScriptableObject koso in recipeSO.KitchenObjectSOList)
        {
            RectTransform rectTransform = new GameObject("Icon", typeof(RectTransform)).GetComponent<RectTransform>();
            rectTransform.SetParent(_iconContainer);
            rectTransform.sizeDelta = new Vector2(40, 40);
            rectTransform.gameObject.AddComponent<Image>().sprite = koso.Sprite;
        }
    }

    public RecipeScriptableObject GetRecipeSO() => _recipeSO;
}
