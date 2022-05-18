using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpellSelectEventInfo
{
    public SpellSelectButtonInfo buttonInfo;
    public int layer;

    public SpellSelectEventInfo(SpellSelectButtonInfo buttonInfo, int layer) : this()
    {
        this.buttonInfo = buttonInfo;
        this.layer = layer;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/SpellSelectEvent")]
public class SpellSelectEvent: GameEvent<SpellSelectEventInfo>
{
}
