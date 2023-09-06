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
        private bool moving;
        private bool attacking;

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
            if (moving)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
            else if (attacking && !moving)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Attack));
            else
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        /// <summary>
        /// Handle to move transition
        /// </summary>
        /// <param name="input"></param>
        private void CheckMoving(float input)
        {
            moving = input != 0;
            movementState.dir = input;
        }

        /// <summary>
        /// Handle to attack transition
        /// </summary>
        private void CheckAttack()
        {
            attacking = true;
        }
    }
}