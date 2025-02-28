using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private static UnityEngine.Camera MainCam => UnityEngine.Camera.main;

        [Header("SpawnSettings")] [SerializeField]
        private int maxWave = 5;

        [SerializeField] private float minWaitTime = .5f;
        [SerializeField] private float maxWaitTime = 3f;
        
        [Header("PositionSettings")] [SerializeField]
        private float positionOffset = -5f;

        [Header("References")] [SerializeField]
        private Ship ship;
        
        [SerializeField] private List<ShipVariants> variants;

        private List<Ship> activeShips = new();

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

        private void Update()
        {
            Vector3 vector3 = transform.position;
            vector3.x = MainCam.transform.position.x + positionOffset;
            
            transform.position = vector3;
            
            foreach (Ship activeShip in activeShips.Where(activeShip => activeShip.ShouldReturn()).ToList())
                _pool.Release(activeShip);
        }

        /// <summary>
        /// Called when a ship is returned to the pool.
        /// Disables the ship's game object and unsubscribes from the Return event.
        /// </summary>
        /// <param name="obj">The ship being returned to the pool.</param>
        private void OnReturnedToPool(Ship obj)
        {
            activeShips.Remove(obj);
            
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when a ship is taken from the pool.
        /// Resets the ship's position to the spawn position and enables it.
        /// </summary>
        /// <param name="obj">The ship being taken from the pool.</param>
        private void OnTakeFromPool(Ship obj)
        {
            obj.SetVariant(variants[Random.Range(0, variants.Count)]);

            obj.gameObject.SetActive(true);
        }

        /// <summary>
        /// Creates and returns a new instance of the ship.
        /// The ship is instantiated from the given prefab.
        /// </summary>
        /// <returns>A new instance of the ship.</returns>
        private Ship CreateShip()
        {
            ship = Instantiate(ship, transform);
            
            ship.Initialize();
            
            ship.gameObject.SetActive(false);
            
            return ship;
        }

        /// <summary>
        /// Spawns the given amount of ships, waits for a random amount of time and repeats.
        /// </summary>
        /// <returns>A coroutine that runs indefinitely until the <see cref="spawn"/> flag is set to <c>false</c>.</returns>
        private IEnumerator SpawnLoop()
        {
            while (spawn)
            {
                int waveSize = Random.Range(1, maxWave);

                for (int i = 0; i < waveSize; i++)
                    activeShips.Add(_pool.Get());

                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            }
        }
    }
}