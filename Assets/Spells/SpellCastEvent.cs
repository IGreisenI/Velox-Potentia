using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpellCastInfo
{
    public Vector3 at;

    public SpellCastInfo(Vector3 at) : this()
    {
        this.at = at;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/SpellCastEvent")]
public class SpellCastEvent : GameEvent<SpellCastInfo>
{
    
}
