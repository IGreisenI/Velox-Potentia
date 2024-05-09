using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSystemEvents : MonoBehaviour
{
    public MagicCircleBuild magicCircleBuild;
    public SpellBuild spellBuild;
    public SpellCasting spellCasting;

    public SpellSelectEvent spellSelectEvent;
    public SpellBuiltEvent spellBuiltEvent;
    public SpellCastEvent spellCastEvent;
    public MagicCircleBuiltEvent magicCircleBuiltEvent;
    public MaxLayerEvent maxLayerEvent;
    public ResetCastingEvent resetCastingEvent;

    public void createEvents()
    {
        resetCastingEvent = ScriptableObject.CreateInstance("ResetCastingEvent") as ResetCastingEvent;
        spellSelectEvent = ScriptableObject.CreateInstance("SpellSelectEvent") as SpellSelectEvent;
        spellBuiltEvent = ScriptableObject.CreateInstance("SpellBuiltEvent") as SpellBuiltEvent;
        spellCastEvent = ScriptableObject.CreateInstance("SpellCastEvent") as SpellCastEvent;
        magicCircleBuiltEvent = ScriptableObject.CreateInstance("MagicCircleBuiltEvent") as MagicCircleBuiltEvent;
        maxLayerEvent = ScriptableObject.CreateInstance("MaxLayerEvent") as MaxLayerEvent;

        magicCircleBuild.spellSelectEvent = spellSelectEvent;
        spellBuild.spellSelectEvent = spellSelectEvent;

        spellBuild.spellBuiltEvent = spellBuiltEvent;
        spellCasting.spellBuiltEvent = spellBuiltEvent;
        
        spellCasting.spellCastEvent = spellCastEvent;

        magicCircleBuild.magicCircleBuiltEvent = magicCircleBuiltEvent;
        spellCasting.magicCircleBuiltEvent = magicCircleBuiltEvent;

        magicCircleBuild.maxLayerEvent = maxLayerEvent;
        spellBuild.maxLayerEvent = maxLayerEvent;

        magicCircleBuild.resetCastingEvent = resetCastingEvent;
        spellBuild.resetCastingEvent = resetCastingEvent;
        spellCasting.resetCastingEvent = resetCastingEvent;

        magicCircleBuild.enabled = true;
        spellBuild.enabled = true;
        spellCasting.enabled = true;
    }
}
