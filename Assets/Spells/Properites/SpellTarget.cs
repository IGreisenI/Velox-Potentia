using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellTarget", menuName = "ScriptableObjects/SpellProperties/SpellTarget")]
public class SpellTarget : ModifySpell
{
    public override void modifySpell(GameObject spell, string target)
    {
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(target));
    }
}
