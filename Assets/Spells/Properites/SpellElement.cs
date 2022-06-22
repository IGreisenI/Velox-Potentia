using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellElement", menuName = "ScriptableObjects/SpellProperties/SpellElement")]
public class SpellElement : ModifySpell
{ 
    public override void modifySpell(GameObject spell, string element)
    {
        spell.GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Materials/material_" + element);
        spell.GetComponentInChildren<Spell>().stats.element = element;
    }
}
