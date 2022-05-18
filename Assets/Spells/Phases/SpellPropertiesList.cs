using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellPropertiesList", menuName = "ScriptableObjects/SpellProperties/SpellPropertiesList")]
public class SpellPropertiesList : ScriptableObject
{
    public List<ModifySpell> spellPhasesList;
}
