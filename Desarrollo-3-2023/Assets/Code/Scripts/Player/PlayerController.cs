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
                
        private MovementState<PlayerStates> movementState;
        private IdleState<PlayerStates> idleState;
        
        private FiniteStateMachine<PlayerStates> fsm;
        private bool moving;

        private void Awake()
        {
            movementState = new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", speed, transform, rb);
            idleState = new IdleState<PlayerStates>(PlayerStates.Idle, "IdleState");
            
            fsm = new FiniteStateMachine<PlayerStates>();
            
            fsm.AddState(movementState);
            fsm.AddState(idleState);
            
            fsm.SetCurrentState(fsm.GetState(startState));
            
            fsm.Init();
        }

        private void OnEnable()
        {
            InputManager.onMove += CheckMoving;
        }

        private void OnDisable()
        {
            InputManager.onMove -= CheckMoving;
        }

        private void Update()
        {
            CheckPlayerState();
            
            fsm.Update();
        }

        private void CheckPlayerState()
        {
            if (moving)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
            else
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        private void CheckMoving(float input)
        {
            moving = input != 0;
            movementState.dir = input;
        }
    }
}