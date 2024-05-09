using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringInt
{
    public string str;
    public float num;
}

[CreateAssetMenu(fileName = "StringIntListSO", menuName = "ScriptableObjects/StringIntListSO")]
public class StringIntListSO : ScriptableObject
{
    public List<StringInt> stringInt;
}