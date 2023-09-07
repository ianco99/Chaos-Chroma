using System;
using UnityEngine;
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
            {
                print("Block pressed");
                onBlockPressed?.Invoke();
            }
            else
            {
                print("Block released");
                onBlockReleased?.Invoke();
            }
        }

        private void OnJump()
        {
            onJump?.Invoke();
        }
    }
}