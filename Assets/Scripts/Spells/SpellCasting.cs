using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public GameObject spell;
    public GameObject magicCircle;
    public SpellStatsSO spellStats;
    public Camera playerCamera;
    
    public void saveFinishedSpell(GameObject spell)
    {
        this.spell = spell;
        spellStats = spell.GetComponentInChildren<SpellStats>().stats;
    }

    public void leftClickHappened()
    {
        saveFinishedSpell(this.spell);
        if (!spellStats.from.ToLower().Contains("caster"))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject magicCircle1 = Instantiate(magicCircle, hit.point, Quaternion.identity);
            }
        }
    }
}
