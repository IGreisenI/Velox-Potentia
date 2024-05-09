using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[Serializable]
public class Unit : MonoBehaviour, IManaProperty, IHealthProperty
{
    public float maxHealth;
    public float health;
    public float totalHealthRegen;
    public float maxMana;
    public float mana;
    public float totalManaRegen;

    public Bar healthbar;
    public Bar manabar;

    public GameEndEvent gameEndEvent;
    
    private float timeInterval = 0;
    private float basicManaRegen = 1;
    private float healthOnKill = 10;

    private void Start()
    {
        healthbar?.setMaxValue(maxHealth);
        manabar?.setMaxValue(maxMana);
        healthbar?.setPrecentage(health);
        manabar?.setPrecentage(mana);
    }

    private void Update()
    {
        // once per second
        timeInterval += Time.deltaTime;
        if (timeInterval >= 1)
        {
            timeInterval = 0;
            changeMana(basicManaRegen);
        }
    }

    #region HealthAndMana
    public void changeMana(float mana)
    {
        if (this.mana + mana > maxMana)
        {
            this.mana = maxMana;
        }
        else
        {
            this.mana += mana;
        }
        manabar?.setPrecentage(this.mana);
    }

    public IEnumerator changeManaOverTime(float mana, float everySeconds, float times)
    {
        totalManaRegen = mana * times;
        while (totalManaRegen > 0)
        {
            changeMana(mana);
            yield return new WaitForSeconds(everySeconds);
        }
    }

    public void setMana(float mana)
    {
        this.mana = mana;
    }

    public void changeHealth(float health)
    {
        if (this.health + health > maxHealth)
        {
            this.health = maxHealth;
        }
        else
        {
            this.health += health;
        }
        healthbar?.setPrecentage(this.health);

        if (this.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator changeHealthOverTime(float health, float everySeconds, float times)
    {
        totalHealthRegen = health * times;
        while(totalHealthRegen > 0)
        {
            changeHealth(health);
            totalHealthRegen -= health;
            yield return new WaitForSeconds(everySeconds);
        }
    }

    public void setHealth(float health)
    {
        this.health = health;
    }
    #endregion HealthAndMana

    public IEnumerator reduceSpeed(float reduceSpeedPercent)
    {
        float speed = GetComponent<StarterAssets.ThirdPersonController>().MoveSpeed;
        GetComponent<StarterAssets.ThirdPersonController>().MoveSpeed = speed * (reduceSpeedPercent / 100);
        yield return new WaitForSeconds(2);
        GetComponent<StarterAssets.ThirdPersonController>().MoveSpeed = speed;
    }

    public IEnumerator stopBasicManaRegen()
    {
        basicManaRegen = 0;
        yield return new WaitForSeconds(5);
        basicManaRegen = 1;
    }

    public IEnumerator stopKillHealthRegen()
    {
        healthOnKill = 0;
        yield return new WaitForSeconds(5);
        healthOnKill = 10;
    }

    public void killReward()
    {
        changeHealth(healthOnKill);
    }

    private void OnDestroy()
    {
        if (gameObject.tag == "Player")
        {
            gameEndEvent.Raise(new GameEndInfo("You died!!!"));
        }
    }
}
