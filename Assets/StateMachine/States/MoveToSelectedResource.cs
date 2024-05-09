using UnityEngine;
using UnityEngine.AI;

internal class MoveToSelectedResource : IState
{
    private readonly Enemy enemy;
    private readonly NavMeshAgent navMeshAgent;
    private readonly Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private Vector3 _lastPosition = Vector3.zero;

    public float TimeStuck;

    public MoveToSelectedResource(Enemy enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this.enemy = enemy;
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
    }

    public void Tick()
    {
        navMeshAgent.SetDestination(enemy.player.gameObject.transform.position);
        if (Vector3.Distance(enemy.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = enemy.transform.position;
    }

    public void OnEnter()
    {
        TimeStuck = 0f;
        navMeshAgent.enabled = true;
    }

    public void OnExit()
    {
        navMeshAgent.enabled = false;
    }
}