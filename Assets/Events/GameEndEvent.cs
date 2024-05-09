using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameEndInfo
{
    public string endScreenText;


    public GameEndInfo(string endScreenText) : this()
    {
        this.endScreenText = endScreenText;
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvents/GameEndEvent")]
public class GameEndEvent : GameEvent<GameEndInfo>
{

}
