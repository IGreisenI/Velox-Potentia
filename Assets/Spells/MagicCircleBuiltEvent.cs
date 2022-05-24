using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MagicCircleBuiltInfo
{
    public GameObject magicCircleObject;

    public MagicCircleBuiltInfo(GameObject magicCircleObject) : this()
    {
        this.magicCircleObject = magicCircleObject;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/MagicCircleBuiltEvent")]
public class MagicCircleBuiltEvent : GameEvent<MagicCircleBuiltInfo>
{
}
