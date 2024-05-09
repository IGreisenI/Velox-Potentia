
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Don't forget to subscribe to the event in OnEnable and OnDisable methods.
/// </summary>
public interface IGameEventListener<T>
{
    void OnEventRaised(T arg);
}
