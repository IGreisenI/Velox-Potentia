using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellRange", menuName = "ScriptableObjects/SpellProperties/SpellRange")]
public class SpellRange : ModifySpell
{
    public override void modifySpell(GameObject spell, string range)
    {
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.range = Int32.Parse(range);
    }
}
