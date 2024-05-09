using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour, IGameEventListener<ResetCastingInfo>
{
    private StateMachine _stateMachine;
    
    public GameObject player;
    public GameObject playerModel;
    public Unit stats;
    public Unit playerUnit;
    public int range;
    
    public SpellSystemEvents events;
    public EnemyDeathEvent enemyDeathEvent;
    public SpritesSO spritesSO;
    public List<string> choices;
    public int maxLayer = 2;
    public int times = 0;
    internal Vector3 targetCorrection = new Vector3(0,1);

    private void OnEnable()
    {
        events.resetCastingEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        events.resetCastingEvent.UnregisterListener(this);
    }

    private void Awake()
    {
        events.createEvents();

        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        var fleeParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        playerUnit = player.GetComponent<Unit>();
        
        _stateMachine = new StateMachine();

        var search = new Wander(this);
        var moveToSelected = new MoveToSelectedResource(this, navMeshAgent, animator);
        var attack = new AttackUnit(this, playerUnit);

        At(search, moveToSelected, HasTarget());
        At(moveToSelected, search, StuckForOverASecond());
        At(moveToSelected, attack, IsInRange());
        At(attack, moveToSelected, NotInRange());

        _stateMachine.SetState(search);
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        
        Func<bool> HasTarget() => () => player != null;
        Func<bool> StuckForOverASecond() => () => moveToSelected.TimeStuck > 10f;
        Func<bool> IsInRange() => () => this.InRange();
        Func<bool> NotInRange() => () => !this.InRange();
    }

    private void Start()
    {
        UnityEngine.Random.Range(0, 100);
    }

    private void FixedUpdate()
    {
        _stateMachine.Tick();
    }

    private bool InRange()
    {
        if (!player) return true;

        Vector3 towardsPlayer = (player.transform.position + new Vector3(0, 1, 0)) - transform.position;
        RaycastHit hit = new RaycastHit();

        Ray ray = new Ray(transform.position, towardsPlayer.normalized);

        bool playerhit = false;

        if(Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.gameObject == player)
            {
                playerhit = true;
            }
        }
        
        return Vector3.Distance(this.transform.position, player.transform.position) <= range && playerhit;
    }

    public void updateChoices(List<string> choice)
    {
        this.choices = choice;
    }

    public void setMaxSelectLayer(int max)
    {
        this.maxLayer = max;
    }

    public void OnEventRaised(ResetCastingInfo arg)
    {
        choices = new List<string> { "offensive" };
        this.times = 0;
    }

    private void OnDestroy()
    {
        enemyDeathEvent.Raise(new EnemyDeathInfo());
    }
}