using System.Linq;
using UnityEngine;

public class Wander : IState
{
    private readonly Enemy _gatherer;

    public Wander(Enemy gatherer)
    {
        _gatherer = gatherer;
    }
    public void Tick()
    {
        _gatherer.playerUnit = ChooseOneOfTheNearestResources(5);
    }

    private Unit ChooseOneOfTheNearestResources(int pickFromNearest)
    {
        return Object.FindObjectsOfType<Unit>()
            .OrderBy(t => Vector3.Distance(_gatherer.transform.position, t.transform.position))
            .Where(u => u != null)
            .Take(pickFromNearest)
            .OrderBy(t => Random.Range(0, int.MaxValue))
            .FirstOrDefault();
    }

    public void OnEnter() { }
    public void OnExit() { }
}
