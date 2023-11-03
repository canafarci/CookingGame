using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorLookup : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    public Color GetColorByIndex(int index)
    {
        return _colors[index];
    }

    public int GetColorsCount() => _colors.Length;

}
