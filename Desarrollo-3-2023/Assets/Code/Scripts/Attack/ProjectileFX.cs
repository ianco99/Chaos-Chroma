using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Scripts.Attack
{
    public class ProjectileFX : MonoBehaviour
    {
        private static GameObject _hitPSHolder;

        [SerializeField] private ParticleSystem shootPSPrefab;
        [SerializeField] private ParticleSystem hitPSPrefab;

        [SerializeField] private Transform shootPos;

        private ObjectPool<ParticleSystem> shootPSPool;
        private static ObjectPool<ParticleSystem> _hitPSPool;

        private readonly List<ParticleSystem> activeShootPS = new();
        private static readonly List<ParticleSystem> ActiveHitPS = new();

        private void Awake()
        {
            shootPSPool = new ObjectPool<ParticleSystem>(CreateShootPS, OnTakeShootPS, OnReturnedShootPS);
            _hitPSPool = new ObjectPool<ParticleSystem>(CreateHitPS, OnTakeHitPS, OnReturnedHitPS);
        }

        private void Update()
        {
            foreach (ParticleSystem shootPs in activeShootPS.Where(shootPs => !shootPs.isPlaying).ToList())
                shootPSPool.Release(shootPs);

            foreach (ParticleSystem hitPs in ActiveHitPS.Where(hitPs => !hitPs.isPlaying).ToList())
                _hitPSPool.Release(hitPs);
        }

        /// <summary>
        /// Called when a hit particle system is returned to the pool.
        /// Disables the particle system and removes it from the list of active hit particle systems.
        /// </summary>
        /// <param name="obj">The particle system being returned to the pool.</param>
        private void OnReturnedHitPS(ParticleSystem obj)
        {
            obj.gameObject.SetActive(false);

            if (ActiveHitPS.Contains(obj))
                ActiveHitPS.Remove(obj);
        }

        /// <summary>
        /// Called when a hit particle system is taken from the pool.
        /// Enables the particle system and adds it to the list of active hit particle systems.
        /// </summary>
        /// <param name="obj">The particle system being taken from the pool.</param>
        private void OnTakeHitPS(ParticleSystem obj)
        {
            obj.gameObject.SetActive(true);

            ActiveHitPS.Add(obj);
        }

        /// <summary>
        /// Creates and returns a new instance of the hit particle system.
        /// If the holder object for hit particle systems does not exist, it is created.
        /// </summary>
        /// <returns>A new instance of the hit particle system.</returns>
        private ParticleSystem CreateHitPS()
        {
            if (_hitPSHolder == null)
                _hitPSHolder = new GameObject("HitPSHolder");

            ParticleSystem instance = Instantiate(hitPSPrefab, _hitPSHolder.transform);

            return instance;
        }

        /// <summary>
        /// Called when a shoot particle system is returned to the pool.
        /// Disables the particle system and removes it from the list of active shoot particle systems.
        /// </summary>
        /// <param name="obj">The particle system being returned to the pool.</param>
        private void OnReturnedShootPS(ParticleSystem obj)
        {
            obj.gameObject.SetActive(false);

            if (activeShootPS.Contains(obj))
                activeShootPS.Remove(obj);
        }

        /// <summary>
        /// Called when a shoot particle system is taken from the pool.
        /// Enables the particle system and adds it to the list of active shoot particle systems.
        /// </summary>
        /// <param name="obj">The particle system being taken from the pool.</param>
        private void OnTakeShootPS(ParticleSystem obj)
        {
            obj.gameObject.SetActive(true);

            activeShootPS.Add(obj);
        }

        /// <summary>
        /// Creates and returns a new instance of the shoot particle system.
        /// The particle system is instantiated at the shoot position and has its local rotation and position reset.
        /// </summary>
        /// <returns>A new instance of the shoot particle system.</returns>
        private ParticleSystem CreateShootPS()
        {
            ParticleSystem instance = Instantiate(shootPSPrefab, shootPos);

            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localPosition = Vector3.zero;

            return instance;
        }

        /// <summary>
        /// Plays the hit particle system at the specified position.
        /// </summary>
        /// <param name="pos">The position to play the particle system at.</param>
        public void PlayHitFX(Vector3 pos)
        {
            ParticleSystem instance = _hitPSPool.Get();
            instance.transform.position = pos;
            instance.Play();
        }

        /// <summary>
        /// Plays the shoot particle system.
        /// </summary>
        public void PlayShootFX() => shootPSPool.Get().Play();
    }
}