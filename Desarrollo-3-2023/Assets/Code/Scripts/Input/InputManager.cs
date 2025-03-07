using System;
using Code.Scripts.Abstracts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Input
{
    /// <summary>
    /// Controls the input
    /// </summary>
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public static event Action<Vector2> onMove;
        public static event Action onAttackPressed;
        public static event Action onAttackReleased;
        public static event Action onBlockPressed;
        public static event Action onBlockReleased;
        public static event Action onJump;
        public static event Action onGodMode;
        public static event Action onPause;

        /// <summary>
        /// Raises the move event channel with the given axis.
        /// </summary>
        /// <param name="input">The input value containing the axis.</param>
        private void OnMove(InputValue input)
        {
            Vector2 axis = input.Get<Vector2>();

            onMove?.Invoke(axis);
        }

        /// <summary>
        /// Raises the attack event channel with the given button state.
        /// </summary>
        /// <param name="input">The input value containing the button state.</param>
        /// <remarks>
        /// If the button is pressed, the onAttackPressed event channel is invoked.
        /// If the button is released, the onAttackReleased event channel is invoked.
        /// </remarks>
        private void OnAttack(InputValue input)
        {
            if (input.isPressed)
                onAttackPressed?.Invoke();
            else
                onAttackReleased?.Invoke();
        }

        /// <summary>
        /// Raises the block event channel with the given button state.
        /// </summary>
        /// <param name="input">The input value containing the button state.</param>
        /// <remarks>
        /// If the button is pressed, the onBlockPressed event channel is invoked.
        /// If the button is released, the onBlockReleased event channel is invoked.
        /// </remarks>
        private void OnBlock(InputValue input)
        {
            if (input.isPressed)
                onBlockPressed?.Invoke();
            else
                onBlockReleased?.Invoke();
        }

        /// <summary>
        /// Raises the jump event channel when the jump input is triggered.
        /// </summary>
        private void OnJump()
        {
            onJump?.Invoke();
        }

        /// <summary>
        /// Raises the pause event channel.
        /// </summary>
        private void OnPause()
        {
            onPause?.Invoke();
        }
    }
}