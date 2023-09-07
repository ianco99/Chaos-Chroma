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
        [SerializeField] private Damageable damageable;
        [SerializeField] private float parryDuration = 1f;
        
        // States
        private MovementState<PlayerStates> movementState;
        private IdleState<PlayerStates> idleState;
        private AttackState<PlayerStates> attackState;
        private ParryState<PlayerStates> parryState;
        private BlockState<PlayerStates> blockState;
        
        private FiniteStateMachine<PlayerStates> fsm;

        private void Awake()
        {
            movementState = new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", speed, transform, rb);
            idleState = new IdleState<PlayerStates>(PlayerStates.Idle, "IdleState");
            attackState = new AttackState<PlayerStates>(PlayerStates.Attack, "AttackState", hit);
            parryState = new ParryState<PlayerStates>(PlayerStates.Parry, "ParryState", damageable, parryDuration);
            blockState = new BlockState<PlayerStates>(PlayerStates.Block, "BlockState", damageable);
            
            fsm = new FiniteStateMachine<PlayerStates>();
            
            fsm.AddState(movementState);
            fsm.AddState(idleState);
            fsm.AddState(attackState);
            fsm.AddState(parryState);
            fsm.AddState(blockState);
            
            fsm.SetCurrentState(fsm.GetState(startState));
            
            fsm.Init();
        }

        private void OnEnable()
        {
            InputManager.onMove += CheckMoving;
            InputManager.onAttack += CheckAttack;
            InputManager.onBlockPressed += CheckParry;
            InputManager.onBlockReleased += CheckBlock;
        }

        private void OnDisable()
        {
            InputManager.onMove -= CheckMoving;
            InputManager.onAttack -= CheckAttack;
            InputManager.onBlockPressed -= CheckParry;
            InputManager.onBlockReleased -= CheckBlock;
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
                    BlockTransitions();
                    break;
                
                case PlayerStates.Parry:
                    ParryTransitions();
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
        /// Handle to parry transition
        /// </summary>
        private void CheckParry()
        {
            parryState.Enter();
            blockState.Enter();
        }
        
        /// <summary>
        /// Handle to block transition
        /// </summary>
        private void CheckBlock()
        {
            blockState.Exit();
        }

        #region Transitions

        /// <summary>
        /// Idle state transitions manager
        /// </summary>
        private void IdleTransitions()
        {
            if (movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
            else if (attackState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Attack));
            else if (parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Parry));
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

        /// <summary>
        /// Parry state transitions manager
        /// </summary>
        private void ParryTransitions()
        {
            if (!parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Block));
        }

        /// <summary>
        /// Block state transitions manager
        /// </summary>
        private void BlockTransitions()
        {
            if (!blockState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }
        
        #endregion
    }
}