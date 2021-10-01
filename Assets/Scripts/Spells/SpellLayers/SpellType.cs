using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellType : MonoBehaviour, IModifySpell
{
    public void modifySpell(GameObject spell, string type)
    {
        spell.gameObject.GetComponentInChildren<SpellStats>().stats.type = type;
    }
}
