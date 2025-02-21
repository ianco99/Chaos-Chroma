using System.Collections.Generic;
using Code.SOs.Enemy;
using UnityEngine;

namespace Code.Scripts.Enemy
{
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