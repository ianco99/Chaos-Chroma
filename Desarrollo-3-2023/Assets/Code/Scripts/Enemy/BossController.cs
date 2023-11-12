using System;
using Code.Scripts.Abstracts.Character;
using Code.Scripts.Attack;
using Code.Scripts.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    public class BossController : Character
    {
        private FiniteStateMachine<string> fsm;

        [SerializeField] private string initialState;
        [SerializeField] private BossAreaManager areaManager;
        [SerializeField] private FirePunch leftPunch;
        [SerializeField] private FirePunch rightPunch;
        [SerializeField] private RetrievePunch leftRetrieve;
        [SerializeField] private RetrievePunch rightRetrieve;
        
        
        // States
        private IdleState<string> idleState;
        private PunchState<string> punchState;
        private RetrieveState<string> retrieveState; 

        private void Awake()
        {
            fsm = new FiniteStateMachine<string>();
            
            idleState = new IdleState<string>("Idle");
            punchState = new PunchState<string>("Punch", leftPunch, rightPunch);
            retrieveState = new RetrieveState<string>("Retrieve", leftRetrieve, rightRetrieve);
            
            fsm.AddState(idleState);
            fsm.AddState(punchState);
            fsm.AddState(retrieveState);
            
            fsm.AddTransition(idleState, punchState, areaManager.IsPlayerInArea);
            fsm.AddTransition(punchState, retrieveState, () => punchState.Ended);
            fsm.AddTransition(retrieveState, idleState, () => retrieveState.Ended);
            
            fsm.Init();
            
            fsm.SetCurrentState(fsm.GetState(initialState));
        }

        private void OnEnable()
        {
            punchState.onEnter += OnEnterPunchHandler;
        }

        private void OnDisable()
        {
            punchState.onEnter -= OnEnterPunchHandler;
        }

        private void Update()
        {
            fsm.Update();
            
            Debug.Log(fsm.GetCurrentState().ID);
        }

        /// <summary>
        /// Update target position on enter punch state
        /// </summary>
        private void OnEnterPunchHandler()
        {
            punchState.SetTargetDistance(areaManager.GetPositionDifference());
        }
    }
}
