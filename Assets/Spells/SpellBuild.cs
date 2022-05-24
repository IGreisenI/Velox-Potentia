using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>, IGameEventListener<MaxLayer>
{
    [SerializeField] private InputController _inputController = default;

    public GameObject baseSpellPrefab;
    public SpellSelectEvent spellSelectEvent;
    public SpellBuiltEvent spellBuiltEvent;
    public MaxLayerEvent maxLayerEvent;
    [Serializable]
    public class SpellSelectResponse : UnityEvent<List<string>> { }
    public SpellSelectResponse spellSelectResponse;

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
        maxLayerEvent.RegisterListener(this);
        _inputController.cancelSpellInputEvent += OnCancelSpell;
    }

    public void OnDisable()
    {
        spellSelectEvent.UnregisterListener(this);
        maxLayerEvent.UnregisterListener(this);
        _inputController.cancelSpellInputEvent -= OnCancelSpell;
    }

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        //if first layer selected, spawn spell prefab and get spellOrder
        if (arg.layer == 0)
        {
            this.spell = Instantiate(baseSpellPrefab, new Vector3(0, 0, 0), this.transform.rotation);
            this.spell.transform.parent = this.gameObject.transform;
            this.spell.transform.localPosition = new Vector3(0, 1.5f, 2f);
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
        }
        //if within layerlimit remember choice and invoke the appropriate layer event 
        else if (arg.layer <= spellOrder.spellPhasesList.Count)
        {
            this.choice = arg.buttonInfo.choice;
            spellOrder.spellPhasesList[arg.layer - 1].modifySpell(spell, this.choice);
        }
        //update UI
        if (arg.layer < spellOrder.spellPhasesList.Count)
        {
            spellSelectResponse?.Invoke(spellOrder.spellPhasesList[arg.layer].spellStats.strings);
        }
    }

    public void OnEventRaised(MaxLayer arg)
    {
        spellBuiltEvent.Raise(new SpellBuiltEventInfo(spell));
    }

    public void OnCancelSpell()
    {
        cancelSpell();
    }

    public void cancelSpell()
    {
        Destroy(spell);
    }
}