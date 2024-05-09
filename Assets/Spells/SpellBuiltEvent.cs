using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpellBuiltEventInfo
{
    public List<Spell> spellObjects;

    public SpellBuiltEventInfo(List<Spell> spellObjects) : this()
    {
        this.spellObjects = spellObjects;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/SpellBuiltEvent")]
public class SpellBuiltEvent : GameEvent<SpellBuiltEventInfo>
{
}
