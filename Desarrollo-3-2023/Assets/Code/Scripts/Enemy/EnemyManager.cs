using System.Collections.Generic;
using Code.SOs.Enemy;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    /// <summary>
    /// Controls the enemies
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        private static EnemyManager instance;

        public static EnemyManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameObject("EnemyManager").AddComponent<EnemyManager>();

                return instance;
            }
        }

        private List<GameObject> prefabs = new();
        private List<GameObject> spawnedEnemies = new();

        private void Start()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
        }

        /// <summary>
        /// Spawns an enemy of type <paramref name="enemyType"/> at <paramref name="position"/> with the given <paramref name="config"/>.
        /// </summary>
        /// <param name="enemyType">The type of enemy to spawn.</param>
        /// <param name="position">The position at which to spawn the enemy.</param>
        /// <param name="config">The configuration for the spawned enemy.</param>
        public void SpawnEnemy(GameObject enemyType, Vector3 position, BaseEnemySettings config)
        {
            if (!prefabs.Contains(enemyType))
                prefabs.Add(enemyType);

            if (enemyType.TryGetComponent(out BaseEnemyController enemyController))
                enemyController.settings = config;

            GameObject enemy = Instantiate(enemyType, position, Quaternion.identity);
            
            spawnedEnemies.Add(enemy);
        }
    }
}