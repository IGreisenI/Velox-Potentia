using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>, IGameEventListener<MaxLayer>, IGameEventListener<ResetCastingInfo>
{
    #region UnityEvents
    [Serializable]
    public class SpellSelectResponse : UnityEvent<List<string>> { }
    [Serializable]
    public class SetLayer : UnityEvent<int> { }
    #endregion

    public Spell baseSpellPrefab;
    public SpellSelectEvent spellSelectEvent;
    public SpellBuiltEvent spellBuiltEvent;
    public MaxLayerEvent maxLayerEvent;
    public ResetCastingEvent resetCastingEvent;

    public SpellSelectResponse spellSelectResponse;
    public SetLayer setLayer;
    public List<Spell> spells = new();

    [SerializeField]
    private SpellPropertiesList offensiveSpellPhases;
    [SerializeField]
    private SpellPropertiesList defensiveSpellPhases;
    [SerializeField]
    private SpellPropertiesList utilitySpellPhases;

    private SpellPropertiesList spellOrder;
    private string choice;
    private Unit _casterUnit;

    #region CachedVariables
    private Vector3 _spellPositionOffset = new Vector3(0, 1.5f, 2f);
    private Spell _tempSpell;
    #endregion

    public void OnEnable()
    {
        spellSelectEvent.RegisterListener(this);
        maxLayerEvent.RegisterListener(this);
        resetCastingEvent.RegisterListener(this);
    }

    public void OnDisable()
    {
        spellSelectEvent.UnregisterListener(this);
        maxLayerEvent.UnregisterListener(this);
        resetCastingEvent.UnregisterListener(this);
    }

    public void Start()
    {
        _casterUnit = this.GetComponent<Unit>();
    }

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        if (arg.layer == 0)
        {
            this.spells.Clear();

            _tempSpell = Instantiate(baseSpellPrefab, Vector3.zero, this.transform.rotation);
            _tempSpell.Initialize(transform, _spellPositionOffset, arg.buttonInfo.choice, _casterUnit);
            this.spells.Add(_tempSpell);

            // Hardcode, fix after giving
            switch (arg.buttonInfo.choice.ToLower()) {
                case ("utility"):
                    spellOrder = utilitySpellPhases;
                    break;
                case ("offensive"):
                    spellOrder = offensiveSpellPhases;
                    break;
                case ("defensive"):
                    spellOrder = defensiveSpellPhases;
                    break;
                default:
                    break;
            }
            setLayer.Invoke(spellOrder.spellPhasesList.Count + 1);
        }
        //if within layerlimit remember choice and invoke the appropriate layer event 
        else if (arg.layer <= spellOrder.spellPhasesList.Count)
        {
            this.choice = arg.buttonInfo.choice;

            // Move this to SpellFrom.cs
            if (IsChoiceMultiple() && IsMultipleFirstTime())
            {
                spells = this.createMultipleSpells(spells[0], 4, spells[0].transform.position, 1);
            }

            foreach (Spell spell in spells)
            {
                spellOrder.spellPhasesList[arg.layer - 1].modifySpell(spell.gameObject, this.choice);
            }
        }
        //update UI
        if (arg.layer < spellOrder.spellPhasesList.Count)
        {
            spellSelectResponse?.Invoke(spellOrder.spellPhasesList[arg.layer].choiceStats.choices());
        }
    }

    #region BooleanChecks
    private bool IsMultipleFirstTime()
    {
        return spells.Count == 1;
    }

    private bool IsChoiceMultiple()
    {
        return this.choice.ToLower().Contains("multiple");
    }
    #endregion

    public void OnEventRaised(MaxLayer arg)
    {
        spellBuiltEvent.Raise(new SpellBuiltEventInfo(spells));
    }

    public void OnEventRaised(ResetCastingInfo arg)
    {
        cancelSpell();
    }

    public void cancelSpell()
    {
        spells.ForEach(spell => Destroy(spell));
    }

    public List<Spell> createMultipleSpells(Spell spell, int num, Vector3 point, float radius)
    {

        List<Spell> spells = new List<Spell>();

        for (int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            float radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            float x = Mathf.Sin(radians);
            float y = Mathf.Cos(radians);

            Vector3 spawnDir = new Vector3(x, y, 0);
            
            /* Get the spawn position */
            Vector3 spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            Spell spellCopy = Instantiate(spell, point, this.transform.rotation);
            spellCopy.transform.localPosition += spawnDir;

            spells.Add(spellCopy);
        }

        Destroy(spell);
        return spells;
    }
}