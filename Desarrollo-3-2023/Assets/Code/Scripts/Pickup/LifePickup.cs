using Code.Scripts.Abstracts.Character;
using UnityEngine;

namespace Code.Scripts.Pickup
{
    /// <summary>
    /// Pickup for life
    /// </summary>
    public class LifePickup : BasePickup
    {
        [SerializeField] private float lifeBump = 100;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character character;

            collision.TryGetComponent(out character);

            if (character != null)
                Pickup(character);
        }

        /// <summary>
        /// Called when the character enters the trigger of the pickup.
        /// Applies the life bump to the character and destroys the pickup.
        /// </summary>
        /// <param name="character">The character that entered the trigger.</param>
        public override void Pickup(Character character)
        {
            character.lifePickup?.Invoke(lifeBump);

            Destroy(gameObject);
        }
    }
}
