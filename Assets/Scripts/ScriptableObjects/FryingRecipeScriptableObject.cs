using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class FryingRecipeScriptableObject : ScriptableObject
{
    public KitchenObjectScriptableObject Input;
    public KitchenObjectScriptableObject Output;
    public float FryingTimerMax;
    public bool OutputIsBurnedObject;
}
