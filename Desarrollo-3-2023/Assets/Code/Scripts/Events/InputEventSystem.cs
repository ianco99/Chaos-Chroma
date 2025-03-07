using Code.Scripts.Abstracts;
using Code.Scripts.Input;
using kuznickiEventChannel;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class InputEventSystem : MonoBehaviourSingleton<InputEventSystem>
    {
        [SerializeField] private Vector2EventChannel onMoveChannel;
        [SerializeField] private VoidEventChannel onAttackChannel;
        [SerializeField] private VoidEventChannel onBlockChannel;
        [SerializeField] private VoidEventChannel onJumpChannel;

        public Vector2EventChannel OnMoveChannel { get => onMoveChannel; private set => onMoveChannel = value; }
        public VoidEventChannel OnAttackChannel { get => onAttackChannel; private set => onAttackChannel = value; } 
        public VoidEventChannel OnBlockChannel { get => onBlockChannel; private set => onBlockChannel = value; }
        public VoidEventChannel OnJumpChannel { get => onJumpChannel; private set => onJumpChannel = value; }


        private void Start()
        {
            InputManager.onMove += MoveCharacters;
            InputManager.onAttackPressed += AttackPressedAttackers;
            InputManager.onBlockPressed += BlockBlockers;
            InputManager.onJump += JumpJumpers;
        }

        /// <summary>
        /// Raises the move event channel with the given axis.
        /// </summary>
        /// <param name="axis">The axis to pass to the move event channel.</param>
        private void MoveCharacters(Vector2 axis)
        {
            onMoveChannel?.RaiseEvent(axis);
        }

        /// <summary>
        /// Raises the attack event channel.
        /// </summary>
        private void AttackPressedAttackers()
        {
            onAttackChannel.RaiseEvent();
        }
    
        /// <summary>
        /// Raises the block event channel.
        /// </summary>
        private void BlockBlockers()
        {
            onBlockChannel.RaiseEvent();
        }

        /// <summary>
        /// Raises the jump event channel.
        /// </summary>
        private void JumpJumpers()
        {
            onJumpChannel.RaiseEvent();
        }
    }
}
