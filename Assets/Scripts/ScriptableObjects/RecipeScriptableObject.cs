using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class RecipeScriptableObject : ScriptableObject
{
    public List<KitchenObjectScriptableObject> KitchenObjectSOList;
    public string RecipeName;
}
