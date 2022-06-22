using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>, IGameEventListener<MaxLayer>, IGameEventListener<ResetCastingInfo>
{
    public GameObject baseSpellPrefab;
    public SpellSelectEvent spellSelectEvent;
    public SpellBuiltEvent spellBuiltEvent;
    public MaxLayerEvent maxLayerEvent;
    public ResetCastingEvent resetCastingEvent;

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
    private List<GameObject> spells = new List<GameObject>();

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

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        //if first layer selected, spawn spell prefab and get spellOrder
        if (arg.layer == 0)
        {
            this.spells = new List<GameObject>();
            this.spells.Add(Instantiate(baseSpellPrefab, new Vector3(0, 0, 0), this.transform.rotation));
            this.spells[0].transform.parent = this.gameObject.transform;
            this.spells[0].transform.localPosition = new Vector3(0, 1.5f, 2f);
            this.spells[0].AddComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
            this.spells[0].GetComponent<Spell>().stats.spellType = arg.buttonInfo.choice;
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
        }
        //if within layerlimit remember choice and invoke the appropriate layer event 
        else if (arg.layer <= spellOrder.spellPhasesList.Count)
        {
            this.choice = arg.buttonInfo.choice;

            // Move this to SpellFrom.cs
            if (this.choice.ToLower().Contains("multiple"))
            {
                spells = this.createMultipleSpells(spells[0], 4, spells[0].transform.position, 1);
            }

            foreach (GameObject spell in spells)
            {
                spellOrder.spellPhasesList[arg.layer - 1].modifySpell(spell, this.choice);
            }
        }
        //update UIss
        if (arg.layer < spellOrder.spellPhasesList.Count)
        {
            spellSelectResponse?.Invoke(spellOrder.spellPhasesList[arg.layer].spellStats.strings);
        }
    }

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

    public List<GameObject> createMultipleSpells(GameObject spell, int num, Vector3 point, float radius)
    {
        List<GameObject> spells = new List<GameObject>();
        for (int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            float radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            float vertrical = Mathf.Sin(radians);
            float horizontal = Mathf.Cos(radians);

            Vector3 spawnDir = new Vector3(vertrical, horizontal, 0);

            /* Get the spawn position */
            Vector3 spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            GameObject spellCopy = Instantiate(spell, spawnPos, Quaternion.identity);

            /* Adjust height */
            //spellCopy.transform.Translate(new Vector3(0, spell.transform.position.y / 2, 0));

            spellCopy.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();

            spells.Add(spellCopy);
        }

        Destroy(spell);
        return spells;
    }
}