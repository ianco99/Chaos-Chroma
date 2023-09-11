using System;
using UnityEngine;
using Patterns.FSM;

namespace Code.SOs.Enemy
{

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

        private FiniteStateMachine<EnemyStates> fsm;
        private PatrolState<EnemyStates> patrolState;

        private void Awake()
        {
            fsm = new FiniteStateMachine<EnemyStates>();

            patrolState = new PatrolState<EnemyStates>(rb, EnemyStates.Patrol, "PatrolState", transform, settings);
            fsm = new FiniteStateMachine<EnemyStates>();

            fsm.AddState(patrolState);

            fsm.SetCurrentState(fsm.GetState(startingState));

            fsm.Init();
        }

        private void Update()
        {
            fsm.Update();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        private void CheckFieldOfView()
        {
            Vector3 startDirection = transform.right;
        }
    }
}