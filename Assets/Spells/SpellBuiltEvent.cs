using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpellBuiltEventInfo
{
    public GameObject spellObject;

    public SpellBuiltEventInfo(GameObject spellObject) : this()
    {
        this.spellObject = spellObject;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/SpellBuiltEvent")]
public class SpellBuiltEvent : GameEvent<SpellBuiltEventInfo>
{
}
