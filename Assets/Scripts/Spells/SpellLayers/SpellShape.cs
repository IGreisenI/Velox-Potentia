using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShape : MonoBehaviour, IModifySpell
{

    public void modifySpell(GameObject spell, string shape)
    {
        spell.gameObject.GetComponentInChildren<MeshFilter>().mesh = Resources.Load<Mesh>("Mesh/" + shape);
    }
}