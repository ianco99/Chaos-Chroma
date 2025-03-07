using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Input
{
    /// <summary>
    /// Switches the sprite based on the current input device
    /// </summary>
    public class InputTutorialSwitcher : MonoBehaviour
    {
        [SerializeField] private Sprite controllerSprite;
        [SerializeField] private Sprite keyboardMouseSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnInputDeviceChange;
            UpdateSprite();
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnInputDeviceChange;
        }
        
        /// <summary>
        /// Handler for when a device is added or removed
        /// </summary>
        /// <param name="device"></param>
        /// <param name="change"></param>
        private void OnInputDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change is InputDeviceChange.Added or InputDeviceChange.Removed)
            {
                UpdateSprite();
            }
        }

        /// <summary>
        /// Switches the sprite based on the current input device
        /// </summary>
        private void UpdateSprite()
        {
            InputDevice[] devices = InputSystem.devices.ToArray();

            if (devices.Any(device => device is Gamepad))
            {
                spriteRenderer.sprite = controllerSprite;
                return;
            }

            spriteRenderer.sprite = keyboardMouseSprite;
        }
    }
}