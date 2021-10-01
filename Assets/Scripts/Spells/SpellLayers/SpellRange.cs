using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRange : MonoBehaviour, IModifySpell
{
    public void modifySpell(GameObject spell, string range)
    {
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.range = Int32.Parse(range);
    }
}
