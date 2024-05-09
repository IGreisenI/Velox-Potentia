using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

public class SpellCasting : MonoBehaviour, IGameEventListener<SpellBuiltEventInfo>, IGameEventListener<MagicCircleBuiltInfo>, IGameEventListener<ResetCastingInfo>, IGameEventListener<SpellCastInfo>
{
    public SpellBuiltEvent spellBuiltEvent;
    public SpellCastEvent spellCastEvent;
    public MagicCircleBuiltEvent magicCircleBuiltEvent;
    public ResetCastingEvent resetCastingEvent;
    
    public int magicCircleScale = 3;
    public List<Spell> spellObjects;
    public Camera playerCamera;

    [Serializable]
    public class ResetCasting : UnityEvent<int> { }
    public ResetCasting spellSelectResponse;

    private GameObject magicCircle;
    private Spell spell;

    GameObject magicCircleFrom;

    private void OnEnable()
    {
        spellCastEvent.RegisterListener(this);
        spellBuiltEvent.RegisterListener(this);
        magicCircleBuiltEvent.RegisterListener(this);
        resetCastingEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        spellCastEvent.UnregisterListener(this);
        spellBuiltEvent.UnregisterListener(this);
        magicCircleBuiltEvent.UnregisterListener(this);
        resetCastingEvent.UnregisterListener(this);
    }

    public void FixedUpdate()
    {
        if (spellObjects.Count != 0 && this.magicCircleFrom)
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                this.magicCircleFrom.transform.LookAt(hit.point, Vector3.up);
            }
        }
    }

    public void OnCast(Ray ray)
    {
        // If spell/magic circle is not built don't execute past this point
        if (spellObjects.Count == 0 || !magicCircle)
            return;

        SpellsMagicCircle info = SpellFrom.castSpellFrom(this.spellObjects, this.spell, magicCircle, ray, this.magicCircleFrom);
        this.spellObjects = info.spells;
        this.magicCircleFrom = info.magicCircle;

        Spell castedSpell = this.spellObjects.Find(spl => spl.casted);
        if(castedSpell != null)
        {
            gameObject.GetComponent<Unit>().changeMana(-castedSpell.GetComponent<Spell>().stats.manaCost);
            this.spellObjects.Remove(castedSpell);
        }

        if(this.spellObjects.Count == 0)
        {
            // Don't make this loop by putting it into a function that is triggered by the event
            resetCastingEvent.Raise(new ResetCastingInfo());
        }
    }

    public void OnEventRaised(SpellBuiltEventInfo args)
    {
        this.spellObjects = args.spellObjects;
        // We take that all spells are the same
        this.spell = this.spellObjects[0].GetComponent<Spell>();
        this.magicCircleParent();
    }

    public void OnEventRaised(MagicCircleBuiltInfo args)
    {
        this.magicCircle = args.magicCircleObject;
        this.magicCircleParent();
    }

    public void OnEventRaised(ResetCastingInfo arg)
    {
        resetCasting();
    }

    public void resetCasting()
    {
        this.spell = null;
        this.spellObjects.ForEach(spell => Destroy(spell));
        this.spellObjects.Clear();

        // MagicCircleCleanup
        Destroy(magicCircle);
        Destroy(magicCircleFrom, 15);
        this.magicCircle = null;
        this.magicCircleFrom = null;
    }

    public void magicCircleParent()
    {
        if (spellObjects.Count == 0 || !magicCircle)
            return;

        foreach (Spell spell in this.spellObjects)
        {
            spell.transform.parent = this.magicCircle.transform;
        }
    }

    public void OnEventRaised(SpellCastInfo arg)
    {
        Ray ray;
        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            ray = playerCamera.ScreenPointToRay(arg.at);
        }
        else
        {
            Vector3 direction = arg.at - this.transform.position;

            ray = new Ray(this.transform.position, direction.normalized);
        }

        OnCast(ray);
    }
}