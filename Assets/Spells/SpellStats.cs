using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellStats
{
    public string spellType;
    public string element;
    public string type;
    public string from;
    public string target;
    public string position;
    public string shape;
    public int duration;
    public int range;

    public int damage = 0;
    public int speed = 1;
}