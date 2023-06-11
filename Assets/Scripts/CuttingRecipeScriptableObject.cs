using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class CuttingRecipeScriptableObject : ScriptableObject
{
    public KitchenObjectScriptableObject Input;
    public KitchenObjectScriptableObject Output;
}
