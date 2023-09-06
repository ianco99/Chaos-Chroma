using System;
using Code.Scripts.Input;
using Code.Scripts.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Player
{
    [Serializable]
    public enum PlayerStates
    {
        Idle,
        Move,
        Jump,
        Attack,
        Block,
        Parry,
    }

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStates startState = PlayerStates.Idle;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject hit;
        
        // States
        private MovementState<PlayerStates> movementState;
        private IdleState<PlayerStates> idleState;
        private AttackState<PlayerStates> attackState;
        
        private FiniteStateMachine<PlayerStates> fsm;

        private void Awake()
        {
            movementState = new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", speed, transform, rb);
            idleState = new IdleState<PlayerStates>(PlayerStates.Idle, "IdleState");
            attackState = new AttackState<PlayerStates>(PlayerStates.Attack, "AttackState", hit);
            
            fsm = new FiniteStateMachine<PlayerStates>();
            
            fsm.AddState(movementState);
            fsm.AddState(idleState);
            fsm.AddState(attackState);
            
            fsm.SetCurrentState(fsm.GetState(startState));
            
            fsm.Init();
        }

        private void OnEnable()
        {
            InputManager.onMove += CheckMoving;
            InputManager.onAttack += CheckAttack;
        }

        private void OnDisable()
        {
            InputManager.onMove -= CheckMoving;
            InputManager.onAttack -= CheckAttack;
        }

        private void Update()
        {
            CheckPlayerState();
            
            fsm.Update();
        }

        /// <summary>
        /// Sets player's current state
        /// </summary>
        private void CheckPlayerState()
        {
            switch ( fsm.GetCurrentState().ID)
            {
                case PlayerStates.Idle:
                    IdleTransitions();
                    break;
                    
                case PlayerStates.Move:
                    MoveTransitions();
                    break;
                
                case PlayerStates.Jump:
                    break;
                
                case PlayerStates.Attack:
                    AttackTransitions();
                    break;
                
                case PlayerStates.Block:
                    break;
                
                case PlayerStates.Parry:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Handle to move transition
        /// </summary>
        /// <param name="input"></param>
        private void CheckMoving(float input)
        {
            if (input != 0)
                movementState.Enter();
            
            movementState.dir = input;
        }

        /// <summary>
        /// Handle to attack transition
        /// </summary>
        private void CheckAttack()
        {
            attackState.Enter();
        }
        
        /// <summary>
        /// Idle state transitions manager
        /// </summary>
        private void IdleTransitions()
        {
            if (movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
            if (attackState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Attack));
        }

        /// <summary>
        /// Move state transitions manager
        /// </summary>
        private void MoveTransitions()
        {
            if (!movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }
        
        /// <summary>
        /// Attack state transitions manager
        /// </summary>
        private void AttackTransitions()
        {
            if (!attackState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }
    }
}