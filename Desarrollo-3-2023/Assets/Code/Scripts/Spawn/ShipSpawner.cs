using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.SOs.Level;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Code.Scripts.Spawn
{
    /// <summary>
    /// Spawns ships and controls the spawn loop.
    /// </summary>
    public class ShipSpawner : MonoBehaviour
    {
        private static ObjectPool<Ship> _pool;

        [Header("SpawnSettings")] [SerializeField]
        private int maxWave = 5;
        [SerializeField] private float minWaitTime = .5f;
        [SerializeField] private float maxWaitTime = 3f;

        [Header("References")] [SerializeField]
        private Ship ship;

        [SerializeField] private List<ShipVariants> variants;

        private bool spawn = true;

        private void Awake()
        {
            _pool = new ObjectPool<Ship>(CreateShip, OnTakeFromPool, OnReturnedToPool);
        }

        private void OnDestroy()
        {
            spawn = false;
        }

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        /// <summary>
        /// Called when a ship is returned to the pool.
        /// Disables the ship's game object and unsubscribes from the Return event.
        /// </summary>
        /// <param name="obj">The ship being returned to the pool.</param>
        private void OnReturnedToPool(Ship obj)
        {
            ship.Return -= OnReturnShip;

            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when a ship is taken from the pool.
        /// Resets the ship's position to the spawn position and enables it.
        /// </summary>
        /// <param name="obj">The ship being taken from the pool.</param>
        private void OnTakeFromPool(Ship obj)
        {
            obj.gameObject.SetActive(true);

            ship.Return += OnReturnShip;

            obj.SetVariant(variants[Random.Range(0, variants.Count)]);
        }

        /// <summary>
        /// Creates and returns a new instance of the ship.
        /// The ship is instantiated from the given prefab.
        /// </summary>
        /// <returns>A new instance of the ship.</returns>
        private Ship CreateShip()
        {
            ship = Instantiate(ship, transform);

            return ship;
        }

        /// <summary>
        /// Called when a ship returns to the spawner.
        /// Releases the ship back into the pool so it can be reused.
        /// </summary>
        /// <param name="obj">The ship that returned to the spawner.</param>
        private void OnReturnShip(Ship obj)
        {
            _pool.Release(obj);
        }

        /// <summary>
        /// Spawns the given amount of ships, waits for a random amount of time and repeats.
        /// </summary>
        /// <returns>A coroutine that runs indefinitely until the <see cref="spawn"/> flag is set to <c>false</c>.</returns>
        private IEnumerator SpawnLoop()
        {
            while (spawn)
            {
                int waveSize = Random.Range(0, maxWave);

                for (int i = 0; i < waveSize; i++)
                    _pool.Get();

                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            }
        }
    }
}