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
        
        private static UnityEngine.Camera _mainCam;

        private static int _id;
        
        public int ID { get; private set; }

        private void Start()
        {
            if (!_mainCam)
                _mainCam = UnityEngine.Camera.main;
        }

        public void Initialize()
        {
            ID = _id;

            _id++;
            
            gameObject.name = "Ship " + ID;
        }

        private void Update()
        {
            Move();
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
        public bool ShouldReturn()
        {
            float screenPosX = _mainCam.WorldToScreenPoint(transform.position).x;

            return screenPosX > 2f * Screen.width;
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