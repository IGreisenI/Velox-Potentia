using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StringList", menuName = "ScriptableObjects/LayerSO")]
public class LayerSO : ScriptableObject
{
    [SerializeField]
    public List<LayerChoice> layerChoices;

    public List<string> choices()
    {
        return layerChoices.Select(l=>l.choice).ToList<string>();
    }

    public List<SpellStats> modifiers()
    {
        return layerChoices.Select(l => l.modifiers).ToList<SpellStats>();
    }

    public SpellStats getModifier(string choice)
    {
        return layerChoices.Find(l => l.choice == choice).modifiers;
    }
}

[System.Serializable]
public struct LayerChoice
{
    [SerializeField]
    public string choice;
    [SerializeField]
    public SpellStats modifiers;
}
