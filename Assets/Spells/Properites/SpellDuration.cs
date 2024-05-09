using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDuration", menuName = "ScriptableObjects/SpellProperties/SpellDuration")]
public class SpellDuration : ModifySpell
{
    public override void modifySpell(GameObject spell, string duration)
    {
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(duration));
    }
}