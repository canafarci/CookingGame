using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectScriptableObject : ScriptableObject
{
    public Transform Prefab;
    public Sprite Sprite;
    public string ObjectName;
}
