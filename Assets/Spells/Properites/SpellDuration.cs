using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDuration", menuName = "ScriptableObjects/SpellProperties/SpellDuration")]
public class SpellDuration : ModifySpell
{
    public override void modifySpell(GameObject spell, string duration)
    {
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.duration = Int32.Parse(duration);
    }
}