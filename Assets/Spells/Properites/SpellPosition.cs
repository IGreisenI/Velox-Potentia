using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellPosition", menuName = "ScriptableObjects/SpellProperties/SpellPosition")]
public class SpellPosition : ModifySpell
{
    public override void modifySpell(GameObject spell, string position)
    {
        // Motify position relative to target
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(position));

        //Rotate, save rotation of world quaterion
        spell.transform.GetChild(0).transform.localRotation = spellScript.stats.defensiveRotation;
        spellScript.stats.defensiveRotation = spell.transform.GetChild(0).transform.rotation;
    }
}
