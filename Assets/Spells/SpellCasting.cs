using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

public class SpellCasting : MonoBehaviour, IGameEventListener<SpellBuiltEventInfo>, IGameEventListener<MagicCircleBuiltInfo>, IGameEventListener<ResetCastingInfo>
{
    [SerializeField] private InputController _inputController = default;

    public SpellBuiltEvent spellBuiltEvent;
    public MagicCircleBuiltEvent magicCircleBuiltEvent;
    public ResetCastingEvent resetCastingEvent;

    public GameObject player;
    public Camera playerCamera;
    public int magicCircleScale = 3;
    public List<GameObject> spellObjects;
    
    [Serializable]
    public class ResetCasting : UnityEvent<int> { }
    public ResetCasting spellSelectResponse;

    private bool spellBuilt;
    private GameObject magicCircle;
    private Spell spell;

    GameObject magicCircleFrom;

    private void OnEnable()
    {
        _inputController.castSpellInputEvent += OnCast;
        spellBuiltEvent.RegisterListener(this);
        magicCircleBuiltEvent.RegisterListener(this);
        resetCastingEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        _inputController.castSpellInputEvent -= OnCast;
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

    public void OnCast()
    {
        // If spell/magic circle is not built don't execute past this point
        if (spellObjects.Count == 0 || !magicCircle)
            return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        SpellsMagicCircle info = SpellFrom.castSpellFrom(this.spellObjects, spell, magicCircle, ray, this.magicCircleFrom);
        this.spellObjects = info.spells;
        this.magicCircleFrom = info.magicCircle;

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

    public void magicCircleParent()
    {
        if (spellObjects.Count == 0 || !magicCircle)
            return;

        foreach (GameObject spell in this.spellObjects)
        {
            spell.GetComponent<FixedJoint>().connectedBody = this.magicCircle.GetComponent<Rigidbody>();
            spell.transform.parent = this.magicCircle.transform;
        }
    }

    public void resetCasting()
    {
        this.spell = null;
        this.spellObjects.ForEach(spell => Destroy(spell));
        this.spellObjects = new List<GameObject>();

        // MagicCircleCleanup
        Destroy(magicCircle);
        Destroy(magicCircleFrom, 15);
        this.magicCircle = null;
        this.magicCircleFrom = null;
    }
}