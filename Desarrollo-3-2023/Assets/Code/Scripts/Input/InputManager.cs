using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Input
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public static event Action<Vector2> onMove;
        public static event Action onAttack;
        public static event Action onBlockPressed;
        public static event Action onBlockReleased;
        public static event Action onJump;
        public static event Action onGodMode;

        private void OnMove(InputValue input)
        {
            Vector2 axis = input.Get<Vector2>();

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
        
        private void OnGodMode()
        {
            onGodMode?.Invoke();
        }
    }
}