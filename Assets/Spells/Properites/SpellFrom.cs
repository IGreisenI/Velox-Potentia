using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellFrom", menuName = "ScriptableObjects/SpellProperties/SpellFrom")]
public class SpellFrom : ModifySpell
{
    public override void modifySpell(GameObject spell, string shape)
    {
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.from = shape;
    }
}
