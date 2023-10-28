using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Controls the randomizer for attacks. (add different types of attacks to the list)
    /// </summary>
    public class HitsManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> hitObjects;
        
        private readonly List<HitController> hits = new();
        private int index;

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
            index = Random.Range(0, hits.Count);

            hits[index].gameObject.SetActive(true);

            hits[index].OnParried += OnParriedHandler;
        }

        private void OnDisable()
        {
            hits[index].OnParried -= OnParriedHandler;
        }

        private void Update()
        {
            if (!hits[index].gameObject.activeSelf)
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
            if (hits[index].enabled)
                hits[index].Stop();
            
            gameObject.SetActive(false);
        }
    }
}