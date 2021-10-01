using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellElement : MonoBehaviour, IModifySpell
{
    public void modifySpell(GameObject spell, string element)
    {
        spell.gameObject.GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Materials/material_" + element);
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.element = element;
    }
}
