using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>
{
    public GameObject baseSpellPrefab;
    public SpellSelectEvent spellSelectEvent;
    [Serializable]
    public class SpellSelectResponse : UnityEvent<List<string>> { }
    public SpellSelectResponse spellSelectResponse;
    [Serializable]
    public class ReturnSelectMaxLayer : UnityEvent<int> { }
    public ReturnSelectMaxLayer returnMaxLayer;

    [SerializeField]
    public SpellPropertiesList offensiveSpellPhases;
    [SerializeField]
    public SpellPropertiesList defensiveSpellPhases;
    [SerializeField]
    public SpellPropertiesList utilitySpellPhases;

    private SpellPropertiesList spellOrder;

    private string choice;
    private GameObject spell;

    public void OnEnable()
    {
        spellSelectEvent.RegisterListener(this);
    }

    public void OnDisable()
    {
        spellSelectEvent.RegisterListener(this);
    }

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        //if first layer selected, spawn spell prefab and get spellOrder
        if (arg.layer == 0)
        {
            spell = Instantiate(baseSpellPrefab, new Vector3(0, 0, 0), this.transform.rotation);
            spell.transform.parent = this.gameObject.transform;
            spell.transform.localPosition = new Vector3(0, 1.5f, 2f);
            // Hardcode, fix after giving
            switch (arg.buttonInfo.choice) {
                case ("Utility"):
                    spellOrder = utilitySpellPhases;
                    break;
                case ("Offensive"):
                    spellOrder = offensiveSpellPhases;
                    break;
                case ("Defensive"):
                    spellOrder = defensiveSpellPhases;
                    break;
                default:
                    break;
            }
            returnMaxLayer.Invoke(spellOrder.spellPhasesList.Count);
        }
        //if withing layerlimit remember choice and invoke the appropriate layer event 
        else if (arg.layer <= spellOrder.spellPhasesList.Count)
        {
            this.choice = arg.buttonInfo.choice;
            spellOrder.spellPhasesList[arg.layer - 1].modifySpell(spell, choice);
        }
        //update UI
        if (arg.layer < spellOrder.spellPhasesList.Count)
        {
            spellSelectResponse?.Invoke(spellOrder.spellPhasesList[arg.layer].spellStats.strings);
        }
    }

    public void cancelSpell()
    {
        Destroy(spell);
    }
}