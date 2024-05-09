using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IModifySpell
{
    void modifySpell(GameObject spell, string shape);
}

[Serializable]
public abstract class ModifySpell : ScriptableObject, IModifySpell
{
    public LayerSO choiceStats;

    public abstract void modifySpell(GameObject spell, string shape);
}