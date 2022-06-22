using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellType", menuName = "ScriptableObjects/SpellProperties/SpellType")]
public class SpellType : ModifySpell
{
    public override void modifySpell(GameObject spell, string type)
    {
        spell.gameObject.GetComponentInChildren<Spell>().stats.type = type;
    }

}
