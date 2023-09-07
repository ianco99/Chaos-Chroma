using System;
using UnityEngine.InputSystem;

namespace Code.Scripts.Input
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public static event Action<float> onMove;
        public static event Action onAttack;
        public static event Action onBlockPressed;
        public static event Action onBlockReleased;
        public static event Action onJump;

        private void OnMove(InputValue input)
        {
            var axis = input.Get<float>();

            onMove?.Invoke(axis);
        }

        private void OnAttack()
        {
            onAttack?.Invoke();
        }

        private void OnBlock(InputValue input)
        {
            if (input.isPressed)
                onBlockPressed?.Invoke();
            else
                onBlockReleased?.Invoke();
        }

        private void OnJump()
        {
            onJump?.Invoke();
        }
    }
}