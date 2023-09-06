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
        
        private FiniteStateMachine<PlayerStates> fsm;
        private bool moving;

        private void Awake()
        {
            movementState = new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", speed, transform, rb);
            fsm = new FiniteStateMachine<PlayerStates>();
            
            fsm.AddState(movementState);
            
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
            if (moving && fsm.GetCurrentState() != fsm.GetState(PlayerStates.Move))
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
        }

        private void CheckMoving(float input)
        {
            moving = input != 0;
        }
    }
}