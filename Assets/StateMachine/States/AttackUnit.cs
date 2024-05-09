using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUnit : IState
{
    private readonly Enemy enemy;
    private readonly Unit unit;
    private float choiceTime = 1.0f;
    private float time = 0;

    public AttackUnit(Enemy enemy, Unit unit)
    {
        this.enemy = enemy;
        this.unit = unit;
    }

    public void Tick()
    {
        if (unit)
        {
            enemy.transform.LookAt(new Vector3(unit.transform.position.x, 1, unit.transform.position.z));
        }

        this.time += Time.deltaTime;

        if (this.time >= this.choiceTime)
        {
            this.Attack();
            this.time = 0.0f;
        }
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Attack()
    {
        if (enemy.times == 0 && enemy.choices[0] != "offensive")
        {
            return;
        }

        if (enemy.times < enemy.maxLayer)
        {
            System.Random rnd = new System.Random();
            int choice = rnd.Next(0, enemy.choices.Count - 1);
            if (enemy.choices.Contains("From caster"))
            {
                enemy.events.spellSelectEvent.Raise(new SpellSelectEventInfo(new SpellSelectButtonInfo("From caster", enemy.spritesSO.sprites[choice]), enemy.times));
            }
            else
            {
                enemy.events.spellSelectEvent.Raise(new SpellSelectEventInfo(new SpellSelectButtonInfo(enemy.choices[choice], enemy.spritesSO.sprites[choice]), enemy.times));
            }
            if (enemy.choices.Contains("projectile"))
            {
                if (enemy.choices[choice].Contains("projectile"))
                {
                    enemy.targetCorrection = new Vector3(0, 1);
                }
                else
                {
                    enemy.targetCorrection = new Vector3(0, 0);
                }
            }
            enemy.times += 1;
        }
        else if (enemy.times == enemy.maxLayer)
        {
            enemy.events.maxLayerEvent.Raise(new MaxLayer());
            enemy.times += 1;
        }
        else if (enemy.times == enemy.maxLayer + 1)
        {
            if (enemy.player != null)
            {
                enemy.events.spellCastEvent.Raise(new SpellCastInfo(enemy.player.transform.position + enemy.targetCorrection));
            }
        }
    }
}
