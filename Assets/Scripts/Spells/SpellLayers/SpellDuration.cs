using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDuration : MonoBehaviour, IModifySpell
{
    public void modifySpell(GameObject spell, string duration)
    {
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.duration = Int32.Parse(duration);
    }
}
