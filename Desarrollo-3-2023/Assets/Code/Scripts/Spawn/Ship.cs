using System;
using Code.Scripts.SOs.Level;
using UnityEngine;

namespace Code.Scripts.Spawn
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private ShipVariants variant;

        private float speed;
        
        public event Action<Ship> Return;

        private void Update()
        {
            Move();
            CheckPosition();
        }

        /// <summary>
        /// Sets the variant of the ship and its position.
        /// </summary>
        /// <param name="variant">The variant to set the ship to.</param>
        public void SetVariant(ShipVariants variant)
        {
            this.variant = variant;
            
            spriteRenderer.sprite = variant.sprite;
            transform.localPosition = new Vector3(0f, variant.position.Get(), 0f);
            speed = variant.speed.Get();
        }

        /// <summary>
        /// Checks the current position of the ship and invokes the Return event
        /// if the ship's local x position is less than the variant's destroy position.
        /// </summary>
        private void CheckPosition()
        {
            if (transform.localPosition.x < ShipVariants.DestroyPos)
                Return?.Invoke(this);

            Vector3 vector3 = transform.localPosition;
            vector3.x = 0f;
            transform.localPosition = vector3;
        }

        /// <summary>
        /// Move the ship by its speed multiplied by the delta time in the negative right direction.
        /// </summary>
        private void Move()
        {
            transform.Translate(transform.right * (speed * Time.deltaTime));
        }
    }
}