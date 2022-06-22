using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpellBuiltEventInfo
{
    public List<GameObject> spellObjects;

    public SpellBuiltEventInfo(List<GameObject> spellObjects) : this()
    {
        this.spellObjects = spellObjects;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/SpellBuiltEvent")]
public class SpellBuiltEvent : GameEvent<SpellBuiltEventInfo>
{
}
