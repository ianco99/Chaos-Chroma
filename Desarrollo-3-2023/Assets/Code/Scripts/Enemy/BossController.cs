using System;
using Code.Scripts.Abstracts.Character;
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
        
        // States
        private IdleState<string> idleState;

        private void Awake()
        {
            fsm = new FiniteStateMachine<string>();
            
            idleState = new IdleState<string>("Idle");
            
            fsm.AddState(idleState);
            
            fsm.Init();
            
            fsm.SetCurrentState(fsm.GetState(initialState));
        }
        
        private void Update()
        {
            
        }
    }
}
