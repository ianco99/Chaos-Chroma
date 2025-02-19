using System;
using System.Collections.Generic;
using Code.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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

        private bool isHitting;

        public event Action OnParried;

        private void Start()
        {
            InputManager.onMove += SetDir;
        }

        private void OnDestroy()
        {
            InputManager.onMove -= SetDir;
        }
        
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
            isHitting = true;
            
            hits[(int)dir].gameObject.SetActive(true);

            hits[(int)dir].OnParried += OnParriedHandler;
        }

        private void OnDisable()
        {
            isHitting = false;
            
            hits[(int)dir].OnParried -= OnParriedHandler;

            foreach (HitController hit in hits)
                hit.Stop();
        }

        private void Update()
        {
            if (!hits[(int)dir].gameObject.activeSelf)
                gameObject.SetActive(false);
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
                hits[(int)dir].Stop();
            
            gameObject.SetActive(false);
        }

        private void SetDir(Vector2 input)
        {
            if (isHitting)
                return;
            
            float vertical = input.y;

            dir = vertical switch
            {
                > 0 => Dir.Up,
                < 0 => Dir.Down,
                _ => Dir.Side
            };
        }
    }
}