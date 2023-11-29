using System.Collections.Generic;
using Code.Scripts.Enemy;
using Code.SOs.Enemy;
using UnityEngine;

namespace Code.Scripts.Spawn
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> prefabs;
        [SerializeField] private List<EnemySettings> config;

        private void Start()
        {
            if (prefabs.Count > 0)
                Spawn(0);
        }

        /// <summary>
        /// Spawns the enemy on given index of the list
        /// </summary>
        /// <param name="index">Index of the enemy on the list</param>
        private void Spawn(int index)
        {
            EnemyManager.Instance.SpawnEnemy(prefabs[index], transform.position, config[index]);
        }
    }
}