using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManaProperty
{
    void changeMana(float mana);

    IEnumerator changeManaOverTime(float mana, float everySeconds, float times);

    void setMana(float mana);
}