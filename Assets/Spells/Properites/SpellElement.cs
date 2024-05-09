using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellElement", menuName = "ScriptableObjects/SpellProperties/SpellElement")]
public class SpellElement : ModifySpell
{ 
    public override void modifySpell(GameObject spell, string element)
    {
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(element));
    }
}
