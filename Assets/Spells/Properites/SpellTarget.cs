using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellTarget", menuName = "ScriptableObjects/SpellProperties/SpellTarget")]
public class SpellTarget : ModifySpell
{
    public override void modifySpell(GameObject spell, string target)
    {
        spell.gameObject.GetComponentInChildren<Spell>().stats.target = target;
    }
}
