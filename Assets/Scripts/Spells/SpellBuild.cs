using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Events
[Serializable]
public class ElementEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class ShapeEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class FromEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class RangeEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class PositionEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class TypeEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class DurationEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class TargetEvent : UnityEvent<GameObject, string> { }
[Serializable]
public class UpdateChoicesEvent : UnityEvent<string> { }
#endregion

public class SpellBuild : MonoBehaviour
{
    #region EventObjects
    public ElementEvent elementEvent;
    public ShapeEvent shapeEvent;
    public FromEvent fromEvent;
    public RangeEvent rangeEvent;
    public PositionEvent positionEvent;
    public TypeEvent typeEvent;
    public DurationEvent durationEvent;
    public TargetEvent targetEvent;
    public UpdateChoicesEvent updateChoicesEvent;
    #endregion

    public GameObject baseSpellPrefab;
    private GameObject spell;

    [SerializeField]
    public StringListSO offensiveSpellOrder = null;
    [SerializeField]
    public StringListSO defensiveSpellOrder;
    [SerializeField]
    public StringListSO utilitySpellOrder;
    [SerializeField]
    private StringListSO spellOrder;

    private string choice;

    public void onSelected(string choice, string shape, int layer)
    {
        //if first layer selected, spawn spell prefab and get spellOrder
        if (layer == 0)
        {
            spell = Instantiate(baseSpellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            spell.transform.parent = this.gameObject.transform;
            spell.transform.localPosition = new Vector3(0, 1.5f, 1.5f);
            spellOrder = (StringListSO)GetType().GetField(choice.ToLower() + "SpellOrder").GetValue(this);
        }
        //if withing layerlimit remember choice and invoke the appropriate layer event 
        else if (layer <= spellOrder.strings.Count)
        {
            this.choice = choice;
            Invoke(spellOrder.strings[layer - 1], 0f);
        }
        //update UI
        if (layer < spellOrder.strings.Count)
        {
            updateChoicesEvent.Invoke(spellOrder.strings[layer]);
        }
    }
    public void cancelSpell()
    {
        Destroy(spell);
    }

    #region EventFunctions
    public void Element() { elementEvent.Invoke(spell, choice); }
    public void Shape() { shapeEvent.Invoke(spell, choice); }
    public void From() { fromEvent.Invoke(spell, choice); }
    public void Range() { rangeEvent.Invoke(spell, choice); }
    public void Position() { positionEvent.Invoke(spell, choice); }
    public void Type() { typeEvent.Invoke(spell, choice); }
    public void Duration() { durationEvent.Invoke(spell, choice); }
    public void Target() { targetEvent.Invoke(spell, choice); }
    #endregion
}