using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellRange", menuName = "ScriptableObjects/SpellProperties/SpellRange")]
public class SpellRange : ModifySpell
{
    public override void modifySpell(GameObject spell, string range)
    {
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(range));
    }
}
