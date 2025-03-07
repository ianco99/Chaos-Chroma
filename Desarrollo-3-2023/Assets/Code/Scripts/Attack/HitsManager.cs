using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Controls the randomizer for attacks. (add different types of attacks to the list)
    /// </summary>
    public class HitsManager : MonoBehaviour
    {
        private enum Dir
        {
            Side,
            Down,
            Up
        }

        public int DirAsInt => (int)dir;

        [SerializeField] private List<GameObject> hitObjects;

        private readonly List<HitController> hits = new();
        private Dir dir;

        public event Action OnParried;

        private void Awake()
        {
            foreach (GameObject hitObject in hitObjects)
            {
                if (!hitObject.TryGetComponent(out HitController hit)) continue;

                hits.Add(hit);
                hitObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            hits[(int)dir].gameObject.SetActive(true);

            hits[(int)dir].OnParried += OnParriedHandler;
        }

        private void OnDisable()
        {
            hits[(int)dir].OnParried -= OnParriedHandler;

            foreach (HitController hit in hits)
                hit.Stop();
        }

        private void Update()
        {
            if (!hits[(int)dir].gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Callback for parry
        /// </summary>
        private void OnParriedHandler()
        {
            OnParried?.Invoke();
        }

        /// <summary>
        /// Interrupt current attack
        /// </summary>
        public void Stop()
        {
            if (hits[(int)dir].enabled)
            {
                Debug.Log("Stopped");
                hits[(int)dir].Stop();
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the direction of the hits manager based on the given direction.
        /// 
        /// The direction is determined by the vertical component of the given direction.
        /// If it is positive, the direction is set to <see cref="Dir.Up"/>, if it is negative,
        /// the direction is set to <see cref="Dir.Down"/>, and otherwise it is set to
        /// <see cref="Dir.Side"/>.
        /// </summary>
        /// <param name="direction">The direction to set the hits manager to.</param>
        public void SetDir(Vector2 direction)
        {
            float vertical = direction.y;

            dir = vertical switch
            {
                > 0 => Dir.Up,
                < 0 => Dir.Down,
                _ => Dir.Side
            };
        }

        /// <summary>
        /// Gets the direction of the hits manager as an integer.
        ///
        /// The value returned is one of 0, 1, or 2, corresponding to
        /// <see cref="Dir.Side"/>, <see cref="Dir.Up"/>, or
        /// <see cref="Dir.Down"/>, respectively.
        /// </summary>
        /// <returns>The direction of the hits manager as an integer.</returns>
        public int GetDir()
        {
            return DirAsInt;
        }
    }
}