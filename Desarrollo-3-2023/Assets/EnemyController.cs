using System;
using Code.Scripts.Enemy.States;
using UnityEngine;
using Patterns.FSM;

[Serializable]
public enum EnemyStates
{
    Patrol,
    Attack,
    Block,
    Parry,
    Fall
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyStates startingState;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private EnemySettings settings;


    [SerializeField] private float speed;
    
    private FiniteStateMachine<EnemyStates> fsm;
    private PatrolState<EnemyStates> patrolState;

    private void Awake()
    {
        fsm = new FiniteStateMachine<EnemyStates>();

        patrolState = new PatrolState<EnemyStates>(transform, settings, EnemyStates.Patrol, "PatrolState");
        patrolState = new PatrolState<EnemyStates>(rb, EnemyStates.Patrol, "PatrolState", speed, transform);
        fsm = new FiniteStateMachine<EnemyStates>();

        fsm.AddState(patrolState);

        fsm.SetCurrentState(fsm.GetState(startingState));

        fsm.Init();
    }

    private void Update()
    {
        fsm.Update();
    }
}
