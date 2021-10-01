using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringList", menuName = "ScriptableObjects/StringList")]
public class StringListSO : ScriptableObject
{
    public List<string> strings;
}
