using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IModifySpell
{
    void modifySpell(GameObject spell, string shape);
}
