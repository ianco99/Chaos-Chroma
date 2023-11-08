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
            Spawn(0);
        }

        private void Spawn(int index)
        {
            EnemyManager.Instance.SpawnEnemy(prefabs[index], transform.position, config[index]);
        }
    }
}
