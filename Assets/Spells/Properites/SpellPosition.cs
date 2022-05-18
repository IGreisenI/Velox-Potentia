using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellPosition", menuName = "ScriptableObjects/SpellProperties/SpellPosition")]
public class SpellPosition : ModifySpell
{
    public override void modifySpell(GameObject spell, string position)
    {
        // Motify position relative to target
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.position = position;
    }
}
