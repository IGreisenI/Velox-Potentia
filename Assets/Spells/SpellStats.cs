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

    public float range;
    public float damage = 0;
    public float speed = 1;
    public float manaCost = 0;
    public Vector3 scaleOnCast;
    public Quaternion defensiveRotation;

    public void addStats(SpellStats mod)
    {
        this.spellType = mod.spellType != "" ? mod.spellType : this.spellType;
        this.element = mod.element != "" ? mod.element : this.element;
        this.type = mod.type != "" ? mod.type : this.type;
        this.from = mod.from != "" ? mod.from : this.from;
        this.target = mod.target != "" ? mod.target : this.target;
        this.position = mod.position != "" ? mod.position : this.position;
        this.shape = mod.shape != "" ? mod.shape : this.shape;
        this.defensiveRotation = mod.defensiveRotation != new Quaternion(0, 0, 0, 0) ? mod.defensiveRotation : this.defensiveRotation;
        this.duration += mod.duration;
        this.range += mod.range;
        this.damage += mod.damage;
        this.speed += mod.speed;
        this.scaleOnCast += mod.scaleOnCast;
        this.manaCost += mod.manaCost;
    }
}