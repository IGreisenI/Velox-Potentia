using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectList", menuName = "ScriptableObjects/ScriptableObjectList")]
public class ScriptableObjectList : ScriptableObject
{
    public List<ScriptableObject> list;
}
