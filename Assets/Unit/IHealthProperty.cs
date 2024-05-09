using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthProperty
{
    void changeHealth(float health);

    IEnumerator changeHealthOverTime(float health, float everySeconds, float times);

    void setHealth(float health);
}
