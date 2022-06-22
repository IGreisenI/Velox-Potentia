using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellShape", menuName = "ScriptableObjects/SpellProperties/SpellShape")]
public class SpellShape : ModifySpell
{
    public override void modifySpell(GameObject spell, string shape)
    {
        spell.gameObject.GetComponentInChildren<MeshFilter>().mesh = Resources.Load<Mesh>("Mesh/" + shape);
        spell.gameObject.GetComponent<Spell>().stats.shape = shape;

    }
}