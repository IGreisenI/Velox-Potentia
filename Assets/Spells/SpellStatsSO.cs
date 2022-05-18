using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellStats", menuName = "ScriptableObjects/SpellStats")]
public class SpellStatsSO : ScriptableObject
{
    public string element;
    public string type;
    public string from;
    public string target;
    public string position;
    public int duration;
    public int range;
    public int charges;

    public int damage;
    public int speed;
}
