using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct stringColor
{
    [SerializeField]
    public string colorName;
    [ColorUsage(true, true)]
    [SerializeField]
    public Color color;
}

[CreateAssetMenu(fileName = "ColorList", menuName = "ScriptableObjects/ColorList")]
public class ColorListSO : ScriptableObject
{
    [SerializeField]
    public List<stringColor> colors;
}
