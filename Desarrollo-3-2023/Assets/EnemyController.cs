using System;
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
    [SerializeField] EnemyStates startingState;
    [SerializeField] private Rigidbody2D rb;

    private FiniteStateMachine<EnemyStates> fsm;
    private PatrolState<EnemyStates> patrolState;

    private void Awake()
    {
        fsm = new FiniteStateMachine<EnemyStates>();

        patrolState = new PatrolState<EnemyStates>(rb, EnemyStates.Patrol, "PatrolState");
        fsm = new FiniteStateMachine<EnemyStates>();

        fsm.AddState(patrolState);

        patrolState.currentVelocity = Vector2.zero;

        fsm.SetCurrentState(fsm.GetState(startingState));

        fsm.Init();

    }

    private void Update()
    {
        fsm.Update();
    }
}
