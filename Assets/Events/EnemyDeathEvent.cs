using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyDeathInfo
{

}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/EnemyDeathEvent")]
public class EnemyDeathEvent : GameEvent<EnemyDeathInfo>
{

}
